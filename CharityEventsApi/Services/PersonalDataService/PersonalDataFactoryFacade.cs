using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.PersonalDataService
{
    public class PersonalDataFactoryFacade: IPersonalDataFactoryFacade
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly PersonalDataAddressFactory personalDataAddressFactory;
        private readonly PersonalDataFactory personalDataFactory;

        public PersonalDataFactoryFacade(CharityEventsDbContext dbContext, PersonalDataAddressFactory personalDataAddressFactory, PersonalDataFactory personalDataFactory)
        {
            this.dbContext = dbContext;
            this.personalDataAddressFactory = personalDataAddressFactory;
            this.personalDataFactory = personalDataFactory;
        }   

        public void addPersonalData(AddAllPersonalDataDto addAllPersonalDataDto, int userId)
        {
            using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                Address address = personalDataAddressFactory.createAddress(addAllPersonalDataDto);
                dbContext.Addresses.Add(address);
                dbContext.SaveChanges();

                Entities.PersonalData personalData = personalDataFactory.createPersonalData(addAllPersonalDataDto, address.IdAddress, userId);
                dbContext.PersonalData.Add(personalData);
                dbContext.SaveChanges();

                transaction.Commit();
            }
        }
    }
}
