using CatfishExtensions.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public async Task<string> Login(string username, string password)
        {
            var response = await _webClient.PostJson($"{_apiRoot}/api/users/login", new LoginModel() { UserName = username, Password = password });
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> Register(RegistrationModel model)
            => (await _webClient.PostJson($"{_apiRoot}/api/users", model)).StatusCode == HttpStatusCode.OK;

        public async Task<UserMembership> GetMembership(string username)
            => await _webClient.Get<UserMembership>($"{_apiRoot}/api/users/membership/{username}");

    }
}
