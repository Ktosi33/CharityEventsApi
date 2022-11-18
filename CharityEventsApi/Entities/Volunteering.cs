using System;
using System.Collections.Generic;

namespace CharityEventsApi.Entities
{
    public partial class Volunteering
    {
        public Volunteering()
        {
            Charityevents = new HashSet<Charityevent>();
            ImageIdImages = new HashSet<Image>();
            LocationIdLocations = new HashSet<Location>();
            UserIdUsers = new HashSet<User>();
        }

        public int IdVolunteering { get; set; }
        public int AmountOfNeededVolunteers { get; set; }
        public DateTime CreatedEventDate { get; set; }
        public DateTime? EndEventDate { get; set; }
        public sbyte IsActive { get; set; }
        public sbyte IsVerified { get; set; }

        public virtual ICollection<Charityevent> Charityevents { get; set; }

        public virtual ICollection<Image> ImageIdImages { get; set; }
        public virtual ICollection<Location> LocationIdLocations { get; set; }
        public virtual ICollection<User> UserIdUsers { get; set; }
    }
}
