using System.Security.Claims;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Services.UserContextService;

namespace CharityEventsApi.Services.UserAuthService
{
    public class UserAuthService : IUserAuthService
    {
        private readonly IUserContextService userContextService;

        public UserAuthService(IUserContextService userContextService)
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

            if (idUser is not null) {
                isNotForbidden = isNotForbidden || isCurrentUserHasId(idUser.Value);
            }

            if (role is not null) {
                isNotForbidden = isNotForbidden || isCurrentUserInRole(role);
            }

            if (!isNotForbidden) {
                throw new ForbiddenException();
            }
        }
        public void AuthorizeUserIdIfRole(int idUser, string role)
        {
            if(isCurrentUserInRole(role)) {
                if(isCurrentUserHasId(idUser)) { 
                return;
                } else {
                    throw new ForbiddenException();
                }
            }
        }
        private bool isCurrentUserHasId(int idUser)
        {
            return userContextService.getCurrentUserId() == idUser;
        }
        
        private bool isCurrentUserInRole(string role)
        {
            return userContextService.getCurrentUser().IsInRole(role);
        }

    }
}
