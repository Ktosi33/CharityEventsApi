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
            var charityevent = volunteering.Charityevents.FirstOrDefault();
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEventVolunteering doesn't have charity event.");
            }
            if (charityevent.IsVerified == 0)
            {
                throw new BadRequestException("Firstly verify charityevent");
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
