using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Entities;

namespace CharityEventsApi.Services.PersonalDataService
{
    public class PersonalDataFactory
    {
        public PersonalData createPersonalData(AddAllPersonalDataDto addAllPersonalDataDto, int addressId, int userId)
        {
            PersonalData personalData = new PersonalData
            {
                Email = addAllPersonalDataDto.Email,
                Name = addAllPersonalDataDto.Name,
                PhoneNumber = addAllPersonalDataDto.PhoneNumber,
                Surname = addAllPersonalDataDto.Surname,
                AddressIdAddress = addressId,
                UserIdUser = userId
           
            };

            return personalData;
        }
    }
}
