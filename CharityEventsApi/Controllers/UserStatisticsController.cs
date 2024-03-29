﻿using CharityEventsApi.Services.UserStatisticsService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharityEventsApi.Controllers
{

    [Route("/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class UserStatisticsController : ControllerBase
    {
        private readonly IUserStatisticsService userStatisticsService;

        public UserStatisticsController(IUserStatisticsService userStatisticsService)
        {
            this.userStatisticsService = userStatisticsService;
        }
        [Authorize]
        [HttpGet("donations/{userId}")]
        public ActionResult GetDonationByUserId([FromRoute] int userId)
        {
            return Ok(userStatisticsService.getDonationStatisticByUserId(userId));
        }
        [Authorize]
        [HttpGet("volunteering/{userId}")]
        public ActionResult GetVolunteeringByUserId([FromRoute] int userId)
        {
            return Ok(userStatisticsService.getVolunteeringStatisticsByUserId(userId));
        }
        [Authorize]
        [HttpGet("{userId}")]
        public ActionResult GetUserStatisticsByUserId([FromRoute] int userId)
        {
            return Ok(userStatisticsService.getUserStatisticsByUserId(userId));
        }
        [Authorize]
        [HttpGet("charityEventsWithVolunteering/{userId}")]
        public async Task<ActionResult> GetCharityEventsWithVolunteeringByUserId([FromRoute] int userId)
        {
            return Ok(await userStatisticsService.getCharityEventsWithVolunteeringByUserId(userId));
        }
        [Authorize]
        [HttpGet("charityEvents/{organizerId}")]
        public async Task<ActionResult> GetCharityEventsByOrganizerId([FromRoute] int organizerId, [FromQuery] bool? volunteeringOrFundraisingIsActive, [FromQuery] bool? volunteeringOrFundraisingIsVerified,
            [FromQuery] bool? volunteeringOrFundraisingIsDenied)
        {
            return Ok(await userStatisticsService.getCharityEventsByOrganizerId(organizerId, volunteeringOrFundraisingIsActive, volunteeringOrFundraisingIsVerified, volunteeringOrFundraisingIsDenied));
        }

    }
}
