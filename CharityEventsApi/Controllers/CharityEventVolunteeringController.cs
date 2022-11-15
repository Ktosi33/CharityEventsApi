using Microsoft.AspNetCore.Mvc;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.CharityEvent;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPost("{charityeventId}")]
        public ActionResult AddCharityEventVolunteering([FromBody] AddCharityEventVolunteeringDto charityEventDto, [FromRoute] int charityeventId)
        {
            VolunteeringService.Add(charityEventDto, charityeventId);
            return Ok();
        } 
      

        [AllowAnonymous]
        [HttpPut("{VolunteeringId}")]
        public ActionResult EditVolunteering([FromBody] EditCharityEventVolunteeringDto VolunteeringDto, [FromRoute] int VolunteeringId)
        {
            VolunteeringService.Edit(VolunteeringDto, VolunteeringId);
            return Ok();
        }
        [AllowAnonymous]
        [HttpPatch("{VolunteeringId}")]
        public ActionResult SetDataVolunteering([FromRoute] int VolunteeringId, [FromQuery] bool? isVerified, [FromQuery] bool? isActive)
        {
            if (isVerified != null)
            {
                VolunteeringService.SetVerify(VolunteeringId, (bool)isVerified);
            }
            if (isActive != null)
            {
                VolunteeringService.SetActive(VolunteeringId, (bool)isActive);
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
        public ActionResult GetCharityEventById([FromRoute] int VolunteeringId)
        {
            return Ok(VolunteeringService.GetById(VolunteeringId));
        }

    }
}
