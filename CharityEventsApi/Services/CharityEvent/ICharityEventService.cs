using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.CharityEvent
{
    public interface ICharityEventService
    {
        public void Add(AddAllCharityEventsDto charityEventDto);
        public void Edit(EditCharityEventDto charityEventDto, int charityEventId);
        public GetCharityEventDto GetCharityEventById(int id);
        public void SetActive(int chariyEventId, bool isActive);
        public void SetVerify(int chariyEventId, bool isVerified);
    }
}
