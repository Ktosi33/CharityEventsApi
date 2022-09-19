using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.CharityEvent
{
    public interface ICharityEventVolunteeringService
    {
        public void AddLocation(AddLocationDto locationDto);
        public void EditLocation(EditLocationDto locationDto, int locationId);
        public void EditCharityEventVolunteering(EditCharityEventVolunteeringDto charityEventVolunteeringDto, int charityEventVolunteeringId);
        public void EndCharityEventVolunteering(int charityEventVolunteeringId);
    }
}
