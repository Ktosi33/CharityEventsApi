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
        public async Task<CharityEvent> CreateCharityEvent(AddAllCharityEventsDto charityEventDto, User organizer)
        {
            CharityEvent charityevent = new()
            {
                Title = charityEventDto.Title,
                Description = charityEventDto.Description ?? "",
                IdOrganizer = charityEventDto.OrganizerId,
                IsActive = 0,
                IsVerified = 0,
                IsDenied = 0,
                IdOrganizerNavigation = organizer,
                IdImage = await imageService.SaveImageAsync(charityEventDto.ImageCharityEvent),
                CreatedEventDate = DateTime.Now

            };
            if (charityEventDto.ImagesCharityEvent != null)
            {
               charityevent.IdImages = imageService.getImageObjectsByIds(await imageService.SaveImagesAsync(charityEventDto.ImagesCharityEvent));
            }

            return charityevent;
        }

        public async Task<CharityEvent> CreateCharityEvent(AddCharityEventDto charityEventDto, User organizer)
        {
            CharityEvent charityevent = new ()
            {
                Title = charityEventDto.Title,
                Description = charityEventDto.Description ?? "",
                IdOrganizer = charityEventDto.OrganizerId,
                IsActive = 0,
                IsVerified = 0,
                IsDenied = 0,
                IdOrganizerNavigation = organizer,
                CreatedEventDate = DateTime.Now,
                IdImage = await imageService.SaveImageAsync(charityEventDto.Image)
            };

            if (charityEventDto.Images != null)
            {
                charityevent.IdImages = imageService.getImageObjectsByIds(await imageService.SaveImagesAsync(charityEventDto.Images));
            }

            return charityevent;
        }


    }
}
