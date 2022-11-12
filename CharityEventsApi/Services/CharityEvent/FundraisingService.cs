using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
namespace CharityEventsApi.Services.CharityEvent
{
    public class FundraisingService : IFundraisingService
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly FundraisingFactory charityEventFundraisingFactory;

        public FundraisingService(CharityEventsDbContext dbContext, FundraisingFactory charityEventFundraisingFactory)
        {
            this.dbContext = dbContext;
            this.charityEventFundraisingFactory = charityEventFundraisingFactory;
        }

        public void Add(AddCharityEventFundraisingDto dto, int charityEventId)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(f => f.IdCharityEvent == charityEventId);
            if(charityevent is null)
            {
                throw new NotFoundException("Charity event with given id doesn't exist");
            }
            Charityfundraising cf =  charityEventFundraisingFactory.CreateCharityEvent(dto);
            dbContext.Charityfundraisings.Add(cf);
            dbContext.SaveChanges();

        }
        public void Edit(EditCharityEventFundraisingDto FundraisingDto, int FundraisingId)
        {
            var charityevent = dbContext.Charityfundraisings.FirstOrDefault(f => f.IdCharityFundraising == FundraisingId);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEventFundraising with given id doesn't exist");
            }
            if (FundraisingDto.AmountOfMoneyToCollect != null)
            {
                charityevent.AmountOfMoneyToCollect = (decimal)FundraisingDto.AmountOfMoneyToCollect;
            }
            charityevent.FundTarget = FundraisingDto.FundTarget;
            dbContext.SaveChanges();
        }
        public void SetActive(int FundraisingId, bool isActive)
        {
            if (isActive)
            {
                active(FundraisingId);
            }
            else if (!isActive)
            {
                disactive(FundraisingId);
            }
            else
            {
                throw new BadRequestException("Bad query");
            }
        }
        public void SetVerify(int FundraisingId, bool isVerified) 
        {
            if (isVerified)
            {
                verify(FundraisingId); //TODO: Verify can only admin
            }
            else if (!isVerified)
            {
                unverify(FundraisingId);
            }
            else
            {
                throw new BadRequestException("Bad query");
            }
        }
        private void active(int FundraisingId)
        {
            var fundraising = dbContext.Charityfundraisings.Include(ce => ce.Charityevents).FirstOrDefault(f => f.IdCharityFundraising == FundraisingId);
            if (fundraising == null)
            {
                throw new NotFoundException("CharityEventFundraising with given id doesn't exist");
            }
            var charityevent = fundraising.Charityevents.FirstOrDefault();
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEventFundraising doesn't have charity event.");
            }
           
            if(charityevent.IsActive == 0 || charityevent.IsVerified == 0 || fundraising.IsVerified == 0)
            {
                throw new BadRequestException("You cant active fundraising while charity event isn't active or verified");
            }
            fundraising.IsActive = 1;
            dbContext.SaveChanges();
        }
        private void verify(int FundraisingId)
        {
            var fundraising = dbContext.Charityfundraisings.Include(ce => ce.Charityevents).FirstOrDefault(f => f.IdCharityFundraising == FundraisingId);
            if (fundraising == null)
            {
                throw new NotFoundException("CharityEventFundraising with given id doesn't exist");
            }
            var charityevent = fundraising.Charityevents.FirstOrDefault();
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEventFundraising doesn't have charity event.");
            }
            if (charityevent.IsVerified == 0 )
            {
                throw new BadRequestException("Firstly verify charityevent");
            }
            fundraising.IsVerified = 1;
            dbContext.SaveChanges();
        }
        private void unverify(int FundraisingId)
        {
            var fundraising = dbContext.Charityfundraisings.Include(ce => ce.Charityevents).FirstOrDefault(f => f.IdCharityFundraising == FundraisingId);
            if (fundraising == null)
            {
                throw new NotFoundException("CharityEventFundraising with given id doesn't exist");
            }
            disactive(FundraisingId);
            fundraising.IsVerified = 0;
            dbContext.SaveChanges();
        }
        private void disactive(int FundraisingId)
        {
            var fundraising = dbContext.Charityfundraisings.Include(ce => ce.Charityevents).FirstOrDefault(f => f.IdCharityFundraising == FundraisingId);
            if (fundraising == null)
            {
                throw new NotFoundException("CharityEventFundraising with given id doesn't exist");
            }
            fundraising.EndEventDate = DateTime.Now;
            fundraising.IsActive = 0;
            var charityevent = fundraising.Charityevents.FirstOrDefault();
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEventFundraising dont have charity event.");
            }

            if (charityevent.VolunteeringIdVolunteering == null)
            {
                charityevent.IsActive = 0;
            }
            else
            {
                var cv = dbContext.Volunteerings.FirstOrDefault(cv => cv.IdVolunteering == charityevent.VolunteeringIdVolunteering);
                if (cv != null)
                {
                    if (cv.EndEventDate != null)
                    {
                        charityevent.IsActive = 0;
                    }
                }
            }

          
            dbContext.SaveChanges();
        }
        public GetCharityFundrasingDto GetById(int id)
        {
            var c = dbContext.Charityfundraisings.FirstOrDefault(c => c.IdCharityFundraising == id);
            if (c is null)
            {
                throw new NotFoundException("Given id doesn't exist");
            }


            return new GetCharityFundrasingDto
            {
                AmountOfAlreadyCollectedMoney = c.AmountOfAlreadyCollectedMoney,
                AmountOfMoneyToCollect = c.AmountOfMoneyToCollect,
                CreatedEventDate = c.CreatedEventDate,
                EndEventDate = c.EndEventDate,
                FundTarget = c.FundTarget,
                IsActive = c.IsActive,
                isVerified = c.IsVerified
            };
        }
        
    }
}
