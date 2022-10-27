using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.CharityEvent
{
    public class CharityEventService : ICharityEventService
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly ICharityEventFactoryFacade charityEventFactoryFacade;

        public CharityEventService(CharityEventsDbContext dbContext, ICharityEventFactoryFacade charityEventFactoryFacade)
        {
            this.dbContext = dbContext;
            this.charityEventFactoryFacade = charityEventFactoryFacade;
        }

        public void AddCharityEvent(AddAllCharityEventsDto charityEventDto)
        {
           charityEventFactoryFacade.AddCharityEvent(charityEventDto);
        }

        public void EditCharityEvent(EditCharityEventDto charityEventDto, int charityEventId)
        {
           var charityevent = dbContext.Charityevents.FirstOrDefault(ce => ce.IdCharityEvent == charityEventId);
            if(charityevent == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }
            charityevent.Title = charityEventDto.Title;
            charityevent.Description = charityEventDto.Description;
            dbContext.SaveChanges();
        }
     
        public void EndCharityEvent(int charityEventId)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(ce => ce.IdCharityEvent == charityEventId);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }

            charityevent.IsActive = 0;
            if (charityevent.CharityFundraisingIdCharityFundraising != null)
            {
                var cf = dbContext.Charityfundraisings.FirstOrDefault(cf => cf.IdCharityFundraising == charityevent.CharityFundraisingIdCharityFundraising);
                


                if (cf != null)
                { 
                    if(cf.EndEventDate == null)
                    { 
                     cf.EndEventDate = DateTime.Now;
                    }
                    cf.IsActive = 0;
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
                    cv.IsActive = 0;
                }
            }

            dbContext.SaveChanges();

        }
      

    }
}
