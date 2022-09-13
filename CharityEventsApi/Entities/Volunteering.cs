using System;
using System.Collections.Generic;

namespace CharityEventsApi.Entities
{
    public partial class Volunteering
    {
        public Volunteering()
        {
            Charityevents = new HashSet<Charityevent>();
            LocationIdLocations = new HashSet<Location>();
        }

        public int IdVolunteering { get; set; }
        public int AmountOfNeededVolunteers { get; set; }
        public DateTime CreatedEventDate { get; set; }
        public DateTime? EndEventDate { get; set; }

        public virtual ICollection<Charityevent> Charityevents { get; set; }

        public virtual ICollection<Location> LocationIdLocations { get; set; }
    }
}
