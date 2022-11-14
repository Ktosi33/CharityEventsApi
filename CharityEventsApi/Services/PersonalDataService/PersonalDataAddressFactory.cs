using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.PersonalDataService
{
    public class PersonalDataAddressFactory
    {
        public Address createAddress(AddAllPersonalDataDto addAllPersonalDataDto)
        {
            Address address = new Address
            {
               FlatNumber = addAllPersonalDataDto.FlatNumber,
               HouseNumber = addAllPersonalDataDto.HouseNumber,
               PostalCode = addAllPersonalDataDto.PostalCode,
               Street = addAllPersonalDataDto.Street,
               Town = addAllPersonalDataDto.Town
            };

            return address;
        }


    }
}
