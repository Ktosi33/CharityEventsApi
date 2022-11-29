using System.Text.Json.Serialization;

namespace CharityEventsApi.Models.DataTransferObjects
{
    public class AddCharityEventFundraisingDto
    {
        public int CharityEventId { get; set; }
        public string FundTarget { get; set; } = null!;
        public decimal AmountOfMoneyToCollect { get; set; }
    }
}
