using System;
using System.Collections.Generic;

namespace CharityEventsApi.Entities
{
    public partial class Image
    {
        public Image()
        {
            Charityevents = new HashSet<Charityevent>();
            CharityEventIdCharityEvents = new HashSet<Charityevent>();
        }

        public int IdImages { get; set; }
        public string Path { get; set; } = null!;
        public string ContentType { get; set; } = null!;

        public virtual ICollection<Charityevent> Charityevents { get; set; }

        public virtual ICollection<Charityevent> CharityEventIdCharityEvents { get; set; }
    }
}
