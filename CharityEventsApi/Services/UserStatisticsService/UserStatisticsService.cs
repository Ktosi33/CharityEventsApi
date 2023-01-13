using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Exceptions;
using Microsoft.EntityFrameworkCore;
using CharityEventsApi.Services.SearchService;

namespace CharityEventsApi.Services.UserStatisticsService
{
    public class UserStatisticsService: IUserStatisticsService
    {

        private readonly CharityEventsDbContext dbContext;
        private readonly ISearchService searchService;

        public UserStatisticsService(CharityEventsDbContext dbContext, ISearchService searchService)
        {
            this.dbContext = dbContext;
            this.searchService = searchService;
        }

        public List<DonationDto> getDonationStatisticByUserId(int id)
        {
            List < DonationDto > donations = new List<DonationDto>();
            var donation = dbContext.Donations.Where(d => d.IdUser == id);

            if (donation is null)
            {
                throw new NotFoundException("Nie znaleziono donacji"); 
            }

            foreach (Entities.Donation d in donation)
            {
                donations.Add(new DonationDto { IdDonation = d.IdDonation, AmountOfDonation = d.AmountOfDonation, 
                IdCharityFundraising = d.IdCharityFundraising, 
                Description = d.Description, DonationDate = d.DonationDate});
            }

            return donations;
        }

        public List<VolunteeringDto> getVolunteeringStatisticsByUserId(int id)
        {
            List<VolunteeringDto> volunteerings = new List<VolunteeringDto>();
            var volunteering = dbContext.CharityVolunteerings.Where(v => v.IdUsers.Any(u => u.IdUser == id));
            
            if (volunteering is null)
            {
                throw new NotFoundException("Nie znaleziono akcji wolontariackiej");
            }

            foreach (CharityVolunteering v in volunteering)
            {
                volunteerings.Add(new VolunteeringDto
                {
                    IdCharityVolunteering = v.IdCharityVolunteering,
                    AmountOfNeededVolunteers = v.AmountOfNeededVolunteers,
                    CreatedEventDate = v.CreatedEventDate,
                    EndEventDate = v.EndEventDate,
                    IsActive = v.IsActive,
                    IsVerified = v.IsVerified
                });
            }
            return volunteerings;
        }

        public GetUserStatisticsDto getUserStatisticsByUserId(int userId)
        {
            var volunteerings = dbContext.CharityVolunteerings.Where(v => v.IdUsers.Any(u => u.IdUser == userId));
            var donations = dbContext.Donations.Where(d => d.IdUser == userId);
            var charityEventsAsOrganizer = dbContext.CharityEvents.Where(f => f.IdOrganizer == userId); 

            decimal totalValueDonations = 0;
            foreach(var donation in donations)
            {
                totalValueDonations += donation.AmountOfDonation;
            }

            int numberFundrasingsAsOrganizer = 0;
            int numberVolunteeringsAsOrganizer = 0;
            foreach(var charityEvent in charityEventsAsOrganizer)
            {
                if (charityEvent.IdCharityFundraising != null)
                    numberFundrasingsAsOrganizer += 1;
                if (charityEvent.IdCharityVolunteering != null)
                    numberVolunteeringsAsOrganizer += 1;
            }

            return new GetUserStatisticsDto
            {
                NumberDonations = donations.Count(),
                TotalValueDonations = totalValueDonations,
                NumberActionsAsVolunteer = volunteerings.Count(),
                NumberFundrasingsAsOrganizer = numberFundrasingsAsOrganizer,
                NumberVolunteeringsAsOrganizer = numberVolunteeringsAsOrganizer     
            };
        }

        public async Task<List<GetAllDetailsCharityEventDto>> getCharityEventsWithVolunteeringByUserId(int userId)
        {
            List<GetAllDetailsCharityEventDto> charityEvents = new List<GetAllDetailsCharityEventDto>();

            var volunteerings = await dbContext.CharityVolunteerings
                .Where(v => v.IdUsers.Any(u => u.IdUser == userId))
                .Include(v => v.CharityEvents)
                .ToListAsync();

            if (volunteerings is null)
                throw new NotFoundException("Nie znaleziono akcji");

            foreach (CharityVolunteering v in volunteerings)
            {
                var charityEvent = v.CharityEvents.FirstOrDefault(c => c.IdCharityVolunteering == v.IdCharityVolunteering);
                if (charityEvent != null)
                    charityEvents.Add(await searchService.GetCharityEventsById(charityEvent.IdCharityEvent));
            }

            return charityEvents;
        }

        public async Task<List<GetAllDetailsCharityEventDto>> getCharityEventsByOrganizerId(int organizerId, bool? volunteeringOrFundraisingIsActive, bool? volunteeringOrFundraisingIsVerified, bool? volunteeringOrFundraisingIsDenied)
        {
            var charityEvents = dbContext.CharityEvents
               .Include(c => c.IdCharityFundraisingNavigation)
               .Include(c => c.IdCharityVolunteeringNavigation)
               .Where(c => c.IdOrganizer == organizerId)
               .Where(c => volunteeringOrFundraisingIsActive == null || (c.IdCharityFundraisingNavigation == null && c.IdCharityVolunteeringNavigation == null) || (c.IdCharityFundraisingNavigation != null && c.IdCharityFundraisingNavigation.IsActive == Convert.ToSByte(volunteeringOrFundraisingIsActive)) || (c.IdCharityVolunteeringNavigation != null && c.IdCharityVolunteeringNavigation.IsActive == Convert.ToSByte(volunteeringOrFundraisingIsActive)))
               .Where(c => volunteeringOrFundraisingIsVerified == null || (c.IdCharityFundraisingNavigation == null && c.IdCharityVolunteeringNavigation == null) || (c.IdCharityFundraisingNavigation != null && c.IdCharityFundraisingNavigation.IsVerified == Convert.ToSByte(volunteeringOrFundraisingIsVerified)) || (c.IdCharityVolunteeringNavigation != null && c.IdCharityVolunteeringNavigation.IsVerified == Convert.ToSByte(volunteeringOrFundraisingIsVerified)))
               .Where(c => volunteeringOrFundraisingIsDenied == null || (c.IdCharityFundraisingNavigation == null && c.IdCharityVolunteeringNavigation == null) || (c.IdCharityFundraisingNavigation != null && c.IdCharityFundraisingNavigation.IsDenied == Convert.ToSByte(volunteeringOrFundraisingIsDenied)) || (c.IdCharityVolunteeringNavigation != null && c.IdCharityVolunteeringNavigation.IsDenied == Convert.ToSByte(volunteeringOrFundraisingIsDenied)));

            charityEvents = charityEvents.OrderByDescending(c => c.CreatedEventDate);

            var charityEventsList = await charityEvents.ToListAsync();
            var charityEventsDetails = new List<GetAllDetailsCharityEventDto>();

            foreach (CharityEvent charityEvent in charityEventsList)
                charityEventsDetails.Add(await searchService.getDetails(charityEvent));
           
            return charityEventsDetails;
        }
    }
}
