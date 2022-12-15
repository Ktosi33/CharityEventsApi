using CharityEventsApi.Entities;
using CharityEventsApi.Services.AuthUserService;

namespace CharityEventsApi.Services.CharityEventService
{
    public class AuthCharityEventDecorator
    {
        private readonly IAuthUserService authUserService;
        private readonly ICharityEventService charityEventService;

        public AuthCharityEventDecorator(IAuthUserService authUserService, ICharityEventService charityEventService)
        {
            this.authUserService = authUserService;
            this.charityEventService = charityEventService;
        }

        public void AuthorizeIfOnePassWithIdCharityEvent(int? idCharityEvent, string? role)
        {
            Charityevent? charityevent = null;

            if(idCharityEvent.HasValue) {
                charityevent = charityEventService.GetCharityEventByCharityEventId(idCharityEvent.Value);
            }

            authUserService.AuthorizeIfOnePass(charityevent?.OrganizerId, role);
        }

        public void AuthorizeUserIdIfRoleWithIdCharityEvent(int idCharityEvent, string role)
        {
            var charityevent = charityEventService.GetCharityEventByCharityEventId(idCharityEvent);
            authUserService.AuthorizeUserIdIfRole(charityevent.OrganizerId, role);
        }
    }
}
