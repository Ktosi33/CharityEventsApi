using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.VolunteeringService;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CharityEventsApi.Tests.Unit.Services.VolunteeringService
{
    public class VolunteeringFactoryTests
    {
        [Theory]
        [InlineData("FT", "bbb", "ccc", 1, 2, 1, true, true)]
        [InlineData("Test", "asd", "asd", 1, 0, 1, false, true)]
        public void AddAllCharityEventsDto_CreateNewObject_ReturnsVolunteering
            (string title, string description, string fundTarget,
            decimal amountOfMoneyToCollect, int amountOfNeededVolunteers,
            int organizerId, bool isFundraising, bool isVolunteering)
        {
            //arange
            var formFile = new Mock<IFormFile>();

            VolunteeringFactory vf = new VolunteeringFactory();

            var dto = new AddAllCharityEventsDto()
            {
                IsFundraising = isFundraising,
                IsVolunteering = isVolunteering,
                Title = title,
                Description = description,
                FundTarget = fundTarget,
                AmountOfMoneyToCollect = amountOfMoneyToCollect,
                AmountOfNeededVolunteers = amountOfNeededVolunteers,
                OrganizerId = organizerId,
                ImageCharityEvent = formFile.Object
            };
            //act
            CharityVolunteering result = vf.CreateCharityEvent(dto);

            //assert
            result.IsActive.Should().Be(0);
            result.IsVerified.Should().Be(0);
            result.AmountOfNeededVolunteers.Should().Be(amountOfNeededVolunteers);
        }
    }
}
