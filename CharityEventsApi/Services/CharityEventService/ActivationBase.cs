using CharityEventsApi.Exceptions;

namespace CharityEventsApi.Services.CharityEventService
{
    public abstract class ActivationBase 
    {
        public void SetActive(int id, bool isActive)
        {
            if (isActive)
            {
                active(id);
            }
            else if (!isActive)
            {
                disactive(id);
            }
            else
            {
                throw new BadRequestException("Bad query");
            }
        }
        protected abstract void disactive(int id);
        protected abstract void active(int id);
    }
}
