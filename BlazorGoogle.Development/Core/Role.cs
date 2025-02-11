using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorGoogle.Development.Core.Enums;

namespace BlazorGoogle.Development.Core
{
    public class Role
    {
        public List<PermissionType> Permissions { get; set; } = [];
    }
}
