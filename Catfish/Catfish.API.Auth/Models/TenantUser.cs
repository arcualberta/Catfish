using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.API.Auth.Models
{
    public class TenantUser
    {
        public Guid Id { get; set; }
        public string IdentityUserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public TenantRole Role { get; set; }
        public Guid RoleId { get; set; }
    }
}
