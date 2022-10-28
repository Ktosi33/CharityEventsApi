
namespace CharityEventsApi.Models.DataTransferObjects
{
    public class VolunteeringDto
    {
        public int IdVolunteering { get; set; }
        public int AmountOfNeededVolunteers { get; set; }
        public DateTime CreatedEventDate { get; set; }
        public DateTime? EndEventDate { get; set; }
        public sbyte IsActive { get; set; }

    }
}
