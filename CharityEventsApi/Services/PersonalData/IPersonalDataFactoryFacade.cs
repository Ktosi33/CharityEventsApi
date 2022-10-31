using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.PersonalData
{
    public interface IPersonalDataFactoryFacade
    {
        public void addPersonalData(AddAllPersonalDataDto addAllPersonalDataDto, int userId);
    }
}
