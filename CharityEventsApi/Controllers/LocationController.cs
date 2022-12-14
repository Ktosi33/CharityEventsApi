using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Services.DonationService;
using CharityEventsApi.Services.LocationService;
using CharityEventsApi.Services.VolunteeringService;
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
        private readonly AuthLocationDecorator authLocation;
        private readonly AuthVolunteeringDecorator authVolunteering;

        public LocationController(ILocationService locationService, AuthLocationDecorator authLocation, AuthVolunteeringDecorator authVolunteering)
        {
            this.locationService = locationService;
            this.authLocation = authLocation;
            this.authVolunteering = authVolunteering;
        }

        [Authorize(Roles = "Organizer,Admin")]
        [HttpPost()]
        public ActionResult AddLocation([FromBody] AddLocationDto addLocationDto)
        {
            authVolunteering.AuthorizeUserIdIfRoleWithIdVolunteering(addLocationDto.IdVolunteering, "Organizer");
            locationService.addLocation(addLocationDto);
            return Ok();
        }

        [Authorize(Roles = "Organizer,Admin")]
        [HttpPut("{locationId}")]
        public ActionResult EditLocation([FromBody] EditLocationDto locationDto, [FromRoute] int locationId)
        {
            authLocation.AuthorizeUserIdIfRoleWithLocationId(locationId, "Organizer");
            locationService.editLocation(locationDto, locationId);
            return Ok();
        }

        [Authorize(Roles = "Organizer,Admin")]
        [HttpDelete("{locationId}")]
        public ActionResult DeleteLocation([FromRoute] int locationId)
        {
            authLocation.AuthorizeUserIdIfRoleWithLocationId(locationId, "Organizer");
            locationService.deleteLocation(locationId);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("{locationId}")]
        public ActionResult GetLocationById([FromRoute] int locationId)
        {
            return Ok(locationService.getLocationById(locationId));
        }

        [AllowAnonymous]
        [HttpGet("charityEventVolunteering/{volunteeringId}")]
        public ActionResult GetLocationsByCharityEventVolunteeringId([FromRoute] int volunteeringId)
        {
            return Ok(locationService.getLocationsByVolunteeringId(volunteeringId));
        }
    }
}
