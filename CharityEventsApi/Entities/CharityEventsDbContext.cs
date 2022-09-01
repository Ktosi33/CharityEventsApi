using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Entities
{
    public class CharityEventsDbContext : DbContext
    {
        private string _connectionString =
              "Server=localhost;Port=3306;Database=CharityEventsDb;Username=root;Password=";

      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                 => optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
    }
}
