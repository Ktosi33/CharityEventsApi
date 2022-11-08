using CharityEventsApi.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CharityEventsApi.Tests.Integration.TestHealpers
{
    public class SeedTestData
    {
        private readonly CharityEventsDbContext dbContext;

        public SeedTestData(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Seed()
        {
            if (dbContext.Database.IsInMemory())
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
                var role = new Role { Name = "Volunteer" };
                if (!dbContext.Roles.Any())
                {
                    dbContext.Roles.Add(role);
                    dbContext.SaveChanges();
                }

                if (!dbContext.Users.Any())
                {
                    var user = new User { IdUser = 1, Login = "User", Password = "User", Email = "user@user.pl" };
                    user.RolesNames.Add(role);
                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                }

                if (!dbContext.Charityevents.Any(e => e.IdCharityEvent == 1))
                {
                    var ce = new Charityevent { IdCharityEvent = 1, Title = "test", IsActive = 1, IsVerified = 1, Description = "Aaa", OrganizerId = 1, CreatedEventDate = DateTime.Now };
                    var cf = new Charityfundraising { IdCharityFundraising = 1, FundTarget = "Test", AmountOfMoneyToCollect = 1000, CreatedEventDate = DateTime.Now, IsActive = 1, IsVerified = 1 };
                    var cv = new Volunteering { IdVolunteering = 1, CreatedEventDate = DateTime.Now, AmountOfNeededVolunteers = 3, IsVerified = 1, IsActive = 1 };
                    ce.CharityFundraisingIdCharityFundraising = 1;
                    ce.VolunteeringIdVolunteering = 1;
                    ce.CharityFundraisingIdCharityFundraisingNavigation = cf;
                    ce.VolunteeringIdVolunteeringNavigation = cv;

                    dbContext.Charityevents.Add(ce);
                    dbContext.Charityfundraisings.Add(cf);
                    dbContext.Volunteerings.Add(cv);
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
