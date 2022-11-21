using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace CharityEventsApi.Services.SearchService
{
    public interface ISearchService
    {
        public IEnumerable<GetAllDetailsCharityEventDto> GetCharityEvents(bool? isVerified, bool? isActive, bool? isFundraising, bool? isVolunteering,
            bool? volunteeringIsActive, bool? fundraisingIsActive, bool? volunteeringIsVerified, bool? fundraisingIsVerified);
        public GetAllDetailsCharityEventDto GetCharityEventsById(int charityEventId);
    }
}
