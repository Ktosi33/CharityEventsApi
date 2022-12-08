using System;
using System.Collections.Generic;

namespace CharityEventsApi.Entities
{
    public partial class Charityevent
    {
        public Charityevent()
        {
            ImageIdImages1 = new HashSet<Image>();
        }

        public int IdCharityEvent { get; set; }
        public sbyte IsActive { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int? VolunteeringIdVolunteering { get; set; }
        public int? CharityFundraisingIdCharityFundraising { get; set; }
        public int OrganizerId { get; set; }
        public sbyte IsVerified { get; set; }
        public DateTime CreatedEventDate { get; set; }
        public DateTime? EndEventDate { get; set; }
        public int ImageIdImages { get; set; }
        public sbyte IsDenied { get; set; }

        public virtual Charityfundraising? CharityFundraisingIdCharityFundraisingNavigation { get; set; }
        public virtual Image ImageIdImagesNavigation { get; set; } = null!;
        public virtual User Organizer { get; set; } = null!;
        public virtual Volunteering? VolunteeringIdVolunteeringNavigation { get; set; }

        public virtual ICollection<Image> ImageIdImages1 { get; set; }
    }
}
