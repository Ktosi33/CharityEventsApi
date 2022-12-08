using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Services.CharityEventService;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.FundraisingService
{
    public class FundraisingVerification : BooleanCharityEventQueryBase
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly FundraisingActivation fundraisingActivation;

        public FundraisingVerification(CharityEventsDbContext dbContext, FundraisingActivation fundraisingActivation)
        {
            this.dbContext = dbContext;
            this.fundraisingActivation = fundraisingActivation;
        }

        protected override void setTrue(int FundraisingId)
        {
            var fundraising = dbContext.Charityfundraisings.FirstOrDefault(f => f.IdCharityFundraising == FundraisingId);
            if (fundraising == null)
            {
                throw new NotFoundException("CharityEventFundraising with given id doesn't exist");
            }
            fundraising.IsVerified = 1;
            dbContext.SaveChanges();
        }
        protected override void setFalse(int FundraisingId)
        {
            var fundraising = dbContext.Charityfundraisings.FirstOrDefault(f => f.IdCharityFundraising == FundraisingId);
            if (fundraising == null)
            {
                throw new NotFoundException("CharityEventFundraising with given id doesn't exist");
            }
            fundraisingActivation.SetValue(FundraisingId,false);
            fundraising.IsVerified = 0;
            dbContext.SaveChanges();
        }
    }
}
