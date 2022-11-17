using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;

namespace CharityEventsApi.Services.CharityEventService
{
    public class CharityEventActivation : ActivationBase
    {
        private readonly CharityEventsDbContext dbContext;

        public CharityEventActivation(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override void active(int charityEventId)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(ce => ce.IdCharityEvent == charityEventId);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }

            if (charityevent.IsVerified == 0)
            {
                throw new BadRequestException("You cant active charity event while event isn't active or verified");
            }
            charityevent.IsActive = 1;
            dbContext.SaveChanges();
        }
        protected override void disactive(int charityEventId)
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
