using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.CharityEvent
{
    public interface IFundraisingService
    {
        public void Add(AddCharityEventFundraisingDto dto, int charityEventId);
        public void Edit(EditCharityEventFundraisingDto FundraisingDto, int FundraisingId);
        public GetCharityFundrasingDto GetById(int id);
        public void SetActive(int FundraisingId, bool isActive);
        public void SetVerify(int FundraisingId, bool isVerified);
    }
}
