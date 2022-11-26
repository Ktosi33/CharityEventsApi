using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Services.ImageService;
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
        private readonly ICharityEventFactoryFacade charityEventFactoryFacade;
        private readonly IImageService imageService;

        public FundraisingService(CharityEventsDbContext dbContext, FundraisingFactory charityEventFundraisingFactory, 
            FundraisingVerification fundraisingVerification, FundraisingActivation fundraisingActivation,
            CharityEventVerification charityEventVerification, ICharityEventFactoryFacade charityEventFactoryFacade,
            IImageService imageService)
        {
            this.dbContext = dbContext;
            this.charityEventFundraisingFactory = charityEventFundraisingFactory;
            this.fundraisingVerification = fundraisingVerification;
            this.fundraisingActivation = fundraisingActivation;
            this.charityEventVerification = charityEventVerification;
            this.charityEventFactoryFacade = charityEventFactoryFacade;
            this.imageService = imageService;
        }

        public async Task Add(AddCharityEventFundraisingDto dto)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(f => f.IdCharityEvent == dto.CharityEventId);
            if (charityevent is null)
            {
                throw new NotFoundException("Charity event with given id doesn't exist");
            }
            if (charityevent.CharityFundraisingIdCharityFundraising is not null)
            {
                throw new BadRequestException("Can't add charity event fundraising, because another one already exists in this charity event");
            }

            await charityEventFactoryFacade.AddCharityEventFundraising(dto, charityevent);
            charityEventVerification.SetVerify(dto.CharityEventId, false);
        }
        public async Task AddOneImage(IFormFile image, int fundraisingId)
        {
            using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                var cf = await dbContext.Charityfundraisings.FirstOrDefaultAsync(f => f.IdCharityFundraising == fundraisingId);
                if (cf is null)
                {
                    throw new NotFoundException("Charity event with given id doesn't exist");
                }
                int imageId = await imageService.SaveImageAsync(image);
                var img = await dbContext.Images.FirstOrDefaultAsync(i => i.IdImages == imageId);
                if (img is null)
                {
                    throw new BadRequestException("something went wrong");
                }
                cf.ImageIdImages.Add(img);
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }
        public async Task DeleteImage(int idImage, int idFundraising)
        {
            using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                var cf = await dbContext.Charityfundraisings.FirstOrDefaultAsync(f => f.IdCharityFundraising == idFundraising);
                if(cf is null)
                {
                    throw new NotFoundException("CharityEventFundraising with given id doesn't exist");
                }
                var image = await dbContext.Images.FirstOrDefaultAsync(i => i.IdImages == idImage);
                if(image is null)
                {
                    throw new NotFoundException("Image with given id doesn't exist");
                }

                cf.ImageIdImages.Remove(image);
                await imageService.DeleteImageByObjectAsync(image);

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
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
