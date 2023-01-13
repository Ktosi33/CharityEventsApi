using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.VolunteeringService
{
    public class VolunteeringFactory 
    {

        public CharityVolunteering CreateCharityEvent(AddAllCharityEventsDto charityEventDto)
        {
            CharityVolunteering volunteering = new CharityVolunteering
            {
                AmountOfNeededVolunteers = charityEventDto.AmountOfNeededVolunteers != null ? (int)charityEventDto.AmountOfNeededVolunteers : 0, 
                CreatedEventDate = DateTime.Now,
                IsActive = 0,
                IsVerified = 0,
                IsDenied = 0
            };

            return volunteering;
        }

        public CharityVolunteering CreateCharityEvent(AddCharityEventVolunteeringDto charityEventDto)
        {
            CharityVolunteering volunteering = new CharityVolunteering
            {
                AmountOfNeededVolunteers = charityEventDto.AmountOfNeededVolunteers, 
                CreatedEventDate = DateTime.Now,
                IsActive = 0,
                IsVerified = 0,
                IsDenied = 0
            };

            return volunteering;
        }

    }
}