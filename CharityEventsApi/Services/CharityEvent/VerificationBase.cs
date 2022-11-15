using CharityEventsApi.Exceptions;

namespace CharityEventsApi.Services.CharityEvent
{
    public abstract class VerificationBase
    {
        public void SetVerify(int id, bool isVerified)
        {
            if (isVerified)
            {
                verify(id);
            }
            else if (!isVerified)
            {
                unverify(id);
            }
            else
            {
                throw new BadRequestException("Bad query");
            }
        }
        protected abstract void verify(int id);
        protected abstract void unverify(int id);
    }
}
