﻿

using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

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
            var keys = await GetPublicKeys();
            if (keys.Length == 0)
                return new LoginResult();

            var issuer = _configuration.GetSection("Google:Identity:Issuer").Value;
            var audience = _configuration.GetSection("Google:ClientId").Value;

            var tokenValidationParams = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = !string.IsNullOrEmpty(issuer),
                ValidateAudience = !string.IsNullOrEmpty(audience),
                ValidateLifetime = false,
                ValidIssuer = issuer,
                ValidAudience = audience
            };

            JwtSecurityToken? token = null;
            foreach (var key in keys)
            {
                //We have to simply try each key until a successful key is found or all keys were tried.
                try
                {
                    tokenValidationParams.IssuerSigningKey = key;
                    token = _jwtProcessor.ReadToken(jwt, key, tokenValidationParams);

                    if (token != null)
                    {
                        var result = new LoginResult()
                        {
                            Success = true,
                            Email = token?.Payload.FirstOrDefault(pair => pair.Key == "email").Value as string,
                            Name = token?.Payload.FirstOrDefault(pair => pair.Key == "name").Value as string
                        };

                        return result;
                    }
                    break;
                }
                catch (Exception)
                {
                }
            }

            return new LoginResult() { Success = false };
        }

        #endregion

        #region Private Methods
        private async Task<JsonWebKey[]> GetPublicKeys()
        {
            var publicKeyApi = _configuration.GetSection("Google:Identity:PublicKeyApiJwk").Value;

            if (string.IsNullOrEmpty(publicKeyApi))
                throw new CatfishException("Googke Identity Public Key API not defined in appsettings.json");

            var response = await _catfishWebClient.Get(publicKeyApi);
            var jwkPublicKeyStrings = await response.Content.ReadAsStringAsync();

            JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var wrapper = JsonSerializer.Deserialize<KeyWrapper>(jwkPublicKeyStrings, options);

            return wrapper == null ? new JsonWebKey[0] : wrapper.keys;
        }

        private string GetPrivateKey()
        {
           return  _configuration.GetSection("Google:Identity:rsa_privateKey").Value;
        }

       
        #endregion
    }

    internal class KeyWrapper
    {
        public JsonWebKey[] keys { get; set; }
    }
}
