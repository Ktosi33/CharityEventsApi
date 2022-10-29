using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.PersonalData
{
    public class PersonalDataService: IPersonalDataService
    {
        private readonly CharityEventsDbContext dbContext;

        public PersonalDataService(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        public GetPersonalDataWithAddressDto getPersonalDataById(int id)
        {
            var d = dbContext
                .PersonalData
                .Include(d => d.AddressIdAddressNavigation)
                .FirstOrDefault(d => d.UserIdUser == id);
                
            if (d is null)
            {
                throw new NotFoundException("PersonalData for this user id doesn't exist");
            }


            return new GetPersonalDataWithAddressDto
            {
                Name = d.Name,
                Surname = d.Surname,
                Email = d.Email,
                PhoneNumber = d.PhoneNumber,
                Town = d.AddressIdAddressNavigation.Town,
                PostalCode = d.AddressIdAddressNavigation.PostalCode,
                Street = d.AddressIdAddressNavigation.Street,
                HouseNumber = d.AddressIdAddressNavigation.HouseNumber,
                FlatNumber = d.AddressIdAddressNavigation.FlatNumber
                
            };
        }
            
    }
}
