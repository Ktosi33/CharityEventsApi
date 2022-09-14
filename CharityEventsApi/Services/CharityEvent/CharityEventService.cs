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

        public void EditCharityEvent(EditCharityEventDto charityEventDto)
        {
           var charityevent = dbContext.Charityevents.FirstOrDefault(ce => ce.IdCharityEvent == charityEventDto.CharityEventId);
            if(charityevent == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }
            charityevent.Title = charityEventDto.Title;
            charityevent.Description = charityEventDto.Description;
            dbContext.SaveChanges();
        }
     
        public void EndCharityEvent(int CharityEventId)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(ce => ce.IdCharityEvent == CharityEventId);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }

            charityevent.IsActive = 0;
            if (charityevent.CharityFundraisingIdCharityFundraising != null)
            {
                var cf = dbContext.Charityfundraisings.FirstOrDefault(cf => cf.IdCharityFundraising == charityevent.CharityFundraisingIdCharityFundraising);
                if(cf != null)
                { 
                    if(cf.EndEventDate == null)
                    { 
                     cf.EndEventDate = DateTime.Now;
                    }
                }
            }
            if (charityevent.VolunteeringIdVolunteering != null)
            {
                var cv = dbContext.Volunteerings.FirstOrDefault(cv => cv.IdVolunteering == charityevent.VolunteeringIdVolunteering);
                if (cv != null)
                {
                    if (cv.EndEventDate == null)
                    {
                        cv.EndEventDate = DateTime.Now;
                    }
                }
            }

            dbContext.SaveChanges();

        }
      

    }
}
