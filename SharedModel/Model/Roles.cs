using System;
using System.Collections.Generic;

namespace Model
{
    public partial class Roles
    {
        public Roles()
        {
            Users = new HashSet<Users>();
        }

        public string Name { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
