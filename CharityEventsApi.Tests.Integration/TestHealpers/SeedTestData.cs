using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Seeders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CharityEventsApi.Tests.Integration.TestHealpers
{
    public class SeedTestData
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly RoleSeeder roleSeeder;

        public SeedTestData(CharityEventsDbContext dbContext, RoleSeeder roleSeeder)
        {
            this.dbContext = dbContext;
            this.roleSeeder = roleSeeder;
        }

        public void Seed()
        {
            if (dbContext.Database.IsInMemory())
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
                
                if (!dbContext.Roles.Any())
                {
                    roleSeeder.Seed();
                }
                var role = dbContext.Roles.FirstOrDefault(r => r.Name == "Volunteer");
                if(role is null)
                {
                    throw new NotFoundException("Role Volunteer doesn't exist");
                }
                var img = new Image { IdImage = 1, Path = "aaa", ContentType = "image/jpeg" };
                if (!dbContext.Images.Any())
                {
                    dbContext.Images.Add(img);
                    dbContext.SaveChanges();
                }
                if (!dbContext.Users.Any())
                {
                    var user = new User { IdUser = 1, Login = "User", Password = "User", Email = "user@user.pl" };
                    user.RoleNames.Add(role);
                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                }

                if (!dbContext.CharityEvents.Any())
                {
                    var ce = new CharityEvent { IdCharityEvent = 1, Title = "test", IsActive = 1, IsVerified = 1, IsDenied =0, Description = "Aaa", 
                        IdOrganizer = 1, CreatedEventDate = DateTime.Now, IdImageNavigation = img, IdImages = new List<Image>() { img } };
                    var cf = new CharityFundraising { IdCharityFundraising = 1, FundTarget = "Test", AmountOfMoneyToCollect = 1000, 
                        CreatedEventDate = DateTime.Now, IsActive = 1, IsVerified = 1, IsDenied = 0, AmountOfAlreadyCollectedMoney = 0 };
                    var cv = new CharityVolunteering { IdCharityVolunteering = 1, CreatedEventDate = DateTime.Now, AmountOfNeededVolunteers = 3,  IsVerified = 1, IsActive = 1, IsDenied = 0 };
                    ce.IdCharityFundraising = 1;
                    ce.IdCharityVolunteering = 1;
                    ce.IdCharityFundraisingNavigation = cf;
                    ce.IdCharityVolunteeringNavigation = cv;

                    dbContext.CharityEvents.Add(ce);
                    dbContext.CharityFundraisings.Add(cf);
                    dbContext.CharityVolunteerings.Add(cv);
                    dbContext.SaveChanges();
                }
             
                }
        }
    }
}
