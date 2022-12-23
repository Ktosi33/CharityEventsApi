namespace CharityEventsApi.Models.DataTransferObjects
{
    public class AddDonationDto
    {
        public decimal AmountOfDonation { get; set; }
        public string? Description { get; set; }
        public int? IdUser { get; set; }
        public int IdCharityFundraising { get; set; }
    }
}
