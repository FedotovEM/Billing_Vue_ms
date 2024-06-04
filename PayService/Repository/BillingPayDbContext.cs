using Microsoft.EntityFrameworkCore;
using Npgsql;
using PayService.Helpers;
using PayService.Repository.Models;

#nullable disable

namespace PayService.Repository
{
    public partial class BillingPayDbContext : DbContext
    {
        private string ConnectionString;
        public BillingPayDbContext()
        {
            ConnectionString = ConfigurationHelper.GetSectionValue("ConnectionStrings:BillingPostgreSQL");
        }

        public BillingPayDbContext(DbContextOptions<BillingPayDbContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public virtual DbSet<Abonent> Abonents { get; set; }
        public virtual DbSet<AbonentMode> AbonentModes { get; set; }
        public virtual DbSet<Mode> Modes { get; set; }
        public virtual DbSet<PaySumma> PaySummas { get; set; }
        public virtual DbSet<ReceptionPoint> ReceptionPoints { get; set; }
        public virtual DbSet<Remain> Remains { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Street> Streets { get; set; }

        /// <summary>
        /// Разносит платёж
        /// </summary>
        /// <param name="BillPaidAccountcd">Лицевой счёт абонента</param>
        /// <param name="PaidServicecd">Код услуги</param>
        /// <param name="paymonth">Месяц, за который производится платёж</param>
        /// <param name="payyear">Год, за который производится платёж</param>
        /// <param name="payvolume">Объём потребленной услуги</param>
        /// <param name="paidsum">Значение суммы платежа</param>
        /// <param name="newreceptionpointcd">Код источника платежа</param>
        /// <returns></returns>
        public void pay(string BillPaidAccountcd, int PaidServicecd, int paymonth, int payyear,
            decimal? payvolume, decimal paidsum, int newreceptionpointcd)
        {
            string volume = "";
            if (payvolume == null || payvolume <= 0)
            {
                volume = "null";
            }
            var sum = paidsum.ToString().Replace(",", ".");
            string query = $"select pay('{BillPaidAccountcd}'::varchar(6), {PaidServicecd}::pkfield, {paymonth}::tmonth, {payyear}::tyear, {volume}::numeric(15,2), {sum}::currency, {newreceptionpointcd}::pkfield);";

            string connectionString = ConfigurationHelper.GetSectionValue("ConnectionStrings:BillingPostgreSQL");
            using (var connection = new NpgsqlConnection(connectionString))
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
            modelBuilder.Entity<Abonent>(entity =>
            {
                entity.Property(e => e.AccountCd).IsUnicode(false);

                entity.Property(e => e.District).IsUnicode(false);

                entity.Property(e => e.Fio).IsUnicode(false);

                entity.Property(e => e.Phone).IsUnicode(false);
            });

            modelBuilder.Entity<AbonentMode>(entity =>
            {
                entity.Property(e => e.AccountCd).IsUnicode(false);
            });
            modelBuilder.Entity<Mode>(entity =>
            {
                entity.Property(e => e.ModeName).IsUnicode(false);
            });

            modelBuilder.Entity<PaySumma>(entity =>
            {
                entity.Property(e => e.PayType).IsUnicode(false);
            });

            modelBuilder.Entity<ReceptionPoint>(entity =>
            {
                entity.Property(e => e.ReceptionName).IsUnicode(false);
            });

            modelBuilder.Entity<Remain>(entity =>
            {
                entity.Property(e => e.AccountCd).IsUnicode(false);

                entity.Property(e => e.Remainsum).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.Property(e => e.ServiceName).IsUnicode(false);
            });

            modelBuilder.Entity<Street>(entity =>
            {
                entity.Property(e => e.StreetCd);
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
