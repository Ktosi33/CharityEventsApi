using CharityEventsApi.Entities;
using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Services.AuthUserService;

namespace CharityEventsApi.Services.VolunteeringService
{
    public class AuthVolunteeringDecorator
    {
        private readonly IAuthUserService authUserService;
        private readonly ICharityEventService charityEventService;

        public AuthVolunteeringDecorator(IAuthUserService authUserService, ICharityEventService charityEventService)
        {
            this.authUserService = authUserService;
            this.charityEventService = charityEventService;
        }

        public void AuthorizeIfOnePassWithIdVolunteering(int? idVolunteering, string? role)
        {
            CharityEvent? charityevent = null;

            if (idVolunteering.HasValue)
            {
                charityevent = charityEventService.GetCharityEventByVolunteeringId(idVolunteering.Value);
            }

            authUserService.AuthorizeIfOnePass(charityevent?.IdOrganizer, role);
        }

        public void AuthorizeUserIdIfRoleWithIdVolunteering(int idVolunteering, string role)
        {
            var charityevent = charityEventService.GetCharityEventByVolunteeringId(idVolunteering);
            authUserService.AuthorizeUserIdIfRole(charityevent.IdOrganizer, role);
        }
    }
}
