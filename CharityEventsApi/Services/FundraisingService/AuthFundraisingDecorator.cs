using CharityEventsApi.Entities;
using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Services.UserAuthService;

namespace CharityEventsApi.Services.FundraisingService
{
    public class AuthFundraisingDecorator
    {
        private readonly IUserAuthService userAuthService;
        private readonly ICharityEventService charityEventService;

        public AuthFundraisingDecorator(IUserAuthService userAuthService, ICharityEventService charityEventService)
        {
            this.userAuthService = userAuthService;
            this.charityEventService = charityEventService;
        }

        public void AuthorizeIfOnePassWithIdFundraising(int? idFundraising, string? role)
        {
            Charityevent? charityevent = null;

            if (idFundraising.HasValue)
            {
                charityevent = charityEventService.getCharityEventByFundraisingId(idFundraising.Value);
            }

            userAuthService.AuthorizeIfOnePass(charityevent?.OrganizerId, role);
        }

        public void AuthorizeUserIdIfRoleWithIdFundraising(int idFundraising, string role)
        {
            var charityevent = charityEventService.getCharityEventByFundraisingId(idFundraising);
            userAuthService.AuthorizeUserIdIfRole(charityevent.OrganizerId, role);
        }
    } 
}
