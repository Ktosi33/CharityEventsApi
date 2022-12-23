using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.FundraisingService;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CharityEventsApi.Tests.Unit.Services.FundraisingService
{
    public class FundraisingFactoryTests
    {
        [Theory]
        [InlineData("FT", "bbb", "ccc", 1, 2, 1, true, true)]
        [InlineData("Test", "asd", "asd", 1, 0, 1, true, false)]
        public void AddAllCharityEventsDto_CreateNewObject_ReturnsCharityfundraising
            (string title, string description, string fundTarget,
            decimal amountOfMoneyToCollect, int amountOfNeededVolunteers,
            int organizerId, bool isFundraising, bool isVolunteering)
        {
            //arange
            var formFile = new Mock<IFormFile>();

            FundraisingFactory ff = new FundraisingFactory();

            var dto = new AddAllCharityEventsDto()
            {
                IsFundraising = isFundraising,
                IsVolunteering = isVolunteering,
                Title = title,
                Description = description,
                FundTarget = fundTarget,
                AmountOfMoneyToCollect = amountOfMoneyToCollect,
                AmountOfNeededVolunteers = amountOfNeededVolunteers,
                IdOrganizer = organizerId,
                ImageCharityEvent = formFile.Object
            };
            //act
            CharityFundraising result = ff.CreateCharityEvent(dto);

            //assert
            result.IsActive.Should().Be(0);
            result.IsVerified.Should().Be(0);
            result.FundTarget.Should().Be(fundTarget);
            result.AmountOfMoneyToCollect.Should().Be(amountOfMoneyToCollect); 
            result.AmountOfAlreadyCollectedMoney.Should().Be(0);

        }


    }
}
