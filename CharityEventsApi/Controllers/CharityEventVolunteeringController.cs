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
        public CharityEventVolunteeringController(IVolunteeringService VolunteeringService)
        {
            this.VolunteeringService = VolunteeringService;
        }
        [Authorize(Roles = "Organizer,Admin")]
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
            VolunteeringService.Edit(VolunteeringDto, idVolunteering);
            return Ok();
        }
        [Authorize(Roles = "Organizer,Admin")]
        [HttpPatch("{idVolunteering}")]
        public ActionResult SetFieldVolunteering([FromRoute] int idVolunteering, [FromQuery] bool? isVerified, [FromQuery] bool? isActive)
        {
            if (isVerified != null)
            {
                VolunteeringService.SetVerify(idVolunteering, (bool)isVerified);
            }
            if (isActive != null)
            {
                VolunteeringService.SetActive(idVolunteering, (bool)isActive);
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
