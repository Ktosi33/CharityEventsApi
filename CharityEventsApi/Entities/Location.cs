using System;
using System.Collections.Generic;

namespace CharityEventsApi.Entities
{
    public partial class Location
    {
        public Location()
        {
            VolunteeringIdVolunteerings = new HashSet<Volunteering>();
        }

        public int IdLocation { get; set; }
        public string PostalCode { get; set; } = null!;
        public string Town { get; set; } = null!;
        public string Street { get; set; } = null!;

        public virtual ICollection<Volunteering> VolunteeringIdVolunteerings { get; set; }
    }
}
