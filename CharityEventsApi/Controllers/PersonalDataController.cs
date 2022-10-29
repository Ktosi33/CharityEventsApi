using CharityEventsApi.Services.PersonalData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharityEventsApi.Controllers
{
    [Route("/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonalDataController: ControllerBase
    {
        private readonly IPersonalDataService personalDataService;

        public PersonalDataController(IPersonalDataService personalDataService)
        {
            this.personalDataService = personalDataService;
        }

        [AllowAnonymous]
        [HttpGet("{userId}")]
        public ActionResult GetPersonalDataById([FromRoute] int userId)
        {
            return Ok(personalDataService.getPersonalDataById(userId));
        }
    }
}
