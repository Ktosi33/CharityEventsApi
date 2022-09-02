using System;
using System.Collections.Generic;

namespace CharityEventsApi.Entities
{
    public partial class Address
    {
        public Address()
        {
            PersonalData = new HashSet<PersonalDatum>();
        }

        public int IdAddress { get; set; }
        public string PostalCode { get; set; } = null!;
        public string Town { get; set; } = null!;
        public string Street { get; set; } = null!;
        public int HouseNumber { get; set; }
        public int? FlatNumber { get; set; }

        public virtual ICollection<PersonalDatum> PersonalData { get; set; }
    }
}
