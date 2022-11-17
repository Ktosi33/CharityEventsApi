using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Services.DonationService;
using CharityEventsApi.Services.LocationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharityEventsApi.Controllers
{

    [Route("/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationController: ControllerBase
    {
        private readonly ILocationService locationService;

        public LocationController(ILocationService locationService)
        {
            this.locationService = locationService;
        }

        [AllowAnonymous]
        [HttpPost()]
        public ActionResult AddLocation([FromBody] AddLocationDto addLocationDto)
        {
            locationService.addLocation(addLocationDto);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("{locationId}")]
        public ActionResult EditLocation([FromBody] EditLocationDto locationDto, [FromRoute] int locationId)
        {
            locationService.editLocation(locationDto, locationId);
            return Ok();
        }

        [AllowAnonymous]
        [HttpDelete("{locationId}")]
        public ActionResult DeleteLocation([FromRoute] int locationId)
        {
            locationService.deleteLocation(locationId);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("{locationId}")]
        public ActionResult GetLocationById([FromRoute] int locationId)
        {
            return Ok(locationService.getLocationById(locationId));
        }
    }
}
