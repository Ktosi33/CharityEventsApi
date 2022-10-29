using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.PersonalData
{
    public interface IPersonalDataService
    {
        public GetPersonalDataDto getPersonalDataById(int id);
    }
}
