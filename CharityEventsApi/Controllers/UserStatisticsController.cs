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
        public ActionResult GetDonationByUserId([FromRoute] int userId)
        {
            return Ok(userStatisticsService.getDonationStatisticByUserId(userId));
        }

        [HttpGet("volunteering/{userId}")]
        public ActionResult GetVolunteeringByUserId([FromRoute] int userId)
        {
            return Ok(userStatisticsService.getVolunteeringStatisticsByUserId(userId));
        }

        [HttpGet("{userId}")]
        public ActionResult GetUserStatisticsByUserId([FromRoute] int userId)
        {
            return Ok(userStatisticsService.getUserStatisticsByUserId(userId));
        }
    }
}
