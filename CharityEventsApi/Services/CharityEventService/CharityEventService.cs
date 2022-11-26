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
        public IEnumerable<GetCharityEventDto> GetAll()
        {
            var charityevents = dbContext.Charityevents.Select(ce => new GetCharityEventDto
                { 
                Id = ce.IdCharityEvent,
                IsActive = ce.IsActive,
                Description = ce.Description,
                FundraisingId = ce.CharityFundraisingIdCharityFundraising,
                isVerified = ce.IsVerified,
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
            charityevent.Title = charityEventDto.Title;
            charityevent.Description = charityEventDto.Description;
            charityevent.ImageIdImages = (int)charityEventDto.ImageId;
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
            using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                var cv = getCharityEventFromDbById(idCharityEvent);

                await imageService.DeleteImageByIdAsync(cv.ImageIdImages);

                cv.ImageIdImages = await imageService.SaveImageAsync(image);

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }

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
                isVerified = c.IsVerified
            };
        }
      

    }
}
