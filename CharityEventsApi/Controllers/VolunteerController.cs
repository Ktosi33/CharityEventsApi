using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.DonationService;
using CharityEventsApi.Services.AuthUserService;
using CharityEventsApi.Services.VolunteeringService;
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
        private readonly AuthVolunteeringDecorator authVolunteering;
        private readonly IAuthUserService authUser;

        public VolunteerController(IVolunteerService volunteerService, AuthVolunteeringDecorator authVolunteering, IAuthUserService authUser)
        {
            this.volunteerService = volunteerService;
            this.authVolunteering = authVolunteering;
            this.authUser = authUser;
        }

        [Authorize(Roles = "Volunteer,Admin")]
        [HttpPost()]
        public ActionResult AddVolunteer([FromBody] AddVolunteerDto addVolunteerDto)
        {
            authUser.AuthorizeUserIdIfRole(addVolunteerDto.IdUser, "Volunteer");

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
            authUser.AuthorizeUserIdIfRole(deleteVolunteerDto.IdUser, "Volunteer");
            authVolunteering.AuthorizeUserIdIfRoleWithIdVolunteering(deleteVolunteerDto.IdVolunteering, "Organizer");

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
