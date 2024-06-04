using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Helpers;
using webapi.Repository;
using webapi.Repository.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreetsController : ControllerBase
    {
        private readonly BillingAbonentDbContext _context;

        public StreetsController(BillingAbonentDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает все улицы
        /// </summary>
        /// <returns>Возвращает все улицы</returns>
        [HttpGet]
        [Authorization]
        public async Task<ActionResult<IEnumerable<Street>>> GetStreets()
        {
            if (_context.Streets == null) return NotFound();
            return await _context.Streets.ToListAsync();
        }

        /// <summary>
        /// Получает определенную улицу
        /// </summary>
        /// <param name="id">Код улицы</param>
        /// <returns>Объект улицы</returns>
        [HttpGet("{id}")]
        [Authorization]
        public async Task<ActionResult<Street>> GetStreet(int id)
        {
            if (_context.Streets == null) return NotFound();
            var street = await _context.Streets.FindAsync(id);

            if (street == null) return NotFound();
            return street;
        }

        /// <summary>
        /// Обновляет значения определенной улицы
        /// </summary>
        /// <param name="street">Модель улицы с новыми значениями</param>
        /// <returns>Статус выполнения обновления</returns>
        [HttpPut]
        [Authorization]
        public async Task<IActionResult> PutStreet(Street street)
        {
            _context.Streets.Update(street);
            await _context.SaveChangesAsync();
            return Ok(street);
        }

        /// <summary>
        /// Добавляет новую улицу
        /// </summary>
        /// <param name="street">Модель улицы с новыми значениями</param>
        /// <returns>Статус выполнения добавления</returns>
        [HttpPost]
        [Authorization]
        public async Task<ActionResult> PostStreet(Street street)
        {
            if (_context.Streets == null) 
                return Problem("Entity set 'BillingDbContext.Streets'  is null.");
            var maxStreetCD = await _context.Streets.OrderByDescending(s => s.StreetCd).FirstAsync();
            street.StreetCd = maxStreetCD.StreetCd + 1;
            _context.Streets.Add(street);
            _context.SaveChanges();

            return Ok(street);
        }

        /// <summary>
        /// Удаляет определенную улицу
        /// </summary>
        /// <param name="id">Код улицы</param>
        /// <returns>Статус выполнения удаления</returns>
        [HttpDelete]
        [Route("{id:int}")]
        [Authorization]
        public async Task<IActionResult> DeleteStreet(int id)
        {
            if (_context.Streets == null) return NotFound();
            var street = await _context.Streets.FindAsync(id);
            if (street == null) return NotFound();
            if (!_context.Abonents.Any(a => a.StreetCd == street.StreetCd))
            {
                _context.Streets.Remove(street);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }
    }
}
