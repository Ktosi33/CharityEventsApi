namespace CharityEventsApi.Models.DataTransferObjects
{
    public class GetCharityEventDto
    {
        public sbyte IsActive { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int? VolunteeringId { get; set; }
        public int? FundraisingId { get; set; }


    }
}
