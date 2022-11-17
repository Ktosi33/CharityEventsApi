using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.PersonalDataService
{
    public interface IPersonalDataFactoryFacade
    {
        public void addPersonalData(AddAllPersonalDataDto addAllPersonalDataDto, int userId);

     
    }
}
