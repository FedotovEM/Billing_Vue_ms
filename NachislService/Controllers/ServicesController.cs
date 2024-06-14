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
    public class ServicesController : ControllerBase
    {
        private readonly BillingDbContext _context;

        public ServicesController(BillingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает все услуги
        /// </summary>
        /// <returns>Возвращает все услуги с указанием значений из родительских таблиц</returns>
        [HttpGet]
        [Authorization]
        public async Task<ActionResult<IEnumerable<ServiceDTO>>> GetServices()
        {
            if (_context.Services == null) return NotFound();

            var services = await _context.Services.ToListAsync();
            var units = await _context.Units.ToListAsync();
            List<ServiceDTO> serviceDTOs = new List<ServiceDTO>();
            ServiceDTO dTO;

            var appInformationToReturn = _context.Services
                .Join(_context.Units,
                s => s.UnitsCd,
                u => u.UnitCd,
                (s, u) => new
                {
                    ServiceCd = s.ServiceCd,
                    ServiceName = s.ServiceName,
                    UnitsCd = u.UnitCd,
                    UnitsName = u.UnitsName
                });

            foreach (var item in appInformationToReturn)
            {
                dTO = new ServiceDTO()
                {
                    ServiceCd = item.ServiceCd,
                    ServiceName = item.ServiceName,
                    UnitsName = item.UnitsName,
                    UnitsCd = item.UnitsCd
                };
                serviceDTOs.Add(dTO);
            }
            return serviceDTOs;
        }

        /// <summary>
        /// Получает определенную услугу
        /// </summary>
        /// <param name="id">Код услуги</param>
        /// <returns>Объект услуги с указанием значений из родительских таблиц</returns>
        [HttpGet("{id}")]
        [Authorization]
        public async Task<ActionResult<ServiceDTO>> GetService(int id)
        {
            if (_context.Services == null) return NotFound();

            var service = await _context.Services.FindAsync(id);
            var unit = await _context.Units.FindAsync(service.UnitsCd);

            if (service == null || unit == null) return NotFound();

            ServiceDTO dTO = new ServiceDTO()
            {
                ServiceCd = service.ServiceCd,
                ServiceName = service.ServiceName,
                UnitsName = unit.UnitsName,
                UnitsCd = unit.UnitCd
            };

            return dTO;
        }

        /// <summary>
        /// Обновляет значения определенной услуги
        /// </summary>
        /// <param name="dTO">Модель услуги с новыми значениями</param>
        /// <returns>Статус выполнения обновления</returns>
        [HttpPut]
        [Authorization]
        public async Task<IActionResult> PutService(ServiceDTO dTO)
        {
            var service = await _context.Services.FindAsync(dTO.ServiceCd);
            var unit = _context.Units.Where(u => u.UnitsName == dTO.UnitsName).FirstOrDefault();
            if (service == null || unit == null) return NotFound();
            service.ServiceName = dTO.ServiceName;
            service.UnitsCd = unit.UnitCd;
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
            return Ok(dTO);
        }

        /// <summary>
        /// Добавляет новую услугу
        /// </summary>
        /// <param name="dTO">Модель услуги с новыми значениями</param>
        /// <returns>Статус выполнения добавления</returns>
        [HttpPost]
        [Authorization]
        public async Task<ActionResult<Service>> PostService(ServiceDTO dTO)
        {
          if (_context.Services == null)
              return Problem("Entity set 'BillingDbContext.Services'  is null.");
          
            var units = await _context.Units.ToListAsync();
            Service service = new Service();
            var maxServiceCD = await _context.Services.OrderByDescending(s => s.ServiceCd).FirstAsync();
            service.ServiceCd = maxServiceCD.ServiceCd + 1;
            service.ServiceName = dTO.ServiceName;
            var unit = units.FirstOrDefault(u => u.UnitsName == dTO.UnitsName);
            service.UnitsCd = unit.UnitCd;
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return Ok(service);
        }

        /// <summary>
        /// Удаляет определенную услугу
        /// </summary>
        /// <param name="id">Код услуги</param>
        /// <returns>Статус выполнения удаления</returns>
        [HttpDelete("{id}")]
        [Authorization]
        public async Task<IActionResult> DeleteService(int id)
        {
            if (_context.Services == null) return NotFound();

            var service = await _context.Services.FindAsync(id);

            if (service == null) return NotFound();

            if (!_context.Modes.Any(m => m.ServiceCd == service.ServiceCd) && !_context.Remains.Any(r => r.ServiceCd == service.ServiceCd))
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
