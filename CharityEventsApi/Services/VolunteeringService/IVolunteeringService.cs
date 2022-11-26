using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.VolunteeringService
{
    public interface IVolunteeringService
    {
        public Task Add(AddCharityEventVolunteeringDto dto);
        public Task AddOneImage(IFormFile image, int idVolunteering);
        public Task DeleteImage(int idImage, int idFundraising);
        public void AddLocation(AddLocationDto locationDto);
        public void EditLocation(EditLocationDto locationDto, int locationId);
        public void Edit(EditCharityEventVolunteeringDto VolunteeringDto, int VolunteeringId);
        public void SetActive(int VolunteeringId, bool isActive);
        public void SetVerify(int VolunteeringId, bool isVerified);
        public GetCharityEventVolunteeringDto GetById(int id);
        public IEnumerable<GetCharityEventVolunteeringDto> GetAll();
    }
}
