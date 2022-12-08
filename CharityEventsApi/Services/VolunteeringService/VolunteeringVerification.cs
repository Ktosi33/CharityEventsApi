using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Services.CharityEventService;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.VolunteeringService
{
    public class VolunteeringVerification : BooleanCharityEventQueryBase
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly VolunteeringActivation volunteeringActivation;

        public VolunteeringVerification(CharityEventsDbContext dbContext, VolunteeringActivation volunteeringActivation)
        {
            this.dbContext = dbContext;
            this.volunteeringActivation = volunteeringActivation;
        }
        protected override void setTrue(int VolunteeringId)
        {
            var volunteering = dbContext.Volunteerings.Include(ce => ce.Charityevents).FirstOrDefault(v => v.IdVolunteering == VolunteeringId);
            if (volunteering == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }
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
