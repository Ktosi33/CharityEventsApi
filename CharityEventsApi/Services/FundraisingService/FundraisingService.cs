using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.CharityEventService;
using Microsoft.EntityFrameworkCore;
namespace CharityEventsApi.Services.FundraisingService
{
    public class FundraisingService : IFundraisingService
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly FundraisingFactory charityEventFundraisingFactory;
        private readonly FundraisingVerification fundraisingVerification;
        private readonly FundraisingActivation fundraisingActivation;
        private readonly CharityEventVerification charityEventVerification;

        public FundraisingService(CharityEventsDbContext dbContext, FundraisingFactory charityEventFundraisingFactory, 
            FundraisingVerification fundraisingVerification, FundraisingActivation fundraisingActivation, CharityEventVerification charityEventVerification)
        {
            this.dbContext = dbContext;
            this.charityEventFundraisingFactory = charityEventFundraisingFactory;
            this.fundraisingVerification = fundraisingVerification;
            this.fundraisingActivation = fundraisingActivation;
            this.charityEventVerification = charityEventVerification;
        }

        public void Add(AddCharityEventFundraisingDto dto, int charityEventId)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(f => f.IdCharityEvent == charityEventId);
            if(charityevent is null)
            {
                throw new NotFoundException("Charity event with given id doesn't exist");
            }
            if (charityevent.CharityFundraisingIdCharityFundraising is not null)
            {
                throw new BadRequestException("Can't add charity event fundraising, because another one already exists in this charity event");
            }
            Charityfundraising cf =  charityEventFundraisingFactory.CreateCharityEvent(dto);
            dbContext.Charityfundraisings.Add(cf);
            charityEventVerification.SetVerify(charityEventId, false);
            charityevent.CharityFundraisingIdCharityFundraising = cf.IdCharityFundraising;
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
            fundraisingActivation.SetActive(FundraisingId, isActive);
        }
        public void SetVerify(int FundraisingId, bool isVerified) 
        {
            fundraisingVerification.SetVerify(FundraisingId, isVerified);
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
                Id = c.IdCharityFundraising,
                AmountOfAlreadyCollectedMoney = c.AmountOfAlreadyCollectedMoney,
                AmountOfMoneyToCollect = c.AmountOfMoneyToCollect,
                CreatedEventDate = c.CreatedEventDate,
                EndEventDate = c.EndEventDate,
                FundTarget = c.FundTarget,
                IsActive = c.IsActive,
                isVerified = c.IsVerified
            };
        }
        public IEnumerable<GetCharityFundrasingDto> GetAll()
        {
            var fundraisings = dbContext.Charityfundraisings.Select(c => new GetCharityFundrasingDto
            {
                Id = c.IdCharityFundraising,
                AmountOfAlreadyCollectedMoney = c.AmountOfAlreadyCollectedMoney,
                AmountOfMoneyToCollect = c.AmountOfMoneyToCollect,
                CreatedEventDate = c.CreatedEventDate,
                EndEventDate = c.EndEventDate,
                FundTarget = c.FundTarget,
                IsActive = c.IsActive,
                isVerified = c.IsVerified
            }
            );
            if (fundraisings == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }
            return fundraisings;
        }

    }
}
