using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.ImageService;
using CharityEventsApi.Services.AuthUserService;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.CharityEventService
{
    public class CharityEventService : ICharityEventService
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly ICharityEventFactoryFacade charityEventFactoryFacade;
        private readonly CharityEventVerification charityEventVerification;
        private readonly CharityEventActivation charityEventActivation;
        private readonly IImageService imageService;
        private readonly CharityEventDenial charityEventDenial;

        public CharityEventService(CharityEventsDbContext dbContext, ICharityEventFactoryFacade charityEventFactoryFacade, 
            CharityEventVerification charityEventVerification, CharityEventActivation charityEventActivation,
            IImageService imageService, CharityEventDenial charityEventDenial)
        {
            this.dbContext = dbContext;
            this.charityEventFactoryFacade = charityEventFactoryFacade;
            this.charityEventVerification = charityEventVerification;
            this.charityEventActivation = charityEventActivation;
            this.imageService = imageService;
            this.charityEventDenial = charityEventDenial;
        }

        public async Task AddAllCharityEvents(AddAllCharityEventsDto charityEventDto)
        {
          await charityEventFactoryFacade.AddCharityEvent(charityEventDto);
        }
        
        public async Task AddOneImage(IFormFile image, int idCharityEvent)
        {
            using var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
            var ce = GetCharityEventByCharityEventId(idCharityEvent);


            int imageId = await imageService.SaveImageAsync(image);
            var img = await dbContext.Images.FirstOrDefaultAsync(i => i.IdImage == imageId);
            if (img is null)
            {
                throw new BadRequestException("something went wrong");
            }
            ce.IdImages.Add(img);
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        public async Task<IEnumerable<ImageDto>> GetImagesAsync(int idCharityEvent)
        {
            var ce = await dbContext.CharityEvents.Include(c => c.IdImages).FirstOrDefaultAsync(c => c.IdCharityEvent == idCharityEvent);
            if (ce is null)
            {
                throw new NotFoundException("Charity event with given id doesn't exist");
            }
            IEnumerable<ImageDto> imageDtos = await imageService.GetImagesDtoByImages(ce.IdImages);

            return imageDtos;
        }
        public async Task DeleteImage(int idImage, int idCharityEvent)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            var ce = GetCharityEventByCharityEventId(idCharityEvent);
     

            var image = await dbContext.Images.FirstOrDefaultAsync(i => i.IdImage == idImage);
            if (image is null)
            {
                throw new NotFoundException("Image with given id doesn't exist");
            }

            ce.IdImages.Remove(image);
            await imageService.DeleteImageByObjectAsync(image);

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
       
        public IEnumerable<GetCharityEventDto> GetAllCharityEventDto()
        {
            var charityevents = dbContext.CharityEvents.Select(ce => new GetCharityEventDto
                { 
                Id = ce.IdCharityEvent,
                IsActive = ce.IsActive,
                Description = ce.Description,
                IdCharityFundraising = ce.IdCharityFundraising,
                IsVerified = ce.IsVerified,
                Title = ce.Title,
                IdCharityVolunteering = ce.IdCharityVolunteering
             }
            );

            return charityevents;
        }

        public void Edit(EditCharityEventDto charityEventDto, int idCharityEvent)
        {
            var charityevent = GetCharityEventByCharityEventId(idCharityEvent);
            if(charityEventDto.Title != null)
            {
                charityevent.Title = charityEventDto.Title;
            }
            if(charityEventDto.Description != null)
            { 
            charityevent.Description = charityEventDto.Description;
            }


            dbContext.SaveChanges();
        }
        public CharityEvent GetCharityEventByCharityEventId(int idCharityEvent)
        {
            var charityevent = dbContext.CharityEvents.FirstOrDefault(ce => ce.IdCharityEvent == idCharityEvent);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }

            return charityevent;
        }
        public CharityEvent GetCharityEventByFundraisingId(int idFundraising)
        {
            CharityEvent? charityevent = dbContext.CharityEvents.FirstOrDefault(ce => ce.IdCharityFundraising == idFundraising);

            if (charityevent is null)
            {
                throw new InternalServerErrorException();
            }

            return charityevent;
        }
        public CharityEvent GetCharityEventByVolunteeringId(int idVolunteering)
        {
            CharityEvent? charityevent = dbContext.CharityEvents.FirstOrDefault(ce => ce.IdCharityVolunteering == idVolunteering);

            if (charityevent is null)
            {
                throw new InternalServerErrorException();
            }

            return charityevent;
        }
        public async Task ChangeImage(IFormFile image, int idCharityEvent)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            var ce = GetCharityEventByCharityEventId(idCharityEvent);
            int oldImageId = ce.IdImage;
            ce.IdImage = await imageService.SaveImageAsync(image);

            await imageService.DeleteImageByIdAsync(oldImageId);

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public void SetActive(int idCharityEvent, bool isActive)
        {
            charityEventActivation.SetValue(idCharityEvent, isActive);
        }
     
        public void SetVerify(int idCharityEvent, bool isVerified)
        {
            charityEventVerification.SetValue(idCharityEvent, isVerified);
        }
        public void SetDeny(int idCharityEvent, bool isDenied)
        {
            charityEventDenial.SetValue(idCharityEvent, isDenied);
        }
        public GetCharityEventDto GetCharityEventDtoById(int id)
        {
            var c = GetCharityEventByCharityEventId(id);

            return new GetCharityEventDto {
                Id = c.IdCharityEvent,
                Description = c.Description,
                IsActive = c.IsActive,
                Title = c.Title,
                IdCharityVolunteering = c?.IdCharityVolunteering,
                IdCharityFundraising = c?.IdCharityFundraising,
                IsVerified = c!.IsVerified
            };
        }

      
    }
}
