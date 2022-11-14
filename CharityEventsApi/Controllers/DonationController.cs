using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.Donation;
using CharityEventsApi.Services.PersonalData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharityEventsApi.Controllers
{

    [Route("/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class DonationController: ControllerBase
    {

        private readonly IDonationService donationService;

        public DonationController(IDonationService donationService)
        {
            this.donationService = donationService;
        }

        [AllowAnonymous]
        [HttpPost()]
        public ActionResult AddAllPersonalData([FromBody] AddDonationDto addDonationDto)
        {
            donationService.addDonation(addDonationDto);
            return Ok();
        }
    }
}
