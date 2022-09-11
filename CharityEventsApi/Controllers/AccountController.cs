using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CharityEventsApi.Models;
using CharityEventsApi.Services.Account;

namespace CharityEventsApi.Controllers
{
    [Route("/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }
      
        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            accountService.RegisterUser(dto);
            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginUserDto dto)
        {
            string token = accountService.GenerateJwt(dto);
            return Ok(token);
        }

        [Authorize(Roles = "Volunteer")]
        [HttpGet("isLogged")]
        public ActionResult isLogged()
        {
            return Ok();
        }
    }
}
