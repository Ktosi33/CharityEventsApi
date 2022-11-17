using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.LocationService
{
    public interface ILocationService
    {
        public void addLocation(AddLocationDto addLocationDto);

        public void editLocation(EditLocationDto editLocationDto, int locationId);

        public void deleteLocation(int locationId);

        public GetLocationDto getLocationById(int locationId);
    }
}
