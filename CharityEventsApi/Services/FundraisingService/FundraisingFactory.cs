using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.FundraisingService
{
    public class FundraisingFactory 
    {
        public Charityfundraising CreateCharityEvent(AddAllCharityEventsDto charityEventDto)
        {
            Charityfundraising charityfundraising = new Charityfundraising
            {
                AmountOfMoneyToCollect = charityEventDto.AmountOfMoneyToCollect != null ? (decimal)charityEventDto.AmountOfMoneyToCollect : 0, //TODO: can make problems
                FundTarget = charityEventDto.FundTarget != null ? charityEventDto.FundTarget : "",
                CreatedEventDate = DateTime.Now,
                IsActive = 0,
                IsVerified = 0
            };

            return charityfundraising;
        }
        public Charityfundraising CreateCharityEvent(AddCharityEventFundraisingDto charityEventDto)
        {
            Charityfundraising charityfundraising = new Charityfundraising
            {
                AmountOfMoneyToCollect = charityEventDto.AmountOfMoneyToCollect != null ? (decimal)charityEventDto.AmountOfMoneyToCollect : 0, //TODO: can make problems
                FundTarget = charityEventDto.FundTarget != null ? charityEventDto.FundTarget : "",
                CreatedEventDate = DateTime.Now,
                IsActive = 0,
                IsVerified = 0
            };

            return charityfundraising;
        }
    }
}
