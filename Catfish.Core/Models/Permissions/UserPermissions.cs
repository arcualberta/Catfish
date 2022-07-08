using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Permissions
{
    public class UserPermissions
    {
        public Guid FormId { get; set; }
        public string FormType { get; set; }
        public List<Permission> Permissions { get; set; } 
    }
}
