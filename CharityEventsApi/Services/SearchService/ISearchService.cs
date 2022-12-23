using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace CharityEventsApi.Services.SearchService
{
    public interface ISearchService
    {
        public Task<IEnumerable<GetAllDetailsCharityEventDto>> GetCharityEvents(bool? isVerified, bool? isActive, bool? isFundraising, bool? isVolunteering,
            bool? volunteeringIsActive, bool? fundraisingIsActive, bool? volunteeringIsVerified, bool? fundraisingIsVerified, string? sortBy, string? sortDirection);
        public Task<PagedResultDto<GetAllDetailsCharityEventDto>> GetCharityEventsWithPagination(bool? isVerified, bool? isActive, bool? isFundraising, bool? isVolunteering,
            bool? volunteeringIsActive, bool? fundraisingIsActive, bool? volunteeringIsVerified, bool? fundraisingIsVerified, string? sortBy, string? sortDirection,
            int pageNumber, int pageSize, bool? volunteeringOrFundrasingIsActive, bool? volunteeringOrFundrasingIsVerified, bool? volunteeringOrFundraising);
        public Task<GetAllDetailsCharityEventDto> GetCharityEventsById(int charityEventId);
        public Task<List<GetAllDetailsCharityEventDto>> GetMostPopularFundraisings(int numberOfEvents);
        public Task<GetAllDetailsCharityEventDto> getDetails(CharityEvent charityEvent);
    }
}
