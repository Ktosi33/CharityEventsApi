using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.CharityEventService
{
    public interface ICharityEventFactoryFacade
    {
        public Task AddCharityEvent(AddAllCharityEventsDto charityEventDto);
        public Task AddCharityEventFundraising(AddCharityEventFundraisingDto charityEventDto, CharityEvent charityEvent);
        public Task AddCharityEventVolunteering(AddCharityEventVolunteeringDto charityEventDto, CharityEvent charityevent);
    }
}
