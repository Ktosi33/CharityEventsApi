using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using Newtonsoft.Json.Bson;

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
            var volunteering = dbContext.Volunteerings.FirstOrDefault(v => v.IdVolunteering == locationDto.idVolunteering);
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

    }
}
