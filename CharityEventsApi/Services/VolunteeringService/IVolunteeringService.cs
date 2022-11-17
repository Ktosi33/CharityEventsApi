using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.VolunteeringService
{
    public interface IVolunteeringService
    {
        public void Add(AddCharityEventVolunteeringDto dto, int charityEventId);
        public void AddLocation(AddLocationDto locationDto);
        public void EditLocation(EditLocationDto locationDto, int locationId);
        public void Edit(EditCharityEventVolunteeringDto VolunteeringDto, int VolunteeringId);
        public void SetActive(int VolunteeringId, bool isActive);
        public void SetVerify(int VolunteeringId, bool isVerified);
        public GetCharityEventVolunteeringDto GetById(int id);
    }
}
