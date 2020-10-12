using Piranha.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Catfish.Core.Models
{
    [Table("Catfish_UserGroupRoles")]
    public class UserGroupRole
    {
        public Guid Id { get; set; }
        //public Guid GroupId { get; set; }
        public Guid GroupRoleId { get; set; }
        public GroupRole GroupRole { get; set; }
        public Guid UserId { get; set; }

        [NotMapped]
        public User User { get; set; }
    }
}
