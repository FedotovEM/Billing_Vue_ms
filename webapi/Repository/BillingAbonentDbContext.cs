using Microsoft.EntityFrameworkCore;
using Npgsql;
using webapi.Helpers;
using webapi.Repository.Models;

#nullable disable

namespace webapi.Repository
{
    public partial class BillingAbonentDbContext : DbContext
    {
        private string ConnectionString;
        public BillingAbonentDbContext()
        {
            ConnectionString = Environment.GetEnvironmentVariable("BillingPostgreSQL");
        }

        public BillingAbonentDbContext(DbContextOptions<BillingAbonentDbContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        
        public virtual DbSet<Abonent> Abonents { get; set; }
        public virtual DbSet<AbonentMode> AbonentModes { get; set; }
        public virtual DbSet<Mode> Modes { get; set; }
        public virtual DbSet<Remain> Remains { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Street> Streets { get; set; }
        public virtual DbSet<AbonentHist> AbonentHist { get; set; }

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
            
            modelBuilder.Entity<Remain>(entity =>
            {
                entity.Property(e => e.AccountCd).IsUnicode(false);

                entity.Property(e => e.Remainsum).HasDefaultValueSql("((0))");
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
