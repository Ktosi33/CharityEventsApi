using System;
using System.Collections.Generic;

namespace CharityEventsApi.Entities
{
    public partial class CharityVolunteering
    {
        public CharityVolunteering()
        {
            CharityEvents = new HashSet<CharityEvent>();
            IdLocations = new HashSet<Location>();
            IdUsers = new HashSet<User>();
        }

        public int IdCharityVolunteering { get; set; }
        public int AmountOfNeededVolunteers { get; set; }
        public DateTime CreatedEventDate { get; set; }
        public DateTime? EndEventDate { get; set; }
        public sbyte IsActive { get; set; }
        public sbyte IsVerified { get; set; }
        public sbyte IsDenied { get; set; }

        public virtual ICollection<CharityEvent> CharityEvents { get; set; }

        public virtual ICollection<Location> IdLocations { get; set; }
        public virtual ICollection<User> IdUsers { get; set; }
    }
}
