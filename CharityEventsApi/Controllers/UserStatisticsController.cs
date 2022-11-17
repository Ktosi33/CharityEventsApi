using CharityEventsApi.Services.UserStatistics;
using Microsoft.AspNetCore.Mvc;

namespace CharityEventsApi.Controllers
{

    [Route("/v1/[controller]")]
    [ApiController]
    public class UserStatisticsController: ControllerBase
    {
        private readonly IUserStatisticsService userStatisticsService;

        public UserStatisticsController(IUserStatisticsService userStatisticsService) 
        {
            this.userStatisticsService = userStatisticsService;
        }
        
        [HttpGet("donations/{userId}")]
        public ActionResult getDonationByUserId([FromRoute] int userId)
        {
            return Ok(userStatisticsService.getDonationStatisticByUserId(userId));
        }

        [HttpGet("volunteering/{userId}")]
        public ActionResult getVolunteeringByUserId([FromRoute] int userId)
        {
            return Ok(userStatisticsService.getVolunteeringStatisticsByUserId(userId));
        }

        [HttpGet("{userId}")]
        public ActionResult getUserStatisticsByUserId([FromRoute] int userId)
        {
            return Ok(userStatisticsService.getUserStatisticsByUserId(userId));
        }
    }
}
