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
        public void Edit(EditCharityEventDto charityEventDto, int idCharityEvent);
        public void SetActive(int idCharityEvent, bool isActive);
        public void SetVerify(int idCharityEvent, bool isVerified);
        public void SetDeny(int idCharityEvent, bool isDenied);
        public Task<IEnumerable<ImageDto>> GetImagesAsync(int idCharityEvent);
        public GetCharityEventDto GetCharityEventDtoById(int id);
        public IEnumerable<GetCharityEventDto> GetAllCharityEventDto();
        public CharityEvent GetCharityEventByCharityEventId(int idCharityEvent);
        public CharityEvent GetCharityEventByVolunteeringId(int idVolunteering);
        public CharityEvent GetCharityEventByFundraisingId(int idFundraising);


    }
}
