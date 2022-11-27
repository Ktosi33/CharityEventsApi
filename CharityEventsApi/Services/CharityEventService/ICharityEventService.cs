using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.CharityEventService
{
    public interface ICharityEventService
    {
        public Task Add(AddAllCharityEventsDto charityEventDto);
        public Task ChangeImage(IFormFile image, int idCharityEvent);
        public Task AddOneImage(IFormFile image, int idCharityEvent);
        public Task DeleteImage(int idImage, int idCharityEvent);
        public void Edit(EditCharityEventDto charityEventDto, int charityEventId);
        public GetCharityEventDto GetCharityEventById(int id);
        public void SetActive(int chariyEventId, bool isActive);
        public void SetVerify(int chariyEventId, bool isVerified);
        public IEnumerable<GetCharityEventDto> GetAll();
        
    }
}
