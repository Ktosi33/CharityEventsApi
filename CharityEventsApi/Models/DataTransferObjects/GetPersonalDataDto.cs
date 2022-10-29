namespace CharityEventsApi.Models.DataTransferObjects
{
    public class GetPersonalDataDto
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int AddressIdAddress { get; set; }
    }
}
