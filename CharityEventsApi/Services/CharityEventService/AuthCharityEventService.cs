using CharityEventsApi.Entities;
using CharityEventsApi.Services.UserAuthService;

namespace CharityEventsApi.Services.CharityEventService
{
    public class AuthCharityEventService
    {
        private readonly IUserAuthService userAuthService;
        private readonly ICharityEventService charityEventService;

        public AuthCharityEventService(IUserAuthService userAuthService, ICharityEventService charityEventService)
        {
            this.userAuthService = userAuthService;
            this.charityEventService = charityEventService;
        }

        public void AuthorizeIfOnePassWithIdCharityEvent(int? idCharityEvent, string? role)
        {
            Charityevent? charityevent = null;

            if(idCharityEvent.HasValue) {
                charityevent = charityEventService.getCharityEventFromDbById(idCharityEvent.Value);
            }

            userAuthService.AuthorizeIfOnePass(charityevent?.IdCharityEvent, role);
        }

        public void AuthorizeUserIdIfRoleWithIdCharityEvent(int idCharityEvent, string role)
        {
            var charityevent = charityEventService.getCharityEventFromDbById(idCharityEvent);
            userAuthService.AuthorizeUserIdIfRole(charityevent.IdCharityEvent, role);
        }
    }
}
