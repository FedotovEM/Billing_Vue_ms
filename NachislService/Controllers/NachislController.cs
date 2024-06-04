using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NachislService.DTO;
using NachislService.Helpers;
using NachislService.Models;
using NachislService.Repository;
using NachislService.Repository.Models;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace NachislService.Controllers
{
    [Route("api/nachisls")]
    [ApiController]
    public class NachislController : ControllerBase
    {
        private readonly BillingDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _payServiceURL = ConfigurationHelper.GetSectionValue("PayServiceURL");

        public NachislController(BillingDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Выдает данные начислений по остатку
        /// </summary>
        /// <param name="id">Код остатка</param>
        /// <returns>Модель начислений</returns>
        [HttpGet("{id}")]
        [Authorization]
        public async Task<ActionResult<NachislSummaDTO>> GetAbonentNachisl(int id)
        {
            var remain = await _context.Remains.FindAsync(id);
            if (remain == null) return NotFound();
            var service = await _context.Services.FirstOrDefaultAsync(s => s.ServiceCd == remain.ServiceCd);
            NachislSummaDTO newNach = new NachislSummaDTO()
            {
                AccountCd = remain.AccountCd,
                ServiceName = service.ServiceName,
                NachislMonth = remain.Remmonth,
                NachislYear = remain.Remyear,
                NachislSum = (decimal)remain.Remainsum
            };

            var abonMode = _context.AbonentModes.Where(ab => ab.AccountCd == remain.AccountCd);
            var modes = _context.Modes.Where(m => m.ServiceCd == remain.ServiceCd);
            var res = abonMode
                    .Join(modes,
                          t1 => t1.ModeCd,
                          t2 => t2.ModeCd,
                          (t1, t2) => new
                          {
                              AccountCd = t1.AccountCd,
                              Counter = t1.Counterr,
                              AbonentModeСd = t1.AbonentModeСd
                          }).FirstOrDefault();
            if (res == null) return NotFound();
            newNach.AbonentModeСd = res.AbonentModeСd;
            newNach.NachType = res.Counter ? "фактический" : "нормативный";
            return newNach;
        }

        /// <summary>
        /// Выдает карточку абонента
        /// </summary>
        /// <param name="model">Модель запроса составления карточки</param>
        /// <returns>Объект карточки абонента</returns>
        [HttpPost]
        [Route("search-abonent")]
        [Authorization]
        public SearchAbonentCardResponse SearchAbonent([FromBody] SearchAbonentModel model)
        {
            Abonent abonent = new Abonent();
            if (!string.IsNullOrEmpty(model.accountCd))
            {
                string accountCd = model.accountCd.Split(' ')[0];
                abonent = _context.Abonents.First(a => a.AccountCd == accountCd);
            }
            if (abonent == null) return null;

            SearchAbonentCardResponse abonentResp = new SearchAbonentCardResponse(abonent);
            var street = _context.Streets.FirstOrDefault(s => s.StreetCd == abonent.StreetCd);
            abonentResp.StreetName = street.StreetName;
            abonentResp.AbonentCards = _context.getAbonentCard(abonent.AccountCd);

            return abonentResp;
        }

        /// <summary>
        /// Составляет историю оплат и начислений по заданным абоненту и услуге
        /// </summary>
        /// <param name="model">Модель запроса составления истории</param>
        /// <returns>Список истории оплат и начислений по месяцам</returns>
        [HttpPost]
        [Route("pay-nachisl-hist")]
        [Authorization]
        public async Task<List<PayNachislHistory>> GetPayNachislHist([FromBody] PayNachislHistoryRequest model)
        {
            var appInformationToReturn = _context.Services
               .Join(_context.Modes,
               s => s.ServiceCd,
               m => m.ServiceCd,
               (s, m) => new
               {
                   ModeCd = m.ModeCd,
                   ServiceName = s.ServiceName,
                   ServiceCd = s.ServiceCd,
               }).Join(_context.AbonentModes,
               ns => ns.ModeCd,
               combinedEntry => combinedEntry.ModeCd,
               (ns, combinedEntry) => new
               {
                   AbonentModeСd = combinedEntry.AbonentModeСd,
                   AccountCd = combinedEntry.AccountCd,
                   ModeCd = ns.ModeCd,
                   ServiceName = ns.ServiceName,
                   ServiceCd = ns.ServiceCd,
               }).Join(_context.NachislSummas,
               combinedEntry => combinedEntry.AbonentModeСd,
               ns => ns.AbonentModeСd,
               (combinedEntry, ns) => new
               {
                   NachislSum = ns.NachislSum,
                   NachislYear = ns.NachislYear,
                   NachislMonth = ns.NachislMonth,
                   AbonentModeСd = ns.AbonentModeСd,
                   CountResources = ns.CountResources,
                   AccountCd = combinedEntry.AccountCd,
                   ServiceName = combinedEntry.ServiceName,
                   ServiceCd = combinedEntry.ServiceCd,
               }).Where(n => n.AccountCd == model.AccountCd && n.ServiceCd == model.ServiceCd)
               .GroupBy(n => new { n.NachislYear, n.NachislMonth, n.ServiceCd, n.ServiceName})
               .ToList();

            List<PayNachislHistory> payNachislHistories = new List<PayNachislHistory>();

            foreach (var item in appInformationToReturn)
            {
                var remainEnd = _context.Remains
                    .FirstOrDefault(r => r.Remmonth == item.Key.NachislMonth && r.Remyear == item.Key.NachislYear && r.ServiceCd == item.Key.ServiceCd
                    && r.AccountCd == model.AccountCd);

                int beginMonth = item.Key.NachislMonth - 1;
                int beginYear = item.Key.NachislYear;
                if (beginMonth <= 0)
                {
                    beginMonth = 12;
                    beginYear = beginYear - 1;
                } 

                var remainBegin = _context.Remains
                    .FirstOrDefault(r => r.Remmonth == beginMonth && r.Remyear == beginYear && r.ServiceCd == item.Key.ServiceCd
                    && r.AccountCd == model.AccountCd);

                PayRequestHist PayRequestHist = new PayRequestHist()
                {
                    Month = item.Key.NachislMonth,
                    Year = item.Key.NachislYear,
                    ServiceCd = item.Key.ServiceCd,
                    AccountCd = model.AccountCd,
                };

                PayResponseHist payMonthResult = await SenderByURL.SendHTTPRequest<PayRequestHist>(PayRequestHist, _payServiceURL + "/pays/pays-abonent-hist");

                PayNachislHistory payNachislHistory = new PayNachislHistory()
                {
                    ServiceCd = item.Key.ServiceCd,
                    ServiceName = item.Key.ServiceName,
                    AccountingMonth = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(item.Key.NachislMonth)} {item.Key.NachislYear}",
                    BeginRemainSum = remainBegin == null ? 0 : remainBegin.Remainsum,
                    NachislSum = item.Sum(n => n.NachislSum),
                    CountResources = item.Sum(n => n.CountResources),
                    PaySum = payMonthResult == null ? 0 : payMonthResult.PaySumm,
                    EndRemainSum = remainEnd == null ? 0 : remainEnd.Remainsum,
                    Month = item.Key.NachislMonth,
                };
                payNachislHistories.Add(payNachislHistory);
            }

            return payNachislHistories
               .OrderByDescending(p => p.Month).ToList();
        }
    }
}
