using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.CharityEventService
{
    public class CharityEventActivation : ActivationBase
    {
        private readonly CharityEventsDbContext dbContext;

        public CharityEventActivation(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override void Active(int charityEventId)
        {
            var charityevent = dbContext.Charityevents
                .Include(ce => ce.CharityFundraisingIdCharityFundraisingNavigation)
                .Include(ce => ce.VolunteeringIdVolunteeringNavigation)
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
            if(charityevent.CharityFundraisingIdCharityFundraisingNavigation != null)
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
            dbContext.SaveChanges();
        }
        protected override void Disactive(int charityEventId)
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
                    if (cf.EndEventDate == null)
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
