using System;
using System.Collections.Generic;

namespace CharityEventsApi.Entities
{
    public partial class CharityEvent
    {
        public CharityEvent()
        {
            IdImages = new HashSet<Image>();
        }

        public int IdCharityEvent { get; set; }
        public sbyte IsActive { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int? IdCharityVolunteering { get; set; }
        public int? IdCharityFundraising { get; set; }
        public int IdOrganizer { get; set; }
        public sbyte IsVerified { get; set; }
        public DateTime CreatedEventDate { get; set; }
        public DateTime? EndEventDate { get; set; }
        public int IdImage { get; set; }
        public sbyte IsDenied { get; set; }

        public virtual CharityFundraising? IdCharityFundraisingNavigation { get; set; }
        public virtual Image IdImageNavigation { get; set; } = null!;
        public virtual User IdOrganizerNavigation { get; set; } = null!;
        public virtual CharityVolunteering? IdCharityVolunteeringNavigation { get; set; }

        public virtual ICollection<Image> IdImages { get; set; }
    }
}
