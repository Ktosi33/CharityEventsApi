﻿using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Services.PersonalDataService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharityEventsApi.Controllers
{
    [Route("/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonalDataController: ControllerBase
    {
        private readonly IPersonalDataService personalDataService;

        public PersonalDataController(IPersonalDataService personalDataService)
        {
            this.personalDataService = personalDataService;
        }


        [Authorize]
        [HttpPost("{userId}")]
        public ActionResult AddAllPersonalData([FromBody] AddAllPersonalDataDto personalDataDto, [FromRoute] int userId)
        {
            personalDataService.addAllPersonalData(personalDataDto, userId);
            return Ok();
        }

        [Authorize]
        [HttpPut("{userId}")]
        public ActionResult EditAllPersonalData([FromBody] EditAllPersonalDataDto personalDataDto, [FromRoute] int userId)
        {
            personalDataService.editAllPersonalData(personalDataDto, userId);
            return Ok();
        }

        [Authorize]
        [HttpGet("{userId}")]
        public ActionResult GetAllPersonalDataById([FromRoute] int userId)
        {
            return Ok(personalDataService.getPersonalDataById(userId));
        }

        [AllowAnonymous]
        [HttpGet("some/{userId}")]
        public ActionResult GetSomePersonalDataById([FromRoute] int userId)
        {
            return Ok(personalDataService.getSomePersonalDataById(userId));
        }

        [AllowAnonymous]
        [HttpGet("exists/{userId}")]
        public ActionResult PersonalDataExists([FromRoute] int userId)
        {
            return Ok(personalDataService.doesPersonalDataExists(userId));
        }
    }
}
