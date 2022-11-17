namespace CharityEventsApi.Models.DataTransferObjects
{
    public class AddDonationDto
    {
        public decimal AmountOfDonation { get; set; }
        public DateTime DonationDate { get; set; }
        public string? Description { get; set; }
        public int UserIdUser { get; set; }
        public int CharityFundraisingIdCharityFundraising { get; set; }
    }
}
