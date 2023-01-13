using CharityEventsApi.Entities;
using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Services.AuthUserService;

namespace CharityEventsApi.Services.FundraisingService
{
    public class AuthFundraisingDecorator
    {
        private readonly IAuthUserService authUserService;
        private readonly ICharityEventService charityEventService;

        public AuthFundraisingDecorator(IAuthUserService authUserService, ICharityEventService charityEventService)
        {
            this.authUserService = authUserService;
            this.charityEventService = charityEventService;
        }

        public void AuthorizeIfOnePassWithIdFundraising(int? idFundraising, string? role)
        {
            CharityEvent? charityevent = null;

            if (idFundraising.HasValue)
            {
                charityevent = charityEventService.GetCharityEventByFundraisingId(idFundraising.Value);
            }

            authUserService.AuthorizeIfOnePass(charityevent?.IdOrganizer, role);
        }

        public void AuthorizeUserIdIfRoleWithIdFundraising(int idFundraising, string role)
        {
            var charityevent = charityEventService.GetCharityEventByFundraisingId(idFundraising);
            authUserService.AuthorizeUserIdIfRole(charityevent.IdOrganizer, role);
        }
    } 
}
