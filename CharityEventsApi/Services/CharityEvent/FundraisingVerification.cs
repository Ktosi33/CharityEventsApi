using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.CharityEvent
{
    public class FundraisingVerification : VerificationBase
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly FundraisingActivation fundraisingActivation;

        public FundraisingVerification(CharityEventsDbContext dbContext, FundraisingActivation fundraisingActivation)
        {
            this.dbContext = dbContext;
            this.fundraisingActivation = fundraisingActivation;
        }

        protected override void verify(int FundraisingId)
        {
            var fundraising = dbContext.Charityfundraisings.Include(ce => ce.Charityevents).FirstOrDefault(f => f.IdCharityFundraising == FundraisingId);
            if (fundraising == null)
            {
                throw new NotFoundException("CharityEventFundraising with given id doesn't exist");
            }
            var charityevent = fundraising.Charityevents.FirstOrDefault();
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEventFundraising doesn't have charity event.");
            }
            if (charityevent.IsVerified == 0)
            {
                throw new BadRequestException("Firstly verify charityevent");
            }
            fundraising.IsVerified = 1;
            dbContext.SaveChanges();
        }
        protected override void unverify(int FundraisingId)
        {
            var fundraising = dbContext.Charityfundraisings.Include(ce => ce.Charityevents).FirstOrDefault(f => f.IdCharityFundraising == FundraisingId);
            if (fundraising == null)
            {
                throw new NotFoundException("CharityEventFundraising with given id doesn't exist");
            }
            fundraisingActivation.SetActive(FundraisingId,false);
            fundraising.IsVerified = 0;
            dbContext.SaveChanges();
        }
    }
}
