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
                var img = new Image { IdImages = 1, Path = "aaa", ContentType = "image/jpeg" };
                if (!dbContext.Images.Any())
                {
                    dbContext.Images.Add(img);
                    dbContext.SaveChanges();
                }
                if (!dbContext.Users.Any())
                {
                    var user = new User { IdUser = 1, Login = "User", Password = "User", Email = "user@user.pl" };
                    user.RolesNames.Add(role);
                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                }

                if (!dbContext.Charityevents.Any())
                {
                    var ce = new Charityevent { IdCharityEvent = 1, Title = "test", IsActive = 1, IsVerified = 1, IsDenied =0, Description = "Aaa", 
                        OrganizerId = 1, CreatedEventDate = DateTime.Now, ImageIdImagesNavigation = img, ImageIdImages1 = new List<Image>() { img } };
                    var cf = new Charityfundraising { IdCharityFundraising = 1, FundTarget = "Test", AmountOfMoneyToCollect = 1000, 
                        CreatedEventDate = DateTime.Now, IsActive = 1, IsVerified = 1, IsDenied = 0, AmountOfAlreadyCollectedMoney = 0 };
                    var cv = new Volunteering { IdVolunteering = 1, CreatedEventDate = DateTime.Now, AmountOfNeededVolunteers = 3, 
                        AmountOfAttendedVolunteers = 0,  IsVerified = 1, IsActive = 1, IsDenied = 0 };
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
