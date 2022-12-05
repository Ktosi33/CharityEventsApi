using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.DonationService;
using CharityEventsApi.Services.PersonalDataService;
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
        public ActionResult AddDonation([FromBody] AddDonationDto addDonationDto)
        {
            donationService.addDonation(addDonationDto);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("{donationId}")]
        public ActionResult GetDonationById([FromRoute] int donationId)
        {
            return Ok(donationService.getDonationById(donationId));
        }

        [AllowAnonymous]
        [HttpGet("charityEventFundraising/{idCharityEventFundraising}")]
        public ActionResult GetDonationsByCharityEventFundraisingId([FromRoute] int idCharityEventFundraising)
        {
            return Ok(donationService.getDonationsByCharityFundraisingId(idCharityEventFundraising));
        }
    }
}
