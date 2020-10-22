using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.ViewModels
{
    public class GroupRoleUserAssignmentVM
    {
        public Guid GroupRoleUserId { get; set; }
        public Guid RoleGroupId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool Assigned { get; set; }
    }
}
