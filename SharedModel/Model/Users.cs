using System;
using System.Collections.Generic;

namespace Model
{
    public partial class Users
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int? Permission { get; set; }

        public virtual SecurityLabels PermissionNavigation { get; set; }
        public virtual Roles RoleNavigation { get; set; }
    }
}
