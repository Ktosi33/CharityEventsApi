using System.Security.Claims;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Services.UserContextService;

namespace CharityEventsApi.Services.UserContextAuthService
{
    public class UserContextAuthService : IUserContextAuthService
    {
        private readonly IUserContextService userContextService;

        public UserContextAuthService(IUserContextService userContextService)
        {
            this.userContextService = userContextService;
        }


        public void AuthorizeIfOnePass(int? idUser, string? role)
        {
            if (idUser is null && role is null)
            {
                return;
            }

            bool isNotForbidden = false;

            if (idUser != null) {
                isNotForbidden = isNotForbidden || (userContextService.getCurrentUserId() == idUser);
            }

            if (role != null){
                isNotForbidden = isNotForbidden || userContextService.getCurrentUser().IsInRole(role);
            }

            if (!isNotForbidden)
            {
                throw new ForbiddenException();
            }
        }
    }
}
