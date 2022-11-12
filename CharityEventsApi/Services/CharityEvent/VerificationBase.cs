using CharityEventsApi.Exceptions;

namespace CharityEventsApi.Services.CharityEvent
{
    public abstract class VerificationBase
    {
        public void SetVerify(int Id, bool isVerified)
        {
            if (isVerified)
            {
                verify(Id);
            }
            else if (!isVerified)
            {
                unverify(Id);
            }
            else
            {
                throw new BadRequestException("Bad query");
            }
        }
        protected abstract void verify(int Id);
        protected abstract void unverify(int Id);
    }
}
