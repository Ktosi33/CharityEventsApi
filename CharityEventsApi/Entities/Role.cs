using System;
using System.Collections.Generic;

namespace CharityEventsApi.Entities
{
    public partial class Role
    {
        public Role()
        {
            UserIdUsers = new HashSet<User>();
        }

        public string Name { get; set; } = null!;

        public virtual ICollection<User> UserIdUsers { get; set; }
    }
}
