using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.DTO
{
    public class UserMembership
    {
        public UserInfo User { get; set; }
        public List<TenantInfo> Tenancy { get; set; } = new List<TenantInfo>();
    }
}
