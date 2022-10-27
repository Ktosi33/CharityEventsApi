using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.CharityEvent
{
    public class CharityEventVolunteeringService : ICharityEventVolunteeringService
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly ICharityEventFactoryFacade charityEventFactoryFacade;

        public CharityEventVolunteeringService(CharityEventsDbContext dbContext, ICharityEventFactoryFacade charityEventFactoryFacade)
        {
            this.dbContext = dbContext;
            this.charityEventFactoryFacade = charityEventFactoryFacade;
        }

        public void AddLocation(AddLocationDto locationDto)
        {
            charityEventFactoryFacade.AddLocation(locationDto);
        }

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

        public void EditCharityEventVolunteering(EditCharityEventVolunteeringDto charityEventVolunteeringDto, int charityEventVolunteeringId)
        {
            var charityevent = dbContext.Volunteerings.FirstOrDefault(v => v.IdVolunteering == charityEventVolunteeringId);
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

        public void EndCharityEventVolunteering(int charityEventVolunteeringId)
        {
            var volunteering = dbContext.Volunteerings.Include(ce => ce.Charityevents).FirstOrDefault(v => v.IdVolunteering == charityEventVolunteeringId);
            if (volunteering == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }
            volunteering.EndEventDate = DateTime.Now;
            volunteering.IsActive = 0;

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
