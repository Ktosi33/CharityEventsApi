using System.Security.Claims;
using CharityEventsApi.Exceptions;
namespace CharityEventsApi.Services.UserContextService
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? getCurrentUserOrDefault()
          => httpContextAccessor?.HttpContext?.User;

        private Claim? getCurrentUserIdOrDefault()
            => getCurrentUserOrDefault()?.Claims.FirstOrDefault(r => r.Type == ClaimTypes.NameIdentifier);


        public ClaimsPrincipal getCurrentUser()
        {
            var user = getCurrentUserOrDefault();

            if (user == null)
            {
                throw new UnauthorizedException("JWT User doesn't exist");
            }

            return user;
        }

        public int getCurrentUserId()
        {
            var idUser = getCurrentUserIdOrDefault();

            if (idUser == null)
            {
                throw new UnauthorizedException("JWT Id claim doesn't exist");
            }

            return int.Parse(idUser.Value);
        }

    }
}
