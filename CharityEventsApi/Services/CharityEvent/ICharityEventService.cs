using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.CharityEvent
{
    public interface ICharityEventService
    {
        public void AddCharityEvent(AddAllCharityEventsDto charityEventDto);
        public void EditCharityEvent(EditCharityEventDto charityEventDto, int charityEventId);
        public void EndCharityEvent(int charityEventId);
    }
}
