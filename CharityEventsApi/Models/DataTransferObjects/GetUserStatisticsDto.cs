namespace CharityEventsApi.Models.DataTransferObjects
{
    public class GetUserStatisticsDto
    {
        public int NumberDonations { get; set; }
        public decimal TotalValueDonations { get; set; }
        public int NumberActionsAsVolunteer { get; set; }
        public int NumberVolunteeringsAsOrganizer { get; set; }
        public int NumberFundrasingsAsOrganizer { get; set; }

    }
}
