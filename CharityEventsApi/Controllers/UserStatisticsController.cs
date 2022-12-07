using CharityEventsApi.Services.UserStatisticsService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharityEventsApi.Controllers
{

    [Route("/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class UserStatisticsController: ControllerBase
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
    }
}
