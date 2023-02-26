using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.DTO
{
    public class TenantRoleInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; } = "";
        public IList<UserInfo>? Users { get; protected set; } = new List<UserInfo>();
    }
}
