using System;
using System.Collections.Generic;

namespace Model
{
    public partial class SecurityLabels
    {
        public SecurityLabels()
        {
            Articles = new HashSet<Articles>();
            Authors = new HashSet<Authors>();
            Comments = new HashSet<Comments>();
            DocumentTypes = new HashSet<DocumentTypes>();
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public short? Level { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }

        public virtual ICollection<Articles> Articles { get; set; }
        public virtual ICollection<Authors> Authors { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<DocumentTypes> DocumentTypes { get; set; }
        public virtual ICollection<Users> Users { get; set; }
    }
}
