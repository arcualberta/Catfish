using CatfishExtensions.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.Services
{
    public class AuthApiProxy : IAuthApiProxy
    {
        private readonly ICatfishWebClient _webClient;
        private readonly IConfiguration _configuration;
        private readonly string? _apiRoot;

        public AuthApiProxy(ICatfishWebClient webClient, IConfiguration configuration)
        {
            _webClient = webClient;
            _configuration = configuration;
            _apiRoot = configuration.GetSection("SiteConfig:AuthMicroserviceUrl").Value?.TrimEnd('/');
        }

        public async Task<UserMembership> GetMembership(string username)
        {
            var membership = await _webClient.Get<UserMembership>($"{_apiRoot}/membership/{username}");

            return membership;
        }

    }
}
