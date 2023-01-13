namespace CharityEventsApi.Models.DataTransferObjects
{
    public class GetAllDetailsCharityEventDto
    {
        public int IdCharityEvent { get; set; }
        public sbyte IsActive { get; set; }
        public sbyte IsVerified { get; set; }
        public sbyte IsDenied { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public ImageDto? imageDto { get; set; }
        public int? IdCharityVolunteering { get; set; }
        public GetCharityEventVolunteeringDto? CharityEventVolunteering { get; set; }  
        public int? IdCharityFundraising { get; set; }
        public GetCharityFundraisingDto? CharityEventFundraising { get; set; }

    }
}
