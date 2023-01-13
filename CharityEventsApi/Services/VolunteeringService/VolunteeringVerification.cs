using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Services.AccountService;
using CharityEventsApi.Services.CharityEventService;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.VolunteeringService
{
    public class VolunteeringVerification : BooleanCharityEventQueryBase
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly VolunteeringActivation volunteeringActivation;
        private readonly ICharityEventService charityEventService;
        private readonly IAccountService accountService;

        public VolunteeringVerification(CharityEventsDbContext dbContext, VolunteeringActivation volunteeringActivation,
            ICharityEventService charityEventService, IAccountService accountService)
        {
            this.dbContext = dbContext;
            this.volunteeringActivation = volunteeringActivation;
            this.charityEventService = charityEventService;
            this.accountService = accountService;
        }
        protected override void setTrue(int idVolunteering)
        {
            var volunteering = dbContext.CharityVolunteerings.Include(ce => ce.CharityEvents).FirstOrDefault(v => v.IdCharityVolunteering == idVolunteering);
            if (volunteering is null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }

            volunteering.IsVerified = 1;
            dbContext.SaveChanges();
        }
        protected override void setFalse(int VolunteeringId)
        {
            var volunteering = dbContext.CharityVolunteerings.Include(ce => ce.CharityEvents).FirstOrDefault(v => v.IdCharityVolunteering == VolunteeringId);
            if (volunteering is null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }
            volunteeringActivation.SetValue(VolunteeringId, false);
            volunteering.IsVerified = 0;
            dbContext.SaveChanges();
        }
    }
}
