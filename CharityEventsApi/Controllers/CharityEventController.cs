using Microsoft.AspNetCore.Mvc;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.CharityEventService;
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
            charityEventService.Add(charityEventDto);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("{charityEventId}")]
        public ActionResult EditCharityEvent([FromBody] EditCharityEventDto charityEventDto, [FromRoute] int charityEventId)
        {
            charityEventService.Edit(charityEventDto, charityEventId);
            return Ok();
        }
      
        [AllowAnonymous]
        [HttpPatch("{charityEventId}")]
        public ActionResult SetDataCharityEvent([FromRoute] int charityEventId, [FromQuery] bool? isVerified, [FromQuery] bool? isActive)
        {
            if (isVerified != null)
            {
                charityEventService.SetVerify(charityEventId, (bool)isVerified);
            }
            if (isActive != null)
            {
                charityEventService.SetActive(charityEventId, (bool)isActive);
            }
            return Ok();
        }
       
        [AllowAnonymous]
        [HttpGet("{charityEventId}")]
        public ActionResult GetCharityEventById([FromRoute] int charityEventId)
        {
            return Ok(charityEventService.GetCharityEventById(charityEventId));
        }


    }
}
