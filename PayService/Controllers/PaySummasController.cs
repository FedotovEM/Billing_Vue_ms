using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayService.DTO;
using PayService.Helpers;
using PayService.Repository;
using PayService.Repository.Models;

namespace PayService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaySummasController : ControllerBase
    {
        private readonly BillingPayDbContext _context;

        public PaySummasController(BillingPayDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает все платежи
        /// </summary>
        /// <returns>Возвращает все платежи с указанием значений из родительских таблиц</returns>
        [HttpGet]
        [Authorization]
        public async Task<ActionResult<IEnumerable<PaySummaDTO>>> GetPaySummas()
        {
          if (_context.PaySummas == null) return NotFound();

            List<PaySummaDTO> paySummaDTOs = new List<PaySummaDTO>();
            PaySummaDTO dTO;

            var appInformationToReturn = _context.Services
                .Join(_context.Modes,
                s => s.ServiceCd,
                m => m.ServiceCd,
                (s, m) => new
                {
                    ModeCd = m.ModeCd,
                    ServiceName = s.ServiceName
                }).Join(_context.AbonentModes,
                ns => ns.ModeCd,
                combinedEntry => combinedEntry.ModeCd,
                (ns, combinedEntry) => new
                {
                    AbonentModeСd = combinedEntry.AbonentModeСd,
                    AccountCd = combinedEntry.AccountCd,
                    ModeCd = ns.ModeCd,
                    ServiceName = ns.ServiceName
                }).Join(
                _context.PaySummas,
                combinedEntry => combinedEntry.AbonentModeСd,
                ns => ns.AbonentModeСd,
                (combinedEntry, ns) => new
                {
                    PayFactCd = ns.PayFactCd,
                    PaySum = ns.PaySum,
                    PayDate = ns.PayDate,
                    PayMonth = ns.PayMonth,
                    PayYear = ns.PayYear,
                    AbonentModeСd = ns.AbonentModeСd,
                    ReceptionPointCd = ns.ReceptionPointCd,
                    PayType = ns.PayType,
                    AccountCd = combinedEntry.AccountCd,
                    ServiceName = combinedEntry.ServiceName
                }).Join(
                _context.ReceptionPoints,
                combinedEntry => combinedEntry.ReceptionPointCd,
                ns => ns.ReceptionPointCd,
                (combinedEntry, ns) => new
                {
                    PayFactCd = combinedEntry.PayFactCd,
                    PaySum = combinedEntry.PaySum,
                    PayDate = combinedEntry.PayDate,
                    PayMonth = combinedEntry.PayMonth,
                    PayYear = combinedEntry.PayYear,
                    AbonentModeСd = combinedEntry.AbonentModeСd,
                    PayType = combinedEntry.PayType,
                    AccountCd = combinedEntry.AccountCd,
                    ServiceName = combinedEntry.ServiceName,
                    ReceptionName = ns.ReceptionName,
                });

            foreach (var item in appInformationToReturn)
            {
                dTO = new PaySummaDTO()
                {
                    PayFactCd = item.PayFactCd,
                    PaySum = item.PaySum,
                    PayDate = item.PayDate,
                    PayMonth = item.PayMonth,
                    PayYear = item.PayYear,
                    AbonentModeСd = item.AbonentModeСd,
                    PayType = item.PayType,
                    AccountCd = item.AccountCd,
                    ServiceName = item.ServiceName,
                    ReceptionName = item.ReceptionName,
                };
                paySummaDTOs.Add(dTO);
            }
            return paySummaDTOs;
        }

        /// <summary>
        /// Получает определенный платёж
        /// </summary>
        /// <param name="id">Код платежа</param>
        /// <returns>Объект платежа с указанием значений из родительских таблиц</returns>
        [HttpGet("{id}")]
        [Authorization]
        public async Task<ActionResult<PaySummaDTO>> GetPaySumma(int id)
        {
            var paySumma = await _context.PaySummas.FindAsync(id);
            if (paySumma == null) return NotFound();
            PaySummaDTO dTO = new PaySummaDTO()
            {
                PayFactCd = paySumma.PayFactCd,
                PaySum = paySumma.PaySum,
                PayDate = paySumma.PayDate,
                PayMonth = paySumma.PayMonth,
                PayYear = paySumma.PayYear,
                PayType = paySumma.PayType,
            };

            var appInformationToReturn = _context.PaySummas
                .Join(_context.AbonentModes,
                n => n.AbonentModeСd,
                am => am.AbonentModeСd,
                (n, am) => new
                {
                    AbonentModeСd = n.AbonentModeСd,
                    AccountCd = am.AccountCd,
                    ModeCd = am.ModeCd,
                }).Where(r => r.AbonentModeСd == paySumma.AbonentModeСd).FirstOrDefault();

            var appInformationToReturn2 = _context.Services
            .Join(_context.Modes,
            n => n.ServiceCd,
            am => am.ServiceCd,
            (n, am) => new
            {
                ServiceName = n.ServiceName,
                ModeCd = am.ModeCd,
            }).Where(r => r.ModeCd == appInformationToReturn.ModeCd).FirstOrDefault();

            dTO.AccountCd = appInformationToReturn.AccountCd;
            dTO.ServiceName = appInformationToReturn2.ServiceName;

            var receptionPoint = await _context.ReceptionPoints.FindAsync(paySumma.ReceptionPointCd);
            dTO.ReceptionName = receptionPoint.ReceptionName;
            return dTO;
        }

        /// <summary>
        /// Обновляет значения определенного платежа
        /// </summary>
        /// <param name="dTO">Модель платежа с новыми значениями</param>
        /// <returns>Статус выполнения обновления</returns>
        [HttpPut]
        [Authorization]
        public async Task<IActionResult> PutPaySumma(PaySummaDTO dTO)
        {
            var paySumma = await _context.PaySummas.FindAsync(dTO.PayFactCd);
            if (paySumma == null) return NotFound();
            paySumma.PaySum = dTO.PaySum;
            paySumma.PayDate = dTO.PayDate;
            paySumma.PayMonth = dTO.PayMonth;
            paySumma.PayYear = dTO.PayYear;
            paySumma.PayType = dTO.PayType;

            var appInformationToReturn = _context.Services
                .Join(_context.Modes,
                s => s.ServiceCd,
                m => m.ServiceCd,
                (s, m) => new
                {
                    ModeCd = m.ModeCd,
                    ServiceName = s.ServiceName
                }).Where(r => r.ServiceName == dTO.ServiceName)
                .Join(_context.AbonentModes,
                ns => ns.ModeCd,
                combinedEntry => combinedEntry.ModeCd,
                (ns, combinedEntry) => new
                {
                    AbonentModeСd = combinedEntry.AbonentModeСd,
                    ModeCd = ns.ModeCd,
                    AccountCd = combinedEntry.AccountCd
                }).Where(r => r.AccountCd == dTO.AccountCd).FirstOrDefault();

            if (appInformationToReturn == null) return NotFound();
            paySumma.AbonentModeСd = appInformationToReturn.AbonentModeСd;
            var receptionPoint = await _context.ReceptionPoints.FirstOrDefaultAsync(rp => rp.ReceptionName == dTO.ReceptionName);
            paySumma.ReceptionPointCd = receptionPoint.ReceptionPointCd;
            _context.PaySummas.Update(paySumma);
            _context.SaveChanges();
            return Ok(dTO);
        }

        /// <summary>
        /// Добавляет новый платёж
        /// </summary>
        /// <param name="dTO">Модель платежа с новыми значениями</param>
        /// <returns>Статус выполнения добавления</returns>
        [HttpPost]
        [Authorization]
        public async Task<ActionResult<PaySumma>> PostPaySumma(PaySummaDTO dTO)
        {
          if (_context.PaySummas == null)
              return Problem("Entity set 'BillingDbContext.PaySummas'  is null.");
          
            PaySumma paySumma = new PaySumma();
            paySumma.PaySum = dTO.PaySum;
            paySumma.PayYear = dTO.PayYear;
            paySumma.PayMonth = dTO.PayMonth;
            paySumma.PayType = dTO.PayType;
            paySumma.PayDate = dTO.PayDate;

            BillingPayDbContext context = new BillingPayDbContext();

            var appInformationToReturn = context.Services
                .Join(context.Modes,
                s => s.ServiceCd,
                m => m.ServiceCd,
                (s, m) => new
                {
                    ModeCd = m.ModeCd,
                    ServiceName = s.ServiceName,
                    ServiceCd = s.ServiceCd
                }).Where(r => r.ServiceName == dTO.ServiceName)
                .Join(context.AbonentModes,
                ns => ns.ModeCd,
                combinedEntry => combinedEntry.ModeCd,
                (ns, combinedEntry) => new
                {
                    AbonentModeСd = combinedEntry.AbonentModeСd,
                    ModeCd = ns.ModeCd,
                    ServiceCd = ns.ServiceCd,
                    AccountCd = combinedEntry.AccountCd
                }).Where(r => r.AccountCd == dTO.AccountCd).FirstOrDefault();

            paySumma.AbonentModeСd = appInformationToReturn.AbonentModeСd;

            var receptionPoint = await context.ReceptionPoints.FirstOrDefaultAsync(rp => rp.ReceptionName == dTO.ReceptionName);
            paySumma.ReceptionPointCd = receptionPoint.ReceptionPointCd;

            context.pay(appInformationToReturn.AccountCd, appInformationToReturn.ServiceCd, paySumma.PayMonth, paySumma.PayYear, null, paySumma.PaySum, paySumma.ReceptionPointCd);
            
            return CreatedAtAction("GetPaySumma", new { id = paySumma.PayFactCd }, paySumma);
        }

        /// <summary>
        /// Удаляет определенный платёж
        /// </summary>
        /// <param name="id">Код платежа</param>
        /// <returns>Статус выполнения удаления</returns>
        [HttpDelete("{id}")]
        [Authorization]
        public async Task<IActionResult> DeletePaySumma(int id)
        {
            if (_context.PaySummas == null) return NotFound();
            var paySumma = await _context.PaySummas.FindAsync(id);

            if (paySumma == null) return NotFound();

            _context.PaySummas.Remove(paySumma);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
