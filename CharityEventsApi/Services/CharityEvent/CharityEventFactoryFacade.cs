using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.CharityEvent
{
    public class CharityEventFactoryFacade : ICharityEventFactoryFacade
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly FundraisingFactory FundraisingFactory;
        private readonly VolunteeringFactory VolunteeringFactory;
        private readonly CharityEventFactory charityEventFactory;

        public CharityEventFactoryFacade(CharityEventsDbContext dbContext, FundraisingFactory FundraisingFactory, VolunteeringFactory VolunteeringFactory, CharityEventFactory charityEventFactory)
        {
            this.dbContext = dbContext;
            this.FundraisingFactory = FundraisingFactory;
            this.VolunteeringFactory = VolunteeringFactory;
            this.charityEventFactory = charityEventFactory;
        }
        public void AddCharityEvent(AddAllCharityEventsDto charityEventDto)
        {
            using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                var organizer = dbContext.Users.FirstOrDefault(u => u.IdUser == charityEventDto.OrganizerId);
                if (organizer is null)
                {
                    throw new BadRequestException("Organizer ID can't match with any user");
                }
           
                Charityevent charityevent = charityEventFactory.CreateCharityEvent(charityEventDto, organizer);
                dbContext.Charityevents.Add(charityevent);
                dbContext.SaveChanges();

                if (charityEventDto.isFundraising)
                {
                    AddCharityEventFundraising(charityEventDto, charityevent);
                }

                if (charityEventDto.isVolunteering)
                {
                    AddCharityEventVolunteering(charityEventDto, charityevent);
                }

                transaction.Commit();
            }
        }
      
        public void AddCharityEventVolunteering(AddAllCharityEventsDto charityEventDto, Charityevent charityEvent)
        {
            Volunteering volunteering = VolunteeringFactory.CreateCharityEvent(charityEventDto);
            dbContext.Volunteerings.Add(volunteering);
            
            charityEvent.VolunteeringIdVolunteeringNavigation = volunteering;
            charityEvent.VolunteeringIdVolunteering = volunteering.IdVolunteering;
            dbContext.SaveChanges();
        }

        public void AddCharityEventFundraising(AddAllCharityEventsDto charityEventDto, Charityevent charityEvent)
        {
            Charityfundraising charityfundraising = FundraisingFactory.CreateCharityEvent(charityEventDto);
            dbContext.Charityfundraisings.Add(charityfundraising);
            charityEvent.CharityFundraisingIdCharityFundraisingNavigation = charityfundraising;
            charityEvent.CharityFundraisingIdCharityFundraising = charityfundraising.IdCharityFundraising;
            dbContext.SaveChanges();
        }
        public void AddLocation(AddLocationDto locationDto)
        {
            var vol = dbContext.Volunteerings.FirstOrDefault(v => v.IdVolunteering == locationDto.idVolunteering);
            if(vol == null)
            {
                throw new BadRequestException("Volunteering ID doesnt exist");
            }
            var newLocation = VolunteeringFactory.newLocation(locationDto, vol);
            vol.LocationIdLocations.Add(newLocation);
            dbContext.Locations.Add(newLocation);
            dbContext.SaveChanges();
        }

    }
}
