using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.CharityEvent
{
    public interface ICharityEventFundraisingService
    {
        public void EditCharityEventFundraising(EditCharityEventFundraisingDto charityEventFundraisingDto, int charityEventFundraisingId);
        public void EndCharityEventFundraising(int CharityEventFundraisingId);
    }
}
