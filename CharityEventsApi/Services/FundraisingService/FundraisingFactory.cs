using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.ImageService;

namespace CharityEventsApi.Services.FundraisingService
{
    public class FundraisingFactory 
    {
        public CharityFundraising CreateCharityEvent(AddAllCharityEventsDto charityEventDto)
        {
            CharityFundraising charityfundraising = new ()
            {
                AmountOfMoneyToCollect = charityEventDto.AmountOfMoneyToCollect != null ? (decimal)charityEventDto.AmountOfMoneyToCollect : 0, //TODO: can make problems
                FundTarget = charityEventDto.FundTarget ?? "",
                CreatedEventDate = DateTime.Now,
                IsActive = 0,
                IsVerified = 0,
                IsDenied = 0
            };

            return charityfundraising;
        }
        public CharityFundraising CreateCharityEvent(AddCharityEventFundraisingDto charityEventDto)
        {
            CharityFundraising charityfundraising = new ()
            {
                AmountOfMoneyToCollect =  charityEventDto.AmountOfMoneyToCollect,
                FundTarget = charityEventDto.FundTarget ?? "",
                CreatedEventDate = DateTime.Now,
                IsActive = 0,
                IsVerified = 0,
                IsDenied = 0

            };

            return charityfundraising;
        }
    }
}
