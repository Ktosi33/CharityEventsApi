using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.ImageService;
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

        public CharityEventService(CharityEventsDbContext dbContext, ICharityEventFactoryFacade charityEventFactoryFacade, 
            CharityEventVerification charityEventVerification, CharityEventActivation charityEventActivation,
            IImageService imageService)
        {
            this.dbContext = dbContext;
            this.charityEventFactoryFacade = charityEventFactoryFacade;
            this.charityEventVerification = charityEventVerification;
            this.charityEventActivation = charityEventActivation;
            this.imageService = imageService;
        }

        public async Task Add(AddAllCharityEventsDto charityEventDto)
        {
          await charityEventFactoryFacade.AddCharityEvent(charityEventDto);
        }
        
        public async Task AddOneImage(IFormFile image, int idCharityEvent)
        {
            using var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
            var ce = await dbContext.Charityevents.FirstOrDefaultAsync(c => c.IdCharityEvent == idCharityEvent);
            if (ce is null)
            {
                throw new NotFoundException("Charity event with given id doesn't exist");
            }
            int imageId = await imageService.SaveImageAsync(image);
            var img = await dbContext.Images.FirstOrDefaultAsync(i => i.IdImages == imageId);
            if (img is null)
            {
                throw new BadRequestException("something went wrong");
            }
            // ce.ImageIdImages.Add(img);
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        public async Task<IEnumerable<ImageDto>> GetImagesAsync(int idCharityEvent)
        {
            var ce = await dbContext.Charityevents.Include(c => c.ImageIdImages1).FirstOrDefaultAsync(c => c.IdCharityEvent == idCharityEvent);
            if (ce is null)
            {
                throw new NotFoundException("Charity event with given id doesn't exist");
            }
            IEnumerable<ImageDto> imageDtos = await imageService.GetImagesDtoByImages(ce.ImageIdImages1);

            return imageDtos;
        }
        public async Task DeleteImage(int idImage, int idCharityEvent)
        {
            using var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
            var ce = await dbContext.Charityevents.FirstOrDefaultAsync(c => c.IdCharityEvent == idCharityEvent);
            if (ce is null)
            {
                throw new NotFoundException("Charity event with given id doesn't exist");
            }
            var image = await dbContext.Images.FirstOrDefaultAsync(i => i.IdImages == idImage);
            if (image is null)
            {
                throw new NotFoundException("Image with given id doesn't exist");
            }

            ce.ImageIdImages1.Remove(image);
            await imageService.DeleteImageByObjectAsync(image);

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
       
        public IEnumerable<GetCharityEventDto> GetAll()
        {
            var charityevents = dbContext.Charityevents.Select(ce => new GetCharityEventDto
                { 
                Id = ce.IdCharityEvent,
                IsActive = ce.IsActive,
                Description = ce.Description,
                FundraisingId = ce.CharityFundraisingIdCharityFundraising,
                IsVerified = ce.IsVerified,
                Title = ce.Title,
                VolunteeringId = ce.VolunteeringIdVolunteering
             }
            );
            if (charityevents == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }
            return charityevents;
        }

        public void Edit(EditCharityEventDto charityEventDto, int idCharityEvent)
        {
            var charityevent = getCharityEventFromDbById(idCharityEvent);
            if(charityEventDto.Title != null)
            {
                charityevent.Title = charityEventDto.Title;
            }
            if(charityEventDto.Description != null)
            { 
            charityevent.Description = charityEventDto.Description;
            }
            if(charityEventDto.ImageId.HasValue)
            { 
            charityevent.ImageIdImages = charityEventDto.ImageId.Value;
            }

            dbContext.SaveChanges();
        }
        private Charityevent getCharityEventFromDbById(int idCharityEvent)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(ce => ce.IdCharityEvent == idCharityEvent);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }

            return charityevent;
        }
        public async Task ChangeImage(IFormFile image, int idCharityEvent)
        {
            using var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
            var cv = getCharityEventFromDbById(idCharityEvent);

            await imageService.DeleteImageByIdAsync(cv.ImageIdImages);

            cv.ImageIdImages = await imageService.SaveImageAsync(image);

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

        }

        public void SetActive(int chariyEventId, bool isActive)
        {
            charityEventActivation.SetActive(chariyEventId, isActive);
        }
     
        public void SetVerify(int chariyEventId, bool isVerified)
        {
            charityEventVerification.SetVerify(chariyEventId, isVerified);
        }
        public GetCharityEventDto GetCharityEventById(int id)
        {
            var c = getCharityEventFromDbById(id);

            return new GetCharityEventDto {
                Id = c.IdCharityEvent,
                Description = c.Description,
                IsActive = c.IsActive,
                Title = c.Title,
                VolunteeringId = c?.VolunteeringIdVolunteering,
                FundraisingId = c?.CharityFundraisingIdCharityFundraising,
                IsVerified = c!.IsVerified
            };
        }
      

    }
}
