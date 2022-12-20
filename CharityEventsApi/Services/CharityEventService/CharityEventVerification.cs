using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Services.AccountService;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.CharityEventService
{
    public class CharityEventVerification : BooleanCharityEventQueryBase
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly CharityEventActivation charityEventActivation;
        private readonly IAccountService accountService;

        public CharityEventVerification(CharityEventsDbContext dbContext, CharityEventActivation charityEventActivation,
            IAccountService accountService)
        {
            this.dbContext = dbContext;
            this.charityEventActivation = charityEventActivation;
            this.accountService = accountService;
        }

        protected override void setTrue(int charityEventId)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(ce => ce.IdCharityEvent == charityEventId);
            if (charityevent is null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }
            accountService.GiveRole(charityevent.OrganizerId, "Organizer");


            charityevent.IsVerified = 1;
            dbContext.SaveChanges();
        }

        protected override void setFalse(int charityEventId)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(ce => ce.IdCharityEvent == charityEventId);
            if (charityevent is null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }
            charityEventActivation.SetValue(charityEventId, false);
            charityevent.IsVerified = 0;
            dbContext.SaveChanges();
        }
    }
}
