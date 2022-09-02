using System;
using System.Collections.Generic;

namespace CharityEventsApi.Entities
{
    public partial class Charityevent
    {
        public Charityevent()
        {
            UserIdUsers = new HashSet<User>();
        }

        public int IdCharityEvent { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int VolunteeringIdVolunteering { get; set; }
        public int CharityFundraisingIdCharityFundraising { get; set; }
        public int OrganizerId { get; set; }

        public virtual Charityfundraising CharityFundraisingIdCharityFundraisingNavigation { get; set; } = null!;
        public virtual User Organizer { get; set; } = null!;
        public virtual Volunteering VolunteeringIdVolunteeringNavigation { get; set; } = null!;

        public virtual ICollection<User> UserIdUsers { get; set; }
    }
}
