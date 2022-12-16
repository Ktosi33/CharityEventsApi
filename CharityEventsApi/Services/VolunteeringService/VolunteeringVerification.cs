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
            var volunteering = dbContext.Volunteerings.Include(ce => ce.Charityevents).FirstOrDefault(v => v.IdVolunteering == idVolunteering);
            if (volunteering == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }

            accountService.GiveRole(charityEventService
                .GetCharityEventByVolunteeringId(idVolunteering).OrganizerId, "Organizer");

            volunteering.IsVerified = 1;
            dbContext.SaveChanges();
        }
        protected override void setFalse(int VolunteeringId)
        {
            var volunteering = dbContext.Volunteerings.Include(ce => ce.Charityevents).FirstOrDefault(v => v.IdVolunteering == VolunteeringId);
            if (volunteering == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }
            volunteeringActivation.SetValue(VolunteeringId, false);
            volunteering.IsVerified = 0;
            dbContext.SaveChanges();
        }
    }
}
