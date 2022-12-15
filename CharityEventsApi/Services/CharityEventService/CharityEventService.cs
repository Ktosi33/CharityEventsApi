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

        public async Task Add(AddAllCharityEventsDto charityEventDto)
        {
          await charityEventFactoryFacade.AddCharityEvent(charityEventDto);
        }
        
        public async Task AddOneImage(IFormFile image, int idCharityEvent)
        {
            using var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
            var ce = GetCharityEventByCharityEventId(idCharityEvent);


            int imageId = await imageService.SaveImageAsync(image);
            var img = await dbContext.Images.FirstOrDefaultAsync(i => i.IdImages == imageId);
            if (img is null)
            {
                throw new BadRequestException("something went wrong");
            }
            ce.ImageIdImages1.Add(img);
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
            var ce = GetCharityEventByCharityEventId(idCharityEvent);
     

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
       
        public IEnumerable<GetCharityEventDto> GetAllCharityEventDto()
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
            if(charityEventDto.ImageId.HasValue)
            { 
            charityevent.ImageIdImages = charityEventDto.ImageId.Value;
            }

            dbContext.SaveChanges();
        }
        public Charityevent GetCharityEventByCharityEventId(int idCharityEvent)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(ce => ce.IdCharityEvent == idCharityEvent);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }

            return charityevent;
        }
        public Charityevent GetCharityEventByFundraisingId(int idFundraising)
        {
            Charityevent? charityevent = dbContext.Charityevents.FirstOrDefault(ce => ce.CharityFundraisingIdCharityFundraising == idFundraising);

            if (charityevent is null)
            {
                throw new InternalServerErrorException();
            }

            return charityevent;
        }
        public Charityevent GetCharityEventByVolunteeringId(int idVolunteering)
        {
            Charityevent? charityevent = dbContext.Charityevents.FirstOrDefault(ce => ce.VolunteeringIdVolunteering == idVolunteering);

            if (charityevent is null)
            {
                throw new InternalServerErrorException();
            }

            return charityevent;
        }
        public async Task ChangeImage(IFormFile image, int idCharityEvent)
        {
            using var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
            var ce = GetCharityEventByCharityEventId(idCharityEvent);
            await imageService.DeleteImageByIdAsync(ce.ImageIdImages);

            ce.ImageIdImages = await imageService.SaveImageAsync(image);

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
                VolunteeringId = c?.VolunteeringIdVolunteering,
                FundraisingId = c?.CharityFundraisingIdCharityFundraising,
                IsVerified = c!.IsVerified
            };
        }

      
    }
}
