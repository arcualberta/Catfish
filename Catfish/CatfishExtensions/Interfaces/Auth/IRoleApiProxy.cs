using CatfishExtensions.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.Interfaces.Auth
{
    public interface IRoleApiProxy
    {
        Task<List<TenantRoleInfo>> GetRoles(int offset = 0, int max = int.MaxValue, string? jwtBearerToken = null);
        Task<bool> PatchRole(AuthPatchModel dto, string? jwtBearerToken = null);
       
        Task<bool> PutRole(TenantRoleInfo dto, string? jwtToken = null);
       
    }
}
