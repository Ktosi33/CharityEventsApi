using Microsoft.AspNetCore.Mvc;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using CharityEventsApi.Services.FundraisingService;
using CharityEventsApi.Services.ImageService;

namespace CharityEventsApi.Controllers
{
    [Route("/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class CharityEventFundraisingController : ControllerBase
    {
        private readonly IFundraisingService FundraisingService;
        private readonly IImageService imageService;

        public CharityEventFundraisingController(IFundraisingService FundraisingService, IImageService imageService)
        {
            this.FundraisingService = FundraisingService;
            this.imageService = imageService;
        }
        [Authorize(Roles = "Organizer,Admin")]
        [HttpPost()]
        public async Task<ActionResult> AddCharityEventFundraisingAsync([FromForm] AddCharityEventFundraisingDto charityEventDto)
        {
            await FundraisingService.Add(charityEventDto);
            return Ok();
        }

        [Authorize(Roles = "Organizer,Admin")]
        [HttpPut("{idFundraising}")]
        public ActionResult EditFundraising([FromBody] EditCharityEventFundraisingDto FundraisingDto, [FromRoute] int idFundraising)
        {
            FundraisingService.Edit(FundraisingDto, idFundraising);
            return Ok();
        }
        [Authorize(Roles = "Organizer,Admin")]
        [HttpPatch("{idFundraising}")]
        public ActionResult SetFieldFundraising([FromRoute] int idFundraising, [FromQuery] bool? isVerified,
            [FromQuery] bool? isActive, [FromQuery] bool? isDenied)
        {
            if (isVerified != null) {
                FundraisingService.SetVerify(idFundraising, (bool)isVerified);
            }
            if (isActive != null) {
                FundraisingService.SetActive(idFundraising, (bool)isActive);
            }
            if (isDenied != null) {
                FundraisingService.SetDeny(idFundraising, (bool)isDenied);
            }

            return Ok();
        }
        [AllowAnonymous]
        [HttpGet("{idFundraising}")]
        public ActionResult GetCharityEventFundraisingById([FromRoute] int idFundraising)
        {
            return Ok(FundraisingService.GetById(idFundraising));
        }
        [AllowAnonymous]
        [HttpGet()]
        public ActionResult GetFundraisings()
        {
            return Ok(FundraisingService.GetAll());
        }

     

    }
}
