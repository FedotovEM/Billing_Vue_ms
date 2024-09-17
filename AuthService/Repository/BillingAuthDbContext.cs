using Microsoft.EntityFrameworkCore;
using AuthService.Repository.Models;
using Npgsql;

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

        /// <summary>
        /// Создает резервные копии данных рабочих таблиц
        /// </summary>
        /// <returns></returns>
        public void СreateTableBackup()
        {
            string query = $"select create_table_backup();";
            var connectionString = Environment.GetEnvironmentVariable("BillingPostgreSQL");
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand(query, connection);
                var result = command.ExecuteScalar();
                connection.Close();
            }
        }

        /// <summary>
        /// Перезаписывает исходные данные в рабочие таблицы
        /// </summary>
        /// <returns></returns>
        public void RewriteTableData()
        {
            string query = $"select rewrite_table_data();";
            var connectionString = Environment.GetEnvironmentVariable("BillingPostgreSQL");
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
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
