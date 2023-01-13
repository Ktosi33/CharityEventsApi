using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.ImageService;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CharityEventsApi.Services.SearchService
{
    public class SearchService : ISearchService
    {

        private readonly CharityEventsDbContext dbContext;
        private readonly IImageService imageService;

        public SearchService(CharityEventsDbContext dbContext, IImageService imageService)
        {
            this.dbContext = dbContext;
            this.imageService = imageService;
        }

        [Obsolete("GetCharityEvents is deprecated, please use getCharityEventsWithPagination instead")]
        public async Task<IEnumerable<GetAllDetailsCharityEventDto>> GetCharityEvents(bool? isVerified, bool? isActive, bool? isFundraising, bool? isVolunteering,
           bool? volunteeringIsActive, bool? fundraisingIsActive, bool? volunteeringIsVerified, bool? fundraisingIsVerified, string? sortBy, string? sortDirection)
        {
            var charityEvents = dbContext.CharityEvents
                .Include(c => c.IdCharityFundraisingNavigation)
                .Include(c => c.IdCharityVolunteeringNavigation)
                .Where(c => isVerified == null || c.IsVerified == Convert.ToSByte(isVerified))
                .Where(c => isActive == null || c.IsActive == Convert.ToSByte(isActive))
                .Where(c => isFundraising == null || c.IdCharityFundraising.Equals(null) == !isFundraising)
                .Where(c => isVolunteering == null || c.IdCharityVolunteering.Equals(null) == !isVolunteering)
                .Where(c => volunteeringIsActive == null || c.IdCharityVolunteeringNavigation == null || c.IdCharityVolunteeringNavigation.IsActive == Convert.ToSByte(volunteeringIsActive))
                .Where(c => fundraisingIsActive == null || c.IdCharityFundraisingNavigation == null || c.IdCharityFundraisingNavigation.IsActive == Convert.ToSByte(fundraisingIsActive))
                .Where(c => volunteeringIsVerified == null || c.IdCharityVolunteeringNavigation == null || c.IdCharityVolunteeringNavigation.IsVerified == Convert.ToSByte(volunteeringIsVerified))
                .Where(c => fundraisingIsVerified == null || c.IdCharityFundraisingNavigation == null || c.IdCharityFundraisingNavigation.IsVerified == Convert.ToSByte(fundraisingIsVerified));

            charityEvents = sort(charityEvents, sortBy, sortDirection);

            var charityEventsList = await charityEvents.ToListAsync();
            var charityEventsDetails = new List<GetAllDetailsCharityEventDto>();

            foreach (CharityEvent charityEvent in charityEventsList)
                charityEventsDetails.Add(await getDetails(charityEvent));

            return charityEventsDetails;
        }

        public async Task<PagedResultDto<GetAllDetailsCharityEventDto>> GetCharityEventsWithPagination(bool? isVerified, bool? isActive, bool? isFundraising, bool? isVolunteering,
           bool? volunteeringIsActive, bool? fundraisingIsActive, bool? volunteeringIsVerified, bool? fundraisingIsVerified, string? sortBy, string? sortDirection,
           int pageNumber, int pageSize, bool? volunteeringOrFundraisingIsActive, bool? volunteeringOrFundraisingIsVerified, bool? volunteeringOrFundraisingIsDenied)
        {
            var charityEvents = dbContext.CharityEvents
                .Include(c => c.IdCharityFundraisingNavigation)
                .Include(c => c.IdCharityVolunteeringNavigation)
                .Where(c => isVerified == null || c.IsVerified == Convert.ToSByte(isVerified))
                .Where(c => isActive == null || c.IsActive == Convert.ToSByte(isActive))
                .Where(c => isFundraising == null || c.IdCharityFundraising.Equals(null) == !isFundraising)
                .Where(c => isVolunteering == null || c.IdCharityVolunteering.Equals(null) == !isVolunteering)
                .Where(c => volunteeringIsActive == null || c.IdCharityVolunteeringNavigation == null || c.IdCharityVolunteeringNavigation.IsActive == Convert.ToSByte(volunteeringIsActive))
                .Where(c => fundraisingIsActive == null || c.IdCharityFundraisingNavigation == null || c.IdCharityFundraisingNavigation.IsActive == Convert.ToSByte(fundraisingIsActive))
                .Where(c => volunteeringIsVerified == null || c.IdCharityVolunteeringNavigation == null || c.IdCharityVolunteeringNavigation.IsVerified == Convert.ToSByte(volunteeringIsVerified))
                .Where(c => fundraisingIsVerified == null || c.IdCharityFundraisingNavigation == null || c.IdCharityFundraisingNavigation.IsVerified == Convert.ToSByte(fundraisingIsVerified))
                .Where(c => volunteeringOrFundraisingIsActive == null || (c.IdCharityFundraisingNavigation == null && c.IdCharityVolunteeringNavigation == null) || (c.IdCharityFundraisingNavigation != null && c.IdCharityFundraisingNavigation.IsActive == Convert.ToSByte(volunteeringOrFundraisingIsActive)) || (c.IdCharityVolunteeringNavigation != null && c.IdCharityVolunteeringNavigation.IsActive == Convert.ToSByte(volunteeringOrFundraisingIsActive)))
                .Where(c => volunteeringOrFundraisingIsVerified == null || (c.IdCharityFundraisingNavigation == null && c.IdCharityVolunteeringNavigation == null) || (c.IdCharityFundraisingNavigation != null && c.IdCharityFundraisingNavigation.IsVerified == Convert.ToSByte(volunteeringOrFundraisingIsVerified)) || (c.IdCharityVolunteeringNavigation != null && c.IdCharityVolunteeringNavigation.IsVerified == Convert.ToSByte(volunteeringOrFundraisingIsVerified)))
                .Where(c => volunteeringOrFundraisingIsDenied == null || (c.IdCharityFundraisingNavigation == null && c.IdCharityVolunteeringNavigation == null) || (c.IdCharityFundraisingNavigation != null && c.IdCharityFundraisingNavigation.IsDenied == Convert.ToSByte(volunteeringOrFundraisingIsDenied)) || (c.IdCharityVolunteeringNavigation != null && c.IdCharityVolunteeringNavigation.IsDenied == Convert.ToSByte(volunteeringOrFundraisingIsDenied)));

            charityEvents = sort(charityEvents, sortBy, sortDirection);

            List<CharityEvent> charityEventsList = new();
            var charityEventsDetails = new List<GetAllDetailsCharityEventDto>();

            if (pageNumber <= 0 || pageSize <= 0)
                throw new BadRequestException("Values ​​needed for pagination cannot be non-positive");

            charityEventsList = await charityEvents
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            var totalItemsCount = charityEvents.Count();

            foreach (CharityEvent charityEvent in charityEventsList)
                charityEventsDetails.Add(await getDetails(charityEvent));
            
            return new PagedResultDto<GetAllDetailsCharityEventDto>(charityEventsDetails, totalItemsCount, pageSize, pageNumber);
        }

        public async Task<GetAllDetailsCharityEventDto> GetCharityEventsById(int charityEventId)
        {
            var charityEvent = await dbContext.CharityEvents
                .Where(c => c.IdCharityEvent == charityEventId)
                .Include(x => x.IdCharityVolunteeringNavigation)
                .ThenInclude(x => x!.IdUsers)
                .Include(x => x.IdCharityFundraisingNavigation)
                .ToListAsync();

            return await getDetails(charityEvent.First());
        }

        public async Task<List<GetAllDetailsCharityEventDto>> GetMostPopularFundraisings(int numberOfEvents)
        {
            List<GetAllDetailsCharityEventDto> mostPopularFundraisingsList = new List<GetAllDetailsCharityEventDto>();

            var fundraisings = dbContext.CharityFundraisings
                .Include(f => f.Donations)
                .Include(f => f.CharityEvents)
                .Where(f => f.IsActive == 1)
                .Where(f => f.IsVerified == 1);

            fundraisings = fundraisings.OrderByDescending(f => f.Donations.Count);

            var fundraisingsList = await fundraisings
                .Take(numberOfEvents)
                .ToListAsync();

            foreach (var fundraising in fundraisingsList)
            {
                var charityEvent = fundraising.CharityEvents.FirstOrDefault(c => c.IdCharityFundraising == fundraising.IdCharityFundraising);
                if (charityEvent != null)
                    mostPopularFundraisingsList.Add(await GetCharityEventsById(charityEvent.IdCharityEvent));
            }

            return mostPopularFundraisingsList;

        }

        public async Task<GetAllDetailsCharityEventDto> getDetails(CharityEvent charityEvent)
        {
            GetAllDetailsCharityEventDto charityEventDetails = new GetAllDetailsCharityEventDto()
            {
                IdCharityEvent = charityEvent.IdCharityEvent,
                IsActive = charityEvent.IsActive,
                Description = charityEvent.Description,
                IdCharityFundraising = charityEvent.IdCharityFundraising,
                IsVerified = charityEvent.IsVerified,
                IsDenied = charityEvent.IsDenied,
                Title = charityEvent.Title,
                imageDto = await imageService.GetImageAsync(charityEvent.IdImage),
                IdCharityVolunteering = charityEvent.IdCharityVolunteering
            };

            if (charityEvent.IdCharityVolunteeringNavigation != null)
            {
                charityEventDetails.CharityEventVolunteering = new GetCharityEventVolunteeringDto
                {
                    AmountOfNeededVolunteers = charityEvent.IdCharityVolunteeringNavigation.AmountOfNeededVolunteers,
                    CreatedEventDate = charityEvent.IdCharityVolunteeringNavigation.CreatedEventDate,
                    EndEventDate = charityEvent.IdCharityVolunteeringNavigation.EndEventDate,
                    IsActive = charityEvent.IdCharityVolunteeringNavigation.IsActive,
                    IsVerified = charityEvent.IdCharityVolunteeringNavigation.IsVerified,
                    IsDenied = charityEvent.IdCharityVolunteeringNavigation.IsDenied,
                    Id = charityEvent.IdCharityVolunteeringNavigation.IdCharityVolunteering,
                    AmountOfAttendedVolunteers = charityEvent.IdCharityVolunteeringNavigation.IdUsers.Count,
                };
            }

            if (charityEvent.IdCharityFundraisingNavigation != null)
            {
                charityEventDetails.CharityEventFundraising = new GetCharityFundraisingDto
                {
                    AmountOfAlreadyCollectedMoney = charityEvent.IdCharityFundraisingNavigation.AmountOfAlreadyCollectedMoney,
                    AmountOfMoneyToCollect = charityEvent.IdCharityFundraisingNavigation.AmountOfMoneyToCollect,
                    CreatedEventDate = charityEvent.IdCharityFundraisingNavigation.CreatedEventDate,
                    EndEventDate = charityEvent.IdCharityFundraisingNavigation.EndEventDate,
                    FundTarget = charityEvent.IdCharityFundraisingNavigation.FundTarget,
                    IsActive = charityEvent.IdCharityFundraisingNavigation.IsActive,
                    isVerified = charityEvent.IdCharityFundraisingNavigation.IsVerified,
                    IsDenied = charityEvent.IdCharityFundraisingNavigation.IsDenied,
                    IdCharityFundraising = charityEvent.IdCharityFundraisingNavigation.IdCharityFundraising
                };
            }

            return charityEventDetails;
        }

        public IQueryable<CharityEvent> sort(IQueryable<CharityEvent> charityEvents, string? sortBy, string? sortDirection)
        {
            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(sortDirection))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<CharityEvent, object>>>
                {
                    { nameof(CharityEvent.CreatedEventDate), c => c.CreatedEventDate },
                };

                if (!columnsSelectors.ContainsKey(sortBy))
                    throw new BadRequestException("Invalid sorting rule");

                var selectedColumn = columnsSelectors[sortBy];

                if (sortDirection == "ASC")
                    charityEvents = charityEvents.OrderBy(c => c.CreatedEventDate);
                else if (sortDirection == "DESC")
                    charityEvents = charityEvents.OrderByDescending(c => c.CreatedEventDate);
                else
                    throw new BadRequestException("Invalid direction");
            }
            else
            {
                charityEvents = charityEvents.OrderByDescending(c => c.CreatedEventDate);
            }

            return charityEvents;
        }

    }
}
