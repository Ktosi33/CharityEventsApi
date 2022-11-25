using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.VolunteerService
{
    public interface IVolunteerService
    {
        public void addVolunteer(AddVolunteerDto addVolunteerDto);
    }
}
