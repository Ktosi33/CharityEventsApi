using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Services.CharityEventService;

namespace CharityEventsApi.Services.VolunteeringService
{
    public class VolunteeringDenial : BooleanCharityEventQueryBase
    {
        private readonly CharityEventsDbContext dbContext;

        public VolunteeringDenial(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        protected override void setTrue(int idVolunteering)
        {
            var volunteering = dbContext.CharityVolunteerings.FirstOrDefault(v => v.IdCharityVolunteering == idVolunteering);
            if (volunteering == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }
            volunteering.IsDenied = 1;
            dbContext.SaveChanges();
        }
        protected override void setFalse(int idVolunteering)
        {
            var volunteering = dbContext.CharityVolunteerings.FirstOrDefault(v => v.IdCharityVolunteering == idVolunteering);
            if (volunteering == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }

            volunteering.IsDenied = 0;
            dbContext.SaveChanges();
        }
    }
}
