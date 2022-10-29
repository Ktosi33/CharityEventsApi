using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.PersonalData
{
    public interface IPersonalDataService
    {
        public GetPersonalDataWithAddressDto getPersonalDataById(int id);
    }
}
