using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.PersonalDataService;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.VolunteerService
{
    public class VolunteerService: IVolunteerService
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly IPersonalDataService personalDataService;

        public VolunteerService(CharityEventsDbContext dbContext, IPersonalDataService personalDataService)
        {
            this.dbContext = dbContext;
            this.personalDataService = personalDataService;
        }

        public void addVolunteer(AddVolunteerDto addVolunteerDto)
        {
            var volunteer = dbContext.Users.FirstOrDefault(u => u.IdUser == addVolunteerDto.IdUser);
            var volunteering = dbContext.CharityVolunteerings
                .Include(v => v.IdUsers)
                .FirstOrDefault(v => v.IdCharityVolunteering == addVolunteerDto.IdCharityVolunteering);
            
            if (volunteer == null)
                throw new NotFoundException("User about this id does not exist");

            if (volunteering != null)
            {
                if (volunteering.IsVerified == 0 || volunteering.IsActive == 0)
                    throw new BadRequestException("Action must be verified and active");

                if (volunteering.IdUsers.Contains(volunteer))
                    throw new BadRequestException("This volunteer has already been assigned to this action");

                volunteering.IdUsers.Add(volunteer);
                dbContext.SaveChanges();
            }
            else
                throw new NotFoundException("Volunteering about this id does not exist");
        }

        public List<GetVolunteerDto> getVolunteersByVolunteeringId(int volunteeringId)
        {
            var volunteering = dbContext.CharityVolunteerings
                .Include(v => v.IdUsers)
                .ThenInclude(u => u.PersonalData)
                .FirstOrDefault(v => v.IdCharityVolunteering == volunteeringId);
            
            if (volunteering == null)
                throw new NotFoundException("Volunteering about this id does not exist");

            var volunteers = volunteering.IdUsers;
            List<GetVolunteerDto> volunteersList = new List<GetVolunteerDto>();

            foreach (var volunteer in volunteers)
            {
                volunteersList.Add(getVolunteerDetails(volunteer));
            }

            return volunteersList;
        }

        public GetVolunteerDto getVolunteerDetails(User volunteer)
        {
            GetVolunteerDto getVolunteerDto = new GetVolunteerDto
            {
                IdUser = volunteer.IdUser,
                Login = volunteer.Login,
                Email = volunteer.Email
               
            };

            if (volunteer.PersonalData != null)
                getVolunteerDto.allPersonalData = personalDataService.getPersonalDataById(volunteer.IdUser);

            return getVolunteerDto;
        }

        public void deleteVolunteer(DeleteVolunteerDto deleteVolunteerDto)
        {
            var volunteer = dbContext.Users.FirstOrDefault(u => u.IdUser == deleteVolunteerDto.IdUser);
            var volunteering = dbContext.CharityVolunteerings
                .Include(v => v.IdUsers)
                .FirstOrDefault(v => v.IdCharityVolunteering == deleteVolunteerDto.IdCharityVolunteering);

            if (volunteer == null)
                throw new NotFoundException("User about this id does not exist");

            if (volunteering == null)
                throw new NotFoundException("Volunteering about this id does not exist");

            if (!volunteering.IdUsers.Contains(volunteer))
                throw new BadRequestException("This volunteer is not assigned to this action");

            volunteering.IdUsers.Remove(volunteer);
            dbContext.SaveChanges();
        }

        public bool isVolunteer(int idUser, int idVolunteering)
        {
            var volunteer = dbContext.Users.FirstOrDefault(u => u.IdUser == idUser);
            var volunteering = dbContext.CharityVolunteerings
                .Include(v => v.IdUsers)
                .FirstOrDefault(v => v.IdCharityVolunteering == idVolunteering);

            if (volunteer == null)
                throw new NotFoundException("User about this id does not exist");

            if (volunteering == null)
                throw new NotFoundException("Volunteering about this id does not exist");

            if (volunteering.IdUsers.Contains(volunteer))
                return true;
            else
                return false;

        }
    }
}
