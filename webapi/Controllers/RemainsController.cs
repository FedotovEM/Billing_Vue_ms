using Microsoft.AspNetCore.Mvc;
using webapi.DTO;
using webapi.Helpers;
using webapi.Repository;
using webapi.Repository.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemainsController : ControllerBase
    {
        private readonly BillingAbonentDbContext _context;

        public RemainsController(BillingAbonentDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает все остатки
        /// </summary>
        /// <returns>Возвращает все остатки с указанием значений из родительских таблиц</returns>
        [HttpGet]
        [Authorization]
        public async Task<ActionResult<IEnumerable<RemainDTO>>> GetRemains()
        {
            if (_context.Remains == null) return NotFound();
            List<RemainDTO> remainDTOs = new List<RemainDTO>();
            RemainDTO dTO;

            var appInformationToReturn = _context.Remains
                .Join(_context.Services,
                r => r.ServiceCd,
                s => s.ServiceCd,
                (r, s) => new
                {
                    RemainCd = r.RemainCd,
                    AccountCd = r.AccountCd,
                    ServiceCd = r.ServiceCd,
                    Remmonth = r.Remmonth,
                    Remyear = r.Remyear,
                    Remainsum = r.Remainsum,
                    ServiceName = s.ServiceName,
                });

            foreach (var item in appInformationToReturn)
            {
                dTO = new RemainDTO()
                {
                    RemainCd = item.RemainCd,
                    AccountCd = item.AccountCd,
                    ServiceCd = item.ServiceCd,
                    Remmonth = item.Remmonth,
                    Remyear = item.Remyear,
                    Remainsum = item.Remainsum,
                    ServiceName = item.ServiceName,
                };
                remainDTOs.Add(dTO);
            }
            return remainDTOs;
        }

        /// <summary>
        /// Получает определенный остаток
        /// </summary>
        /// <param name="id">Код остатка</param>
        /// <returns>Объект остатка с указанием значений из родительских таблиц</returns>
        [HttpGet("{id}")]
        [Authorization]
        public async Task<ActionResult<RemainDTO>> GetRemain(int id)
        {
            if (_context.Remains == null) return NotFound();
            var remain = await _context.Remains.FindAsync(id);
            if (remain == null) return NotFound();

            var service = await _context.Services.FindAsync(remain.ServiceCd);
            if (service == null) return NotFound();

            RemainDTO dTO = new RemainDTO()
            {
                RemainCd = remain.RemainCd,
                AccountCd = remain.AccountCd,
                ServiceCd = remain.ServiceCd,
                Remmonth = remain.Remmonth,
                Remyear = remain.Remyear,
                Remainsum = remain.Remainsum,
                ServiceName = service.ServiceName,
            };
            return dTO;
        }

        /// <summary>
        /// Обновляет значения определенного остатка
        /// </summary>
        /// <param name="dTO">Модель остатка с новыми значениями</param>
        /// <returns>Статус выполнения обновления</returns>
        [HttpPut]
        [Authorization]
        public async Task<IActionResult> PutRemain(RemainDTO dTO)
        {
            var remain = await _context.Remains.FindAsync(dTO.RemainCd);
            if (remain == null) return NotFound();
            var service = _context.Services.Where(s => s.ServiceName == dTO.ServiceName).FirstOrDefault();

            remain.AccountCd = dTO.AccountCd;
            remain.Remmonth = dTO.Remmonth;
            remain.Remyear = dTO.Remyear;
            remain.Remainsum = dTO.Remainsum;
            remain.ServiceCd = service.ServiceCd;

            _context.Remains.Update(remain);
            await _context.SaveChangesAsync();
            return Ok(dTO);
        }

        /// <summary>
        /// Добавляет новый остаток
        /// </summary>
        /// <param name="dTO">Модель остатка с новыми значениями</param>
        /// <returns>Статус выполнения добавления</returns>
        [HttpPost]
        [Authorization]
        public async Task<ActionResult<Remain>> PostRemain(RemainDTO dTO)
        {
            if (_context.Remains == null)
                return Problem("Entity set 'BillingDbContext.Remains'  is null.");
            Remain remain = new Remain()
            {
                AccountCd = dTO.AccountCd,
                Remmonth = dTO.Remmonth,
                Remyear = dTO.Remyear,
                Remainsum = dTO.Remainsum,
            };
            var service = _context.Services.FirstOrDefault(s => s.ServiceName == dTO.ServiceName);
            remain.ServiceCd = service.ServiceCd;

            _context.Remains.Add(remain);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRemain", new { id = remain.RemainCd }, remain);
        }

        /// <summary>
        /// Удаляет определенный остаток
        /// </summary>
        /// <param name="id">Код остатка</param>
        /// <returns>Статус выполнения удаления</returns>
        [HttpDelete("{id}")]
        [Authorization]
        public async Task<IActionResult> DeleteRemain(int id)
        {
            if (_context.Remains == null) return NotFound();
            var remain = await _context.Remains.FindAsync(id);
            if (remain == null) return NotFound();            

            _context.Remains.Remove(remain);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
