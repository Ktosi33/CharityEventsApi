using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Services.CharityEventService;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.VolunteeringService
{
    public class VolunteeringVerification : VerificationBase
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly VolunteeringActivation volunteeringActivation;

        public VolunteeringVerification(CharityEventsDbContext dbContext, VolunteeringActivation volunteeringActivation)
        {
            this.dbContext = dbContext;
            this.volunteeringActivation = volunteeringActivation;
        }
        protected override void verify(int VolunteeringId)
        {
            var volunteering = dbContext.Volunteerings.Include(ce => ce.Charityevents).FirstOrDefault(v => v.IdVolunteering == VolunteeringId);
            if (volunteering == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }
            volunteering.IsVerified = 1;
            dbContext.SaveChanges();
        }
        protected override void unverify(int VolunteeringId)
        {
            var volunteering = dbContext.Volunteerings.Include(ce => ce.Charityevents).FirstOrDefault(v => v.IdVolunteering == VolunteeringId);
            if (volunteering == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }
            volunteeringActivation.SetActive(VolunteeringId, false);
            volunteering.IsVerified = 0;
            dbContext.SaveChanges();
        }
    }
}
