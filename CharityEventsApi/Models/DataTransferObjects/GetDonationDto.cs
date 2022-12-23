namespace CharityEventsApi.Models.DataTransferObjects
{
    public class GetDonationDto
    {
        public int IdCharityFundraisingNavigation { get; set; }
        public int? IdDonation { get; set; }
        public decimal AmountOfDonation { get; set; }
        public DateTime DonationDate { get; set; }
        public string? Description { get; set; }
        public int? IdUser { get; set; }
        public GetUserDto? User { get; set; } 
        
    }
}
