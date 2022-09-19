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
        [HttpPost()]
        public ActionResult AddCharityEvent([FromBody] AddAllCharityEventsDto charityEventDto)
        {
            charityEventService.AddCharityEvent(charityEventDto);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("{charityEventId}")]
        public ActionResult EditCharityEvent([FromBody] EditCharityEventDto charityEventDto, [FromRoute] int charityEventId)
        {
            charityEventService.EditCharityEvent(charityEventDto, charityEventId);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("end/{charityEventId}")]
        public ActionResult EndCharityEvent([FromRoute] int charityEventId)
        {
            charityEventService.EndCharityEvent(charityEventId);
            return Ok();
        }

    }
}
