using System;
using System.Collections.Generic;

namespace CharityEventsApi.Entities
{
    public partial class Address
    {
        public Address()
        {
            PersonalData = new HashSet<PersonalData>();
        }

        public int IdAddress { get; set; }
        public string PostalCode { get; set; } = null!;
        public string Town { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string HouseNumber { get; set; } = null!;
        public string? FlatNumber { get; set; }

        public virtual ICollection<PersonalData> PersonalData { get; set; }
    }
}
