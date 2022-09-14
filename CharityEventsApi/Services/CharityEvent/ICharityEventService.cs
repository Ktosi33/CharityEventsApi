using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.CharityEvent
{
    public interface ICharityEventService
    {
        public void AddCharityEvent(CharityEventDto charityEventDto);
        public void EditCharityEvent(EditCharityEventDto charityEventDto);
        public void EndCharityEvent(int CharityEventId);
    }
}
