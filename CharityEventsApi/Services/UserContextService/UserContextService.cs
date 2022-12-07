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

            if(user == null)
            {
                throw new NotFoundException("JWT User doesn't exist");
            }
           
            return user;
        }
       
        public int getCurrentUserId()
        {
            var userId = getCurrentUserIdOrDefault();

            if (userId == null) {
                throw new NotFoundException("JWT Id claim doesn't exist");
            }

            return int.Parse(userId.Value);
        }

    }
}
