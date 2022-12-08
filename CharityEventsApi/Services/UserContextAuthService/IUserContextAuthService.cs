using System.Security.Claims;

namespace CharityEventsApi.Services.UserContextAuthService
{
    public interface IUserContextAuthService
    {
        public void AuthorizeIfOnePass(int? idUser, string? role);
    }
}
