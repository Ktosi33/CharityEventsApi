using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.VolunteeringService
{
    public class VolunteeringFactory 
    {

        public Volunteering CreateCharityEvent(AddAllCharityEventsDto charityEventDto)
        {
            Volunteering volunteering = new Volunteering
            {
                AmountOfNeededVolunteers = charityEventDto.AmountOfNeededVolunteers != null ? (int)charityEventDto.AmountOfNeededVolunteers : 0, 
                CreatedEventDate = DateTime.Now,
                IsActive = 0,
                IsVerified = 0
            };

            return volunteering;
        }

        public Volunteering CreateCharityEvent(AddCharityEventVolunteeringDto charityEventDto)
        {
            Volunteering volunteering = new Volunteering
            {
                AmountOfNeededVolunteers = charityEventDto.AmountOfNeededVolunteers, 
                CreatedEventDate = DateTime.Now,
                IsActive = 0,
                IsVerified = 0
            };

            return volunteering;
        }

    }
}