using Microsoft.AspNetCore.Mvc;
using NachislService.DTO;
using NachislService.Helpers;
using NachislService.Repository;
using NachislService.Repository.Models;

namespace NachislService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModesController : ControllerBase
    {
        private readonly BillingDbContext _context;

        public ModesController(BillingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает все режимы потребления
        /// </summary>
        /// <returns>Возвращает все режимы потребления с указанием наименования услуги</returns>
        [HttpGet]
        [Authorization]
        public async Task<ActionResult<IEnumerable<ModeDTO>>> GetModes()
        {
          if (_context.Modes == null) return NotFound();

            List<ModeDTO> modeDTOs = new List<ModeDTO>();
            ModeDTO dTO;

            var appInformationToReturn = _context.Modes
                .Join(_context.Services,
                m => m.ServiceCd,
                s => s.ServiceCd,
                (m, s) => new
                {
                    ModeCd = m.ModeCd,
                    ModeName = m.ModeName,
                    Norma = m.Norma,
                    ServiceCd = s.ServiceCd,
                    ServiceName = s.ServiceName
                });

            foreach (var item in appInformationToReturn)
            {
                dTO = new ModeDTO()
                {
                    ModeCd = item.ModeCd,
                    ModeName = item.ModeName,
                    Norma = item.Norma,
                    ServiceCd = item.ServiceCd,
                    ServiceName= item.ServiceName
                };
                modeDTOs.Add(dTO);
            }
            return modeDTOs;
        }

        /// <summary>
        /// Получает определенный режим потребления
        /// </summary>
        /// <param name="id">Код режима потребления</param>
        /// <returns>Объект режима потребления с указанием услуги</returns>
        [HttpGet("{id}")]
        [Authorization]
        public async Task<ActionResult<ModeDTO>> GetMode(int id)
        {
            if (_context.Modes == null) return NotFound();

            var mode = await _context.Modes.FindAsync(id);
            if (mode == null) return NotFound();

            var service = await _context.Services.FindAsync(mode.ServiceCd);
            if (service == null) return NotFound();

            ModeDTO dTO = new ModeDTO()
            {
                ModeCd = mode.ModeCd,
                ModeName = mode.ModeName,
                Norma = mode.Norma,
                ServiceCd = service.ServiceCd,
                ServiceName = service.ServiceName
            };

            return dTO;
        }

        /// <summary>
        /// Обновляет значения определенного режима потребления
        /// </summary>
        /// <param name="dTO">Модель режима потребления с новыми значениями</param>
        /// <returns>Статус выполнения обновления</returns>
        [HttpPut]
        [Authorization]
        public async Task<IActionResult> PutMode(ModeDTO dTO)
        {
            var mode = await _context.Modes.FindAsync(dTO.ModeCd);
            var service = _context.Services.Where(s => s.ServiceName== dTO.ServiceName).FirstOrDefault();
            if (mode == null || service == null) return NotFound();
            mode.ModeName = dTO.ModeName;
            mode.Norma = dTO.Norma;
            mode.ServiceCd = service.ServiceCd;
            _context.Modes.Update(mode);
            await _context.SaveChangesAsync();
            return Ok(dTO);
        }

        /// <summary>
        /// Добавляет новый режим потребления
        /// </summary>
        /// <param name="dTO">Модель режима потребления с новыми значениями</param>
        /// <returns>Статус выполнения добавления</returns>
        [HttpPost]
        [Authorization]
        public async Task<ActionResult<Mode>> PostMode(ModeDTO dTO)
        {
          if (_context.Modes == null)
              return Problem("Entity set 'BillingDbContext.Modes'  is null.");
          
            Mode mode = new Mode();
            mode.ModeName = dTO.ModeName;
            mode.Norma = dTO.Norma;
            var service = _context.Services.FirstOrDefault(s => s.ServiceName == dTO.ServiceName);
            mode.ServiceCd = service.ServiceCd;

            _context.Modes.Add(mode);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMode", new { id = mode.ModeCd }, mode);
        }

        /// <summary>
        /// Удаляет определенный режим потребления
        /// </summary>
        /// <param name="id">Код режима потребления</param>
        /// <returns>Статус выполнения удаления</returns>
        [HttpDelete("{id}")]
        [Authorization]
        public async Task<IActionResult> DeleteMode(int id)
        {
            if (_context.Modes == null) return NotFound();
            var mode = await _context.Modes.FindAsync(id);

            if (mode == null) return NotFound();

            if (!_context.Prices.Any(p => p.ModeCd == mode.ModeCd) &&  !_context.AbonentModes.Any(am => am.ModeCd == mode.ModeCd))
            {
                _context.Modes.Remove(mode);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }
    }
}
