using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.PersonalData
{
    public class PersonalDataService: IPersonalDataService
    {
        private readonly CharityEventsDbContext dbContext;

        public PersonalDataService(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        public GetPersonalDataDto getPersonalDataById(int id)
        {
            var d = dbContext.PersonalData.FirstOrDefault(d => d.UserIdUser == id);
            if (d is null)
            {
                throw new NotFoundException("PersonalData for this user id doesn't exist");
            }


            return new GetPersonalDataDto
            {
                Name = d.Name,
                Surname = d.Surname,
                Email = d.Email,
                PhoneNumber = d.PhoneNumber,
                AddressIdAddress = d.AddressIdAddress
            };
        }
            
    }
}
