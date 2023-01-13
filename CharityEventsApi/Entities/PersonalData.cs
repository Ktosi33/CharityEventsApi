using System;
using System.Collections.Generic;

namespace CharityEventsApi.Entities
{
    public partial class PersonalData
    {
        public int IdUser { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int IdAddress { get; set; }

        public virtual Address IdAddressNavigation { get; set; } = null!;
        public virtual User IdUserNavigation { get; set; } = null!;
    }
}
