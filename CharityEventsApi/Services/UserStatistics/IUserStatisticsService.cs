using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.UserStatistics
{
    public interface IUserStatisticsService
    {
        public List<DonationDto> getStatisticByUserId(int id);
    }
}
