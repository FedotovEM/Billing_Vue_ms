using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepairRequestsService.Helpers;
using RepairRequestsService.Repository;
using RepairRequestsService.Repository.Models;

namespace RepairRequestsService.Controllers
{
    [Route("api/repairrequests/[controller]")]
    [ApiController]
    public class ExecutorsController : ControllerBase
    {
        private readonly BillingRepairRequestsDbContext _context;

        public ExecutorsController(BillingRepairRequestsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает всех исполнителей
        /// </summary>
        /// <returns>Возвращает всех исполнителей</returns>
        [HttpGet]
        [Authorization]
        public async Task<ActionResult<IEnumerable<Executor>>> GetExecutors()
        {
            if (_context.Executors == null) return NotFound();
            return await _context.Executors.ToListAsync();
        }

        /// <summary>
        /// Получает определенного исполнителя
        /// </summary>
        /// <param name="id">Код исполнителя</param>
        /// <returns>Объект исполнителя</returns>
        [HttpGet("{id}")]
        [Authorization]
        public async Task<ActionResult<Executor>> GetExecutor(int id)
        {
            if (_context.Executors == null) return NotFound();
            var executor = await _context.Executors.FindAsync(id);

            if (executor == null) return NotFound();

            return executor;
        }

        /// <summary>
        /// Обновляет значения определенного исполнителя
        /// </summary>
        /// <param name="executor">Модель исполнителя с новыми значениями</param>
        /// <returns>Статус выполнения обновления</returns>
        [HttpPut]
        [Authorization]
        public async Task<IActionResult> PutExecutor(Executor executor)
        {
            _context.Executors.Update(executor);
            await _context.SaveChangesAsync();
            return Ok(executor);
        }

        /// <summary>
        /// Добавляет нового исполнителя
        /// </summary>
        /// <param name="executor">Модель исполнителя с новыми значениями</param>
        /// <returns>Статус выполнения добавления</returns>
        [HttpPost]
        [Authorization]
        public async Task<ActionResult<Executor>> PostExecutor(Executor executor)
        {
            if (_context.Executors == null)
                return Problem("Entity set 'BillingDbContext.Executors'  is null.");
            var maxExecutorCD = await _context.Executors.OrderByDescending(s => s.ExecutorCd).FirstAsync();
            executor.ExecutorCd = maxExecutorCD.ExecutorCd + 1;
            _context.Executors.Add(executor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExecutor", new { id = executor.ExecutorCd }, executor);
        }

        /// <summary>
        /// Удаляет определенного исполнителя
        /// </summary>
        /// <param name="id">Код исполнителя</param>
        /// <returns>Статус выполнения удаления</returns>
        [HttpDelete("{id}")]
        [Authorization]
        public async Task<IActionResult> DeleteExecutor(int id)
        {
            if (_context.Executors == null) return NotFound();
            var executor = await _context.Executors.FindAsync(id);

            if (executor == null) return NotFound();

            if (!_context.Requests.Any(r => r.ExecutorCd == executor.ExecutorCd))
            {
                _context.Executors.Remove(executor);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
