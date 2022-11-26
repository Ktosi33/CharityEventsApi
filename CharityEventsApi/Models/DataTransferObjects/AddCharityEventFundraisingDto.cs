using System.Text.Json.Serialization;

namespace CharityEventsApi.Models.DataTransferObjects
{
    public class AddCharityEventFundraisingDto
    {
        public int CharityEventId { get; set; }
        public string FundTarget { get; set; }
        public decimal AmountOfMoneyToCollect { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
