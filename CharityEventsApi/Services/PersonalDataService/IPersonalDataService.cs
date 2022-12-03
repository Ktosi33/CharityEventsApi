using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.PersonalDataService
{
    public interface IPersonalDataService
    {
        public GetAllPersonalDataDto getPersonalDataById(int id);
        public void addAllPersonalData(AddAllPersonalDataDto personalDataDto, int userId);
        public void editAllPersonalData(EditAllPersonalDataDto personalDataDto, int idPersonalData);
        public bool doesPersonalDataExists(int id);
    }
}
