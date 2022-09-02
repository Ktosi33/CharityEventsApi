using System;
using System.Collections.Generic;

namespace CharityEventsApi.Entities
{
    public partial class PersonalDatum
    {
        public int UserIdUser { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int AddressIdAddress { get; set; }

        public virtual Address AddressIdAddressNavigation { get; set; } = null!;
        public virtual User UserIdUserNavigation { get; set; } = null!;
    }
}
