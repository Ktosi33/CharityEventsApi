using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Services.ImageService;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.VolunteeringService
{
    public class VolunteeringService : IVolunteeringService
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly VolunteeringFactory charityEventVolunteeringFactory;
        private readonly ICharityEventFactoryFacade charityEventFactoryFacade;
        private readonly VolunteeringActivation volunteeringActication;
        private readonly VolunteeringVerification volunteeringVerification;
        private readonly CharityEventVerification charityEventVerification;
        private readonly IImageService imageService;

        public VolunteeringService(CharityEventsDbContext dbContext, VolunteeringFactory charityEventVolunteeringFactory, 
            ICharityEventFactoryFacade charityEventFactoryFacade, VolunteeringActivation volunteeringActication, 
            VolunteeringVerification volunteeringVerification, CharityEventVerification charityEventVerification,
            IImageService imageService)
        {
            this.dbContext = dbContext;
            this.charityEventVolunteeringFactory = charityEventVolunteeringFactory;
            this.charityEventFactoryFacade = charityEventFactoryFacade;
            this.volunteeringActication = volunteeringActication;
            this.volunteeringVerification = volunteeringVerification;
            this.charityEventVerification = charityEventVerification;
            this.imageService = imageService;
        }
        [Obsolete("AddLocation is deprecated, please use location controller instead")]
        public void AddLocation(AddLocationDto locationDto)
        {
            charityEventFactoryFacade.AddLocation(locationDto);
        }

        [Obsolete("EditLocation is deprecated, please use location controller instead")]
        public void EditLocation(EditLocationDto locationDto, int locationId)
        {
            var location = dbContext.Locations.FirstOrDefault(l => l.IdLocation == locationId);
            if (location == null)
            {
                throw new NotFoundException("Location with given id doesn't exist");
            }
            location.Street = locationDto.Street;
            location.PostalCode = locationDto.PostalCode;
            location.Town = locationDto.Town;
            dbContext.SaveChanges();

        }
        public async Task AddOneImage(IFormFile image, int idVolunteering)
        {
            using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                var cv = await dbContext.Volunteerings.FirstOrDefaultAsync(v => v.IdVolunteering == idVolunteering);
                if (cv is null)
                {
                    throw new NotFoundException("Charity event with given id doesn't exist");
                }
                int imageId = await imageService.SaveImageAsync(image);
                var img = await dbContext.Images.FirstOrDefaultAsync(i => i.IdImages == imageId);
                if (img is null)
                {
                    throw new BadRequestException("something went wrong");
                }
                cv.ImageIdImages.Add(img);
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }
        public async Task DeleteImage(int idImage, int idVolunteering)
        {
            using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                var cv = await dbContext.Volunteerings.FirstOrDefaultAsync(v => v.IdVolunteering == idVolunteering);
                if (cv is null)
                {
                    throw new NotFoundException("Charity event with given id doesn't exist");
                }
                var image = await dbContext.Images.FirstOrDefaultAsync(i => i.IdImages == idImage);
                if (image is null)
                {
                    throw new NotFoundException("Image with given id doesn't exist");
                }

                cv.ImageIdImages.Remove(image);
                await imageService.DeleteImageByObjectAsync(image);

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }
        public async Task Add(AddCharityEventVolunteeringDto dto)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(f => f.IdCharityEvent == dto.CharityEventId);
            if (charityevent is null)
            {
                throw new NotFoundException("Charity event with given id doesn't exist");
            }
            if(charityevent.VolunteeringIdVolunteering is not null)
            {
                throw new BadRequestException("Can't add charity event volunteering, because another one already exists in this charity event");
            }
            await charityEventFactoryFacade.AddCharityEventVolunteering(dto, charityevent);

            charityEventVerification.SetVerify(dto.CharityEventId, false);
        }
        public void SetActive(int VolunteeringId, bool isActive)
        {
            volunteeringActication.SetActive(VolunteeringId, isActive);
        }
        public void SetVerify(int VolunteeringId, bool isVerified)
        {
            volunteeringVerification.SetVerify(VolunteeringId, isVerified);
        }
        public void Edit(EditCharityEventVolunteeringDto VolunteeringDto, int VolunteeringId)
        {
            var charityevent = dbContext.Volunteerings.FirstOrDefault(v => v.IdVolunteering == VolunteeringId);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }
            if (VolunteeringDto.AmountOfNeededVolunteers != null)
            {
                charityevent.AmountOfNeededVolunteers = (int)VolunteeringDto.AmountOfNeededVolunteers;
            }
            dbContext.SaveChanges();
        }
      
        public GetCharityEventVolunteeringDto GetById(int id)
        {
            var c = dbContext.Volunteerings.FirstOrDefault(c => c.IdVolunteering == id);
            if (c is null)
            {
                throw new NotFoundException("Given id doesn't exist");
            }


            return new GetCharityEventVolunteeringDto
            {
                Id = c.IdVolunteering,
                AmountOfNeededVolunteers = c.AmountOfNeededVolunteers,
                CreatedEventDate = c.CreatedEventDate,
                EndEventDate = c.EndEventDate,
                IsActive = c.IsActive,
                isVerified = c.IsVerified
            };
        }
        public IEnumerable<GetCharityEventVolunteeringDto> GetAll()
        {
            var volunteerings = dbContext.Volunteerings.Select(c => new GetCharityEventVolunteeringDto
            {
                Id = c.IdVolunteering,
                AmountOfNeededVolunteers = c.AmountOfNeededVolunteers,
                CreatedEventDate = c.CreatedEventDate,
                EndEventDate = c.EndEventDate,
                IsActive = c.IsActive,
                isVerified = c.IsVerified
            }
            );
            if (volunteerings == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }
            return volunteerings;
        }

    }
}
