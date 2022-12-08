using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.DonationService;
using CharityEventsApi.Services.LocationService;
using CharityEventsApi.Services.VolunteerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharityEventsApi.Controllers
{
    [Route("/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class VolunteerController: ControllerBase
    {
        private readonly IVolunteerService volunteerService;

        public VolunteerController(IVolunteerService volunteerService)
        {
            this.volunteerService = volunteerService;
        }

        [Authorize(Roles = "Volunteer,Organizer,Admin")]
        [HttpPost()]
        public ActionResult AddVolunteer([FromBody] AddVolunteerDto addVolunteerDto)
        {
            volunteerService.addVolunteer(addVolunteerDto);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("{volunteeringId}")]
        public ActionResult GetVolunteers([FromRoute] int volunteeringId)
        {
            return Ok(volunteerService.getVolunteersByVolunteeringId(volunteeringId));
        }

        [Authorize(Roles = "Volunteer,Organizer,Admin")]
        [HttpDelete()]
        public ActionResult DeleteVolunteer([FromBody] DeleteVolunteerDto deleteVolunteerDto)
        {
            volunteerService.deleteVolunteer(deleteVolunteerDto);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("exist")]
        public ActionResult UserIsVolunteer([FromQuery] int idUser, [FromQuery] int idVolunteering)
        {
            return Ok(volunteerService.isVolunteer(idUser, idVolunteering));        
        }
    }
}
