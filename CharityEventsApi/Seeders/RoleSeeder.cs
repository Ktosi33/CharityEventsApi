using CharityEventsApi.Entities;

namespace CharityEventsApi.Seeders
{
    public class RoleSeeder
    {
        private readonly CharityEventsDbContext dbContext;

        public RoleSeeder(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Seed()
        {
            if (dbContext.Database.CanConnect())
            {
                if (!dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    dbContext.Roles.AddRange(roles);
                    dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "Volunteer"
                },
                new Role()
                {
                    Name = "Organizer"
                },
                new Role()
                {
                    Name = "Admin"
                }
            };

            return roles;
        }
    }
}
