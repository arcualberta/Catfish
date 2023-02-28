using CatfishExtensions.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.Interfaces
{
    public interface ITenantApiProxy
    {
        Task<TenantInfo> GetTenantByName(string tenantName);
        Task<TenantInfo> CreateTenant(TenantInfo tenant);
        Task<bool> PatchTenant(AuthPatchModel patchModel);
        Task EnsureTenancy();
    }
}
