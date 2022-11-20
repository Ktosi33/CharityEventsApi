using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Services.FundraisingService;
using CharityEventsApi.Services.VolunteeringService;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CharityEventsApi.Services.SearchService
{
    public class SearchService: ISearchService
    {
        
        private readonly CharityEventsDbContext dbContext;

        public SearchService(CharityEventsDbContext dbContext, IFundraisingService fundraisingService, IVolunteeringService volunteeringService)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<GetAllDetailsCharityEventDto> GetCharityEvents()
        {
            var charityEvents = dbContext.Charityevents
                .Include(c => c.CharityFundraisingIdCharityFundraisingNavigation)
                .Include(c => c.VolunteeringIdVolunteeringNavigation);

            var charityEventsDetails = new List<GetAllDetailsCharityEventDto>();

            foreach(Charityevent charityEvent in charityEvents)
                charityEventsDetails.Add(getDetails(charityEvent));
            

            return charityEventsDetails;
        }

        public GetAllDetailsCharityEventDto GetCharityEventsById(int charityEventId)
        {
            var charityEvent = dbContext.Charityevents
                .Where(c => c.IdCharityEvent == charityEventId)
                .Include(x => x.VolunteeringIdVolunteeringNavigation)
                .Include(x => x.CharityFundraisingIdCharityFundraisingNavigation);

            return getDetails(charityEvent.First());
        }

        public GetAllDetailsCharityEventDto getDetails(Charityevent charityEvent)
        {
            GetAllDetailsCharityEventDto charityEventDetails = new GetAllDetailsCharityEventDto()
            {
                IdCharityEvent = charityEvent.IdCharityEvent,
                IsActive = charityEvent.IsActive,
                Description = charityEvent.Description,
                FundraisingId = charityEvent.CharityFundraisingIdCharityFundraising,
                isVerified = charityEvent.IsVerified,
                Title = charityEvent.Title,
                VolunteeringId = charityEvent.VolunteeringIdVolunteering
            };

            if (charityEvent.VolunteeringIdVolunteeringNavigation != null)
            {
                charityEventDetails.CharityEventVolunteering = new GetCharityEventVolunteeringDto
                {
                    AmountOfNeededVolunteers = charityEvent.VolunteeringIdVolunteeringNavigation.AmountOfNeededVolunteers,
                    CreatedEventDate = charityEvent.VolunteeringIdVolunteeringNavigation.CreatedEventDate,
                    EndEventDate = charityEvent.VolunteeringIdVolunteeringNavigation.EndEventDate,
                    IsActive = charityEvent.VolunteeringIdVolunteeringNavigation.IsActive,
                    isVerified = charityEvent.VolunteeringIdVolunteeringNavigation.IsVerified
                };
            }

            if (charityEvent.CharityFundraisingIdCharityFundraisingNavigation != null)
            {
                charityEventDetails.CharityEventFundrasing = new GetCharityFundrasingDto
                {
                    AmountOfAlreadyCollectedMoney = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.AmountOfAlreadyCollectedMoney,
                    AmountOfMoneyToCollect = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.AmountOfMoneyToCollect,
                    CreatedEventDate = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.CreatedEventDate,
                    EndEventDate = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.EndEventDate,
                    FundTarget = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.FundTarget,
                    IsActive = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.IsActive,
                    isVerified = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.IsVerified
                };
            }

            return charityEventDetails;
        }

    }
}
