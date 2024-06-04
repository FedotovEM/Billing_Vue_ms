using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayService.DTO;
using PayService.Helpers;
using PayService.Models;
using PayService.Repository;
using PayService.Repository.Models;

namespace PayService.Controllers
{
    [Route("api/pays")]
    [ApiController]
    public class PaysController : ControllerBase
    {
        private readonly BillingPayDbContext _context;
        private readonly IConfiguration _configuration;

        public PaysController(BillingPayDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Выдаёт данные для оплаты по услугам абонента
        /// </summary>
        /// <param name="model">Модель поиска абонента</param>
        /// <returns>Объект данных для оплаты услуг по абоненту</returns>
        [HttpPost]
        [Route("search-abonent")]
        public SearchAbonentResponse SearchAbonent([FromBody] SearchAbonentModel model)
        {
            Abonent abonent = new Abonent();
            if (!string.IsNullOrEmpty(model.accountCd))
            {
                string accountCd = model.accountCd.Split(' ')[0];
                abonent = _context.Abonents.First(a => a.AccountCd == accountCd);
            }

            SearchAbonentResponse abonentResp = new SearchAbonentResponse(abonent);
            var street = _context.Streets.FirstOrDefault(s => s.StreetCd == abonent.StreetCd);
            abonentResp.StreetName = street.StreetName;

            var resultAM = _context.AbonentModes.Where(a => a.AccountCd == abonent.AccountCd);
            var serviceCounterList = resultAM.Join(_context.Modes,
                                  t2 => t2.ModeCd,
                                  t3 => t3.ModeCd,
                                  (t2, t3) => new { t2, t3 })
                            .Join(_context.Services,
                                  t => t.t3.ServiceCd,
                                  t4 => t4.ServiceCd,
                                  (t, t4) => new
                                  {
                                      AccountCd = t.t2.AccountCd,
                                      Counter = t.t2.Counterr,
                                      ServiceName = t4.ServiceName,
                                      ServiceCd = t4.ServiceCd
                                  }).ToList();

            var abonentRemainsList = _context.Abonents.Where(a => a.AccountCd == abonent.AccountCd)
                .Join(_context.Remains,
                        t1 => t1.AccountCd,
                        t2 => t2.AccountCd,
                        (t1, t2) => new { t1, t2 })
                .Join(_context.Services,
                         t => t.t2.ServiceCd,
                         t3 => t3.ServiceCd,
                         (t, t3) => new
                         {
                             AccountCd = t.t1.AccountCd,
                             ServiceCd = t3.ServiceCd,
                             RemCd = t.t2.RemainCd,
                             RemMonth = t.t2.Remmonth,
                             RemYear = t.t2.Remyear,
                             RemSum = t.t2.Remainsum
                         }).ToList();
            if (!serviceCounterList.Any() || !abonentRemainsList.Any()) return abonentResp;

            var maxRemainYear = abonentRemainsList.Max(r => r.RemYear);
            var fullAbonentRemainData = serviceCounterList
                .Join(abonentRemainsList,
                      t1 => t1.ServiceCd,
                      t2 => t2.ServiceCd,
                      (t1, t2) => new
                      {
                          AccountCd = t1.AccountCd,
                          ServiceCd = t1.ServiceCd,
                          ServiceName = t1.ServiceName,
                          Counter = t1.Counter,
                          RemCd = t2.RemCd,
                          RemMonth = t2.RemMonth,
                          RemYear = t2.RemYear,
                          RemSum = t2.RemSum
                      })
                .Where(ab => ab.RemYear == maxRemainYear).ToList();

            var maxRemainMonth = fullAbonentRemainData.Max(r => r.RemMonth);
            fullAbonentRemainData = fullAbonentRemainData.Where(ab => ab.RemMonth == maxRemainMonth).ToList();

            foreach (var AbonentRemainData in fullAbonentRemainData)
            {
                RemainsData remainsData = new RemainsData()
                {
                    ServiceCd = AbonentRemainData.ServiceCd,
                    ServiceName = AbonentRemainData.ServiceName,
                    Counter = AbonentRemainData.Counter,
                    RemainCd = AbonentRemainData.RemCd,
                    RemainMonth = AbonentRemainData.RemMonth,
                    RemainYear = AbonentRemainData.RemYear,
                    RenSum = (decimal)AbonentRemainData.RemSum
                };
                abonentResp.AbonentRemains.Add(remainsData);
            }

            return abonentResp;
        }

        /// <summary>
        /// Выдаёт данные к оплате по остатку
        /// </summary>
        /// <param name="id">Код остатка</param>
        /// <returns>Количество платежей</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PaySummaDTO>> GetAbonentPay(int id)
        {
            var remain = await _context.Remains.FindAsync(id);
            if (remain == null) return NotFound();
            var service = await _context.Services.FirstOrDefaultAsync(s => s.ServiceCd == remain.ServiceCd);
            PaySummaDTO newPay = new PaySummaDTO()
            {
                AccountCd = remain.AccountCd,
                ServiceName = service.ServiceName,
                PayMonth = remain.Remmonth,
                PayYear = remain.Remyear,
                PaySum = (decimal)remain.Remainsum
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
            newPay.AbonentModeСd = res.AbonentModeСd;
            newPay.PayType = res.Counter ? "фактический" : "нормативный";

            var receptionPoint = _context.ReceptionPoints.FirstOrDefault(rp => rp.ReceptionPointCd == 1);
            if (receptionPoint != null) newPay.ReceptionName = receptionPoint.ReceptionName;
            return newPay;
        }

        /// <summary>
        /// Получает статистические данные
        /// </summary>
        /// <returns>Количество платежей</returns>
        [HttpPost]
        [Route("get-stat-count")]
        //[Authorization]
        public int GetStatisticCount()
        {
            var countPays = _context.PaySummas.Count();
            return countPays;
        }

        /// <summary>
        /// Получает сумму платежей по абоненту для истории платежей и начислений
        /// </summary>
        /// <param name="model">Модель запроса для формирования суммы платежей</param>
        /// <returns>Объект со значением суммы платежей</returns>
        [HttpPost]
        [Route("pays-abonent-hist")]
        //[Authorization]
        public PayResponse GetPaysAbonentHist([FromBody] PayRequestHist model)
        {
            var paysByMonthNachisl = _context.Services
               .Join(_context.Modes,
               s => s.ServiceCd,
               m => m.ServiceCd,
               (s, m) => new
               {
                   ModeCd = m.ModeCd,
                   ServiceCd = s.ServiceCd,
               }).Join(_context.AbonentModes,
               ns => ns.ModeCd,
               combinedEntry => combinedEntry.ModeCd,
               (ns, combinedEntry) => new
               {
                   AbonentModeСd = combinedEntry.AbonentModeСd,
                   AccountCd = combinedEntry.AccountCd,
                   ModeCd = ns.ModeCd,
                   ServiceCd = ns.ServiceCd,
               }).Join(_context.PaySummas,
               combinedEntry => combinedEntry.AbonentModeСd,
               ns => ns.AbonentModeСd,
               (combinedEntry, ns) => new
               {
                   PaySum = ns.PaySum,
                   PayYear = ns.PayYear,
                   PayMonth = ns.PayMonth,
                   AccountCd = combinedEntry.AccountCd,
                   ServiceCd = combinedEntry.ServiceCd,
               }).Where(p => p.PayMonth == model.Month && p.PayYear == model.Year &&
                        p.ServiceCd == model.ServiceCd && p.AccountCd == model.AccountCd).ToList();

            PayResponse pay = new PayResponse()
            {
                PaySumm = paysByMonthNachisl == null ? 0 : paysByMonthNachisl.Sum(p => p.PaySum),
            };
            return pay;
        }

        /// <summary>
        /// Получает сумму платежей по абоненту для ОСВ с расшифровкой по каждому абоненту
        /// </summary>
        /// <param name="model">Модель запроса для формирования суммы платежей</param>
        /// <returns>Объект со значением суммы платежей</returns>
        [HttpPost]
        [Route("pays-each-abonent")]
        //[Authorization]
        public PayResponse GetPaysEachAbonent([FromBody] OSVEachAbonent model)
        {
            var paysByMonthNachisl = _context.Services
               .Join(_context.Modes,
               s => s.ServiceCd,
               m => m.ServiceCd,
               (s, m) => new
               {
                   ModeCd = m.ModeCd,
                   ServiceCd = s.ServiceCd,
               }).Join(_context.AbonentModes,
               ns => ns.ModeCd,
               combinedEntry => combinedEntry.ModeCd,
               (ns, combinedEntry) => new
               {
                   AbonentModeСd = combinedEntry.AbonentModeСd,
                   AccountCd = combinedEntry.AccountCd,
                   ModeCd = ns.ModeCd,
                   ServiceCd = ns.ServiceCd,
               }).Join(_context.PaySummas,
               combinedEntry => combinedEntry.AbonentModeСd,
               ns => ns.AbonentModeСd,
               (combinedEntry, ns) => new
               {
                   PaySum = ns.PaySum,
                   PayYear = ns.PayYear,
                   PayMonth = ns.PayMonth,
                   AccountCd = combinedEntry.AccountCd,
                   ServiceCd = combinedEntry.ServiceCd,
               }).Where(p => p.PayMonth >= model.StartMonth && p.PayMonth <= model.EndMonth && p.PayYear >= model.StartYear &&
                        p.PayYear <= model.EndYear && p.ServiceCd == model.ServiceCd && p.AccountCd == model.AccountCd).ToList();

            PayResponse pay = new PayResponse()
            {
                PaySumm = paysByMonthNachisl == null ? 0 : paysByMonthNachisl.Sum(p => p.PaySum),
            };
            return pay;
        }
    }
}
