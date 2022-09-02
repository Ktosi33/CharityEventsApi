using System;
using System.Collections.Generic;

namespace CharityEventsApi.Entities
{
    public partial class Donation
    {
        public int IdDonations { get; set; }
        public decimal AmountOfDonation { get; set; }
        public DateTime DonationDate { get; set; }
        public string? Description { get; set; }
        public int UserIdUser { get; set; }
        public int CharityFundraisingIdCharityFundraising { get; set; }

        public virtual Charityfundraising CharityFundraisingIdCharityFundraisingNavigation { get; set; } = null!;
        public virtual User UserIdUserNavigation { get; set; } = null!;
    }
}
