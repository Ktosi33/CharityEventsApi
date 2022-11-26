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
        private readonly IImageService imageService;
        public CharityEventController(ICharityEventService charityEventService, IImageService imageService)
        {
            this.charityEventService = charityEventService;
            this.imageService = imageService;
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<ActionResult> AddCharityEvent([FromForm] AddAllCharityEventsDto charityEventDto)
        {
           await charityEventService.Add(charityEventDto);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("{idCharityEvent}")]
        public ActionResult EditCharityEvent([FromBody] EditCharityEventDto charityEventDto, [FromRoute] int idCharityEvent)
        {
            charityEventService.Edit(charityEventDto, idCharityEvent);
            return Ok();
        }
        [AllowAnonymous]
        [HttpPut("image/{idCharityEvent}")]
        public async Task<ActionResult> ChangeImageAsync(IFormFile image, [FromRoute] int idCharityEvent)
        {
            await charityEventService.ChangeImage(image, idCharityEvent);
            return Ok();
        }
        /*
        [AllowAnonymous]
        [HttpPost("/images")]
        public async Task<ActionResult> AddImages(List<IFormFile> files)
        {
            await imageService.SaveImagesAsync(files);
            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("/image")]
        public async Task<ActionResult> AddImage(IFormFile file)
        {
            await imageService.SaveImageAsync(file);
            return Ok();
        }
        [AllowAnonymous]
        [HttpGet("/image/{id}")]
        public async Task<ActionResult> GetImageById(int id)
        {
            return Ok(await imageService.GetImageAsync(id));
        }

        [AllowAnonymous]
        [HttpPut("/image")]
        public async Task<ActionResult> GetImages([FromBody]List<int> ids)
        {
            return Ok(imageService.GetImagesInRangeAsync(ids));
        }
        */
        [AllowAnonymous]
        [HttpPatch("{idCharityEvent}")]
        public ActionResult SetDataCharityEvent([FromRoute] int idCharityEvent, [FromQuery] bool? isVerified, [FromQuery] bool? isActive)
        {
            if (isVerified != null)
            {
                charityEventService.SetVerify(idCharityEvent, (bool)isVerified);
            }
            if (isActive != null)
            {
                charityEventService.SetActive(idCharityEvent, (bool)isActive);
            }
            return Ok();
        }
       
        [AllowAnonymous]
        [HttpGet("{idCharityEvent}")]
        public ActionResult GetCharityEventById([FromRoute] int idCharityEvent)
        {
            return Ok(charityEventService.GetCharityEventById(idCharityEvent));
        }
        [AllowAnonymous]
        [HttpGet()]
        public ActionResult GetCharityEvents()
        {
            return Ok(charityEventService.GetAll());
        }


    }
}
