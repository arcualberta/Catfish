using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.DTO
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string IdentityUserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}
