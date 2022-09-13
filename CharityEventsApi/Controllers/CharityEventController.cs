using Microsoft.AspNetCore.Mvc;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.CharityEvent;
using Microsoft.AspNetCore.Authorization;

namespace CharityEventsApi.Controllers
{
    [Route("/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class CharityEventController : ControllerBase
    {
        private readonly ICharityEventService charityEventService;
        public CharityEventController(ICharityEventService charityEventService)
        {
            this.charityEventService = charityEventService;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public ActionResult AddCharityEvent([FromBody] CharityEventDto charityEventDto)
        {
            charityEventService.AddCharityEvent(charityEventDto);
            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("addlocation")]
        public ActionResult AddLocation([FromBody] AddLocationDto locationDto)
        {
            charityEventService.AddLocation(locationDto);
            return Ok();
        }
    }
}
