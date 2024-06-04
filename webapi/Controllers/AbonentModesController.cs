using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.DTO;
using webapi.Helpers;
using webapi.Repository;
using webapi.Repository.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbonentModesController : ControllerBase
    {
        private readonly BillingAbonentDbContext _context;

        public AbonentModesController(BillingAbonentDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает все режимы потребления абонентов
        /// </summary>
        /// <returns>Возвращает режимы потребления абонентов с указанием значений из родительских таблиц</returns>
        [HttpGet]
        [Authorization]
        public async Task<ActionResult<IEnumerable<AbonentModeDTO>>> GetAbonentModes()
        {
            if (_context.AbonentModes == null) return NotFound();

            List<AbonentModeDTO> modeDTOs = new List<AbonentModeDTO>();
            AbonentModeDTO dTO;

            var appInformationToReturn = _context.Modes
                .Join(_context.AbonentModes,
                m => m.ModeCd,
                am => am.ModeCd,
                (m, am) => new
                {
                    AbonentModeСd = am.AbonentModeСd,
                    ModeCd = m.ModeCd,
                    ModeName = m.ModeName,
                    AccountCd = am.AccountCd,
                    Counterr = am.Counterr
                });

            foreach (var item in appInformationToReturn)
            {
                dTO = new AbonentModeDTO()
                {
                    ModeCd = item.ModeCd,
                    ModeName = item.ModeName,
                    AbonentModeСd = item.AbonentModeСd,
                    AccountCd = item.AccountCd,
                    Counterr = item.Counterr
                };
                modeDTOs.Add(dTO);
            }
            return modeDTOs;
        }

        /// <summary>
        /// Получает определенный режимы потребления абонента
        /// </summary>
        /// <param name="id">Код режима потребления абонента</param>
        /// <returns>Объект режима потребления абонента с указанием значений из родительских таблиц</returns>
        [HttpGet("{id}")]
        [Authorization]
        public async Task<ActionResult<AbonentModeDTO>> GetAbonentMode(int id)
        {
            if (_context.AbonentModes == null) return NotFound();
            
            var abonentMode = await _context.AbonentModes.FindAsync(id);
            if (abonentMode == null) return NotFound();

            var mode = await _context.Modes.FindAsync(abonentMode.ModeCd);
            if (mode == null) return NotFound();

            AbonentModeDTO dTO = new AbonentModeDTO()
            {
                AbonentModeСd = abonentMode.AbonentModeСd,
                AccountCd = abonentMode.AccountCd,
                ModeCd = mode.ModeCd,
                ModeName = mode.ModeName,
                Counterr = abonentMode.Counterr
            };

            return dTO;
        }

        /// <summary>
        /// Обновляет значения определенного режима потребления абонента
        /// </summary>
        /// <param name="dTO">Модель режима потребления абонента с новыми значениями</param>
        /// <returns>Статус выполнения обновления</returns>
        [HttpPut]
        [Authorization]
        public async Task<IActionResult> PutAbonentMode(AbonentModeDTO dTO)
        {
            var abonentMode = await _context.AbonentModes.FindAsync(dTO.AbonentModeСd);
            if (abonentMode == null) return NotFound();
            string cleanModeName = Regex.Replace(dTO.ModeName, @"\s*\[.*?\]\s*", "");
            var mode = _context.Modes.FirstOrDefault(m => m.ModeName == cleanModeName);
            abonentMode.ModeCd = mode.ModeCd;

            abonentMode.AccountCd = dTO.AccountCd;
            abonentMode.Counterr = dTO.Counterr;

            _context.AbonentModes.Update(abonentMode);
            await _context.SaveChangesAsync();
            return Ok(dTO);
        }

        /// <summary>
        /// Добавляет новый режим потребления абонента
        /// </summary>
        /// <param name="dTO">Модель режима потребления абонента с новыми значениями</param>
        /// <returns>Статус выполнения добавления</returns>
        [HttpPost]
        [Authorization]
        public async Task<ActionResult<AbonentMode>> PostAbonentMode(AbonentModeDTO dTO)
        {
            if (_context.AbonentModes == null)
                return Problem("Entity set 'BillingDbContext.AbonentModes'  is null.");
            
            AbonentMode abonentMode = new AbonentMode()
            {
                AccountCd = dTO.AccountCd,
                Counterr = dTO.Counterr
            };
            string cleanModeName = Regex.Replace(dTO.ModeName, @"\s*\[.*?\]\s*", "");
            var mode = _context.Modes.FirstOrDefault(m => m.ModeName == cleanModeName);
            abonentMode.ModeCd = mode.ModeCd;

            _context.AbonentModes.Add(abonentMode);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAbonentMode", new { id = abonentMode.AbonentModeСd }, abonentMode);
        }

        /// <summary>
        /// Удаляет определенный режим потребления абонента
        /// </summary>
        /// <param name="id">Код режима потребления абонента</param>
        /// <returns>Статус выполнения удаления</returns>
        [HttpDelete("{id}")]
        [Authorization]
        public async Task<IActionResult> DeleteAbonentMode(int id)
        {
            if (_context.AbonentModes == null) return NotFound();

            var abonentMode = await _context.AbonentModes.FindAsync(id);
            if (abonentMode == null) return NotFound();

            _context.AbonentModes.Remove(abonentMode);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
