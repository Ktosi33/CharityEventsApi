using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.VolunteerService
{
    public interface IVolunteerService
    {
        public void addVolunteer(AddVolunteerDto addVolunteerDto);
        public void deleteVolunteer(DeleteVolunteerDto deleteVolunteerDto);
        public List<GetVolunteerDto> getVolunteersByVolunteeringId(int volunteeringId);
    }
}
