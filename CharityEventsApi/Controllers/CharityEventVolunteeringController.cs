using Microsoft.AspNetCore.Mvc;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using CharityEventsApi.Services.VolunteeringService;

namespace CharityEventsApi.Controllers
{
    [Route("/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class CharityEventVolunteeringController : ControllerBase
    {
        private readonly IVolunteeringService VolunteeringService;
        private readonly AuthVolunteeringService authVolunteeringService;

        public CharityEventVolunteeringController(IVolunteeringService VolunteeringService, AuthVolunteeringService authVolunteeringService)
        {
            this.VolunteeringService = VolunteeringService;
            this.authVolunteeringService = authVolunteeringService;
        }
        [Authorize(Roles = "Volunteer,Admin")]
        [HttpPost()]
        public async Task<ActionResult> AddCharityEventVolunteeringAsync([FromForm] AddCharityEventVolunteeringDto charityEventDto)
        {
            await VolunteeringService.Add(charityEventDto);
            return Ok();
        }

        [Authorize(Roles = "Organizer,Admin")]
        [HttpPut("{idVolunteering}")]
        public ActionResult EditVolunteering([FromBody] EditCharityEventVolunteeringDto VolunteeringDto, [FromRoute] int idVolunteering)
        {
            authVolunteeringService.AuthorizeUserIdIfRoleWithIdVolunteering(idVolunteering, "Organizer");
            VolunteeringService.Edit(VolunteeringDto, idVolunteering);
            return Ok();
        }
        [Authorize(Roles = "Organizer,Admin")]
        [HttpPatch("{idVolunteering}")]
        public ActionResult SetFieldVolunteering([FromRoute] int idVolunteering, [FromQuery] bool? isVerified, 
            [FromQuery] bool? isActive, [FromQuery] bool? isDenied)
        {
            if (isVerified != null) {
                authVolunteeringService.AuthorizeIfOnePassWithIdVolunteering(null, "Admin");
                VolunteeringService.SetVerify(idVolunteering, (bool)isVerified);
            }
            if (isActive != null) {
                authVolunteeringService.AuthorizeUserIdIfRoleWithIdVolunteering(idVolunteering, "Organizer");
                VolunteeringService.SetActive(idVolunteering, (bool)isActive);
            }
            if (isDenied != null) {
                authVolunteeringService.AuthorizeIfOnePassWithIdVolunteering(null, "Admin");
                VolunteeringService.SetDeny(idVolunteering, (bool)isDenied);
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("{idVolunteering}")]
        public ActionResult GetCharityEventById([FromRoute] int idVolunteering)
        {
            return Ok(VolunteeringService.GetById(idVolunteering));
        }
        [AllowAnonymous]
        [HttpGet()]
        public ActionResult GetFundraisings()
        {
            return Ok(VolunteeringService.GetAll());
        }
    }
}
