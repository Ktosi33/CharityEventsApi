using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.AccountService;

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
            string token = accountService.LoginUser(dto);
            return Ok(token);
        }

        [Authorize()]
        [HttpGet("isLogged")]
        public ActionResult IsLogged()
        {
            return Ok();
        }
    }
}
