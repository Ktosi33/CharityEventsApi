using CharityEventsApi.Exceptions;

namespace CharityEventsApi.Services.CharityEventService
{
    public abstract class ActivationBase 
    {
        public void SetActive(int id, bool isActive)
        {
            if (isActive)
            {
                Active(id);
            }
            else if (!isActive)
            {
                Disactive(id);
            }
            else
            {
                throw new BadRequestException("Bad query");
            }
        }
        protected abstract void Disactive(int id);
        protected abstract void Active(int id);
    }
}
