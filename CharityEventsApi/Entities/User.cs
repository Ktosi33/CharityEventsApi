using System;
using System.Collections.Generic;

namespace CharityEventsApi.Entities
{
    public partial class User
    {
        public User()
        {
            CharityEvents = new HashSet<CharityEvent>();
            Donations = new HashSet<Donation>();
            RoleNames = new HashSet<Role>();
            IdCharityVolunteerings = new HashSet<CharityVolunteering>();
        }

        public int IdUser { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;

        public virtual PersonalData PersonalData { get; set; } = null!;
        public virtual ICollection<CharityEvent> CharityEvents { get; set; }
        public virtual ICollection<Donation> Donations { get; set; }

        public virtual ICollection<Role> RoleNames { get; set; }
        public virtual ICollection<CharityVolunteering> IdCharityVolunteerings { get; set; }
    }
}
