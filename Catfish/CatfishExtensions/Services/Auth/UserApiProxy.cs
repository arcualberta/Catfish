//using CatfishExtensions.DTO;
using ARC.Security.Lib.DTO;
using ARC.Security.Lib.Google.Interfaces;
using CatfishExtensions.Interfaces.Auth;

using System.Net;


namespace CatfishExtensions.Services.Auth
{
    public class UserApiProxy : IUserApiProxy
    {
        private readonly ICatfishWebClient _webClient;
        private readonly IConfiguration _configuration;
        private readonly string? _apiRoot;

        public UserApiProxy(ICatfishWebClient webClient, IConfiguration configuration)
        {
            _webClient = webClient;
            _configuration = configuration;
            _apiRoot = configuration.GetSection("SiteConfig:AuthMicroserviceUrl").Value?.TrimEnd('/');
        }

        public async Task<string> Login(string username, string password)
        {
            var response = await _webClient.PostJson($"{_apiRoot}/api/users/login", new LoginModel() { UserName = username, Password = password });
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            else
                return "";
        }

        public async Task<bool> Register(RegistrationModel model)
            => (await _webClient.PostJson($"{_apiRoot}/api/users", model)).StatusCode == HttpStatusCode.OK;

        public async Task<UserMembershipDto> GetMembership(string username)
            => await _webClient.Get<UserMembershipDto>($"{_apiRoot}/api/users/membership/{username}");

        public async Task<List<UserInfo>> GetUsers(int offset = 0, int max = int.MaxValue, string? jwtBearerToken = null)
          => await _webClient.Get<List<UserInfo>>($"{_apiRoot}/api/users?offset={offset}&max={max}", jwtBearerToken);

        public async Task<bool> PutUser(UserInfo dto, string? jwtToken = null)
         => (await _webClient.PutJson($"{_apiRoot}/api/users/", dto, jwtToken)).IsSuccessStatusCode;

        public async Task<bool> DeleteUser(string userName, string? jwtToken = null)
         => (await _webClient.Delete($"{_apiRoot}/api/users/{userName}", jwtToken)).IsSuccessStatusCode;

    }
}
