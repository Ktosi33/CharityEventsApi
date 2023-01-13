namespace CharityEventsApi.Models.DataTransferObjects
{
    public class EditCharityEventDto
    {
        public string? Title { get; set; } 
        public string? Description { get; set; }
        public int? IdOrganizer { get; set; }
    }
}
