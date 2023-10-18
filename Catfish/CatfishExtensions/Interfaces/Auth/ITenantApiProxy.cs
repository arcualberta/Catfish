
using ARC.Security.Lib.DTO;


namespace CatfishExtensions.Interfaces.Auth
{
    public interface ITenantApiProxy
    {
        Task<TenantInfo> GetTenantByName(string tenantName, string? jwtBearerToken = null);
        Task<TenantInfo> CreateTenant(TenantInfo tenant);
        Task<bool> PatchTenant(AuthPatchModel patchModel, string? jwtBearerToken = null);
        Task EnsureTenancy();
        Task<List<TenantInfo>> GetTenants(int offset = 0, int max = int.MaxValue, bool includeRoles = false, string? jwtBearerToken = null);
        Task<TenantInfo> GetTenantById(Guid id, string? jwtBearerToken = null);
        Task<TenantInfo> PostTenant(TenantInfo dto, string? jwtToken = null);

        Task<bool> PutTenant(TenantInfo dto, string? jwtToken = null);
        Task<bool> DeleteTenant(Guid id, string? jwtToken = null);
    }
}
