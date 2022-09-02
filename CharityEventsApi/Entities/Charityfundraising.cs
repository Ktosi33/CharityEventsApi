﻿using System;
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
        public DateTime? EventDate { get; set; }

        public virtual ICollection<Charityevent> Charityevents { get; set; }
        public virtual ICollection<Donation> Donations { get; set; }
    }
}
