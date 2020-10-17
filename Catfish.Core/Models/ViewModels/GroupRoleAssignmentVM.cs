using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.ViewModels
{
    public class GroupRoleAssignmentVM
    {
        public Guid RoleId { get; set; }
        public Guid? RoleGroupId { get; set; }
        public string RoleName { get; set; }
        public bool Assigned { get; set; }
        public bool HasUsers { get; set; }
    }
}
