namespace CharityEventsApi.Models.DataTransferObjects
{
    public class GetUserDto
    {
        public int IdUser { get; set; }
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
