using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.CharityEvent
{
    public interface ICharityEventFactory
    {
        public Charityevent CreateCharityEvent(AddAllCharityEventsDto charityEventDto, User organizer);
    }
}