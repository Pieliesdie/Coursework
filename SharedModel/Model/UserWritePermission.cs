using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Model
{
    public class UserWritePermission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short Level { get; set; }
    }
}
