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
        private readonly AuthFundraisingDecorator authFundraising;

        public CharityEventFundraisingController(IFundraisingService FundraisingService, AuthFundraisingDecorator authFundraising)
        {
            this.FundraisingService = FundraisingService;
            this.authFundraising = authFundraising;
        }
        [Authorize(Roles = "Volunteer,Admin")]
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
            authFundraising.AuthorizeUserIdIfRoleWithIdFundraising(idFundraising, "Organizer");
            FundraisingService.Edit(FundraisingDto, idFundraising);
            return Ok();
        }
        [Authorize(Roles = "Organizer,Admin")]
        [HttpPatch("{idFundraising}")]
        public ActionResult SetFieldFundraising([FromRoute] int idFundraising, [FromQuery] bool? isVerified,
            [FromQuery] bool? isActive, [FromQuery] bool? isDenied)
        {
            if (isVerified != null) {
                authFundraising.AuthorizeIfOnePassWithIdFundraising(null, "Admin");
                FundraisingService.SetVerify(idFundraising, (bool)isVerified);
            }
            if (isActive != null) {
                authFundraising.AuthorizeUserIdIfRoleWithIdFundraising(idFundraising, "Organizer");
                FundraisingService.SetActive(idFundraising, (bool)isActive);
            }
            if (isDenied != null) {
                authFundraising.AuthorizeIfOnePassWithIdFundraising(null, "Admin");
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
