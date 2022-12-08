using CharityEventsApi.Exceptions;

namespace CharityEventsApi.Services.CharityEventService
{
    public abstract class BooleanCharityEventQueryBase 
    {
        public void SetValue(int id, bool hasValue)
        {
            if (hasValue)
            {
                setTrue(id);
            }
            else if (!hasValue)
            {
                setFalse(id);
            }
            else
            {
                throw new BadRequestException("Bad query");
            }
        }
        protected abstract void setTrue(int id);
        protected abstract void setFalse(int id);
        
    }
}
