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
        private readonly ICharityEventVolunteeringService charityEventVolunteeringService;
        public CharityEventVolunteeringController(ICharityEventVolunteeringService charityEventVolunteeringService)
        {
            this.charityEventVolunteeringService = charityEventVolunteeringService;
        }
       /* [AllowAnonymous]
        [HttpPost()]
        public ActionResult AddCharityEventVolunteering([FromBody] CharityEventDto charityEventDto)
        {
            charityEventVolunteeringService.AddCharityEvent(charityEventDto);
            return Ok();
        } 
       */

        [AllowAnonymous]
        [HttpPut("{charityEventVolunteeringId}")]
        public ActionResult EditCharityEventVolunteering([FromBody] EditCharityEventVolunteeringDto charityEventVolunteeringDto,[FromRoute] int charityEventVolunteeringId)
        {
            charityEventVolunteeringService.EditCharityEventVolunteering(charityEventVolunteeringDto, charityEventVolunteeringId);
            return Ok();
        }
        [AllowAnonymous]
        [HttpPut("end/{charityEventVolunteeringId}")]
        public ActionResult EndCharityEvent([FromRoute] int charityEventVolunteeringId)
        {
            charityEventVolunteeringService.EndCharityEventVolunteering(charityEventVolunteeringId);
            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("location")]
        public ActionResult AddLocation([FromBody] AddLocationDto locationDto)
        {
            charityEventVolunteeringService.AddLocation(locationDto);
            return Ok();
        }
        [AllowAnonymous]
        [HttpPut("location/{locationId}")]
        public ActionResult EditLocation([FromBody] EditLocationDto locationDto, [FromRoute] int locationId)
        {
            charityEventVolunteeringService.EditLocation(locationDto, locationId);
            return Ok();
        }

    }
}
