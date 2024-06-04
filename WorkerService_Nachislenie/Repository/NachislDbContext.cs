using WorkerService_Nachislenie.Helpers;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using WorkerService_Nachislenie.Repository.Models;

#nullable disable

namespace WorkerService_Nachislenie.Repository
{
    public partial class NachislDbContext : DbContext
    {
        private string ConnectionString;
        public NachislDbContext(string _connectionString)
        {
            ConnectionString = _connectionString;
        }

        public NachislDbContext(DbContextOptions<NachislDbContext> options)
            : base(options)  
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public virtual DbSet<Abonent> Abonents { get; set; }
        public virtual DbSet<AbonentMode> AbonentModes { get; set; }
        public virtual DbSet<Mode> Modes { get; set; }
        public virtual DbSet<NachislSumma> NachislSummas { get; set; }
        public virtual DbSet<Price> Prices { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<WorkNachislService> WorkNachislServices { get; set; }

        /// <summary>
        /// Производит расчёт начислений по нормативу по всем абонентам
        /// </summary>
        /// <param name="nmonth">Месяц, за который производится расчёт начислений</param>
        /// <param name="nyear">Год, за который производится расчёт начислений</param>
        /// <returns>Флаг статуса выполнения расчёта</returns>
        public void NachislToNorm(int nmonth, int nyear)
        {
            string query = $"select nachi_to_norm({nmonth}::tmonth, {nyear}::tyear);";
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand(query, connection);
                var result = command.ExecuteScalar();
                connection.Close();
            }
        }
        public void RecalculateNachislToNorm(string accountcd, int nmonth, int nyear)
        {
            string query = $"select nachi_to_norm_account({accountcd}::varchar(6), {nmonth}::tmonth, {nyear}::tyear);";
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand(query, connection);
                var result = command.ExecuteScalar();
                connection.Close();
            }
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
           OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
