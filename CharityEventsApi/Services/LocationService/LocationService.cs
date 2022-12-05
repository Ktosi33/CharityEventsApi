using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json.Bson;
using System.Linq;

namespace CharityEventsApi.Services.LocationService
{
    public class LocationService: ILocationService
    {
        private readonly CharityEventsDbContext dbContext;

        public LocationService(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void addLocation(AddLocationDto locationDto)
        {
            var volunteering = dbContext.Volunteerings.FirstOrDefault(v => v.IdVolunteering == locationDto.IdVolunteering);
            if (volunteering == null)
            {
                throw new BadRequestException("Volunteering ID doesnt exist");
            }

            var location = new Location
            {
                PostalCode = locationDto.PostalCode,
                Street = locationDto.Street,
                Town = locationDto.Town,      
            };

            location.VolunteeringIdVolunteerings.Add(volunteering);
            dbContext.Locations.Add(location);
            dbContext.SaveChanges();

        }

        public void editLocation(EditLocationDto locationDto, int locationId)
        {
            var location = dbContext.Locations.FirstOrDefault(l => l.IdLocation == locationId);
            if (location == null)
            {
                throw new NotFoundException("Location with given id doesn't exist");
            }

            location.Street = locationDto.Street;
            location.PostalCode = locationDto.PostalCode;
            location.Town = locationDto.Town;
            dbContext.SaveChanges();
        }

        public void deleteLocation(int locationId)
        {
            
            var location = dbContext.Locations.FirstOrDefault(l => l.IdLocation == locationId);

            if (location == null)
                throw new NotFoundException("Location with given id doesn't exist");

            dbContext.Locations.Remove(location);
            dbContext.SaveChanges();                 
        }

        public GetLocationDto getLocationById(int locationId)
        {
            var location = dbContext.Locations.FirstOrDefault(l => l.IdLocation == locationId);

            if (location is null)
                throw new NotFoundException("Location about this id does not exist");


            return new GetLocationDto
            {
                IdLocation = location.IdLocation,
                PostalCode = location.PostalCode,
                Street = location.Street,
                Town = location.Town
               
            };
        }

        public List<GetLocationDto> getLocationsByVolunteeringId(int volunteeringId)
        {
            var volunteering = dbContext.Volunteerings
                .Include(v => v.LocationIdLocations)
                .FirstOrDefault(v => v.IdVolunteering == volunteeringId);

            if (volunteering == null)
                throw new NotFoundException("Volunteering about this id does not exist");

            var locations = volunteering.LocationIdLocations;
            List<GetLocationDto> locationsList = new List<GetLocationDto>();

            foreach (var location in locations)
            {
                locationsList.Add(getLocationById(location.IdLocation));
            }

            return locationsList;
        }
    }
}
