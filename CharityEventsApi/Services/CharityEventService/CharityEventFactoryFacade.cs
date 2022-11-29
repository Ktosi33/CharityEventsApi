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
            var organizer = await dbContext.Users.FirstOrDefaultAsync(u => u.IdUser == charityEventDto.OrganizerId);
            if (organizer is null)
            {
                throw new BadRequestException("Organizer ID can't match with any user");
            }

            Charityevent charityevent = await charityEventFactory.CreateCharityEvent(charityEventDto, organizer);

            await dbContext.Charityevents.AddAsync(charityevent);

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
        
        private async Task addCharityEventVolunteering(AddAllCharityEventsDto charityEventDto, Charityevent charityEvent)
        {
            Volunteering volunteering = volunteeringFactory.CreateCharityEvent(charityEventDto);
            await dbContext.Volunteerings.AddAsync(volunteering);

            charityEvent.VolunteeringIdVolunteeringNavigation = volunteering;

            await dbContext.SaveChangesAsync();
        }
        public async Task AddCharityEventVolunteering(AddCharityEventVolunteeringDto charityEventDto, Charityevent charityevent)
        {
            using var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
            Volunteering cv = volunteeringFactory.CreateCharityEvent(charityEventDto);
            await dbContext.Volunteerings.AddAsync(cv);

            charityevent.VolunteeringIdVolunteeringNavigation = cv;

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        private async Task addCharityEventFundraising(AddAllCharityEventsDto charityEventDto, Charityevent charityEvent)
        {           
            Charityfundraising charityfundraising = fundraisingFactory.CreateCharityEvent(charityEventDto);
            await dbContext.Charityfundraisings.AddAsync(charityfundraising);

            charityEvent.CharityFundraisingIdCharityFundraisingNavigation = charityfundraising;

            await dbContext.SaveChangesAsync();
        }
        public async Task AddCharityEventFundraising(AddCharityEventFundraisingDto charityEventDto, Charityevent charityEvent)
        {
            using var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);

            Charityfundraising cf = fundraisingFactory.CreateCharityEvent(charityEventDto);
            await dbContext.Charityfundraisings.AddAsync(cf);

            charityEvent.CharityFundraisingIdCharityFundraisingNavigation = cf;

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
       

    }
}
