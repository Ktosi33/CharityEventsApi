using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.CharityEvent
{
    public interface ICharityEventVolunteeringFactory
    {
        public Volunteering CreateCharityEvent(AddAllCharityEventsDto charityEventDto);
        public Volunteering CreateCharityEvent(AddCharityEventVolunteeringDto charityEventDto);
        public Location newLocation(AddLocationDto locationDto, Volunteering volunteering);
    }
}
