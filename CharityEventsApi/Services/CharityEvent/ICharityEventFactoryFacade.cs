using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.CharityEvent
{
    public interface ICharityEventFactoryFacade
    {
        public void AddCharityEvent(AddAllCharityEventsDto charityEventDto);

        public void AddCharityEventVolunteering(AddAllCharityEventsDto charityEventDto, Charityevent charityEvent);

        public void AddCharityEventFundraising(AddAllCharityEventsDto charityEventDto, Charityevent charityEvent);
        public void AddLocation(AddLocationDto locationDto);
    }
}
