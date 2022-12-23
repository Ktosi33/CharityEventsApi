using System;
using System.Collections.Generic;

namespace CharityEventsApi.Entities
{
    public partial class Donation
    {
        public int IdDonation { get; set; }
        public decimal AmountOfDonation { get; set; }
        public DateTime DonationDate { get; set; }
        public string? Description { get; set; }
        public int? IdUser { get; set; }
        public int IdCharityFundraising { get; set; }

        public virtual CharityFundraising IdCharityFundraisingNavigation { get; set; } = null!;
        public virtual User? IdUserNavigation { get; set; }
    }
}
