using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.CharityEvent;
using CharityEventsApi.Exceptions;

namespace CharityEventsApi.Services.UserStatistics
{
    public class UserStatisticsService: IUserStatisticsService
    {

        private readonly CharityEventsDbContext dbContext;

        public UserStatisticsService(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<DonationDto> getStatisticByUserId(int id)
        {
            List < DonationDto > donations = new List<DonationDto>();
            var donation = dbContext.Donations.Where(d => d.UserIdUser == id);

            if (donation is null)
            {
                throw new NotFoundException("Nie znaleziono donacji"); 
            }

            foreach (Donation d in donation)
            {
                donations.Add(new DonationDto { IdDonations = d.IdDonations, AmountOfDonation = d.AmountOfDonation, 
                CharityFundraisingIdCharityFundraising = d.CharityFundraisingIdCharityFundraising, 
                Description = d.Description, DonationDate = d.DonationDate});
            }

            return donations;
        }
    }
}
