using System;
using System.Collections.Generic;

namespace CharityEventsApi.Entities
{
    public partial class Charityfundraising
    {
        public Charityfundraising()
        {
            Charityevents = new HashSet<Charityevent>();
            Donations = new HashSet<Donation>();
        }

        public int IdCharityFundraising { get; set; }
        public string FundTarget { get; set; } = null!;
        public decimal AmountOfMoneyToCollect { get; set; }
        public decimal AmountOfAlreadyCollectedMoney { get; set; }
        public DateTime CreatedEventDate { get; set; }
        public DateTime? EndEventDate { get; set; }
        public sbyte IsActive { get; set; }
        public sbyte IsVerified { get; set; }
        public sbyte IsDenied { get; set; }

        public virtual ICollection<Charityevent> Charityevents { get; set; }
        public virtual ICollection<Donation> Donations { get; set; }
    }
}
