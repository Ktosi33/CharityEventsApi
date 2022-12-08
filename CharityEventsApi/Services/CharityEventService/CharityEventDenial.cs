using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Services.VolunteeringService;

namespace CharityEventsApi.Services.CharityEventService
{
    public class CharityEventDenial : BooleanCharityEventQueryBase
    {
        private readonly CharityEventsDbContext dbContext;

        public CharityEventDenial(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        protected override void setTrue(int IdCharityEvent)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(v => v.IdCharityEvent == IdCharityEvent);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }
            charityevent.IsDenied = 1;
            dbContext.SaveChanges();
        }
        protected override void setFalse(int IdCharityEvent)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(v => v.IdCharityEvent == IdCharityEvent);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }

            charityevent.IsDenied = 0;
            dbContext.SaveChanges();
        }
    }
}
