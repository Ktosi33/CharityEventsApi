using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.CharityEvent
{
    public class CharityEventFundraisingFactory
    {
        public Charityfundraising CreateCharityEvent(CharityEventDto charityEventDto)
        {
            Charityfundraising charityfundraising = new Charityfundraising
            {
                AmountOfMoneyToCollect = charityEventDto.AmountOfMoneyToCollect != null ? (decimal)charityEventDto.AmountOfMoneyToCollect : 0, //TODO: can make problems
                FundTarget = charityEventDto.FundTarget != null ? charityEventDto.FundTarget : "",
                CreatedEventDate = DateTime.Now
            };

            return charityfundraising;
        }
    }
}
