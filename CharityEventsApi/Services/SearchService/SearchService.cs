﻿using CharityEventsApi.Entities;
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
            var charityEvents = dbContext.Charityevents
                .Include(c => c.CharityFundraisingIdCharityFundraisingNavigation)
                .Include(c => c.VolunteeringIdVolunteeringNavigation)
                .Where(c => isVerified == null || c.IsVerified == Convert.ToSByte(isVerified))
                .Where(c => isActive == null || c.IsActive == Convert.ToSByte(isActive))
                .Where(c => isFundraising == null || c.CharityFundraisingIdCharityFundraising.Equals(null) == !isFundraising)
                .Where(c => isVolunteering == null || c.VolunteeringIdVolunteering.Equals(null) == !isVolunteering)
                .Where(c => volunteeringIsActive == null || c.VolunteeringIdVolunteeringNavigation == null || c.VolunteeringIdVolunteeringNavigation.IsActive == Convert.ToSByte(volunteeringIsActive))
                .Where(c => fundraisingIsActive == null || c.CharityFundraisingIdCharityFundraisingNavigation == null || c.CharityFundraisingIdCharityFundraisingNavigation.IsActive == Convert.ToSByte(fundraisingIsActive))
                .Where(c => volunteeringIsVerified == null || c.VolunteeringIdVolunteeringNavigation == null || c.VolunteeringIdVolunteeringNavigation.IsVerified == Convert.ToSByte(volunteeringIsVerified))
                .Where(c => fundraisingIsVerified == null || c.CharityFundraisingIdCharityFundraisingNavigation == null || c.CharityFundraisingIdCharityFundraisingNavigation.IsVerified == Convert.ToSByte(fundraisingIsVerified));

            charityEvents = sort(charityEvents, sortBy, sortDirection);

            var charityEventsList = await charityEvents.ToListAsync();
            var charityEventsDetails = new List<GetAllDetailsCharityEventDto>();

            foreach (Charityevent charityEvent in charityEventsList)
                charityEventsDetails.Add(await getDetails(charityEvent));

            return charityEventsDetails;
        }

        public async Task<PagedResultDto<GetAllDetailsCharityEventDto>> GetCharityEventsWithPagination(bool? isVerified, bool? isActive, bool? isFundraising, bool? isVolunteering,
           bool? volunteeringIsActive, bool? fundraisingIsActive, bool? volunteeringIsVerified, bool? fundraisingIsVerified, string? sortBy, string? sortDirection,
           int pageNumber, int pageSize, bool? volunteeringOrFundraisingIsActive, bool? volunteeringOrFundraisingIsVerified, bool? volunteeringOrFundraisingIsDenied)
        {
            var charityEvents = dbContext.Charityevents
                .Include(c => c.CharityFundraisingIdCharityFundraisingNavigation)
                .Include(c => c.VolunteeringIdVolunteeringNavigation)
                .Where(c => isVerified == null || c.IsVerified == Convert.ToSByte(isVerified))
                .Where(c => isActive == null || c.IsActive == Convert.ToSByte(isActive))
                .Where(c => isFundraising == null || c.CharityFundraisingIdCharityFundraising.Equals(null) == !isFundraising)
                .Where(c => isVolunteering == null || c.VolunteeringIdVolunteering.Equals(null) == !isVolunteering)
                .Where(c => volunteeringIsActive == null || c.VolunteeringIdVolunteeringNavigation == null || c.VolunteeringIdVolunteeringNavigation.IsActive == Convert.ToSByte(volunteeringIsActive))
                .Where(c => fundraisingIsActive == null || c.CharityFundraisingIdCharityFundraisingNavigation == null || c.CharityFundraisingIdCharityFundraisingNavigation.IsActive == Convert.ToSByte(fundraisingIsActive))
                .Where(c => volunteeringIsVerified == null || c.VolunteeringIdVolunteeringNavigation == null || c.VolunteeringIdVolunteeringNavigation.IsVerified == Convert.ToSByte(volunteeringIsVerified))
                .Where(c => fundraisingIsVerified == null || c.CharityFundraisingIdCharityFundraisingNavigation == null || c.CharityFundraisingIdCharityFundraisingNavigation.IsVerified == Convert.ToSByte(fundraisingIsVerified))
                .Where(c => volunteeringOrFundraisingIsActive == null || (c.CharityFundraisingIdCharityFundraisingNavigation == null && c.VolunteeringIdVolunteeringNavigation == null) || c.VolunteeringIdVolunteeringNavigation.IsActive == Convert.ToSByte(volunteeringOrFundraisingIsActive) || c.CharityFundraisingIdCharityFundraisingNavigation.IsActive == Convert.ToSByte(volunteeringOrFundraisingIsActive))
                .Where(c => volunteeringOrFundraisingIsVerified == null || (c.CharityFundraisingIdCharityFundraisingNavigation == null && c.VolunteeringIdVolunteeringNavigation == null) || c.VolunteeringIdVolunteeringNavigation.IsVerified == Convert.ToSByte(volunteeringOrFundraisingIsVerified) || c.CharityFundraisingIdCharityFundraisingNavigation.IsVerified == Convert.ToSByte(volunteeringOrFundraisingIsVerified))
                .Where(c => volunteeringOrFundraisingIsDenied == null || (c.CharityFundraisingIdCharityFundraisingNavigation == null && c.VolunteeringIdVolunteeringNavigation == null) || c.VolunteeringIdVolunteeringNavigation.IsDenied == Convert.ToSByte(volunteeringOrFundraisingIsDenied) || c.CharityFundraisingIdCharityFundraisingNavigation.IsDenied == Convert.ToSByte(volunteeringOrFundraisingIsDenied));

            charityEvents = sort(charityEvents, sortBy, sortDirection);

            List<Charityevent> charityEventsList = new();
            var charityEventsDetails = new List<GetAllDetailsCharityEventDto>();

            if (pageNumber <= 0 || pageSize <= 0)
                throw new BadRequestException("Values ​​needed for pagination cannot be non-positive");

            charityEventsList = await charityEvents
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            var totalItemsCount = charityEvents.Count();

            foreach (Charityevent charityEvent in charityEventsList)
                charityEventsDetails.Add(await getDetails(charityEvent));
            
            return new PagedResultDto<GetAllDetailsCharityEventDto>(charityEventsDetails, totalItemsCount, pageSize, pageNumber);
        }

        public async Task<GetAllDetailsCharityEventDto> GetCharityEventsById(int charityEventId)
        {
            var charityEvent = await dbContext.Charityevents
                .Where(c => c.IdCharityEvent == charityEventId)
                .Include(x => x.VolunteeringIdVolunteeringNavigation)
                .ThenInclude(x => x.UserIdUsers)
                .Include(x => x.CharityFundraisingIdCharityFundraisingNavigation)
                .ToListAsync();

            return await getDetails(charityEvent.First());
        }

        public async Task<List<GetAllDetailsCharityEventDto>> GetMostPopularFundraisings(int numberOfEvents)
        {
            List<GetAllDetailsCharityEventDto> mostPopularFundraisingsList = new List<GetAllDetailsCharityEventDto>();

            var fundraisings = dbContext.Charityfundraisings
                .Include(f => f.Donations)
                .Include(f => f.Charityevents)
                .Where(f => f.IsActive == 1)
                .Where(f => f.IsVerified == 1);

            fundraisings = fundraisings.OrderByDescending(f => f.Donations.Count);

            var fundraisingsList = await fundraisings
                .Take(numberOfEvents)
                .ToListAsync();

            foreach (var fundraising in fundraisingsList)
            {
                mostPopularFundraisingsList.Add(await GetCharityEventsById(fundraising.Charityevents.FirstOrDefault(c => c.CharityFundraisingIdCharityFundraising == fundraising.IdCharityFundraising).IdCharityEvent));
            }

            return mostPopularFundraisingsList;

        }

        public async Task<GetAllDetailsCharityEventDto> getDetails(Charityevent charityEvent)
        {
            GetAllDetailsCharityEventDto charityEventDetails = new GetAllDetailsCharityEventDto()
            {
                IdCharityEvent = charityEvent.IdCharityEvent,
                IsActive = charityEvent.IsActive,
                Description = charityEvent.Description,
                FundraisingId = charityEvent.CharityFundraisingIdCharityFundraising,
                IsVerified = charityEvent.IsVerified,
                IsDenied = charityEvent.IsDenied,
                Title = charityEvent.Title,
                imageDto = await imageService.GetImageAsync(charityEvent.ImageIdImages),
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
                    IsVerified = charityEvent.VolunteeringIdVolunteeringNavigation.IsVerified,
                    IsDenied = charityEvent.VolunteeringIdVolunteeringNavigation.IsDenied,
                    Id = charityEvent.VolunteeringIdVolunteeringNavigation.IdVolunteering,
                    AmountOfAttendedVolunteers = charityEvent.VolunteeringIdVolunteeringNavigation.UserIdUsers.Count,
                };
            }

            if (charityEvent.CharityFundraisingIdCharityFundraisingNavigation != null)
            {
                charityEventDetails.CharityEventFundrasing = new GetCharityFundraisingDto
                {
                    AmountOfAlreadyCollectedMoney = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.AmountOfAlreadyCollectedMoney,
                    AmountOfMoneyToCollect = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.AmountOfMoneyToCollect,
                    CreatedEventDate = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.CreatedEventDate,
                    EndEventDate = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.EndEventDate,
                    FundTarget = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.FundTarget,
                    IsActive = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.IsActive,
                    isVerified = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.IsVerified,
                    IsDenied = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.IsDenied,
                    Id = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.IdCharityFundraising
                };
            }

            return charityEventDetails;
        }

        public IQueryable<Charityevent> sort(IQueryable<Charityevent> charityEvents, string? sortBy, string? sortDirection)
        {
            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(sortDirection))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Charityevent, object>>>
                {
                    { nameof(Charityevent.CreatedEventDate), c => c.CreatedEventDate },
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
