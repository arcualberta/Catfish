using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.ViewModels
{
    public class AssignedRoleDataViewModel
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Assigned { get; set; }
    }
}
