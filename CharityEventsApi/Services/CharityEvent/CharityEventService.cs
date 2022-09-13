using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.CharityEvent
{
    public class CharityEventService : ICharityEventService
    {
        private readonly CharityEventsDbContext dbContext;
        public CharityEventService(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddCharityEvent(CharityEventDto charityEventDto)
        {
           CharityEventFactoryFacade charityEventFactoryFacade = new CharityEventFactoryFacade(dbContext);
           charityEventFactoryFacade.AddCharityEvent(charityEventDto);
        }
        public void AddLocation(AddLocationDto locationDto)
        {
            CharityEventFactoryFacade charityEventFactoryFacade = new CharityEventFactoryFacade(dbContext);
            charityEventFactoryFacade.AddLocation(locationDto);
        }

    }
}
