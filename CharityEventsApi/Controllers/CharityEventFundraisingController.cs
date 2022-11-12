using Microsoft.AspNetCore.Mvc;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.CharityEvent;
using Microsoft.AspNetCore.Authorization;
namespace CharityEventsApi.Controllers
{
    [Route("/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class CharityEventFundraisingController : ControllerBase
    {
        private readonly IFundraisingService FundraisingService;
        public CharityEventFundraisingController(IFundraisingService FundraisingService)
        {
            this.FundraisingService = FundraisingService;
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
        [HttpPut("{FundraisingId}")]
        public ActionResult EditFundraising([FromBody] EditCharityEventFundraisingDto FundraisingDto, [FromRoute] int FundraisingId)
        {
            FundraisingService.Edit(FundraisingDto, FundraisingId);
            return Ok();
        }
        [AllowAnonymous]
        [HttpPatch("{FundraisingId}")]
        public ActionResult SetDataFundraising([FromRoute] int FundraisingId, [FromQuery] bool? isVerified, [FromQuery] bool? isActive)
        {
            if (isVerified != null)
            {
                FundraisingService.SetVerify(FundraisingId, (bool)isVerified);
            }
            if (isActive != null)
            {
                FundraisingService.SetActive(FundraisingId, (bool)isActive);
            }
            return Ok();
        }
        [AllowAnonymous]
        [HttpGet("{FundraisingId}")]
        public ActionResult GetCharityEventFundraisingById([FromRoute] int FundraisingId)
        {
            return Ok(FundraisingService.GetById(FundraisingId));
        }
       

    }
}
