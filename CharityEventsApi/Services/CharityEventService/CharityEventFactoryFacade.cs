using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.FundraisingService;
using CharityEventsApi.Services.ImageService;
using CharityEventsApi.Services.VolunteeringService;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CharityEventsApi.Services.CharityEventService
{
    public class CharityEventFactoryFacade : ICharityEventFactoryFacade
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly FundraisingFactory fundraisingFactory;
        private readonly VolunteeringFactory volunteeringFactory;
        private readonly CharityEventFactory charityEventFactory;

        public CharityEventFactoryFacade(CharityEventsDbContext dbContext, FundraisingFactory fundraisingFactory,
            VolunteeringFactory volunteeringFactory, CharityEventFactory charityEventFactory)
        {
            this.dbContext = dbContext;
            this.fundraisingFactory = fundraisingFactory;
            this.volunteeringFactory = volunteeringFactory;
            this.charityEventFactory = charityEventFactory;
        }
        public async Task AddCharityEvent(AddAllCharityEventsDto charityEventDto)
        {
            using var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
            var organizer = await dbContext.Users.FirstOrDefaultAsync(u => u.IdUser == charityEventDto.IdOrganizer);
            if (organizer is null)
            {
                throw new BadRequestException("Organizer ID can't match with any user");
            }

            CharityEvent charityevent = await charityEventFactory.CreateCharityEvent(charityEventDto, organizer);

            await dbContext.CharityEvents.AddAsync(charityevent);

            await dbContext.SaveChangesAsync();

            if (charityEventDto.IsFundraising)
            {
                await addCharityEventFundraising(charityEventDto, charityevent);
            }

            if (charityEventDto.IsVolunteering)
            {
                await addCharityEventVolunteering(charityEventDto, charityevent);
            }

            await transaction.CommitAsync();
        }
        
        private async Task addCharityEventVolunteering(AddAllCharityEventsDto charityEventDto, CharityEvent charityEvent)
        {
            CharityVolunteering volunteering = volunteeringFactory.CreateCharityEvent(charityEventDto);
            await dbContext.CharityVolunteerings.AddAsync(volunteering);

            charityEvent.IdCharityVolunteeringNavigation = volunteering;

            await dbContext.SaveChangesAsync();
        }
        public async Task AddCharityEventVolunteering(AddCharityEventVolunteeringDto charityEventDto, CharityEvent charityevent)
        {
            using var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
            CharityVolunteering cv = volunteeringFactory.CreateCharityEvent(charityEventDto);
            await dbContext.CharityVolunteerings.AddAsync(cv);

            charityevent.IdCharityVolunteeringNavigation = cv;

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        private async Task addCharityEventFundraising(AddAllCharityEventsDto charityEventDto, CharityEvent charityEvent)
        {           
            CharityFundraising charityfundraising = fundraisingFactory.CreateCharityEvent(charityEventDto);
            await dbContext.CharityFundraisings.AddAsync(charityfundraising);

            charityEvent.IdCharityFundraisingNavigation = charityfundraising;

            await dbContext.SaveChangesAsync();
        }
        public async Task AddCharityEventFundraising(AddCharityEventFundraisingDto charityEventDto, CharityEvent charityEvent)
        {
            using var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);

            CharityFundraising cf = fundraisingFactory.CreateCharityEvent(charityEventDto);
            await dbContext.CharityFundraisings.AddAsync(cf);

            charityEvent.IdCharityFundraisingNavigation = cf;

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
       

    }
}
