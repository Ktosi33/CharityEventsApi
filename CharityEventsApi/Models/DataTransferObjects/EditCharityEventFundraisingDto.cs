namespace CharityEventsApi.Models.DataTransferObjects
{
    public class EditCharityEventFundraisingDto
    {
        public int FundraisingId { get; set; }
        public string? FundTarget { get; set; } = null!;
        public decimal? AmountOfMoneyToCollect { get; set; }
    }
}
