using CatfishExtensions.DTO;
using Microsoft.AspNetCore.Mvc;


namespace CatfishExtensions.Interfaces.Auth
{
    public interface ITenantsProxy
    {
        Task<TenantInfo> GetTenantByName(string tenantName);
        Task<TenantInfo> CreateTenant(TenantInfo tenant);
        Task<bool> PatchTenant(AuthPatchModel patchModel);
        Task EnsureTenancy();
        Task<ActionResult<IEnumerable<TenantInfo>>> GetTenants(int offset = 0, int max = int.MaxValue, string? jwtBearerToken = null);
    }
}
