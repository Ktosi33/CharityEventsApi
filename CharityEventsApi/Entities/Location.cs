using System;
using System.Collections.Generic;

namespace CharityEventsApi.Entities
{
    public partial class Location
    {
        public Location()
        {
            IdCharityVolunteerings = new HashSet<CharityVolunteering>();
        }

        public int IdLocation { get; set; }
        public string PostalCode { get; set; } = null!;
        public string Town { get; set; } = null!;
        public string Street { get; set; } = null!;

        public virtual ICollection<CharityVolunteering> IdCharityVolunteerings { get; set; }
    }
}
