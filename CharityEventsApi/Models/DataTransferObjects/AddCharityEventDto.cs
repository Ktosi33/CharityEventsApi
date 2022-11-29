namespace CharityEventsApi.Models.DataTransferObjects
{
    public class AddCharityEventDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int OrganizerId { get; set; }
        public IFormFile Image { get; set; } = null!;
        public List<IFormFile>? Images { get; set; }
    }
}
