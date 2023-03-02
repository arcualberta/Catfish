using CatfishExtensions.DTO;
using CatfishExtensions.Interfaces.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.Services.Auth
{
    public class TenantApiProxy : ITenantApiProxy
    {
        private readonly ICatfishWebClient _webClient;
        private readonly IConfiguration _configuration;
        private readonly string? _apiRoot;

        public TenantApiProxy(ICatfishWebClient webClient, IConfiguration configuration)
        {
            _webClient = webClient;
            _configuration = configuration;
            _apiRoot = configuration.GetSection("SiteConfig:AuthMicroserviceUrl").Value?.TrimEnd('/');
        }

        public async Task EnsureTenancy()
        {
            var tenancyConfig = _configuration.GetSection("TenancyConfig");
            if (tenancyConfig != null)
            {
                string tenantName = tenancyConfig.GetSection("Name").Value;
                var roles = tenancyConfig.GetSection("Roles").Get<string[]>();

                var tenant = await GetTenantByName(tenantName);
                if (tenant == null)
                {
                    //Creating a tenant
                    tenant = await CreateTenant(new TenantInfo() { Name = tenantName });
                }

                //Find any new roles to be added.
                List<string> newRoles = new List<string>();
                foreach (var role in roles)
                {
                    if (!tenant.Roles.Any(r => r.Name == role))
                        newRoles.Add(role);
                }

                //Find any old empty roles to be deleted.
                //Note that we will not automatically delete any roles that contain users.
                List<string> deleteRoles = new List<string>();
                foreach (var role in tenant.Roles.Where(t => !t.Users.Any()))
                {
                    if (!roles.Any(r => r == role.Name))
                        deleteRoles.Add(role.Name);
                }

                if (newRoles.Any() || deleteRoles.Any())
                {
                    var patchModel = new AuthPatchModel()
                    {
                        ParentId = tenant.Id,
                        NewChildren = newRoles.ToArray(),
                        DeleteChildren = deleteRoles.ToArray()
                    };
                    await PatchTenant(patchModel);
                }
            }
        }

        public async Task<TenantInfo> GetTenantByName(string tenantName)
            => await _webClient.Get<TenantInfo>($"{_apiRoot}/api/tenants/by-name/{tenantName}");
        public async Task<TenantInfo> CreateTenant(TenantInfo tenant)
            => await _webClient.PostJson<TenantInfo>($"{_apiRoot}/api/tenants", tenant);
        public async Task<bool> PatchTenant(AuthPatchModel patchModel)
            => (await _webClient.PatchJson($"{_apiRoot}/api/tenants", patchModel)).IsSuccessStatusCode;

        public async Task<List<TenantInfo>> GetTenants(int offset = 0, int max = int.MaxValue, string? jwtBearerToken = null)
        {
           
                var url = $"{_apiRoot}/api/Tenants?offset={offset}&max={max}";
                var result = await _webClient.Get<List<TenantInfo>>(url, jwtBearerToken);
                return result;
           
        }
        public async Task<TenantInfo> GetTenantById(Guid id, string? jwtBearerToken = null)
        {

            var url = $"{_apiRoot}/api/Tenants/{id}";
            var result = await _webClient.Get<TenantInfo>(url, jwtBearerToken);

            return result;

        }
    }
}
