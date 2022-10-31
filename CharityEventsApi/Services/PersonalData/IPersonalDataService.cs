using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.PersonalData
{
    public interface IPersonalDataService
    {
        public GetAllPersonalDataDto getPersonalDataById(int id);
        public void addAllPersonalData(AddAllPersonalDataDto personalDataDto, int userId);
    }
}
