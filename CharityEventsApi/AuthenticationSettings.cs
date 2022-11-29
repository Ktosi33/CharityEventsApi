namespace CharityEventsApi
{
    public class AuthenticationSettings
    {
        public string JwtKey { get; set; } = null!;
        public string JwtExpireDays { get; set; } = null!;
        public string JwtIssuer { get; set; } = null!;
    }
}
