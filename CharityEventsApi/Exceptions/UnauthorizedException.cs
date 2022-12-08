namespace CharityEventsApi.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public const int StatusCode = 401;
        public UnauthorizedException(string message) : base(message)
        {

        }
        public UnauthorizedException() : base("Unauthorized exception, status code 401.") { }
      
       
    }
}
