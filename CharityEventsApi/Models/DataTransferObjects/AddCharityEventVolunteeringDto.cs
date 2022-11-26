namespace CharityEventsApi.Models.DataTransferObjects
{
    public class AddCharityEventVolunteeringDto
    {
        public int CharityEventId { get; set; }
        public int AmountOfNeededVolunteers { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
