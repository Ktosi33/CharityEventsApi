namespace CharityEventsApi.Exceptions
{
    public class NotFoundException : Exception
    {
        public const int StatusCode = 404;
        public NotFoundException(string message) : base(message)
        {

        }
        public NotFoundException() : base("Not found exception, status code 404.") { }
    }
}
