using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.VolunteerService
{
    public class VolunteerService: IVolunteerService
    {
        private readonly CharityEventsDbContext dbContext;

        public VolunteerService(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void addVolunteer(AddVolunteerDto addVolunteerDto)
        {
            var volunteer = dbContext.Users.FirstOrDefault(u => u.IdUser == addVolunteerDto.IdUser);
            var volunteering = dbContext.Volunteerings
                .Include(v => v.UserIdUsers)
                .FirstOrDefault(v => v.IdVolunteering == addVolunteerDto.IdVolunteering);
            
            if (volunteer == null)
                throw new NotFoundException("User about this id does not exist");

            if (volunteering != null)
            {
                if (volunteering.UserIdUsers.Contains(volunteer))
                    throw new Exception("This volunteer has already been assigned to this action");

                volunteering.UserIdUsers.Add(volunteer);
                dbContext.SaveChanges();
            }
            else
                throw new NotFoundException("Volunteering about this id does not exist");

        }
    }
}
