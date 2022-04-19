using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Permissions
{
    public class UserPermissions
    {
        public Guid FormId { get; set; }
        public string FormType { get; set; }
        public string[] Permissions { get; set; } = new string[] { };
    }
}
