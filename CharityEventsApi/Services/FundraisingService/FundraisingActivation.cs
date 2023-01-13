using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Services.CharityEventService;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.FundraisingService
{
    public class FundraisingActivation : BooleanCharityEventQueryBase
    {
        private readonly CharityEventsDbContext dbContext;

        public FundraisingActivation(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        protected override void setTrue(int FundraisingId)
        {
            var fundraising = dbContext.CharityFundraisings.Include(ce => ce.CharityEvents).FirstOrDefault(f => f.IdCharityFundraising == FundraisingId);
            if (fundraising == null)
            {
                throw new NotFoundException("CharityEventFundraising with given id doesn't exist");
            }

            if (fundraising.IsVerified == 0)
            {
                throw new BadRequestException("You cant active fundraising while charity event isn't active or verified");
            }
            fundraising.IsActive = 1;
            dbContext.SaveChanges();
        }

        protected override void setFalse(int FundraisingId)
        {
            var fundraising = dbContext.CharityFundraisings.Include(ce => ce.CharityEvents).FirstOrDefault(f => f.IdCharityFundraising == FundraisingId);
            if (fundraising == null)
            {
                throw new NotFoundException("CharityEventFundraising with given id doesn't exist");
            }
            fundraising.EndEventDate = DateTime.Now;
            fundraising.IsActive = 0;
            var charityevent = fundraising.CharityEvents.FirstOrDefault();
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEventFundraising dont have charity event.");
            }

            if (charityevent.IdCharityVolunteering == null)
            {
                charityevent.IsActive = 0;
            }
            else
            {
                var cv = dbContext.CharityVolunteerings.FirstOrDefault(cv => cv.IdCharityVolunteering == charityevent.IdCharityVolunteering);
                if (cv != null)
                {
                    if (cv.EndEventDate != null)
                    {
                        charityevent.IsActive = 0;
                    }
                }
            }


            dbContext.SaveChanges();
        }
    }
}
