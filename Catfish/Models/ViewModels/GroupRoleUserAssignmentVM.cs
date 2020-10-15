using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.ViewModels
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
