using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.UserStatisticsService
{
    public interface IUserStatisticsService
    {
        public List<DonationDto> getDonationStatisticByUserId(int id);
        public List<VolunteeringDto> getVolunteeringStatisticsByUserId(int userId);
        public GetUserStatisticsDto getUserStatisticsByUserId(int userId);
    }
}
