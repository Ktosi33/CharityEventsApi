namespace CharityEventsApi.Exceptions
{
    public class BadRequestException : Exception
    {
        public const int StatusCode = 400;
        public BadRequestException(string message) : base(message)
        {

        }
        public BadRequestException() : base("Bad request exception, status code 400.") { }
    }
}
