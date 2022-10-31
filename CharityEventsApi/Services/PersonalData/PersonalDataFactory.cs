using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Entities;

namespace CharityEventsApi.Services.PersonalData
{
    public class PersonalDataFactory
    {
        public Entities.PersonalData createPersonalData(AddAllPersonalDataDto addAllPersonalDataDto, int addressId, int userId)
        {
            Entities.PersonalData personalData = new Entities.PersonalData
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
