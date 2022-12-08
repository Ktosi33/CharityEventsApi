using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Services.UserContextAuthService;
using Microsoft.EntityFrameworkCore;
namespace CharityEventsApi.Services.FundraisingService
{
    public class FundraisingService : IFundraisingService
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly FundraisingVerification fundraisingVerification;
        private readonly FundraisingActivation fundraisingActivation;
        private readonly CharityEventVerification charityEventVerification;
        private readonly ICharityEventFactoryFacade charityEventFactoryFacade;
        private readonly IUserContextAuthService userContextService;

        public FundraisingService(CharityEventsDbContext dbContext, FundraisingVerification fundraisingVerification,
            FundraisingActivation fundraisingActivation, CharityEventVerification charityEventVerification,
            ICharityEventFactoryFacade charityEventFactoryFacade, IUserContextAuthService userContextService)
        {
            this.dbContext = dbContext;
            this.fundraisingVerification = fundraisingVerification;
            this.fundraisingActivation = fundraisingActivation;
            this.charityEventVerification = charityEventVerification;
            this.charityEventFactoryFacade = charityEventFactoryFacade;
            this.userContextService = userContextService;
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

            charityEventVerification.SetVerify(dto.CharityEventId, false);
        }

        public void Edit(EditCharityEventFundraisingDto FundraisingDto, int FundraisingId)
        {
            var fundraising = dbContext.Charityfundraisings.FirstOrDefault(f => f.IdCharityFundraising == FundraisingId);
            if (fundraising is null)
            {
                throw new NotFoundException("CharityEventFundraising with given id doesn't exist");
            }

            AuthorizeIfUserOrganizerOrAdmin(FundraisingId);

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

        public void SetActive(int FundraisingId, bool isActive)
        {
            AuthorizeIfUserOrganizerOrAdmin(FundraisingId);

            fundraisingActivation.SetActive(FundraisingId, isActive);
        }
        
        public void SetVerify(int FundraisingId, bool isVerified) 
        {
            userContextService.AuthorizeIfOnePass(null, "Admin"); 

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

            return fundraisings;
        }
        private void AuthorizeIfUserOrganizerOrAdmin(int fundraisingId)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(ce => ce.CharityFundraisingIdCharityFundraising == fundraisingId);
            if (charityevent is null)
            {
                throw new InternalServerErrorException();
            }

            userContextService.AuthorizeIfOnePass(charityevent.OrganizerId, "Admin");
        }

    }
}
