using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Services.AccountService;
using CharityEventsApi.Services.CharityEventService;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.FundraisingService
{
    public class FundraisingVerification : BooleanCharityEventQueryBase
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly FundraisingActivation fundraisingActivation;
        private readonly IAccountService accountService;
        private readonly ICharityEventService charityEventService;

        public FundraisingVerification(CharityEventsDbContext dbContext, FundraisingActivation fundraisingActivation,
            IAccountService accountService, ICharityEventService charityEventService)
        {
            this.dbContext = dbContext;
            this.fundraisingActivation = fundraisingActivation;
            this.accountService = accountService;
            this.charityEventService = charityEventService;
        }

        protected override void setTrue(int idFundraising)
        {
            var fundraising = dbContext.CharityFundraisings.FirstOrDefault(f => f.IdCharityFundraising == idFundraising);
            if (fundraising == null)
            {
                throw new NotFoundException("CharityEventFundraising with given id doesn't exist");
            }
            accountService.GiveRole(charityEventService
                .GetCharityEventByFundraisingId(idFundraising).IdOrganizer, "Organizer");

            fundraising.IsVerified = 1;
            dbContext.SaveChanges();
        }
        protected override void setFalse(int FundraisingId)
        {
            var fundraising = dbContext.CharityFundraisings.FirstOrDefault(f => f.IdCharityFundraising == FundraisingId);
            if (fundraising is null)
            {
                throw new NotFoundException("CharityEventFundraising with given id doesn't exist");
            }
            fundraisingActivation.SetValue(FundraisingId,false);
            fundraising.IsVerified = 0;
            dbContext.SaveChanges();
        }
    }
}
