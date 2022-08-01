

namespace CatfishExtensions.Services
{
    public class GoogleIdentity : IGoogleIdentity
    {
        private readonly ICatfishWebClient _catfishWebClient;
        private readonly IJwtProcessor _jwtProcessor;
        private readonly IConfiguration _configuration;
        public GoogleIdentity(ICatfishWebClient catfishWebClient, IJwtProcessor jwtProcessor, IConfiguration configuration)
        {
            _catfishWebClient = catfishWebClient;
            _jwtProcessor = jwtProcessor;
            _configuration = configuration;
        }

        #region Public Methods
        public async Task<LoginResult> GetUserLoginResult(string jwt)
        {
            var key = await GetPublicKey();
            if (key == null)
                return new LoginResult();

            var issuer = _configuration.GetSection("Google:Identity:Issuer").Value;
            var audience = _configuration.GetSection("Google:Identity:Audience").Value;

            var tokenValidationParams = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = !string.IsNullOrEmpty(issuer),
                ValidateAudience = !string.IsNullOrEmpty(audience),
                ValidateLifetime = false,
                ValidIssuer = issuer,
                ValidAudience = audience
            };

            var token = _jwtProcessor.ReadToken(jwt, key, tokenValidationParams);

            var result = new LoginResult()
            {
                Success = true,
                Email = token?.Payload.FirstOrDefault(pair => pair.Key == "email").Value as string,
                Name = token?.Payload.FirstOrDefault(pair => pair.Key == "name").Value as string
            };

            return result;
        }

        #endregion

        #region Private Methods
        private async Task<JsonWebKey?> GetPublicKey()
        {
            var publicKeyApi = _configuration.GetSection("Google:Identity:PublicKeyApiJwk").Value;
            var alg = _configuration.GetSection("Google:Identity:Alg").Value;

            if (string.IsNullOrEmpty(publicKeyApi))
                throw new CatfishException("Googke Identity Public Key API not defined in appsettings.json");

            var response = await _catfishWebClient.Get(publicKeyApi);
            var jwkPublicKeyStrings = await response.Content.ReadAsStringAsync();

            JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var keys = JsonSerializer.Deserialize<KeyWrapper>(jwkPublicKeyStrings, options);
            var key = keys?.keys.FirstOrDefault(key => key.Alg == alg);
            return key;
        }
        #endregion
    }

    internal class KeyWrapper
    {
        public JsonWebKey[] keys { get; set; }
    }
}
