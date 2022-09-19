using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.CharityEvent
{
    public interface ICharityEventFundraisingFactory
    {
        public Charityfundraising CreateCharityEvent(AddAllCharityEventsDto charityEventDto);
        public Charityfundraising CreateCharityEvent(AddCharityEventFundraisingDto charityEventDto);
    }
}
