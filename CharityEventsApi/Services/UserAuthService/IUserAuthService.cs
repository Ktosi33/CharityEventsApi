using System.Security.Claims;

namespace CharityEventsApi.Services.UserAuthService
{
    public interface IUserAuthService
    {
        public void AuthorizeIfOnePass(int? idUser, string? role);
        public void AuthorizeUserIdIfRole(int idUser, string role);
    }
}
