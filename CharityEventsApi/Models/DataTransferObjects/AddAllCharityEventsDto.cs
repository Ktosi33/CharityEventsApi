namespace CharityEventsApi.Models.DataTransferObjects
{
    public class AddAllCharityEventsDto
    {
        public bool IsVolunteering { get; set; }
        public bool IsFundraising { get; set; }
        //CharityEvent
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int IdOrganizer { get; set; }
        public IFormFile ImageCharityEvent { get; set; } = null!;
        public List<IFormFile>? ImagesCharityEvent { get; set; }
        //CharityEventFundraising
        public string? FundTarget { get; set; } = null!;
        public decimal? AmountOfMoneyToCollect { get; set; }
        //CharityEventVolunteering
        public int? AmountOfNeededVolunteers { get; set; }
    }
}
