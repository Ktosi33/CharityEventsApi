using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.SearchService
{
    public interface ISearchService
    {
        public IEnumerable<GetAllDetailsCharityEventDto> GetCharityEvents();
        public GetAllDetailsCharityEventDto GetCharityEventsById(int charityEventId);
    }
}
