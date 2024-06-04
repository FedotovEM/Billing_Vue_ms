using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RepairRequestsService.DTO;
using RepairRequestsService.Helpers;
using RepairRequestsService.Models;
using RepairRequestsService.Repository;

namespace RepairRequestsService.Controllers
{
    [Route("api/reportreq")]
    [ApiController]
    public class RequestReportController : ControllerBase
    {
        private readonly BillingRepairRequestsDbContext _context;
        public RequestReportController(BillingRepairRequestsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Составляет отчёт по ремонтным заявкам за месяц
        /// </summary>
        /// <param name="searchModel">Модель запроса составления отчёта</param>
        /// <returns>Объект отчёта</returns>
        [HttpPost]
        [Route("request-report-month")]
        [Authorization]
        public ActionResult<ReportMonthResponse> GetRequestReportByMonth([FromBody] ReportSearch searchModel)
        {
            if (_context.Requests == null) return NotFound();
            if (_context.Disrepairs == null) return NotFound();
            if (_context.Executors == null) return NotFound();

            var response = _context.getReportResponse(searchModel.Month, searchModel.Year);

            return response;
        }
    }
}
