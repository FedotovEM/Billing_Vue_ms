using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NachislService.Helpers;
using NachislService.Repository;
using NachislService.Repository.Models;

namespace NachislService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsController : ControllerBase
    {
        private readonly BillingDbContext _context;

        public UnitsController(BillingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает все единицы измерений
        /// </summary>
        /// <returns>Возвращает все единицы измерений</returns>
        [HttpGet]
        [Authorization]
        public async Task<ActionResult<IEnumerable<Unit>>> GetUnits()
        {
            if (_context.Units == null) return NotFound();
            return await _context.Units.ToListAsync();
        }

        /// <summary>
        /// Получает определенную единицу измерений
        /// </summary>
        /// <param name="id">Код единицы измерений</param>
        /// <returns>Объект единицы измерений</returns>
        [HttpGet("{id}")]
        [Authorization]
        public async Task<ActionResult<Unit>> GetUnit(int id)
        {
          if (_context.Units == null) return NotFound();
            var unit = await _context.Units.FindAsync(id);

            if (unit == null) return NotFound();

            return unit;
        }

        /// <summary>
        /// Обновляет значения единицы измерений
        /// </summary>
        /// <param name="unit">Модель единицы измерений с новыми значениями</param>
        /// <returns>Статус выполнения обновления</returns>
        [HttpPut]
        [Authorization]
        public async Task<IActionResult> PutUnit(Unit unit)
        {
            _context.Units.Update(unit);
            await _context.SaveChangesAsync();
            return Ok(unit);
        }

        /// <summary>
        /// Добавляет новую единицу измерений
        /// </summary>
        /// <param name="unit">Модель единицы измерений с новыми значениями</param>
        /// <returns>Статус выполнения добавления</returns>
        [HttpPost]
        [Authorization]
        public async Task<ActionResult<Unit>> PostUnit(Unit unit)
        {
          if (_context.Units == null)
                return Problem("Entity set 'BillingDbContext.Units'  is null.");
            
            _context.Units.Add(unit);
            await _context.SaveChangesAsync();

            return Ok(unit);
        }

        /// <summary>
        /// Удаляет определенную единицу измерений
        /// </summary>
        /// <param name="id">Код единицы измерений</param>
        /// <returns>Статус выполнения удаления</returns>
        [HttpDelete("{id}")]
        [Authorization]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            if (_context.Units == null) return NotFound();

            var unit = await _context.Units.FindAsync(id);
            if (unit == null) return NotFound();

            if (!_context.Services.Any(s => s.UnitsCd == unit.UnitCd))
            {
                _context.Units.Remove(unit);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
