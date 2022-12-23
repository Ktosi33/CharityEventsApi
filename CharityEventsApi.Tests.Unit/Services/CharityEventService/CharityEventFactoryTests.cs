using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Services.ImageService;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CharityEventsApi.Tests.Unit.Services.CharityEventService
{
    public class CharityEventFactoryTests
    {
        [Theory]
        [InlineData("FT", "bbb", "ccc", 1, 2, 1, false, true)]
        [InlineData("Test", "asd", "asd", 1, 0, 1, true, true)]
        [InlineData("Asd", "", "zzz", 3, 1, 1, false, false)]
        public async Task AddAllCharityEventsDto_CreateNewObject_ReturnsCharityevent
            (string title, string description, string fundTarget,
            decimal amountOfMoneyToCollect, int amountOfNeededVolunteers,
            int organizerId, bool isFundraising, bool isVolunteering)
        {
          
            //arange
            var imageService = new Mock<IImageService>();
            var formFile = new Mock<IFormFile>();
            var organizer = new Mock<User>();
            imageService.Setup(m => m.SaveImageAsync(formFile.Object)).Returns(Task.FromResult(1));
            imageService.Setup(m => m.SaveImagesAsync(new List<IFormFile>() { formFile.Object }))
            .Returns(Task.FromResult(new List<int>() { 1,2}));

            CharityEventFactory cef = new CharityEventFactory(imageService.Object);

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
                ImageCharityEvent = formFile.Object,
                ImagesCharityEvent = new List<IFormFile>() { formFile.Object }

            };
            //act
            CharityEvent result = await cef.CreateCharityEvent(dto, organizer.Object);

            //assert
            result.Title.Should().Be(title);
            result.Description.Should().Be(description);
            result.IsActive.Should().Be(0);
            result.IsVerified.Should().Be(0);
            result.IdOrganizer.Should().Be(organizerId);
            result.IdImage.Should().Be(1);
        }
    }
}
