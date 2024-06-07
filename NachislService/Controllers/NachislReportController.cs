using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NachislService.Helpers;
using NachislService.Models;
using NachislService.Repository;

namespace NachislService.Controllers
{
    [Route("api/reportnachisls")]
    [ApiController]
    public class NachislReportController : ControllerBase
    {
        private readonly BillingDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _payServiceURL = ConfigurationHelper.GetSectionValue("PayServiceURL");

        public NachislReportController(BillingDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Составляет ОСВ с расшифровкой по каждому абоненту
        /// </summary>
        /// <param name="model">Модель запроса составления ОСВ</param>
        /// <returns>Объект ОСВ с расшифровкой по каждому абоненту</returns>
        [HttpPost]
        [Route("osv-each-abonent")]
        [Authorization]
        public async Task<OSVEachAbonentResponse> GetOsvEachAbonent([FromBody] OSVEachAbonentRequest model)
        {
            var appInformationToReturn = _context.Modes
               .Join(_context.AbonentModes,
               ns => ns.ModeCd,
               combinedEntry => combinedEntry.ModeCd,
               (ns, combinedEntry) => new
               {
                   AbonentModeСd = combinedEntry.AbonentModeСd,
                   AccountCd = combinedEntry.AccountCd,
                   ServiceCd = ns.ServiceCd,
               }).Join(_context.NachislSummas,
               combinedEntry => combinedEntry.AbonentModeСd,
               ns => ns.AbonentModeСd,
               (combinedEntry, ns) => new
               {
                   NachislSum = ns.NachislSum,
                   NachislYear = ns.NachislYear,
                   NachislMonth = ns.NachislMonth,
                   AccountCd = combinedEntry.AccountCd,
                   ServiceCd = combinedEntry.ServiceCd,
               }).Where(n => n.ServiceCd == model.ServiceCd && n.NachislMonth >= model.StartMonth && n.NachislMonth <= model.EndMonth
                        && n.NachislYear >= model.StartYear && n.NachislYear <= model.EndYear)
               .GroupBy(n => new { n.AccountCd })
               .ToList();

            List<OSVEachAbonent> OsvEachAbonent = new List<OSVEachAbonent>();

            foreach (var item in appInformationToReturn)
            {
                var remainEnd = _context.Remains
                    .FirstOrDefault(r => r.Remmonth == model.EndMonth && r.Remyear == model.EndYear && r.AccountCd == item.Key.AccountCd
                            && r.ServiceCd == model.ServiceCd);

                var remainBegin = _context.Remains
                    .FirstOrDefault(r => r.Remmonth == model.StartMonth - 1 && r.Remyear == model.StartYear && r.AccountCd == item.Key.AccountCd
                            && r.ServiceCd == model.ServiceCd);

                model.AccountCd = item.Key.AccountCd;
                PayResponseHist payMonthResult = await SenderByURL.SendHTTPRequest<OSVEachAbonentRequest>(model, _payServiceURL + "/api/pays/pays-each-abonent");

                var abonent = _context.Abonents.FirstOrDefault(a => a.AccountCd == item.Key.AccountCd);
                var street = _context.Streets.FirstOrDefault(s => s.StreetCd == abonent.StreetCd);
                var address = $"ул. {street.StreetName}, д. {abonent.HouseNo}, кв. {abonent.FlatNo}";

                var startSum = remainBegin == null ? 0 : remainBegin.Remainsum;
                var endSum = remainEnd == null ? 0 : remainEnd.Remainsum;

                OSVEachAbonent OsvAbonent = new OSVEachAbonent()
                {
                    Accountcd = item.Key.AccountCd,
                    FIO = abonent.Fio,
                    Address = address,
                    BeginDebetSum = startSum > 0 ? startSum : 0,
                    BeginKreditSum = startSum < 0 ? startSum * -1 : 0,
                    NachislSum = item.Sum(n => n.NachislSum),
                    PaySum = payMonthResult == null ? 0 : payMonthResult.PaySumm,
                    FinishDebetSum = endSum > 0 ? endSum : 0,
                    FinishKreditSum = endSum < 0 ? endSum * -1 : 0,
                };
                OsvEachAbonent.Add(OsvAbonent);
            }

            OSVEachAbonentResponse _OSVEachAbonentResponse = new OSVEachAbonentResponse();
            _OSVEachAbonentResponse.OsvEachAbonent = OsvEachAbonent;
            _OSVEachAbonentResponse.TotalStartDebet = OsvEachAbonent.Sum(osv => osv.BeginDebetSum);
            _OSVEachAbonentResponse.TotalStartKredit = OsvEachAbonent.Sum(osv => osv.BeginKreditSum);
            _OSVEachAbonentResponse.TotalNachisl = OsvEachAbonent.Sum(osv => osv.NachislSum);
            _OSVEachAbonentResponse.TotalPay = OsvEachAbonent.Sum(osv => osv.PaySum);
            _OSVEachAbonentResponse.TotalFinishDebet = OsvEachAbonent.Sum(osv => osv.FinishDebetSum);
            _OSVEachAbonentResponse.TotalFinishKredit = OsvEachAbonent.Sum(osv => osv.FinishKreditSum);

            return _OSVEachAbonentResponse;
        }
    }
}
