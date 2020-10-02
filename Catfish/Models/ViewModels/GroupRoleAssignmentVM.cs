using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.ViewModels
{
    public class GroupRoleAssignmentVM
    {
        public Guid RoleId { get; set; }
        public Guid RoleGroupId { get; set; }
        public string RoleName { get; set; }
        public bool Assigned { get; set; }
    }
}
