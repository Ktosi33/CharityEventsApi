namespace CharityEventsApi.Models.DataTransferObjects
{
    public class ImageDto
    {
        public int IdImages { get; set; }
        public string Content { get; set; } = null!;
        public string ContentType { get; set; } = null!;
    }
}
