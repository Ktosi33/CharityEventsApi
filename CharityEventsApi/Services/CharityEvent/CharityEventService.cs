using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.CharityEvent
{
    public class CharityEventService : ICharityEventService
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly ICharityEventFactoryFacade charityEventFactoryFacade;

        public CharityEventService(CharityEventsDbContext dbContext, ICharityEventFactoryFacade charityEventFactoryFacade)
        {
            this.dbContext = dbContext;
            this.charityEventFactoryFacade = charityEventFactoryFacade;
        }

        public void Add(AddAllCharityEventsDto charityEventDto)
        {
           charityEventFactoryFacade.AddCharityEvent(charityEventDto);
        }

        public void Edit(EditCharityEventDto charityEventDto, int charityEventId)
        {
           var charityevent = dbContext.Charityevents.FirstOrDefault(ce => ce.IdCharityEvent == charityEventId);
            if(charityevent == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }
            charityevent.Title = charityEventDto.Title;
            charityevent.Description = charityEventDto.Description;
            dbContext.SaveChanges();
        }

        public void Active(int charityEventId)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(ce => ce.IdCharityEvent == charityEventId);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }

            if ( charityevent.IsVerified == 0)
            {
                throw new BadRequestException("You cant active charity event while event isn't active or verified");
            }
            charityevent.IsActive = 1;
            dbContext.SaveChanges();
        }

        public void SetActive(int chariyEventId, bool isActive)
        {
            if (isActive)
            {
                Active(chariyEventId);
            }
            else if (!isActive)
            {
                Disactive(chariyEventId);
            }
            else
            {
                throw new BadRequestException("Bad query");
            }
        }
        public void SetVerify(int chariyEventId, bool isVerified)
        {
            if (isVerified)
            {
                verify(chariyEventId);
            }
            else if (!isVerified)
            {
                unverify(chariyEventId);
            }
            else
            {
                throw new BadRequestException("Bad query");
            }
        }
        private void verify(int charityEventId)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(ce => ce.IdCharityEvent == charityEventId);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }
            charityevent.IsVerified = 1;
            dbContext.SaveChanges();
        }
        private void unverify(int charityEventId)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(ce => ce.IdCharityEvent == charityEventId);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }
            Disactive(charityEventId);
            charityevent.IsVerified = 0;
            dbContext.SaveChanges();
        }
        public void Disactive(int charityEventId)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(ce => ce.IdCharityEvent == charityEventId);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }

            charityevent.IsActive = 0;
            if (charityevent.CharityFundraisingIdCharityFundraising != null)
            {
                var cf = dbContext.Charityfundraisings.FirstOrDefault(cf => cf.IdCharityFundraising == charityevent.CharityFundraisingIdCharityFundraising);
                


                if (cf != null)
                { 
                    if(cf.EndEventDate == null)
                    { 
                     cf.EndEventDate = DateTime.Now;
                    }
                    cf.IsActive = 0;
                }
            }
            if (charityevent.VolunteeringIdVolunteering != null)
            {
                var cv = dbContext.Volunteerings.FirstOrDefault(cv => cv.IdVolunteering == charityevent.VolunteeringIdVolunteering);
                if (cv != null)
                {
                    if (cv.EndEventDate == null)
                    {
                        cv.EndEventDate = DateTime.Now;
                    }
                    cv.IsActive = 0;
                }
            }

            dbContext.SaveChanges();

        }

        public GetCharityEventDto GetCharityEventById(int id)
        {
            var c = dbContext.Charityevents.FirstOrDefault(c => c.IdCharityEvent == id);
            if(c is null)
            {
                throw new NotFoundException("Given id doesn't exist");
            }
           

            return new GetCharityEventDto { 
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
