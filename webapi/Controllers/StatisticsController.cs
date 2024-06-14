using Microsoft.AspNetCore.Mvc;
using webapi.Helpers;
using webapi.models;
using webapi.Repository;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly BillingAbonentDbContext _context;
        private readonly string _payServiceURL = Environment.GetEnvironmentVariable("PayService_URL");


        public StatisticsController(BillingAbonentDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorization]
        [Route("get-abonent-stat")]
        public async Task<GeneralStatistics> GetGeneralStat()
        {
            var abonents = _context.Abonents.Count();
            var streets = _context.Streets.Count();

            var statistics = new GeneralStatistics
            {
                CountAbonent = abonents,
                CountStreets = streets,
            };
            return statistics;
        }
    }
}
