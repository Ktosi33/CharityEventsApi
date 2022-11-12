using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.CharityEvent
{
    public class VolunteeringService : IVolunteeringService
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly VolunteeringFactory charityEventVolunteeringFactory;
        private readonly ICharityEventFactoryFacade charityEventFactoryFacade;

        public VolunteeringService(CharityEventsDbContext dbContext, VolunteeringFactory charityEventVolunteeringFactory, ICharityEventFactoryFacade charityEventFactoryFacade)
        {
            this.dbContext = dbContext;
            this.charityEventVolunteeringFactory = charityEventVolunteeringFactory;
            this.charityEventFactoryFacade = charityEventFactoryFacade;
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
        public void Add(AddCharityEventVolunteeringDto dto, int charityEventId)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(f => f.IdCharityEvent == charityEventId);
            if (charityevent is null)
            {
                throw new NotFoundException("Charity event with given id doesn't exist");
            }
            Volunteering cv = charityEventVolunteeringFactory.CreateCharityEvent(dto);
            dbContext.Volunteerings.Add(cv);
            dbContext.SaveChanges();
        }
        public void SetActive(int VolunteeringId, bool isActive)
        {
            if (isActive)
            {
                active(VolunteeringId);
            }
            else if (!isActive)
            {
                disactive(VolunteeringId);
            }
            else
            {
                throw new BadRequestException("Bad query");
            }
        }
        public void SetVerify(int VolunteeringId, bool isVerified)
        {
            if (isVerified)
            {
                verify(VolunteeringId);
            }
            else if (!isVerified)
            {
                unverify(VolunteeringId);
            }
            else
            {
                throw new BadRequestException("Bad query");
            }
        }
        public void Edit(EditCharityEventVolunteeringDto VolunteeringDto, int VolunteeringId)
        {
            var charityevent = dbContext.Volunteerings.FirstOrDefault(v => v.IdVolunteering == VolunteeringId);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }
            //TODO: maybe throw warning in same way
            if (VolunteeringDto.AmountOfNeededVolunteers != null)
            {
                charityevent.AmountOfNeededVolunteers = (int)VolunteeringDto.AmountOfNeededVolunteers;
            }
            dbContext.SaveChanges();
        }
        private void active(int VolunteeringId)
        {
            var volunteering = dbContext.Volunteerings.Include(ce => ce.Charityevents).FirstOrDefault(v => v.IdVolunteering == VolunteeringId);
            if (volunteering == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }
            var charityevent = volunteering.Charityevents.FirstOrDefault();
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEventVolunteering doesn't have charity event.");
            }

            if (charityevent.IsActive == 0 || charityevent.IsVerified == 0 || volunteering.IsVerified == 0)
            {
                throw new BadRequestException("You cant active fundraising while charity event isn't active or verified");
            }
            volunteering.IsActive = 1;
            dbContext.SaveChanges();
        }
        private void verify(int VolunteeringId)
        {
            var volunteering = dbContext.Volunteerings.Include(ce => ce.Charityevents).FirstOrDefault(v => v.IdVolunteering == VolunteeringId);
            if (volunteering == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }
            var charityevent = volunteering.Charityevents.FirstOrDefault();
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEventVolunteering doesn't have charity event.");
            }
            if (charityevent.IsVerified == 0)
            {
                throw new BadRequestException("Firstly verify charityevent");
            }
            volunteering.IsVerified = 1;
            dbContext.SaveChanges();
        }
        private void unverify(int VolunteeringId)
        {
            var volunteering = dbContext.Volunteerings.Include(ce => ce.Charityevents).FirstOrDefault(v => v.IdVolunteering == VolunteeringId);
            if (volunteering == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }
            disactive(VolunteeringId);
            volunteering.IsVerified = 0;
            dbContext.SaveChanges();
        }
        private void disactive(int VolunteeringId)
        {
            var volunteering = dbContext.Volunteerings.Include(ce => ce.Charityevents).FirstOrDefault(v => v.IdVolunteering == VolunteeringId);
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
        public GetCharityEventVolunteeringDto GetById(int id)
        {
            var c = dbContext.Volunteerings.FirstOrDefault(c => c.IdVolunteering == id);
            if (c is null)
            {
                throw new NotFoundException("Given id doesn't exist");
            }


            return new GetCharityEventVolunteeringDto
            {
                AmountOfNeededVolunteers = c.AmountOfNeededVolunteers,
                CreatedEventDate = c.CreatedEventDate,
                EndEventDate = c.EndEventDate,
                IsActive = c.IsActive,
                isVerified = c.IsVerified
            };
        }

    }
}
