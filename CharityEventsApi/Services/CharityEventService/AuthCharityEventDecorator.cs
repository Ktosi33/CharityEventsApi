using CharityEventsApi.Entities;
using CharityEventsApi.Services.UserAuthService;

namespace CharityEventsApi.Services.CharityEventService
{
    public class AuthCharityEventDecorator
    {
        private readonly IUserAuthService userAuthService;
        private readonly ICharityEventService charityEventService;

        public AuthCharityEventDecorator(IUserAuthService userAuthService, ICharityEventService charityEventService)
        {
            this.userAuthService = userAuthService;
            this.charityEventService = charityEventService;
        }

        public void AuthorizeIfOnePassWithIdCharityEvent(int? idCharityEvent, string? role)
        {
            Charityevent? charityevent = null;

            if(idCharityEvent.HasValue) {
                charityevent = charityEventService.getCharityEventByCharityEventId(idCharityEvent.Value);
            }

            userAuthService.AuthorizeIfOnePass(charityevent?.OrganizerId, role);
        }

        public void AuthorizeUserIdIfRoleWithIdCharityEvent(int idCharityEvent, string role)
        {
            var charityevent = charityEventService.getCharityEventByCharityEventId(idCharityEvent);
            userAuthService.AuthorizeUserIdIfRole(charityevent.OrganizerId, role);
        }
    }
}
