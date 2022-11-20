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
        public ActionResult GetCharityEventsDetails()
        {
            return Ok(searchService.GetCharityEvents());
        }

        [AllowAnonymous]
        [HttpGet("{charityEventId}")]
        public ActionResult GetAllPersonalDataById([FromRoute] int charityEventId)
        {
            return Ok(searchService.GetCharityEventsById(charityEventId));
        }


    }
}
