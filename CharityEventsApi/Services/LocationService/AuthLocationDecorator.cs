using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Services.UserAuthService;
using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.LocationService
{
    public class AuthLocationDecorator
    {
        private readonly IUserAuthService userAuthService;
        private readonly CharityEventsDbContext dbContext;
        private readonly ICharityEventService charityEventService;

        public AuthLocationDecorator(CharityEventsDbContext dbContext, IUserAuthService userAuthService, ICharityEventService charityEventService )
        {
            this.userAuthService = userAuthService;
            this.dbContext = dbContext;
            this.charityEventService = charityEventService;
        }

        public void AuthorizeUserIdIfRoleWithLocationId(int idLocation, string role)
        {

            var volunteering = getLocation(idLocation).VolunteeringIdVolunteerings.FirstOrDefault();

            var charityevent = charityEventService.getCharityEventByVolunteeringId(volunteering.IdVolunteering);

            userAuthService.AuthorizeUserIdIfRole(charityevent.OrganizerId, role);
        }

        private Location getLocation(int idLocation)
        {
            var location = dbContext.Locations.Include(v => v.VolunteeringIdVolunteerings).SingleOrDefault(l => l.IdLocation == idLocation);
            if (location is null)
            {
                throw new NotFoundException("Location with given id doesn't exist");
            }
            return location;
        }
    }
}

