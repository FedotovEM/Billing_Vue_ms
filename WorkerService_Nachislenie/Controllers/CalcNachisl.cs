using WorkerService_Nachislenie.Helpers;
using Microsoft.EntityFrameworkCore;
using WorkerService_Nachislenie.Repository;

namespace WorkerService_Nachislenie.Controllers
{
    public class CalcNachisl
    {
        /// <summary>
        /// Производит расчёт начислений по нормативу по всем абонентам
        /// </summary>
        /// <param name="_logger">Логгер</param>
        /// <returns>Флаг статуса выполнения расчёта</returns>

        public static bool CalculateToNorm(ILogger<NachislWorker> _logger)
        {
            try
            {
                var ConnStr = ConfigurationHelper.GetSectionValue("ConnectionStrings:BillingPostgreSQL");
                NachislDbContext contextDB = new NachislDbContext(ConnStr);

                DateTime now = DateTime.Now;
                contextDB.NachislToNorm(now.Month - 1, now.Year);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool ReCalculateToNorm(ILogger<NachislWorker> _logger)
        {
            try
            {
                var ConnStr = ConfigurationHelper.GetSectionValue("ConnectionStrings:BillingPostgreSQL");
                NachislDbContext contextDB = new NachislDbContext(ConnStr);

                DateTime currentDate = DateTime.Now;
                var rowsToRecalculate = contextDB.WorkNachislServices.Where(wns => wns.status == 0).ToList();

                foreach (var row in rowsToRecalculate)
                {
                    contextDB.RecalculateNachislToNorm(row.accountcd, row.nachislmonth, row.nachislyear);
                    row.status = 1; // Запись помечается как выполнена
                    contextDB.WorkNachislServices.Update(row);
                    contextDB.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
