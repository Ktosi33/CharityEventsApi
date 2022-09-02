using System;
using System.Collections.Generic;

namespace CharityEventsApi.Entities
{
    public partial class User
    {
        public User()
        {
            Charityevents = new HashSet<Charityevent>();
            Donations = new HashSet<Donation>();
            PersonalData = new HashSet<PersonalDatum>();
            CharityEventIdCharityEvents = new HashSet<Charityevent>();
            RolesNames = new HashSet<Role>();
        }

        public int IdUser { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;

        public virtual ICollection<Charityevent> Charityevents { get; set; }
        public virtual ICollection<Donation> Donations { get; set; }
        public virtual ICollection<PersonalDatum> PersonalData { get; set; }

        public virtual ICollection<Charityevent> CharityEventIdCharityEvents { get; set; }
        public virtual ICollection<Role> RolesNames { get; set; }
    }
}
