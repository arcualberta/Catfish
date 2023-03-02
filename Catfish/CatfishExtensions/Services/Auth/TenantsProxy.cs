using CatfishExtensions.DTO;
using CatfishExtensions.Interfaces.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.Services.Auth
{
    public class TenantsProxy : ITenantsProxy
    {
        private readonly ICatfishWebClient _webClient;
        private readonly IConfiguration _configuration;
        private readonly string? _apiRoot;

        public TenantsProxy(ICatfishWebClient webClient, IConfiguration configuration)
        {
            _webClient = webClient;
            _configuration = configuration;
            _apiRoot = configuration.GetSection("SiteConfig:AuthMicroserviceUrl").Value?.TrimEnd('/');
        }

        public async Task<ActionResult<IEnumerable<TenantInfo>>> GetTenants(int offset = 0, int max = int.MaxValue, string? jwtBearerToken = null)
        {
            var url = $"{_apiRoot}/api/Tenants?offset={offset}&max={max}";
            var result = await _webClient.Get<List<TenantInfo>>(url, jwtBearerToken);
            return result;
        }
    }
}
