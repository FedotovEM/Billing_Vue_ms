using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayService.Helpers;
using PayService.Repository;
using PayService.Repository.Models;

namespace PayService.Controllers
{
    [Route("api/pays/[controller]")]
    [ApiController]
    public class ReceptionPointsController : ControllerBase
    {
        private readonly BillingPayDbContext _context;

        public ReceptionPointsController(BillingPayDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает все источники платежей
        /// </summary>
        /// <returns>Возвращает все источники платежей</returns>
        [HttpGet]
        [Authorization]
        public async Task<ActionResult<IEnumerable<ReceptionPoint>>> GetReceptionPoints()
        {
            if (_context.ReceptionPoints == null) return NotFound();
            return await _context.ReceptionPoints.ToListAsync();
        }

        /// <summary>
        /// Получает определенный источник платежей
        /// </summary>
        /// <param name="id">Код источника платежей</param>
        /// <returns>Объект источника платежей</returns>
        [HttpGet("{id}")]
        [Authorization]
        public async Task<ActionResult<ReceptionPoint>> GetReceptionPoint(int id)
        {
            if (_context.ReceptionPoints == null) return NotFound();
            var receptionPoint = await _context.ReceptionPoints.FindAsync(id);

            if (receptionPoint == null) return NotFound();

            return receptionPoint;
        }

        /// <summary>
        /// Обновляет значения определенного источника платежей
        /// </summary>
        /// <param name="receptionPoint">Модель источника платежей с новыми значениями</param>
        /// <returns>Статус выполнения обновления</returns>
        [HttpPut]
        [Authorization]
        public async Task<IActionResult> PutReceptionPoint(ReceptionPoint receptionPoint)
        {
            _context.ReceptionPoints.Update(receptionPoint);
            await _context.SaveChangesAsync();
            return Ok(receptionPoint);
        }

        /// <summary>
        /// Добавляет новый источник платежей
        /// </summary>
        /// <param name="receptionPoint">Модель источника платежей с новыми значениями</param>
        /// <returns>Статус выполнения добавления</returns>
        [HttpPost]
        [Authorization]
        public async Task<ActionResult<ReceptionPoint>> PostReceptionPoint(ReceptionPoint receptionPoint)
        {
            if (_context.ReceptionPoints == null)
                return Problem("Entity set 'BillingDbContext.ReceptionPoints'  is null.");
            _context.ReceptionPoints.Add(receptionPoint);
            await _context.SaveChangesAsync();

            return Ok(receptionPoint);
        }

        /// <summary>
        /// Удаляет определенный источник платежей
        /// </summary>
        /// <param name="id">Код источника платежей</param>
        /// <returns>Статус выполнения удаления</returns>
        [HttpDelete("{id}")]
        [Authorization]
        public async Task<IActionResult> DeleteReceptionPoint(int id)
        {
            if (_context.ReceptionPoints == null) return NotFound();
            var receptionPoint = await _context.ReceptionPoints.FindAsync(id);
            if (receptionPoint == null) return NotFound();

            if (!_context.PaySummas.Any(p => p.ReceptionPointCd == receptionPoint.ReceptionPointCd))
            {
                _context.ReceptionPoints.Remove(receptionPoint);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
