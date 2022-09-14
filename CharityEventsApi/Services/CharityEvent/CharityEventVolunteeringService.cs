using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.CharityEvent
{
    public class CharityEventVolunteeringService : ICharityEventVolunteeringService
    {
        private readonly CharityEventsDbContext dbContext;

        public CharityEventVolunteeringService(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddLocation(AddLocationDto locationDto)
        {
            CharityEventFactoryFacade charityEventFactoryFacade = new CharityEventFactoryFacade(dbContext);
            charityEventFactoryFacade.AddLocation(locationDto);
        }

        public void EditLocation(EditLocationDto locationDto)
        {
            var location = dbContext.Locations.FirstOrDefault(l => l.IdLocation == locationDto.LocationId);
            if (location == null)
            {
                throw new NotFoundException("Location with given id doesn't exist");
            }
            location.Street = locationDto.Street;
            location.PostalCode = locationDto.PostalCode;
            location.Town = locationDto.Town;
            dbContext.SaveChanges();

        }

        public void EditCharityEventVolunteering(EditCharityEventVolunteeringDto charityEventVolunteeringDto)
        {
            var charityevent = dbContext.Volunteerings.FirstOrDefault(v => v.IdVolunteering == charityEventVolunteeringDto.VolunteeringId);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }
            //TODO: maybe throw warning in same way
            if (charityEventVolunteeringDto.AmountOfNeededVolunteers != null)
            {
                charityevent.AmountOfNeededVolunteers = (int)charityEventVolunteeringDto.AmountOfNeededVolunteers;
            }
            dbContext.SaveChanges();
        }

        public void EndCharityEventVolunteering(int CharityEventVolunteeringId)
        {
            var volunteering = dbContext.Volunteerings.Include(ce => ce.Charityevents).FirstOrDefault(v => v.IdVolunteering == CharityEventVolunteeringId);
            if (volunteering == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }
            volunteering.EndEventDate = DateTime.Now;
            var charityevent = volunteering.Charityevents.FirstOrDefault();
            if (charityevent == null)
            {
                throw new BadRequestException("CharityEventVolunteering dont have charity event.");
            }

            if (charityevent.CharityFundraisingIdCharityFundraising == null)
            {
                charityevent.IsActive = 0;
            }
            else
            {
                var cf = dbContext.Charityfundraisings.FirstOrDefault(cf => cf.IdCharityFundraising == charityevent.CharityFundraisingIdCharityFundraising);
                if (cf != null)
                {
                    if (cf.EndEventDate != null)
                    {
                        charityevent.IsActive = 0;
                    }
                }
            }


          
            dbContext.SaveChanges();

        }

    }
}
