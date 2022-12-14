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
        private readonly AuthVolunteeringDecorator authVolunteering;

        public CharityEventVolunteeringController(IVolunteeringService VolunteeringService, AuthVolunteeringDecorator authVolunteering)
        {
            this.VolunteeringService = VolunteeringService;
            this.authVolunteering = authVolunteering;
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
            authVolunteering.AuthorizeUserIdIfRoleWithIdVolunteering(idVolunteering, "Organizer");
            VolunteeringService.Edit(VolunteeringDto, idVolunteering);
            return Ok();
        }
        [Authorize(Roles = "Organizer,Admin")]
        [HttpPatch("{idVolunteering}")]
        public ActionResult SetFieldVolunteering([FromRoute] int idVolunteering, [FromQuery] bool? isVerified, 
            [FromQuery] bool? isActive, [FromQuery] bool? isDenied)
        {
            if (isVerified != null) {
                authVolunteering.AuthorizeIfOnePassWithIdVolunteering(null, "Admin");
                VolunteeringService.SetVerify(idVolunteering, (bool)isVerified);
            }
            if (isActive != null) {
                authVolunteering.AuthorizeUserIdIfRoleWithIdVolunteering(idVolunteering, "Organizer");
                VolunteeringService.SetActive(idVolunteering, (bool)isActive);
            }
            if (isDenied != null) {
                authVolunteering.AuthorizeIfOnePassWithIdVolunteering(null, "Admin");
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
