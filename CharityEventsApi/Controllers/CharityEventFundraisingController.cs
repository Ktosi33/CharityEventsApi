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
        private readonly ICharityEventFundraisingService charityEventFundraisingService;
        public CharityEventFundraisingController(ICharityEventFundraisingService charityEventFundraisingService)
        {
            this.charityEventFundraisingService = charityEventFundraisingService;
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
        [HttpPut("{charityEventFundraisingId}")]
        public ActionResult EditCharityEventVolunteering([FromBody] EditCharityEventFundraisingDto charityEventFundraisingDto, [FromRoute] int charityEventFundraisingId)
        {
            charityEventFundraisingService.EditCharityEventFundraising(charityEventFundraisingDto, charityEventFundraisingId);
            return Ok();
        }
        [AllowAnonymous]
        [HttpPut("end/{charityEventFundraisingId}")]
        public ActionResult EndCharityEvent([FromRoute] int charityEventFundraisingId)
        {
            charityEventFundraisingService.EndCharityEventFundraising(charityEventFundraisingId);
            return Ok();
        }
       
    }
}
