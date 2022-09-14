namespace CharityEventsApi.Models.DataTransferObjects
{
    public class EditLocationDto
    {
        public int LocationId { get; set; }
        public string PostalCode { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
    }
}
