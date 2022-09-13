namespace CharityEventsApi.Models.DataTransferObjects
{
    public class AddLocationDto
    {
        public int idVolunteering { get; set; }
        public string PostalCode { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
    }
}
