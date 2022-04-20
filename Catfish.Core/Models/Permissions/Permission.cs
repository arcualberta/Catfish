using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Permissions
{
    public class Permission
    {
        public string Action { get; set; }
        public bool IsChild { get; set; }
    }
}
