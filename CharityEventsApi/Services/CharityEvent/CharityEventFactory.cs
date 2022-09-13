using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Exceptions;
namespace CharityEventsApi.Services.CharityEvent
{
    public class CharityEventFactory
    {

        public Charityevent CreateCharityEvent(CharityEventDto charityEventDto, User organizer)
        {
            Charityevent charityevent = new Charityevent {

                Title = charityEventDto.Title,
                Description = charityEventDto.Description == null ? "" : charityEventDto.Description,
                OrganizerId = charityEventDto.OrganizerId,
                IsActive = 1,
                Organizer = organizer
            };

            return charityevent;
        }

       
    }
}
