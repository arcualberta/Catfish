

namespace CatfishExtensions.Services
{
    public class JwtProcessor : IJwtProcessor
    {
        public JwtSecurityToken ReadToken(string jwt) 
            => (new JwtSecurityTokenHandler()).ReadJwtToken(jwt);
        
        public JwtSecurityToken? ReadToken(string jwt, JsonWebKey publicKeyJWK, TokenValidationParameters tokenValidationparams)
        {
            var handler = new JwtSecurityTokenHandler();
            handler.ValidateToken(jwt, tokenValidationparams, out SecurityToken validatedToken);
            return validatedToken as JwtSecurityToken;
        }

    }
}
