using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Services.PersonalDataService;
using CharityEventsApi.Services.SearchService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharityEventsApi.Controllers
{

    [Route("/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class SearchController: ControllerBase
    {

        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        [AllowAnonymous]
        [HttpGet()]
        public ActionResult GetCharityEventsDetails([FromQuery] bool? isVerified, [FromQuery] bool? isActive,
            [FromQuery] bool? isFundraising, [FromQuery] bool? fundraisingIsActive, [FromQuery] bool? fundraisingIsVerified, [FromQuery] bool? isVolunteering, [FromQuery] bool? volunteeringIsActive,
            [FromQuery] bool? volunteeringIsVerified)
        {
            return Ok(searchService.GetCharityEvents(isVerified, isActive, isFundraising, isVolunteering, volunteeringIsActive, fundraisingIsActive, volunteeringIsVerified, fundraisingIsVerified));
        }
        
        [AllowAnonymous]
        [HttpGet("{charityEventId}")]
        public ActionResult GetCharityEventDetailsById([FromRoute] int charityEventId)
        {
            return Ok(searchService.GetCharityEventsById(charityEventId));
        }
    }
}
