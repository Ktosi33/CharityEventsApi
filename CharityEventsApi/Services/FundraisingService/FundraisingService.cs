using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.CharityEventService;

namespace CharityEventsApi.Services.FundraisingService
{
    public class FundraisingService : IFundraisingService
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly FundraisingVerification fundraisingVerification;
        private readonly FundraisingActivation fundraisingActivation;
        private readonly CharityEventVerification charityEventVerification;
        private readonly ICharityEventFactoryFacade charityEventFactoryFacade;
        private readonly FundraisingDenial fundraisingDenial;

        public FundraisingService(CharityEventsDbContext dbContext, FundraisingVerification fundraisingVerification,
            FundraisingActivation fundraisingActivation, CharityEventVerification charityEventVerification,
            ICharityEventFactoryFacade charityEventFactoryFacade, FundraisingDenial fundraisingDenial)
        {
            this.dbContext = dbContext;
            this.fundraisingVerification = fundraisingVerification;
            this.fundraisingActivation = fundraisingActivation;
            this.charityEventVerification = charityEventVerification;
            this.charityEventFactoryFacade = charityEventFactoryFacade;
            this.fundraisingDenial = fundraisingDenial;
        }

        public async Task Add(AddCharityEventFundraisingDto dto)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(f => f.IdCharityEvent == dto.CharityEventId);

            if (charityevent is null) {
                throw new NotFoundException("Charity event with given id doesn't exist");
            }

            if (charityevent.CharityFundraisingIdCharityFundraising is not null) {
                throw new BadRequestException("Can't add charity event fundraising, because another one already exists in this charity event");
            }

            await charityEventFactoryFacade.AddCharityEventFundraising(dto, charityevent);

            charityEventVerification.SetValue(dto.CharityEventId, false);
        }

        public void Edit(EditCharityEventFundraisingDto FundraisingDto, int idFundraising)
        {
            var fundraising = getFundraisingByFundraisingId(idFundraising);

            if (FundraisingDto.AmountOfMoneyToCollect is not null)
            {
                fundraising.AmountOfMoneyToCollect = (decimal)FundraisingDto.AmountOfMoneyToCollect;
            }
            if(FundraisingDto.FundTarget is not null)
            { 
            fundraising.FundTarget = FundraisingDto.FundTarget;
            }
            dbContext.SaveChanges();
        }

        public void SetActive(int idFundraising, bool isActive)
        {
            fundraisingActivation.SetValue(idFundraising, isActive);
        }
        
        public void SetVerify(int idFundraising, bool isVerified) 
        {
            fundraisingVerification.SetValue(idFundraising, isVerified);
        }
        public void SetDeny(int idFundraising, bool isDenied)
        {
            fundraisingDenial.SetValue(idFundraising, isDenied);
        }

        public GetCharityFundraisingDto GetById(int idFundraising)
        {
            var c = getFundraisingByFundraisingId(idFundraising);


            return new GetCharityFundraisingDto
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
        public IEnumerable<GetCharityFundraisingDto> GetAll()
        {
            var fundraisings = dbContext.Charityfundraisings.Select(c => new GetCharityFundraisingDto
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

            return fundraisings;
        }
        private Charityfundraising getFundraisingByFundraisingId(int idfundraising)
        {
            Charityfundraising? fundraising = dbContext.Charityfundraisings.FirstOrDefault(cf => cf.IdCharityFundraising == idfundraising);
            
            if (fundraising == null)
            {
                throw new NotFoundException("Charity event fundraising with given id doesn't exist");
            }

            return fundraising;
        }

    }
}
