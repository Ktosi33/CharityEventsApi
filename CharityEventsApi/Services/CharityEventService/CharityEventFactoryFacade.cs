using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.FundraisingService;
using CharityEventsApi.Services.ImageService;
using CharityEventsApi.Services.VolunteeringService;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CharityEventsApi.Services.CharityEventService
{
    public class CharityEventFactoryFacade : ICharityEventFactoryFacade
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly FundraisingFactory fundraisingFactory;
        private readonly VolunteeringFactory volunteeringFactory;
        private readonly CharityEventFactory charityEventFactory;
        private readonly IImageService imageService;

        public CharityEventFactoryFacade(CharityEventsDbContext dbContext, FundraisingFactory fundraisingFactory,
            VolunteeringFactory volunteeringFactory, CharityEventFactory charityEventFactory, IImageService imageService)
        {
            this.dbContext = dbContext;
            this.fundraisingFactory = fundraisingFactory;
            this.volunteeringFactory = volunteeringFactory;
            this.charityEventFactory = charityEventFactory;
            this.imageService = imageService;
        }
        public async Task AddCharityEvent(AddAllCharityEventsDto charityEventDto)
        {
            using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                var organizer = await dbContext.Users.FirstOrDefaultAsync(u => u.IdUser == charityEventDto.OrganizerId);
                if (organizer is null)
                {
                    throw new BadRequestException("Organizer ID can't match with any user");
                }
           
                Charityevent charityevent = await charityEventFactory.CreateCharityEvent(charityEventDto, organizer);
                charityevent.ImageIdImages = await imageService.SaveImageAsync(charityEventDto.ImageCharityEvent);

                List<int> ids = await imageService.SaveImagesAsync(charityEventDto.ImagesCharityEvent);
                charityevent.ImageIdImages1 = dbContext.Images.Where(img => ids.Contains(img.IdImages)).ToList();

                await dbContext.Charityevents.AddAsync(charityevent);
                await dbContext.SaveChangesAsync();

                if (charityEventDto.isFundraising)
                {
                   await addCharityEventFundraising(charityEventDto, charityevent);
                }

                if (charityEventDto.isVolunteering)
                {
                   await addCharityEventVolunteering(charityEventDto, charityevent);
                }
              
             await transaction.CommitAsync();
          }
        }
        
        private async Task addCharityEventVolunteering(AddAllCharityEventsDto charityEventDto, Charityevent charityEvent)
        {
          //  List<int> ids = await imageService.SaveImagesAsync(charityEventDto.ImagesFundraising);
            Volunteering volunteering = volunteeringFactory.CreateCharityEvent(charityEventDto);
         //   volunteering.ImageIdImages = dbContext.Images.Where(img => ids.Contains(img.IdImages)).ToList();
            await dbContext.Volunteerings.AddAsync(volunteering);
            charityEvent.VolunteeringIdVolunteeringNavigation = volunteering;
            //charityEvent.VolunteeringIdVolunteering = volunteering.IdVolunteering;
            await dbContext.SaveChangesAsync();
        }
        public async Task AddCharityEventVolunteering(AddCharityEventVolunteeringDto charityEventDto, Charityevent charityevent)
        {
            using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
               // List<int> ids = await imageService.SaveImagesAsync(charityEventDto.Images);
                Volunteering cv = volunteeringFactory.CreateCharityEvent(charityEventDto);
              //  cv.ImageIdImages = dbContext.Images.Where(img => ids.Contains(img.IdImages)).ToList();
                await dbContext.Volunteerings.AddAsync(cv);
                charityevent.VolunteeringIdVolunteeringNavigation = cv;
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }
        private async Task addCharityEventFundraising(AddAllCharityEventsDto charityEventDto, Charityevent charityEvent)
        {
         //   List<int> ids = await imageService.SaveImagesAsync(charityEventDto.ImagesFundraising);            
            Charityfundraising charityfundraising = fundraisingFactory.CreateCharityEvent(charityEventDto);
           // charityfundraising.ImageIdImages = dbContext.Images.Where(img => ids.Contains(img.IdImages)).ToList();
            await dbContext.Charityfundraisings.AddAsync(charityfundraising);
            charityEvent.CharityFundraisingIdCharityFundraisingNavigation = charityfundraising;
            //charityEvent.CharityFundraisingIdCharityFundraising = charityfundraising.IdCharityFundraising;
            await dbContext.SaveChangesAsync();
        }
        public async Task AddCharityEventFundraising(AddCharityEventFundraisingDto charityEventDto, Charityevent charityEvent)
        {
            using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
              //  List<int> ids = await imageService.SaveImagesAsync(charityEventDto.Images);
                Charityfundraising cf = await fundraisingFactory.CreateCharityEvent(charityEventDto);
               // cf.ImageIdImages = dbContext.Images.Where(img => ids.Contains(img.IdImages)).ToList();
                await dbContext.Charityfundraisings.AddAsync(cf);
               // await dbContext.SaveChangesAsync();
                charityEvent.CharityFundraisingIdCharityFundraisingNavigation = cf;
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }
        public void AddLocation(AddLocationDto locationDto)
        {
            var vol = dbContext.Volunteerings.FirstOrDefault(v => v.IdVolunteering == locationDto.idVolunteering);
            if(vol == null)
            {
                throw new BadRequestException("Volunteering ID doesnt exist");
            }
            var newLocation = volunteeringFactory.newLocation(locationDto, vol);
            vol.LocationIdLocations.Add(newLocation);
            dbContext.Locations.Add(newLocation);
            dbContext.SaveChanges();
        }

    }
}
