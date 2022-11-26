using System;
using System.Collections.Generic;

namespace CharityEventsApi.Entities
{
    public partial class Image
    {
        public Image()
        {
            Charityevents = new HashSet<Charityevent>();
            CharityFundraisingIdCharityFundraisings = new HashSet<Charityfundraising>();
            VolunteeringIdVolunteerings = new HashSet<Volunteering>();
        }

        public int IdImages { get; set; }
        public string Path { get; set; } = null!;
        public string ContentType { get; set; } = null!;

        public virtual ICollection<Charityevent> Charityevents { get; set; }

        public virtual ICollection<Charityfundraising> CharityFundraisingIdCharityFundraisings { get; set; }
        public virtual ICollection<Volunteering> VolunteeringIdVolunteerings { get; set; }
    }
}
