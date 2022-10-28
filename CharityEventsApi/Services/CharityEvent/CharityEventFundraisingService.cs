using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
namespace CharityEventsApi.Services.CharityEvent
{
    public class CharityEventFundraisingService : ICharityEventFundraisingService
    {
        private readonly CharityEventsDbContext dbContext;

        public CharityEventFundraisingService(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public void EditCharityEventFundraising(EditCharityEventFundraisingDto charityEventFundraisingDto, int charityEventFundraisingId)
        {
            var charityevent = dbContext.Charityfundraisings.FirstOrDefault(f => f.IdCharityFundraising == charityEventFundraisingId);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEventFundraising with given id doesn't exist");
            }
            if (charityEventFundraisingDto.AmountOfMoneyToCollect != null)
            {
                charityevent.AmountOfMoneyToCollect = (decimal)charityEventFundraisingDto.AmountOfMoneyToCollect;
            }
            charityevent.FundTarget = charityEventFundraisingDto.FundTarget;
            dbContext.SaveChanges();
        }

        public void EndCharityEventFundraising(int CharityEventFundraisingId)
        {
            var fundraising = dbContext.Charityfundraisings.Include(ce => ce.Charityevents).FirstOrDefault(f => f.IdCharityFundraising == CharityEventFundraisingId);
            if (fundraising == null)
            {
                throw new NotFoundException("CharityEventFundraising with given id doesn't exist");
            }
            fundraising.EndEventDate = DateTime.Now;
            fundraising.IsActive = 0;
            var charityevent = fundraising.Charityevents.FirstOrDefault();
            if (charityevent == null)
            {
                throw new BadRequestException("CharityEventVolunteering dont have charity event.");
            }

            if (charityevent.VolunteeringIdVolunteering == null)
            {
                charityevent.IsActive = 0;
            }
            else
            {
                var cv = dbContext.Volunteerings.FirstOrDefault(cv => cv.IdVolunteering == charityevent.VolunteeringIdVolunteering);
                if (cv != null)
                {
                    if (cv.EndEventDate != null)
                    {
                        charityevent.IsActive = 0;
                    }
                }
            }

          
            dbContext.SaveChanges();
        }
        public GetCharityFundrasingDto GetCharityEventFundraisingById(int id)
        {
            var c = dbContext.Charityfundraisings.FirstOrDefault(c => c.IdCharityFundraising == id);
            if (c is null)
            {
                throw new NotFoundException("Given id doesn't exist");
            }


            return new GetCharityFundrasingDto
            {
                AmountOfAlreadyCollectedMoney = c.AmountOfAlreadyCollectedMoney,
                AmountOfMoneyToCollect = c.AmountOfMoneyToCollect,
                CreatedEventDate = c.CreatedEventDate,
                EndEventDate = c.EndEventDate,
                FundTarget = c.FundTarget,
                IsActive = c.IsActive
            };
        }

    }
}
