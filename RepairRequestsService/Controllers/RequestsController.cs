using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepairRequestsService.DTO;
using RepairRequestsService.Helpers;
using RepairRequestsService.Repository;
using RepairRequestsService.Repository.Models;

namespace RepairRequestsService.Controllers
{
    [Route("api/repairrequests/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly BillingRepairRequestsDbContext _context;

        public RequestsController(BillingRepairRequestsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает все заявки на ремонт
        /// </summary>
        /// <returns>Возвращает все заявки на ремонт с указанием значений из родительских таблиц</returns>
        [HttpGet]
        [Authorization]
        public async Task<ActionResult<IEnumerable<RequestDTO>>> GetRequests()
        {
            if (_context.Requests == null) return NotFound();
            
            List<RequestDTO> requestDTOs = new List<RequestDTO>();
            RequestDTO dTO;

            var appInformationToReturn = _context.Requests
                .Join(_context.Disrepairs,
                r => r.FailureCd,
                d => d.FailureCd,
                (r, d) => new
                {
                    RequestCd = r.RequestCd,
                    AccountCd = r.AccountCd,
                    ExecutorCd = r.ExecutorCd,
                    IncomingDate = r.IncomingDate,
                    ExecutionDate = r.ExecutionDate,
                    Executed = r.Executed,
                    FailureName = d.FailureName,
                }).Join(
                _context.Executors,
                combinedEntry => combinedEntry.ExecutorCd,
                e => e.ExecutorCd,
                (combinedEntry, e) => new
                {
                    RequestCd = combinedEntry.RequestCd,
                    AccountCd = combinedEntry.AccountCd,
                    ExecutorCd = combinedEntry.ExecutorCd,
                    IncomingDate = combinedEntry.IncomingDate,
                    ExecutionDate = combinedEntry.ExecutionDate,
                    Executed = combinedEntry.Executed,
                    FailureName = combinedEntry.FailureName,
                    Fio = e.Fio,
                });

            foreach (var item in appInformationToReturn)
            {
                dTO = new RequestDTO()
                {
                    RequestCd = item.RequestCd,
                    AccountCd = item.AccountCd,
                    ExecutorCd = item.ExecutorCd,
                    IncomingDate = item.IncomingDate.Date,
                    ExecutionDate = item.ExecutionDate,
                    Executed = item.Executed,
                    FailureName = item.FailureName,
                    Fio = item.Fio,
                };
                if (item.ExecutionDate != null)
                    dTO.ExecutionDate = item.ExecutionDate.Value.Date;
                requestDTOs.Add(dTO);
            }
            return requestDTOs;
        }

        /// <summary>
        /// Получает определенную заявку на ремонт
        /// </summary>
        /// <param name="id">Код заявки на ремонт</param>
        /// <returns>Объект заявки на ремонт с указанием значений из родительских таблиц</returns>
        [HttpGet("{id}")]
        [Authorization]
        public async Task<ActionResult<RequestDTO>> GetRequest(int id)
        {
            if (_context.Requests == null) return NotFound();

            var request = await _context.Requests.FindAsync(id);
            if (request == null) return NotFound();

            var disrepeair = await _context.Disrepairs.FindAsync(request.FailureCd);
            var executor = await _context.Executors.FindAsync(request.ExecutorCd);

            if (disrepeair == null || executor == null) return NotFound();

            RequestDTO dTO = new RequestDTO()
            {
                RequestCd = request.RequestCd,
                AccountCd = request.AccountCd,
                IncomingDate = request.IncomingDate.Date,
                ExecutionDate = request.ExecutionDate,
                Executed = request.Executed,
                FailureName = disrepeair.FailureName,
                Fio = executor.Fio,
            };

            return dTO;
        }

        /// <summary>
        /// Обновляет значения определенной заявки на ремонт
        /// </summary>
        /// <param name="dTO">Модель заявки на ремонт с новыми значениями</param>
        /// <returns>Статус выполнения обновления</returns>
        [HttpPut]
        [Authorization]
        public async Task<IActionResult> PutRequest(RequestDTO dTO)
        {
            var request = await _context.Requests.FindAsync(dTO.RequestCd);
            if (request == null) return NotFound();
            var disrepeair = _context.Disrepairs.FirstOrDefault(d => d.FailureName == dTO.FailureName);
            request.FailureCd = disrepeair.FailureCd;

            var executor = _context.Executors.FirstOrDefault(e => e.Fio == dTO.Fio);
            request.ExecutorCd = executor.ExecutorCd;

            request.RequestCd = dTO.RequestCd;
            request.AccountCd = dTO.AccountCd;
            request.IncomingDate = dTO.IncomingDate.Date;
            request.ExecutionDate = dTO.ExecutionDate;
            request.Executed = dTO.Executed;

            _context.Requests.Update(request);
            await _context.SaveChangesAsync();
            return Ok(dTO);
        }

        /// <summary>
        /// Добавляет новую заявку на ремонт
        /// </summary>
        /// <param name="dTO">Модель заявки на ремонт с новыми значениями</param>
        /// <returns>Статус выполнения добавления</returns>
        [HttpPost]
        [Authorization]
        public async Task<ActionResult<Request>> PostRequest(RequestDTO dTO)
        {
            try
            {
                if (_context.Requests == null)
                    return Problem("Entity set 'BillingDbContext.Requests'  is null.");
                
                var maxRequestCD = await _context.Requests.OrderByDescending(r => r.RequestCd).FirstAsync();
                Request request = new Request()
                {
                    AccountCd = dTO.AccountCd,
                    IncomingDate = dTO.IncomingDate.Date.ToUniversalTime(),
                    ExecutionDate = dTO.ExecutionDate,
                    Executed = dTO.Executed,
                };

                var disrepeair = _context.Disrepairs.FirstOrDefault(d => d.FailureName == dTO.FailureName);
                request.FailureCd = disrepeair.FailureCd;

                var executor = _context.Executors.FirstOrDefault(e => e.Fio == dTO.Fio);
                request.ExecutorCd = executor.ExecutorCd;

                _context.Requests.Add(request);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetRequest", new { id = request.RequestCd }, request);
            }
            catch (Exception ex)
            {
                var mes = ex.Message;
                throw;
            }
        }

        /// <summary>
        /// Удаляет определенную заявку на ремонт
        /// </summary>
        /// <param name="id">Код заявки на ремонт</param>
        /// <returns>Статус выполнения удаления</returns>
        [HttpDelete("{id}")]
        [Authorization]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            if (_context.Requests == null) return NotFound();            
            var request = await _context.Requests.FindAsync(id);

            if (request == null) return NotFound();            

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}