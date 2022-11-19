using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Exceptions;
namespace CharityEventsApi.Services.CharityEventService
{
    public class CharityEventFactory
    {

        public Charityevent CreateCharityEvent(AddAllCharityEventsDto charityEventDto, User organizer)
        {
            Charityevent charityevent = new Charityevent {

                Title = charityEventDto.Title,
                Description = charityEventDto.Description == null ? "" : charityEventDto.Description,
                OrganizerId = charityEventDto.OrganizerId,
                IsActive = 0,
                IsVerified = 0,
                Organizer = organizer,
                ImageIdImages = charityEventDto.ImageId,
                CreatedEventDate = DateTime.Now
            };

            return charityevent;
        }

        public Charityevent CreateCharityEvent(AddCharityEventDto charityEventDto, User organizer)
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
                ImageIdImages = charityEventDto.ImageId
            };

            return charityevent;
        }


    }
}
