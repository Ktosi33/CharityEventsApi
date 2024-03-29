﻿using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Services.AuthUserService;
using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.LocationService
{
    public class AuthLocationDecorator
    {
        private readonly IAuthUserService authUserService;
        private readonly CharityEventsDbContext dbContext;
        private readonly ICharityEventService charityEventService;

        public AuthLocationDecorator(CharityEventsDbContext dbContext, IAuthUserService authUserService, ICharityEventService charityEventService )
        {
            this.authUserService = authUserService;
            this.dbContext = dbContext;
            this.charityEventService = charityEventService;
        }

        public void AuthorizeUserIdIfRoleWithLocationId(int idLocation, string role)
        {

            var volunteering = getLocation(idLocation).IdCharityVolunteerings.FirstOrDefault();
            if(volunteering is null)
            {
                throw new NotFoundException("Volunteering with given location doesnt exist");
            }
            var charityevent = charityEventService.GetCharityEventByVolunteeringId(volunteering.IdCharityVolunteering);

            authUserService.AuthorizeUserIdIfRole(charityevent.IdOrganizer, role);
        }

        private Location getLocation(int idLocation)
        {
            var location = dbContext.Locations.Include(v => v.IdCharityVolunteerings).SingleOrDefault(l => l.IdLocation == idLocation);
            if (location is null)
            {
                throw new NotFoundException("Location with given id doesn't exist");
            }
            return location;
        }
    }
}

