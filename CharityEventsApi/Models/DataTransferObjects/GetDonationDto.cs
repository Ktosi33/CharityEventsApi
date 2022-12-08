namespace CharityEventsApi.Models.DataTransferObjects
{
    public class GetDonationDto
    {
        public int CharityFundraisingIdCharityFundraising { get; set; }
        public int? DonationId { get; set; }
        public decimal AmountOfDonation { get; set; }
        public DateTime DonationDate { get; set; }
        public string? Description { get; set; }
        public int? UserIdUser { get; set; }
        public GetUserDto? User { get; set; } 
        
    }
}
