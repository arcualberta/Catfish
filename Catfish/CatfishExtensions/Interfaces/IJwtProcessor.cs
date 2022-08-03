namespace CatfishExtensions.Interfaces
{
    public interface IJwtProcessor
    {
        public JwtSecurityToken ReadToken(string jwt);
        public JwtSecurityToken? ReadToken(string jwt, JsonWebKey publicKeyJWK, TokenValidationParameters tokenValidationparams);
    }
}
