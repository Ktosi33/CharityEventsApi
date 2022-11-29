namespace CharityEventsApi.Models.DataTransferObjects
{
    public class LoginUserDto
    {
        public string LoginOrEmail { get; set; } = null!;
        public string Password { get; set; } = null!;
                
    }
}
