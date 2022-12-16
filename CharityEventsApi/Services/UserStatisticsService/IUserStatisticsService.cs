using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.UserStatisticsService
{
    public interface IUserStatisticsService
    {
        public List<DonationDto> getDonationStatisticByUserId(int id);
        public List<VolunteeringDto> getVolunteeringStatisticsByUserId(int userId);
        public GetUserStatisticsDto getUserStatisticsByUserId(int userId);
        public Task<List<GetAllDetailsCharityEventDto>> getCharityEventsWithVolunteeringByUserId(int userId);
        public Task<List<GetAllDetailsCharityEventDto>> getCharityEventsByOrganizerId(int organizerId, bool? volunteeringOrFundraisingIsActive, bool? volunteeringOrFundraisingIsVerified, bool? volunteeringOrFundraisingIsDenied);
    }
}
