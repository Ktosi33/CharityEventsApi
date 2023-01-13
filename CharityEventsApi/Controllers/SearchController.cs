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
        public async Task<ActionResult> GetCharityEventsDetails([FromQuery] bool? isVerified, [FromQuery] bool? isActive,
            [FromQuery] bool? isFundraising, [FromQuery] bool? fundraisingIsActive, [FromQuery] bool? fundraisingIsVerified, 
            [FromQuery] bool? isVolunteering, [FromQuery] bool? volunteeringIsActive, [FromQuery] bool? volunteeringIsVerified,
            [FromQuery] string? sortBy, [FromQuery] string? sortDirection)
        {
            return Ok(await searchService.GetCharityEvents(isVerified, isActive, isFundraising, isVolunteering, 
                volunteeringIsActive, fundraisingIsActive, volunteeringIsVerified, fundraisingIsVerified, sortBy, sortDirection));
        }

        [AllowAnonymous]
        [HttpGet("pagination")]
        public async Task<ActionResult> GetCharityEventsDetailsPagination([FromQuery] bool? isVerified, [FromQuery] bool? isActive,
            [FromQuery] bool? isFundraising, [FromQuery] bool? fundraisingIsActive, [FromQuery] bool? fundraisingIsVerified,
            [FromQuery] bool? isVolunteering, [FromQuery] bool? volunteeringIsActive, [FromQuery] bool? volunteeringIsVerified,
            [FromQuery] string? sortBy, [FromQuery] string? sortDirection, [FromQuery] int pageNumber, [FromQuery] int pageSize, 
            [FromQuery] bool? volunteeringOrFundraisingIsActive, [FromQuery] bool? volunteeringOrFundraisingIsVerified,
            [FromQuery] bool? volunteeringOrFundraisingIsDenied)
        {
            return Ok(await searchService.GetCharityEventsWithPagination(isVerified, isActive, isFundraising, isVolunteering,
                volunteeringIsActive, fundraisingIsActive, volunteeringIsVerified, fundraisingIsVerified, sortBy, sortDirection,
                pageNumber, pageSize, volunteeringOrFundraisingIsActive, volunteeringOrFundraisingIsVerified, volunteeringOrFundraisingIsDenied));
        }

        [AllowAnonymous]
        [HttpGet("{charityEventId}")]
        public async Task<ActionResult> GetCharityEventDetailsById([FromRoute] int charityEventId)
        {
            return Ok(await searchService.GetCharityEventsById(charityEventId));
        }

        [AllowAnonymous]
        [HttpGet("mostPopularFundraisings")]
        public async Task<ActionResult> GetMostPopularFundraisings([FromQuery] int numberOfEvents)
        {
            return Ok(await searchService.GetMostPopularFundraisings(numberOfEvents));
        }
    }
}
