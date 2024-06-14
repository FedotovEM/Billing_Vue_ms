using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NachislService.DTO;
using NachislService.Helpers;
using NachislService.Repository;
using NachislService.Repository.Models;

namespace NachislService.Controllers
{
    [Route("api/nachislenie/[controller]")]
    [ApiController]
    public class NachislSummasController : ControllerBase
    {
        private readonly BillingDbContext _context;

        public NachislSummasController(BillingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает все начисления
        /// </summary>
        /// <returns>Возвращает все начисления с указанием значений из родительских таблиц</returns>
        [HttpGet]
        [Authorization]
        public async Task<ActionResult<IEnumerable<NachislSummaDTO>>> GetNachislSummas()
        {
            if (_context.NachislSummas == null) return NotFound();
            List<NachislSummaDTO> modeDTOs = new List<NachislSummaDTO>();
            NachislSummaDTO dTO;

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
                _context.NachislSummas,
                combinedEntry => combinedEntry.AbonentModeСd,
                ns => ns.AbonentModeСd,
                (combinedEntry, ns) => new
                {
                    NachislFactCd = ns.NachislFactCd,
                    NachislSum = ns.NachislSum,
                    NachislYear = ns.NachislYear,
                    NachislMonth = ns.NachislMonth,
                    NachType = ns.NachType,
                    AbonentModeСd = ns.AbonentModeСd,
                    CountResources = ns.CountResources,
                    AccountCd = combinedEntry.AccountCd,
                    ServiceName = combinedEntry.ServiceName
                });

            foreach (var item in appInformationToReturn)
            {
                dTO = new NachislSummaDTO()
                {
                    NachislFactCd = item.NachislFactCd,
                    NachislSum = item.NachislSum,
                    NachislYear = item.NachislYear,
                    NachislMonth = item.NachislMonth,
                    NachType = item.NachType,
                    AbonentModeСd = item.AbonentModeСd,
                    CountResources = item.CountResources,
                    AccountCd = item.AccountCd,
                    ServiceName = item.ServiceName
                };
                modeDTOs.Add(dTO);
            }
            return modeDTOs;
        }

        /// <summary>
        /// Получает определенное начисление
        /// </summary>
        /// <param name="id">Код начисления</param>
        /// <returns>Объект начисления с указанием значений из родительских таблиц</returns>
        [HttpGet("{id}")]
        [Authorization]
        public async Task<ActionResult<NachislSummaDTO>> GetNachislSumma(int id)
        {
            var nachislSumma = await _context.NachislSummas.FindAsync(id);
            if (nachislSumma == null) return NotFound();
            NachislSummaDTO dTO = new NachislSummaDTO()
            {
                NachislFactCd = nachislSumma.NachislFactCd,
                NachislSum = nachislSumma.NachislSum,
                NachislYear = nachislSumma.NachislYear,
                NachislMonth = nachislSumma.NachislMonth,
                NachType = nachislSumma.NachType,
                AbonentModeСd = nachislSumma.AbonentModeСd,
                CountResources = nachislSumma.CountResources
            };

            var appInformationToReturn = _context.NachislSummas
                .Join(_context.AbonentModes,
                n => n.AbonentModeСd,
                am => am.AbonentModeСd,
                (n, am) => new
                {
                    AbonentModeСd = n.AbonentModeСd,
                    AccountCd = am.AccountCd,
                    ModeCd = am.ModeCd,
                }).Where(r => r.AbonentModeСd == nachislSumma.AbonentModeСd).FirstOrDefault();

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

            return dTO;
        }

        /// <summary>
        /// Обновляет значения определенного начисления
        /// </summary>
        /// <param name="dTO">Модель начисления с новыми значениями</param>
        /// <returns>Статус выполнения обновления</returns>
        [HttpPut]
        [Authorization]
        public async Task<IActionResult> PutNachislSumma(NachislSummaDTO dTO)
        {
            var appInformationToReturn = _context.Services
                .Join(_context.Modes,
                s => s.ServiceCd,
                m => m.ServiceCd,
                (s, m) => new
                {
                    Servicecd = s.ServiceCd,
                    ModeCd = m.ModeCd,
                    ServiceName = s.ServiceName
                }).Where(r => r.ServiceName == dTO.ServiceName)
                .Join(_context.AbonentModes,
                ns => ns.ModeCd,
                combinedEntry => combinedEntry.ModeCd,
                (ns, combinedEntry) => new
                {
                    ServiceCd = ns.Servicecd,
                    AbonentModeСd = combinedEntry.AbonentModeСd,
                    ModeCd = ns.ModeCd,
                    AccountCd = combinedEntry.AccountCd
                }).Where(r => r.AccountCd == dTO.AccountCd).FirstOrDefault();
            if (appInformationToReturn == null) return NotFound();

            var nachislSumma = await _context.NachislSummas.FindAsync(dTO.NachislFactCd);
            //if (nachislSumma == null)
            //{
                if (dTO.RemainCd > 0)
                {
                    _context.NachislToFact(appInformationToReturn.AccountCd, appInformationToReturn.ServiceCd, dTO.NachislMonth, dTO.NachislYear, dTO.CountResources);
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            //}

            nachislSumma.NachislSum = dTO.NachislSum;
            nachislSumma.NachislMonth = dTO.NachislMonth;
            nachislSumma.NachislYear = dTO.NachislYear;
            nachislSumma.NachType = dTO.NachType;
            nachislSumma.CountResources = dTO.CountResources;
            nachislSumma.AbonentModeСd = appInformationToReturn.AbonentModeСd;
            
            _context.NachislSummas.Update(nachislSumma);
            await _context.SaveChangesAsync();
            return Ok(dTO);
        }

        /// <summary>
        /// Добавляет новое начисление
        /// </summary>
        /// <param name="dTO">Модель начисления с новыми значениями</param>
        /// <returns>Статус выполнения добавления</returns>
        [HttpPost]
        [Authorization]
        public async Task<ActionResult<NachislSumma>> PostNachislSumma(NachislSummaDTO dTO)
        {
          if (_context.NachislSummas == null)
                return Problem("Entity set 'BillingDbContext.NachislSummas'  is null.");
            
            NachislSumma nachislSumma = new NachislSumma();
            var maxnachislSummaCD = await _context.NachislSummas.OrderByDescending(ns => ns.NachislFactCd).FirstAsync();
            nachislSumma.NachislFactCd = maxnachislSummaCD.NachislFactCd + 1;
            nachislSumma.NachislSum = dTO.NachislSum;
            nachislSumma.NachislYear = dTO.NachislYear;
            nachislSumma.NachislMonth = dTO.NachislMonth;
            nachislSumma.NachType = dTO.NachType;
            nachislSumma.CountResources = dTO.CountResources;

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

            nachislSumma.AbonentModeСd = appInformationToReturn.AbonentModeСd;

            _context.NachislSummas.Add(nachislSumma);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNachislSumma", new { id = nachislSumma.NachislFactCd }, nachislSumma);
        }

        /// <summary>
        /// Удаляет определенное начисление
        /// </summary>
        /// <param name="id">Код начисления</param>
        /// <returns>Статус выполнения удаления</returns>
        [HttpDelete("{id}")]
        [Authorization]
        public async Task<IActionResult> DeleteNachislSumma(int id)
        {
            if (_context.NachislSummas == null) return NotFound();

            var nachislSumma = await _context.NachislSummas.FindAsync(id);
            if (nachislSumma == null) return NotFound();

            _context.NachislSummas.Remove(nachislSumma);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
