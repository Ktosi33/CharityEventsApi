using CharityEventsApi.Entities;
using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Services.UserAuthService;

namespace CharityEventsApi.Services.VolunteeringService
{
    public class AuthVolunteeringDecorator
    {
        private readonly IUserAuthService userAuthService;
        private readonly ICharityEventService charityEventService;

        public AuthVolunteeringDecorator(IUserAuthService userAuthService, ICharityEventService charityEventService)
        {
            this.userAuthService = userAuthService;
            this.charityEventService = charityEventService;
        }

        public void AuthorizeIfOnePassWithIdVolunteering(int? idVolunteering, string? role)
        {
            Charityevent? charityevent = null;

            if (idVolunteering.HasValue)
            {
                charityevent = charityEventService.getCharityEventByVolunteeringId(idVolunteering.Value);
            }

            userAuthService.AuthorizeIfOnePass(charityevent?.OrganizerId, role);
        }

        public void AuthorizeUserIdIfRoleWithIdVolunteering(int idVolunteering, string role)
        {
            var charityevent = charityEventService.getCharityEventByVolunteeringId(idVolunteering);
            userAuthService.AuthorizeUserIdIfRole(charityevent.OrganizerId, role);
        }
    }
}
