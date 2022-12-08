using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Services.VolunteeringService;

namespace CharityEventsApi.Services.FundraisingService
{
    public class FundraisingDenial : BooleanCharityEventQueryBase
    {
        private readonly CharityEventsDbContext dbContext;

        public FundraisingDenial(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        protected override void setTrue(int idfundraising)
        {
            var fundraising = dbContext.Charityfundraisings.FirstOrDefault(cf => cf.IdCharityFundraising == idfundraising);
            if (fundraising == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }
            fundraising.IsDenied = 1;
            dbContext.SaveChanges();
        }
        protected override void setFalse(int idfundraising)
        {
            var fundraising = dbContext.Charityfundraisings.FirstOrDefault(cf => cf.IdCharityFundraising == idfundraising);
            if (fundraising == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }

            fundraising.IsDenied = 0;
            dbContext.SaveChanges();
        }
    }
}
