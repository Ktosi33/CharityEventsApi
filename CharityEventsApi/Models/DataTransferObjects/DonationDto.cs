namespace CharityEventsApi.Models.DataTransferObjects
{
    public class DonationDto
    {
        public int IdDonations { get; set; }
        public decimal AmountOfDonation { get; set; }
        public DateTime DonationDate { get; set; }
        public string? Description { get; set; }
        public int CharityFundraisingIdCharityFundraising { get; set; }
    }
}
