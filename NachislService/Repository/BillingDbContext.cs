using Microsoft.EntityFrameworkCore;
using NachislService.Helpers;
using NachislService.Models;
using NachislService.Repository.Models;
using Npgsql;
using System.Reflection.PortableExecutable;

#nullable disable

namespace NachislService.Repository
{
    public partial class BillingDbContext : DbContext
    {
        private string ConnectionString;
        public BillingDbContext(string _connectionString)
        {
            ConnectionString = _connectionString;
        }

        public BillingDbContext(DbContextOptions<BillingDbContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public virtual DbSet<Abonent> Abonents { get; set; }
        public virtual DbSet<AbonentMode> AbonentModes { get; set; }
        public virtual DbSet<Mode> Modes { get; set; }
        public virtual DbSet<NachislSumma> NachislSummas { get; set; }
        public virtual DbSet<Price> Prices { get; set; }
        public virtual DbSet<Remain> Remains { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Street> Streets { get; set; }
        public virtual DbSet<Unit> Units { get; set; }

        /// <summary>
        /// Производит начисления по факту
        /// </summary>
        /// <param name="newaccountcd">Лицевой счёт абонента</param>
        /// <param name="newservicecd">Код услуги</param>
        /// <param name="newmonth">Месяц, за который производится начисление</param>
        /// <param name="newyear">Год, за который производится начисление</param>
        /// <param name="newvolume">Потребленный объём</param>
        /// <returns></returns>
        public void NachislToFact(string newaccountcd, int newservicecd, int newmonth, int newyear, decimal newvolume)
        {
            string query = $"select nachisl_new_fact('{newaccountcd}'::varchar(6), {newservicecd}::pkfield, {newmonth}::tmonth, {newyear}::tyear, {newvolume}::numeric(10,3));";
            string connectionString = ConfigurationHelper.GetSectionValue("ConnectionStrings:BillingPostgreSQL");
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand(query, connection);
                var result = command.ExecuteScalar();
                connection.Close();
            }
        }

        /// <summary>
        /// Получает карточку абонента
        /// </summary>
        /// <param name="searchAccountcd">Лицевой счёт абонента</param>
        /// <returns>Список услуг по абоненту со всеми характеристиками по ним</returns>
        public List<AbonentCard> getAbonentCard(string searchAccountcd)
        {
            string query = $"select * from get_abonent_card('{searchAccountcd}'::varchar(6));";
            string connectionString = ConfigurationHelper.GetSectionValue("ConnectionStrings:BillingPostgreSQL");
            List<AbonentCard> abonentCardList = new List<AbonentCard>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand(query, connection);
                var result = command.ExecuteReader();

                while (result.Read())
                {
                    AbonentCard abonentCard = new AbonentCard()
                    {
                        AbonentModeСd = (int)result["abonentmodeСd"],
                        AccountCd = (string)result["accountcd"],
                        Counterr = (bool)result["counterr"],
                        ModeCd = (int)result["modecd"],
                        ModeName = (string)result["modename"],
                        Norma = (decimal)result["norma"],
                        NormaName = (string)result["normaname"],
                        PriceCd = (int)result["pricecd"],
                        PriceValue = (decimal)result["pricevalue"],
                        PriceName = (string)result["pricename"],
                        ServiceCd = (int)result["servicecd"],
                        ServiceName = (string)result["servicename"],
                        UnitsCd = (int)result["unitcd"],
                        UnitsName = (string)result["unitsname"],
                    };
                    abonentCardList.Add(abonentCard);
                }
                connection.Close();
            }
            return abonentCardList;
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

            modelBuilder.Entity<NachislSumma>(entity =>
            {
                entity.Property(e => e.NachType).IsUnicode(false);
            });
          
            modelBuilder.Entity<Price>();

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

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.Property(e => e.UnitCd);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
