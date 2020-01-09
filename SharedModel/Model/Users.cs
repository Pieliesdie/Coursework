using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Model
{
    public partial class Users
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int? Permission { get; set; }

        [JsonIgnore]
        public virtual SecurityLabels PermissionNavigation { get; set; }
        [JsonIgnore]
        public virtual Roles RoleNavigation { get; set; }
    }
}
