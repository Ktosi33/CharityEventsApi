using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.CharityEvent
{
    public class CharityEventFactoryFacade
    {
        private readonly CharityEventsDbContext dbContext;
        public CharityEventFactoryFacade(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void AddCharityEvent(CharityEventDto charityEventDto)
        {
            using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                var organizer = dbContext.Users.FirstOrDefault(u => u.IdUser == charityEventDto.OrganizerId);
                if (organizer is null)
                {
                    throw new BadRequestException("Organizer ID can't match with any user");
                }

                CharityEventFactory charityEventFactory = new CharityEventFactory();

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
      
        public void AddCharityEventVolunteering(CharityEventDto charityEventDto, Charityevent charityEvent)
        {
            CharityEventVolunteeringFactory charityEventVolunteeringFactory = new CharityEventVolunteeringFactory();
            Volunteering volunteering = charityEventVolunteeringFactory.CreateCharityEvent(charityEventDto);
            dbContext.Volunteerings.Add(volunteering);
            
            charityEvent.VolunteeringIdVolunteeringNavigation = volunteering;
            charityEvent.VolunteeringIdVolunteering = volunteering.IdVolunteering;
            dbContext.SaveChanges();
        }

        public void AddCharityEventFundraising(CharityEventDto charityEventDto, Charityevent charityEvent)
        {
            CharityEventFundraisingFactory charityEventFundraisingFactory = new CharityEventFundraisingFactory();
            Charityfundraising charityfundraising = charityEventFundraisingFactory.CreateCharityEvent(charityEventDto);
            dbContext.Charityfundraisings.Add(charityfundraising);
            charityEvent.CharityFundraisingIdCharityFundraisingNavigation = charityfundraising;
            charityEvent.CharityFundraisingIdCharityFundraising = charityfundraising.IdCharityFundraising;
            dbContext.SaveChanges();
        }
        public void AddLocation(AddLocationDto locationDto)
        {
            CharityEventVolunteeringFactory charityEventVolunteeringFactory = new CharityEventVolunteeringFactory();
            var vol = dbContext.Volunteerings.FirstOrDefault(v => v.IdVolunteering == locationDto.idVolunteering);
            if(vol == null)
            {
                throw new BadRequestException("Volunteering ID doesnt exist");
            }
            var newLocation = charityEventVolunteeringFactory.newLocation(locationDto, vol);
            vol.LocationIdLocations.Add(newLocation);
            dbContext.Locations.Add(newLocation);
            dbContext.SaveChanges();
        }

    }
}
