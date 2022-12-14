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
        private readonly AuthFundraisingService authFundraisingService;

        public CharityEventFundraisingController(IFundraisingService FundraisingService, AuthFundraisingService authFundraisingService)
        {
            this.FundraisingService = FundraisingService;
            this.authFundraisingService = authFundraisingService;
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
            authFundraisingService.AuthorizeUserIdIfRoleWithIdFundraising(idFundraising, "Organizer");
            FundraisingService.Edit(FundraisingDto, idFundraising);
            return Ok();
        }
        [Authorize(Roles = "Organizer,Admin")]
        [HttpPatch("{idFundraising}")]
        public ActionResult SetFieldFundraising([FromRoute] int idFundraising, [FromQuery] bool? isVerified,
            [FromQuery] bool? isActive, [FromQuery] bool? isDenied)
        {
            if (isVerified != null) {
                authFundraisingService.AuthorizeIfOnePassWithIdFundraising(null, "Admin");
                FundraisingService.SetVerify(idFundraising, (bool)isVerified);
            }
            if (isActive != null) {
                authFundraisingService.AuthorizeUserIdIfRoleWithIdFundraising(idFundraising, "Organizer");
                FundraisingService.SetActive(idFundraising, (bool)isActive);
            }
            if (isDenied != null) {
                authFundraisingService.AuthorizeIfOnePassWithIdFundraising(null, "Admin");
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
