namespace CharityEventsApi.Models.DataTransferObjects
{
    public class AddAllCharityEventsDto
    {
        public bool isVolunteering { get; set; }
        public bool isFundraising { get; set; }
        //CharityEventDto
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int OrganizerId { get; set; }
        public IFormFile ImageCharityEvent { get; set; }
        public List<IFormFile>? ImagesCharityEvent { get; set; }
        //CharityEventFundraisingDto
        public string? FundTarget { get; set; } = null!;
        public decimal? AmountOfMoneyToCollect { get; set; }
        //CharityEventVolunteeringDto
        public int? AmountOfNeededVolunteers { get; set; }
        
    }
}
