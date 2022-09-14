using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.CharityEvent
{
    public interface ICharityEventVolunteeringService
    {
        public void AddLocation(AddLocationDto locationDto);
        public void EditLocation(EditLocationDto locationDto);
        public void EditCharityEventVolunteering(EditCharityEventVolunteeringDto charityEventVolunteeringDto);
        public void EndCharityEventVolunteering(int CharityEventVolunteeringId);
    }
}
