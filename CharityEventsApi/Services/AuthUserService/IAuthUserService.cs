using System.Security.Claims;

namespace CharityEventsApi.Services.AuthUserService
{
    public interface IAuthUserService
    {
        public void AuthorizeIfOnePass(int? idUser, string? role);
        public void AuthorizeUserIdIfRole(int idUser, string role);
    }
}
