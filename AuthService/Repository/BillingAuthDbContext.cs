using Microsoft.EntityFrameworkCore;
using AuthService.Repository.Models;

#nullable disable

namespace AuthService.Repository
{
    public partial class BillingAuthDbContext : DbContext
    {
        private string ConnectionString;
        public BillingAuthDbContext(string _connectionString)
        {
            ConnectionString = _connectionString;
        }

        public BillingAuthDbContext(DbContextOptions<BillingAuthDbContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
               
        public virtual DbSet<User> Users { get; set; }

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
