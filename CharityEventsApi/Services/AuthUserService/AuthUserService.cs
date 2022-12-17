using System.Security.Claims;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Services.UserContextService;

namespace CharityEventsApi.Services.AuthUserService
{
    public class AuthUserService : IAuthUserService
    {
        private readonly IUserContextService userContextService;

        public AuthUserService(IUserContextService userContextService)
        {
            this.userContextService = userContextService;
        }
       
        public void AuthorizeIfOnePass(int? idUser, string? role)
        {
            authorize(isIfOnePass(idUser, role));
        }

        public void AuthorizeUserIdIfRole(int idUser, string role)
        {
            authorize(isUserIdIfRole(idUser, role)); 
        }

        private void authorize(bool isAuthorized)
        {
            if (isAuthorized || isCurrentUserInRole("Admin"))
            {
                return;
            }

            throw new ForbiddenException();
        }

        private bool isIfOnePass(int? idUser, string? role)
        {
            if (idUser is null && role is null)
            {
                return true;
            }

            bool isNotForbidden = false;

            if (idUser is not null)
            {
                isNotForbidden = isNotForbidden || isCurrentUserHasId(idUser.Value);
            }

            if (role is not null)
            {
                isNotForbidden = isNotForbidden || isCurrentUserInRole(role);
            }

            return isNotForbidden;
        }

        private bool isUserIdIfRole(int idUser, string role)
        {
            if (isCurrentUserInRole(role))
            {
                if (isCurrentUserHasId(idUser))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
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
