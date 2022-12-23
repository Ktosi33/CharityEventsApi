using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.CharityEventService
{
    public class CharityEventActivation : BooleanCharityEventQueryBase
    {
        private readonly CharityEventsDbContext dbContext;

        public CharityEventActivation(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override void setTrue(int charityEventId)
        {
            var charityevent = dbContext.CharityEvents
                .Include(ce => ce.IdCharityFundraisingNavigation)
                .Include(ce => ce.IdCharityVolunteeringNavigation)
                .FirstOrDefault(ce => ce.IdCharityEvent == charityEventId);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }

            if (charityevent.IsVerified == 0)
            {
                throw new BadRequestException("You cant active charity event while event isn't active or verified");
            }
            charityevent.IsActive = 1;
          /*  if(charityevent.CharityFundraisingIdCharityFundraisingNavigation != null)
            {
                if(charityevent.CharityFundraisingIdCharityFundraisingNavigation.IsVerified == 0)
                {
                    throw new BadRequestException("Firstly verify fundraising charity");
                }
                charityevent.CharityFundraisingIdCharityFundraisingNavigation.IsActive = 1;
            }
            if (charityevent.VolunteeringIdVolunteeringNavigation != null)
            {
                if (charityevent.VolunteeringIdVolunteeringNavigation.IsVerified == 0)
                {
                    throw new BadRequestException("Firstly verify volunteering charity");
                }
                charityevent.VolunteeringIdVolunteeringNavigation.IsActive = 1;
            } 
          */
            dbContext.SaveChanges();
        }
        protected override void setFalse(int charityEventId)
        {
            var charityevent = dbContext.CharityEvents.FirstOrDefault(ce => ce.IdCharityEvent == charityEventId);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }

            charityevent.IsActive = 0;
            if (charityevent.IdCharityFundraising != null)
            {
                var cf = dbContext.CharityFundraisings.FirstOrDefault(cf => cf.IdCharityFundraising == charityevent.IdCharityFundraising);


                if (cf != null)
                {
                    if (cf.EndEventDate == null)
                    {
                        cf.EndEventDate = DateTime.Now;
                    }
                    //cf.IsActive = 0;
                }
            }
            if (charityevent.IdCharityVolunteering != null)
            {
                var cv = dbContext.CharityVolunteerings.FirstOrDefault(cv => cv.IdCharityVolunteering == charityevent.IdCharityVolunteering);
                if (cv != null)
                {
                    if (cv.EndEventDate == null)
                    {
                        cv.EndEventDate = DateTime.Now;
                    }
                   // cv.IsActive = 0;
                }
            }

            dbContext.SaveChanges();

        }
    }
}
