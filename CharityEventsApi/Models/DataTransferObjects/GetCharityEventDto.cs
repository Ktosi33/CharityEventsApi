namespace CharityEventsApi.Models.DataTransferObjects
{
    public class GetCharityEventDto
    {
        public int Id { get; set; }
        public sbyte IsActive { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int? IdCharityVolunteering { get; set; }
        public int? IdCharityFundraising { get; set; }
        public sbyte IsVerified { get; set; }


    }
}
