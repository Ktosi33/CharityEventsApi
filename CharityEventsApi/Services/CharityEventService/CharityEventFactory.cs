using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Services.ImageService;

namespace CharityEventsApi.Services.CharityEventService
{
    public class CharityEventFactory
    {
        private readonly IImageService imageService;

        public CharityEventFactory(IImageService imageService)
        {
            this.imageService = imageService;
        }
        public async Task<Charityevent> CreateCharityEvent(AddAllCharityEventsDto charityEventDto, User organizer)
        {
            Charityevent charityevent = new Charityevent {
                Title = charityEventDto.Title,
                Description = charityEventDto.Description == null ? "" : charityEventDto.Description,
                OrganizerId = charityEventDto.OrganizerId,
                IsActive = 0,
                IsVerified = 0,
                Organizer = organizer,
                ImageIdImages = await imageService.SaveImageAsync(charityEventDto.ImageCharityEvent),
                CreatedEventDate = DateTime.Now
            };

            return charityevent;
        }

        public async Task<Charityevent> CreateCharityEvent(AddCharityEventDto charityEventDto, User organizer)
        {
            Charityevent charityevent = new Charityevent
            {
                Title = charityEventDto.Title,
                Description = charityEventDto.Description == null ? "" : charityEventDto.Description,
                OrganizerId = charityEventDto.OrganizerId,
                IsActive = 0,
                IsVerified = 0,
                Organizer = organizer,
                CreatedEventDate = DateTime.Now,
                ImageIdImages = await imageService.SaveImageAsync(charityEventDto.Image)
            };

            return charityevent;
        }


    }
}
