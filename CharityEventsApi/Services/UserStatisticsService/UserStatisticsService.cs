﻿using CharityEventsApi.Entities;
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
            var donation = dbContext.Donations.Where(d => d.UserIdUser == id);

            if (donation is null)
            {
                throw new NotFoundException("Nie znaleziono donacji"); 
            }

            foreach (Entities.Donation d in donation)
            {
                donations.Add(new DonationDto { IdDonations = d.IdDonations, AmountOfDonation = d.AmountOfDonation, 
                CharityFundraisingIdCharityFundraising = d.CharityFundraisingIdCharityFundraising, 
                Description = d.Description, DonationDate = d.DonationDate});
            }

            return donations;
        }

        public List<VolunteeringDto> getVolunteeringStatisticsByUserId(int id)
        {
            List<VolunteeringDto> volunteerings = new List<VolunteeringDto>();
            var volunteering = dbContext.Volunteerings.Where(v => v.UserIdUsers.Any(u => u.IdUser == id));
            
            if (volunteering is null)
            {
                throw new NotFoundException("Nie znaleziono akcji wolontariackiej");
            }

            foreach (Volunteering v in volunteering)
            {
                volunteerings.Add(new VolunteeringDto
                {
                    IdVolunteering = v.IdVolunteering,
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
            var volunteerings = dbContext.Volunteerings.Where(v => v.UserIdUsers.Any(u => u.IdUser == userId));
            var donations = dbContext.Donations.Where(d => d.UserIdUser == userId);
            var charityEventsAsOrganizer = dbContext.Charityevents.Where(f => f.OrganizerId == userId); 

            decimal totalValueDonations = 0;
            foreach(var donation in donations)
            {
                totalValueDonations += donation.AmountOfDonation;
            }

            int numberFundrasingsAsOrganizer = 0;
            int numberVolunteeringsAsOrganizer = 0;
            foreach(var charityEvent in charityEventsAsOrganizer)
            {
                if (charityEvent.CharityFundraisingIdCharityFundraising != null)
                    numberFundrasingsAsOrganizer += 1;
                if (charityEvent.VolunteeringIdVolunteering != null)
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

            var volunteerings = await dbContext.Volunteerings
                .Where(v => v.UserIdUsers.Any(u => u.IdUser == userId))
                .Include(v => v.Charityevents)
                .ToListAsync();

            if (volunteerings is null)
                throw new NotFoundException("Nie znaleziono akcji");

            foreach (Volunteering v in volunteerings)
            {
                var charityEventId = v.Charityevents.FirstOrDefault(c => c.VolunteeringIdVolunteering == v.IdVolunteering).IdCharityEvent;
                charityEvents.Add(await searchService.GetCharityEventsById(charityEventId));
            }

            return charityEvents;
        }
    }
}
