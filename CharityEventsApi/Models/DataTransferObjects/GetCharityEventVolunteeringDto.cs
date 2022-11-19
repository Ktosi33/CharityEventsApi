namespace CharityEventsApi.Models.DataTransferObjects
{
    public class GetCharityEventVolunteeringDto
    {
        public int Id { get; set; }
        public int AmountOfNeededVolunteers { get; set; }
        public DateTime CreatedEventDate { get; set; }
        public DateTime? EndEventDate { get; set; }
        public sbyte IsActive { get; set; }
        public sbyte isVerified { get; set; }

    }
}
