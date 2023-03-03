using CatfishExtensions.DTO;
using CatfishExtensions.Interfaces.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.Services.Auth
{
    public class RoleApiProxy : IRoleApiProxy
    {
        private readonly ICatfishWebClient _webClient;
        private readonly IConfiguration _configuration;
        private readonly string? _apiRoot;

        public RoleApiProxy(ICatfishWebClient webClient, IConfiguration configuration)
        {
            _webClient = webClient;
            _configuration = configuration;
            _apiRoot = configuration.GetSection("SiteConfig:AuthMicroserviceUrl").Value?.TrimEnd('/');
        }

        public async Task<List<TenantRoleInfo>> GetRoles(int offset = 0, int max = int.MaxValue, string? jwtBearerToken = null)
            => await _webClient.Get<List<TenantRoleInfo>>($"{_apiRoot}/api/roles?offset={offset}&max={max}", jwtBearerToken);

        public async Task<bool> PatchRole(AuthPatchModel patchModel, string? jwtBearerToken = null)
            => (await _webClient.PatchJson($"{_apiRoot}/api/roles", patchModel, jwtBearerToken)).IsSuccessStatusCode;

      
        public async Task<bool> PutRole(TenantRoleInfo dto, string? jwtToken = null)
            => (await _webClient.PutJson($"{_apiRoot}/api/roles/", dto, jwtToken)).IsSuccessStatusCode;


    }
}
