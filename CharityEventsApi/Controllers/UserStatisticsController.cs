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

        [HttpGet("{userId}")]
        public ActionResult getDonationByUserId([FromRoute] int userId)
        {
            return Ok(userStatisticsService.getStatisticByUserId(userId));
        }
    }
}
