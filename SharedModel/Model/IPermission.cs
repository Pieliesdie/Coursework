using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model
{
    public interface IPermission
    {
        int Id { get; set; }
        int? SecurityLabel { get; set; }
    }
}
