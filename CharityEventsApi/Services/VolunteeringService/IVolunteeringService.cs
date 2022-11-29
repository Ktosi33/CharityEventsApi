using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.VolunteeringService
{
    public interface IVolunteeringService
    {
        public Task Add(AddCharityEventVolunteeringDto dto);
        public void Edit(EditCharityEventVolunteeringDto VolunteeringDto, int VolunteeringId);
        public void SetActive(int VolunteeringId, bool isActive);
        public void SetVerify(int VolunteeringId, bool isVerified);
        public GetCharityEventVolunteeringDto GetById(int id);
        public IEnumerable<GetCharityEventVolunteeringDto> GetAll();
    }
}
