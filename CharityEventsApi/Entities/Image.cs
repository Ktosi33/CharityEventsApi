using System;
using System.Collections.Generic;

namespace CharityEventsApi.Entities
{
    public partial class Image
    {
        public Image()
        {
            CharityEvents = new HashSet<CharityEvent>();
            IdCharityEvents = new HashSet<CharityEvent>();
        }

        public int IdImage { get; set; }
        public string Path { get; set; } = null!;
        public string ContentType { get; set; } = null!;

        public virtual ICollection<CharityEvent> CharityEvents { get; set; }

        public virtual ICollection<CharityEvent> IdCharityEvents { get; set; }
    }
}
