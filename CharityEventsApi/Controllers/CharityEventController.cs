using Microsoft.AspNetCore.Mvc;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.CharityEventService;
using Microsoft.AspNetCore.Authorization;
using CharityEventsApi.Services.ImageService;

namespace CharityEventsApi.Controllers
{
    [Route("/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class CharityEventController : ControllerBase
    {
        private readonly ICharityEventService charityEventService;
        private readonly AuthCharityEventDecorator authCharityEvent;

        public CharityEventController(ICharityEventService charityEventService, AuthCharityEventDecorator authCharityEvent)
        {
            this.charityEventService = charityEventService;
            this.authCharityEvent = authCharityEvent;
        }

        [Authorize(Roles = "Volunteer,Admin")]
        [HttpPost()]
        public async Task<ActionResult> AddCharityEvent([FromForm] AddAllCharityEventsDto charityEventDto)
        {
            await charityEventService.Add(charityEventDto);
            return Ok();
        }
        [Authorize(Roles = "Organizer,Admin")]
        [HttpPost("image/{idCharityEvent}")]
        public async Task<ActionResult> AddOneImageAsync(IFormFile image, [FromRoute] int idCharityEvent)
        {
            authCharityEvent.AuthorizeUserIdIfRoleWithIdCharityEvent(idCharityEvent, "Organizer");
            await charityEventService.AddOneImage(image, idCharityEvent);
            return Ok();
        }
        [Authorize(Roles = "Organizer,Admin")]
        [HttpDelete("image")]
        public async Task<ActionResult> DeleteImageAsync([FromQuery] int idImage, [FromQuery] int idCharityEvent)
        {
            authCharityEvent.AuthorizeUserIdIfRoleWithIdCharityEvent(idCharityEvent, "Organizer");
            await charityEventService.DeleteImage(idImage, idCharityEvent);
            return Ok();
        }

        [Authorize(Roles = "Organizer,Admin")]
        [HttpPut("{idCharityEvent}")]
        public ActionResult EditCharityEvent([FromBody] EditCharityEventDto charityEventDto, [FromRoute] int idCharityEvent)
        {
            authCharityEvent.AuthorizeUserIdIfRoleWithIdCharityEvent(idCharityEvent, "Organizer");
            charityEventService.Edit(charityEventDto, idCharityEvent);
            return Ok();
        }
        [Authorize(Roles = "Organizer,Admin")]
        [HttpPut("image/{idCharityEvent}")]
        public async Task<ActionResult> ChangeMainImageAsync(IFormFile image, [FromRoute] int idCharityEvent)
        {
            authCharityEvent.AuthorizeUserIdIfRoleWithIdCharityEvent(idCharityEvent, "Organizer");
            await charityEventService.ChangeImage(image, idCharityEvent);
            return Ok();
        }
       
        [AllowAnonymous]
        [HttpGet("images/{idCharityEvent}")]
        public async Task<ActionResult> GetImages([FromRoute] int idCharityEvent)
        {
            return Ok(await charityEventService.GetImagesAsync(idCharityEvent));
        }
       
        [Authorize(Roles = "Organizer,Admin")]
        [HttpPatch("{idCharityEvent}")]
        public ActionResult SetFieldCharityEvent([FromRoute] int idCharityEvent, [FromQuery] bool? isVerified,
            [FromQuery] bool? isActive, [FromQuery] bool? isDenied)
        {
            if (isVerified != null) {
                authCharityEvent.AuthorizeIfOnePassWithIdCharityEvent(null, "Admin");
                charityEventService.SetVerify(idCharityEvent, (bool)isVerified);
            }
            if (isActive != null) {
                authCharityEvent.AuthorizeUserIdIfRoleWithIdCharityEvent(idCharityEvent, "Organizer");
                charityEventService.SetActive(idCharityEvent, (bool)isActive);
            }
            if (isDenied != null) {
                authCharityEvent.AuthorizeIfOnePassWithIdCharityEvent(null, "Admin");
                charityEventService.SetDeny(idCharityEvent, (bool)isDenied);
            }

            return Ok();
        }
       
        [AllowAnonymous]
        [HttpGet("{idCharityEvent}")]
        public ActionResult GetCharityEventById([FromRoute] int idCharityEvent)
        {
            return Ok(charityEventService.GetCharityEventDtoById(idCharityEvent));
        }

        [AllowAnonymous]
        [HttpGet()]
        public ActionResult GetCharityEvents()
        {
            return Ok(charityEventService.GetAllCharityEventDto());
        }


    }
}
