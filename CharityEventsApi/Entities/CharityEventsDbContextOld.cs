using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Entities
{
    public class CharityEventsDbContextOld : DbContext
    {
        public CharityEventsDbContextOld(DbContextOptions<CharityEventsDbContextOld> options) : base(options) { }
      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


        }
 
    }
}
