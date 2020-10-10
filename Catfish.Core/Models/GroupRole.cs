using Piranha.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Catfish.Core.Models
{
    [Table("Catfish_GroupRoles")]
    public class GroupRole
    {
        public static readonly string GroupAdmin = "GroupAdmin";
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
        public Guid RoleId { get; set; }

        //[NotMapped]
        //public Role Role { get; set; }
    }


    public class GroupRoleComparer : IEqualityComparer<GroupRole>
    {
        public bool Equals(GroupRole x, GroupRole y)
        {
            if (x.RoleId == y.RoleId && x.GroupId == y.GroupId)
                return true;

            return false;
        }

        public int GetHashCode(GroupRole obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
