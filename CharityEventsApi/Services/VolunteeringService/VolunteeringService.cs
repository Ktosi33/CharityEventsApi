using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Services.ImageService;
using CharityEventsApi.Services.UserContextAuthService;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.VolunteeringService
{
    public class VolunteeringService : IVolunteeringService
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly ICharityEventFactoryFacade charityEventFactoryFacade;
        private readonly VolunteeringActivation volunteeringActication;
        private readonly VolunteeringVerification volunteeringVerification;
        private readonly CharityEventVerification charityEventVerification;
        private readonly IUserContextAuthService userContextService;

        public VolunteeringService(CharityEventsDbContext dbContext, ICharityEventFactoryFacade charityEventFactoryFacade,
            VolunteeringActivation volunteeringActication, VolunteeringVerification volunteeringVerification,
            CharityEventVerification charityEventVerification, IUserContextAuthService userContextService)
        {
            this.dbContext = dbContext;
            this.charityEventFactoryFacade = charityEventFactoryFacade;
            this.volunteeringActication = volunteeringActication;
            this.volunteeringVerification = volunteeringVerification;
            this.charityEventVerification = charityEventVerification;
            this.userContextService = userContextService;
        }

        public async Task Add(AddCharityEventVolunteeringDto dto)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(f => f.IdCharityEvent == dto.CharityEventId);
            if (charityevent is null) 
            {
                throw new NotFoundException("Charity event with given id doesn't exist");
            }
            if(charityevent.VolunteeringIdVolunteering is not null)
            {
                throw new BadRequestException("Can't add charity event volunteering, because another one already exists in this charity event");
            }
            await charityEventFactoryFacade.AddCharityEventVolunteering(dto, charityevent);

            charityEventVerification.SetVerify(dto.CharityEventId, false);
        }
        public void SetActive(int idVolunteering, bool isActive)
        {
            AuthorizeIfUserOrganizerOrAdmin(idVolunteering);
            volunteeringActication.SetActive(idVolunteering, isActive);
        }
        public void SetVerify(int idVolunteering, bool isVerified)
        {
            userContextService.AuthorizeIfOnePass(null, "Admin");
            volunteeringVerification.SetVerify(idVolunteering, isVerified);
        }
        public void Edit(EditCharityEventVolunteeringDto VolunteeringDto, int idVolunteering)
        {
            AuthorizeIfUserOrganizerOrAdmin(idVolunteering);
            var charityevent = dbContext.Volunteerings.FirstOrDefault(v => v.IdVolunteering == idVolunteering);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }
            if (VolunteeringDto.AmountOfNeededVolunteers != null)
            {
                charityevent.AmountOfNeededVolunteers = (int)VolunteeringDto.AmountOfNeededVolunteers;
            }
            dbContext.SaveChanges();
        }
      
        public GetCharityEventVolunteeringDto GetById(int id)
        {
            var c = dbContext.Volunteerings.FirstOrDefault(c => c.IdVolunteering == id);
            if (c is null)
            {
                throw new NotFoundException("Given id doesn't exist");
            }

            return new GetCharityEventVolunteeringDto
            {
                Id = c.IdVolunteering,
                AmountOfNeededVolunteers = c.AmountOfNeededVolunteers,
                CreatedEventDate = c.CreatedEventDate,
                EndEventDate = c.EndEventDate,
                IsActive = c.IsActive,
                IsVerified = c.IsVerified
            };
        }
        public IEnumerable<GetCharityEventVolunteeringDto> GetAll()
        {
            var volunteerings = dbContext.Volunteerings.Select(c => new GetCharityEventVolunteeringDto
            {
                Id = c.IdVolunteering,
                AmountOfNeededVolunteers = c.AmountOfNeededVolunteers,
                CreatedEventDate = c.CreatedEventDate,
                EndEventDate = c.EndEventDate,
                IsActive = c.IsActive,
                IsVerified = c.IsVerified
            }
            );

            return volunteerings;
        }
        private void AuthorizeIfUserOrganizerOrAdmin(int idVolunteering)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(ce => ce.CharityFundraisingIdCharityFundraising == idVolunteering);
            if (charityevent is null)
            {
                throw new InternalServerErrorException();
            }

            userContextService.AuthorizeIfOnePass(charityevent.OrganizerId, "Admin");
        }

    }
}
