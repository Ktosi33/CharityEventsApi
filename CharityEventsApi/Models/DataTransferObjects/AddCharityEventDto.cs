namespace CharityEventsApi.Models.DataTransferObjects
{
    public class AddCharityEventDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int OrganizerId { get; set; }
    }
}
