using System.Security.Claims;

namespace CharityEventsApi.Services.UserContextService
{
    public interface IUserContextService
    {
        public ClaimsPrincipal getCurrentUser();
        public int getCurrentUserId();
    }
}
