using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.FundraisingService
{
    public interface IFundraisingService
    {
        public Task Add(AddCharityEventFundraisingDto dto);
        public void Edit(EditCharityEventFundraisingDto FundraisingDto, int FundraisingId);
        public GetCharityFundrasingDto GetById(int id);
        public void SetActive(int FundraisingId, bool isActive);
        public void SetVerify(int FundraisingId, bool isVerified);
        public IEnumerable<GetCharityFundrasingDto> GetAll();
       // public Task AddOneImage(IFormFile image, int fundraisingId);
       // public Task DeleteImage(int idImage, int idFundraising);
    }
}
