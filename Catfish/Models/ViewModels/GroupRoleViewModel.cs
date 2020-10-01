using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.ViewModels
{
    public class GroupRoleViewModel
    {
        public Guid RoleId { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string RoleName { get; set; }
        public string NormalizedName { get; set; }
        public Guid GroupId { get; set; }
        public bool Checked { get; set; }
    }
}
