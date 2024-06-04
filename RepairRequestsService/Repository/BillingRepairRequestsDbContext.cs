using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using RepairRequestsService.Helpers;
using RepairRequestsService.Models;
using RepairRequestsService.Repository.Models;
using System.Data;

#nullable disable

namespace RepairRequestsService.Repository
{
    public partial class BillingRepairRequestsDbContext : DbContext
    {
        private string ConnectionString;
        public BillingRepairRequestsDbContext()
        {
            ConnectionString = ConfigurationHelper.GetSectionValue("ConnectionStrings:BillingPostgreSQL");
        }

        public BillingRepairRequestsDbContext(DbContextOptions<BillingRepairRequestsDbContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public virtual DbSet<Disrepair> Disrepairs { get; set; }
        public virtual DbSet<Executor> Executors { get; set; }
        public virtual DbSet<Request> Requests { get; set; }

        /// <summary>
        /// Составляет отчёт по ремонтным заявкам за месяц
        /// </summary>
        /// <param name="searchMonth">Месяц, за который составляется отчёт</param>
        /// <param name="searchYear">Год, за который составляется отчёт</param>
        /// <returns>Объект отчёта</returns>
        public ReportMonthResponse getReportResponse(int searchMonth, int searchYear)
        {
            string query = $"select * from get_request_report_by_month({searchMonth}::tmonth, {searchYear}::tyear);";
            string connectionString = ConfigurationHelper.GetSectionValue("ConnectionStrings:BillingPostgreSQL");
            List<ReportResponse> ReportResponseList = new List<ReportResponse>();
            DataTable table = new DataTable();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand(query, connection);
                var result = command.ExecuteReader();
                table.Load(result);

                result.Close();
                connection.Close();
            }

            var failurecdList = table.AsEnumerable()
                .Select(row => row["failurecd"]).Distinct().ToList();

            foreach (var failure in failurecdList)
            {
                var RepByFailure = table.AsEnumerable()
                    .Where(row => row["failurecd"].Equals(failure)).ToList();

                ReportResponse ReportResponse = new ReportResponse()
                {
                    FailureCd = (int)failure,
                    FailureName = (string)RepByFailure.First()["failurename"],
                };

                foreach (var executor in RepByFailure)
                {
                    var fioResult = executor["execfio"].ToString();
                    ExecutorToReportMonth execToRep = new ExecutorToReportMonth();
                    if (!fioResult.IsNullOrEmpty())
                    {
                        execToRep.CountRequest = (int)executor["countReq"];
                        execToRep.CountPreviousMonth = (int)executor["countPreviousMonth"];
                        execToRep.ExecutorFIO = fioResult;
                        ReportResponse.ExecutorsToReportMonths.Add(execToRep);
                    }
                    else
                    {
                        ReportResponse.FullCountRequest = (int)executor["countReq"];
                        ReportResponse.FullCountPreviousMonth = (int)executor["countPreviousMonth"];
                    }
                }
                ReportResponseList.Add(ReportResponse);
            }
            ReportMonthResponse ReportMonthResponse = new ReportMonthResponse()
            {
                SumCountRequest = ReportResponseList.Sum(rr => rr.FullCountRequest),
                SumCountPreviousMonth = ReportResponseList.Sum(rr => rr.FullCountPreviousMonth),
                ReportResponseList = ReportResponseList
            };

            return ReportMonthResponse;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(ConnectionString);                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {                      
            modelBuilder.Entity<Disrepair>(entity =>
            {
                entity.Property(e => e.FailureName).IsUnicode(false);
            });

            modelBuilder.Entity<Executor>(entity =>
            {
                entity.Property(e => e.Fio).IsUnicode(false);
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.Property(e => e.AccountCd).IsUnicode(false);
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
