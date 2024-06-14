using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepairRequestsService.Helpers;
using RepairRequestsService.Repository;
using RepairRequestsService.Repository.Models;

namespace RepairRequestsService.Controllers
{
    [Route("api/repairrequests/[controller]")]
    [ApiController]
    public class DisrepairsController : ControllerBase
    {
        private readonly BillingRepairRequestsDbContext _context;

        public DisrepairsController(BillingRepairRequestsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает все неисправности
        /// </summary>
        /// <returns>Возвращает все неисправности</returns>
        [HttpGet]
        [Authorization]
        public async Task<ActionResult<IEnumerable<Disrepair>>> GetDisrepairs()
        {
            if (_context.Disrepairs == null) return NotFound();
            return await _context.Disrepairs.ToListAsync();
        }

        /// <summary>
        /// Получает определенную неисправность
        /// </summary>
        /// <param name="id">Код неисправности</param>
        /// <returns>Объект неисправности</returns>
        [HttpGet("{id}")]
        [Authorization]
        public async Task<ActionResult<Disrepair>> GetDisrepair(int id)
        {
            if (_context.Disrepairs == null) return NotFound();
            var disrepair = await _context.Disrepairs.FindAsync(id);

            if (disrepair == null) return NotFound();

            return disrepair;
        }

        /// <summary>
        /// Обновляет значения определенной неисправности
        /// </summary>
        /// <param name="disrepair">Модель неисправности с новыми значениями</param>
        /// <returns>Статус выполнения обновления</returns>
        [HttpPut]
        [Authorization]
        public async Task<IActionResult> PutDisrepair(Disrepair disrepair)
        {
            _context.Disrepairs.Update(disrepair);
            await _context.SaveChangesAsync();
            return Ok(disrepair);
        }

        /// <summary>
        /// Добавляет новую неисправность
        /// </summary>
        /// <param name="disrepair">Модель неисправности с новыми значениями</param>
        /// <returns>Статус выполнения добавления</returns>
        [HttpPost]
        [Authorization]
        public async Task<ActionResult<Disrepair>> PostDisrepair(Disrepair disrepair)
        {
            if (_context.Disrepairs == null)
                return Problem("Entity set 'BillingDbContext.Disrepairs'  is null.");
            
            var maxDisrepairCD = await _context.Disrepairs.OrderByDescending(s => s.FailureCd).FirstAsync();
            disrepair.FailureCd = maxDisrepairCD.FailureCd + 1;
            _context.Disrepairs.Add(disrepair);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDisrepair", new { id = disrepair.FailureCd }, disrepair);
        }

        /// <summary>
        /// Удаляет определенную неисправность
        /// </summary>
        /// <param name="id">Код неисправности</param>
        /// <returns>Статус выполнения удаления</returns>
        [HttpDelete("{id}")]
        [Authorization]
        public async Task<IActionResult> DeleteDisrepair(int id)
        {
            if (_context.Disrepairs == null) return NotFound();
            var disrepair = await _context.Disrepairs.FindAsync(id);

            if (disrepair == null) return NotFound();
            if (!_context.Requests.Any(r => r.FailureCd == disrepair.FailureCd))
            {
                _context.Disrepairs.Remove(disrepair);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
