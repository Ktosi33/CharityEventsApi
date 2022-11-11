namespace CharityEventsApi.Exceptions
{
    public class ForbiddenException : Exception
    {
        public const int StatusCode = 403;
        public ForbiddenException(string message) : base(message)
        {

        }
        public ForbiddenException() : base("Forbidden exception, status code 403.") { }
    }
}
