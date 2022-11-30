namespace CharityEventsApi.Models.DataTransferObjects
{
    public class EditLocationDto
    {
        public string PostalCode { get; set; } = null!;
        public string Town { get; set; } = null!;
        public string Street { get; set; } = null!;
    }
}
