using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.UserStatistics
{
    public interface IUserStatisticsService
    {
        public List<DonationDto> getDonationStatisticByUserId(int id);
        public List<VolunteeringDto> getVolunteeringStatisticsByUserId(int userId);
    }
}
