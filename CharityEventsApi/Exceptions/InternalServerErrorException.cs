namespace CharityEventsApi.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        public const int StatusCode = 500;
        public InternalServerErrorException(string message) : base(message)
        {

        }
        public InternalServerErrorException() : base("Internal error, status code 500") 
        {
        
        }
    }
}
