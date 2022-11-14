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

        public List<DonationDto> getDonationStatisticByUserId(int id)
        {
            List < DonationDto > donations = new List<DonationDto>();
            var donation = dbContext.Donations.Where(d => d.UserIdUser == id);

            if (donation is null)
            {
                throw new NotFoundException("Nie znaleziono donacji"); 
            }

            foreach (Entities.Donation d in donation)
            {
                donations.Add(new DonationDto { IdDonations = d.IdDonations, AmountOfDonation = d.AmountOfDonation, 
                CharityFundraisingIdCharityFundraising = d.CharityFundraisingIdCharityFundraising, 
                Description = d.Description, DonationDate = d.DonationDate});
            }

            return donations;
        }

        public List<VolunteeringDto> getVolunteeringStatisticsByUserId(int id)
        {
            List<VolunteeringDto> volunteerings = new List<VolunteeringDto>();
            var volunteering = dbContext.Volunteerings.Where(v => v.UserIdUsers.Any(u => u.IdUser == id));
            
            if (volunteering is null)
            {
                throw new NotFoundException("Nie znaleziono akcji wolontariackiej");
            }

            foreach (Volunteering v in volunteering)
            {
                volunteerings.Add(new VolunteeringDto
                {
                    IdVolunteering = v.IdVolunteering,
                    AmountOfNeededVolunteers = v.AmountOfNeededVolunteers,
                    CreatedEventDate = v.CreatedEventDate,
                    EndEventDate = v.EndEventDate,
                    IsActive = v.IsActive,
                    IsVerified = v.IsVerified
                });
            }
            return volunteerings;
        }


    }
}
