using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.CharityEvent
{
    public interface ICharityEventFundraisingService
    {
        public void EditCharityEventFundraising(EditCharityEventFundraisingDto charityEventFundraisingDto);
        public void EndCharityEventFundraising(int CharityEventFundraisingId);
    }
}
