using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.CharityEvent
{
    public class CharityEventVolunteeringFactory 
    {

        public Volunteering CreateCharityEvent(AddAllCharityEventsDto charityEventDto)
        {
            Volunteering volunteering = new Volunteering
            {
                AmountOfNeededVolunteers = charityEventDto.AmountOfNeededVolunteers != null ? (int)charityEventDto.AmountOfNeededVolunteers : 0, //TODO: can make problems
                CreatedEventDate = DateTime.Now,
                IsActive = 1
            };

            return volunteering;
        }

        public Volunteering CreateCharityEvent(AddCharityEventVolunteeringDto charityEventDto)
        {
            Volunteering volunteering = new Volunteering
            {
                AmountOfNeededVolunteers = charityEventDto.AmountOfNeededVolunteers != null ? (int)charityEventDto.AmountOfNeededVolunteers : 0, //TODO: can make problems
                CreatedEventDate = DateTime.Now,
                IsActive = 1
            };

            return volunteering;
        }

        public Location newLocation(AddLocationDto locationDto, Volunteering volunteering)
        {
            Location location = new Location
            {
                PostalCode = locationDto.PostalCode,
                Street = locationDto.Street,
                Town = locationDto.Town
            };
            location.VolunteeringIdVolunteerings.Add(volunteering);
            return location;
        }
    }
}