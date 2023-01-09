

using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace CatfishExtensions.Services
{
    public class JwtProcessor : IJwtProcessor
    {
        private readonly IConfiguration _configuration;
        public JwtProcessor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtSecurityToken ReadToken(string jwt) 
            => (new JwtSecurityTokenHandler()).ReadJwtToken(jwt);
        
        public JwtSecurityToken? ReadToken(string jwt, JsonWebKey publicKeyJWK, TokenValidationParameters tokenValidationparams)
        {
            var handler = new JwtSecurityTokenHandler();
            handler.ValidateToken(jwt, tokenValidationparams, out SecurityToken validatedToken);
            return validatedToken as JwtSecurityToken;
        }

        public string CreateUserToken(string username, IList<string> roles, string userData, DateTime expiresAt)
        {
            throw new NotImplementedException();
        }

        public string CreateToken(List<Claim> claims, DateTime expiresAt)
        {
            throw new NotImplementedException();
        }


    }
}
