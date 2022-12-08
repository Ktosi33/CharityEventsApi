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
            Charityevent charityevent = new()
            {
                Title = charityEventDto.Title,
                Description = charityEventDto.Description ?? "",
                OrganizerId = charityEventDto.OrganizerId,
                IsActive = 0,
                IsVerified = 0,
                IsDenied = 0,
                Organizer = organizer,
                ImageIdImages = await imageService.SaveImageAsync(charityEventDto.ImageCharityEvent),
                CreatedEventDate = DateTime.Now

            };
            if (charityEventDto.ImagesCharityEvent != null)
            {
               charityevent.ImageIdImages1 = imageService.getImageObjectsByIds(await imageService.SaveImagesAsync(charityEventDto.ImagesCharityEvent));
            }

            return charityevent;
        }

        public async Task<Charityevent> CreateCharityEvent(AddCharityEventDto charityEventDto, User organizer)
        {
            Charityevent charityevent = new ()
            {
                Title = charityEventDto.Title,
                Description = charityEventDto.Description ?? "",
                OrganizerId = charityEventDto.OrganizerId,
                IsActive = 0,
                IsVerified = 0,
                IsDenied = 0,
                Organizer = organizer,
                CreatedEventDate = DateTime.Now,
                ImageIdImages = await imageService.SaveImageAsync(charityEventDto.Image)
            };

            if (charityEventDto.Images != null)
            {
                charityevent.ImageIdImages1 = imageService.getImageObjectsByIds(await imageService.SaveImagesAsync(charityEventDto.Images));
            }

            return charityevent;
        }


    }
}
