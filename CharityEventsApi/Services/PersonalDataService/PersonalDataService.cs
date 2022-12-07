using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.PersonalDataService
{
    public class PersonalDataService: IPersonalDataService
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly IPersonalDataFactoryFacade personalDataFactoryFacade;

        public PersonalDataService(CharityEventsDbContext dbContext, IPersonalDataFactoryFacade personalDataFactoryFacade)
        {
            this.dbContext = dbContext;
            this.personalDataFactoryFacade = personalDataFactoryFacade;
        }

        public bool doesPersonalDataExists(int id)
        {
            var d = dbContext
                .PersonalData
                .FirstOrDefault(d => d.UserIdUser == id);

            if (d == null)
                return false;
            else
                return true;
        }

        public void addAllPersonalData(AddAllPersonalDataDto personalDataDto, int userId)
        {
            if((dbContext.PersonalData.FirstOrDefault(p => p.UserIdUser == userId)) == null)
                personalDataFactoryFacade.addPersonalData(personalDataDto, userId);
            else
                throw new ForbiddenException("User with the given id has data");        
        }

        public void editAllPersonalData(EditAllPersonalDataDto personalDataDto, int idPersonalData)
        {
            var personalData = dbContext.PersonalData.FirstOrDefault(p => p.UserIdUser == idPersonalData);
            if (personalData == null)
                throw new NotFoundException("Personal data with given id doesn't exist");
         
            var address = dbContext.Addresses.FirstOrDefault(a => a.IdAddress == personalData.AddressIdAddress);
            if (address == null)
                throw new NotFoundException("Address with given id doesn't exist");

            personalData.Surname = personalDataDto.Surname;
            personalData.PhoneNumber = personalDataDto.PhoneNumber;
            personalData.Email = personalDataDto.Email;
            personalData.Name = personalDataDto.Name;
            address.Street = personalDataDto.Street;
            address.HouseNumber = personalDataDto.HouseNumber;
            address.FlatNumber = personalDataDto.FlatNumber;
            address.PostalCode = personalDataDto.PostalCode;
            address.Town = personalDataDto.Town;

            dbContext.SaveChanges();

        }

        public GetSomePersonalDataDto getSomePersonalDataById(int userId)
        {
            var d = dbContext
                .PersonalData
                .Include(d => d.AddressIdAddressNavigation)
                .FirstOrDefault(d => d.UserIdUser == userId);

            if (d is null)
                throw new NotFoundException("PersonalData for this user id doesn't exist");


            return new GetSomePersonalDataDto
            {
                Name = d.Name,
                Surname = d.Surname
            };
        }

        public GetAllPersonalDataDto getPersonalDataById(int id)
        {
            var d = dbContext
                .PersonalData
                .Include(d => d.AddressIdAddressNavigation)
                .FirstOrDefault(d => d.UserIdUser == id);
                
            if (d is null)
                throw new NotFoundException("PersonalData for this user id doesn't exist");


            return new GetAllPersonalDataDto
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
