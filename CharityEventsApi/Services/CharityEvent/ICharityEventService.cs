using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.CharityEvent
{
    public interface ICharityEventService
    {
        public void AddCharityEvent(CharityEventDto charityEventDto);
        public void AddLocation(AddLocationDto locationDto);
    }
}
