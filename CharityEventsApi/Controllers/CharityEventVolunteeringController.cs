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
        [AllowAnonymous]
        [HttpPost()]
        public async Task<ActionResult> AddCharityEventVolunteeringAsync([FromForm] AddCharityEventVolunteeringDto charityEventDto)
        {
            await VolunteeringService.Add(charityEventDto);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("image/{idVolunteering}")]
        public async Task<ActionResult> AddOneImageAsync(IFormFile image, [FromRoute] int idVolunteering)
        {
            await VolunteeringService.AddOneImage(image, idVolunteering);
            return Ok();
        }
        [AllowAnonymous]
        [HttpDelete("image")]
        public async Task<ActionResult> DeleteImageAsync([FromQuery] int idImage, [FromQuery] int idVolunteering)
        {
            await VolunteeringService.DeleteImage(idImage, idVolunteering);
            return Ok();
        }
        [AllowAnonymous]
        [HttpPut("{idVolunteering}")]
        public ActionResult EditVolunteering([FromBody] EditCharityEventVolunteeringDto VolunteeringDto, [FromRoute] int idVolunteering)
        {
            VolunteeringService.Edit(VolunteeringDto, idVolunteering);
            return Ok();
        }
        [AllowAnonymous]
        [HttpPatch("{idVolunteering}")]
        public ActionResult SetDataVolunteering([FromRoute] int idVolunteering, [FromQuery] bool? isVerified, [FromQuery] bool? isActive)
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
        [HttpPost("location")]
        public ActionResult AddLocation([FromBody] AddLocationDto locationDto)
        {
            VolunteeringService.AddLocation(locationDto);
            return Ok();
        }
        [AllowAnonymous]
        [HttpPut("location/{locationId}")]
        public ActionResult EditLocation([FromBody] EditLocationDto locationDto, [FromRoute] int locationId)
        {
            VolunteeringService.EditLocation(locationDto, locationId);
            return Ok();
        }
        [AllowAnonymous]
        [HttpGet("{VolunteeringId}")]
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
