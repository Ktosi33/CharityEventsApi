using CharityEventsApi.Exceptions;

namespace CharityEventsApi.Services.CharityEvent
{
    public abstract class ActivationBase 
    {
        public void SetActive(int Id, bool isActive)
        {
            if (isActive)
            {
                active(Id);
            }
            else if (!isActive)
            {
                disactive(Id);
            }
            else
            {
                throw new BadRequestException("Bad query");
            }
        }
        protected abstract void disactive(int Id);
        protected abstract void active(int Id);
    }
}
