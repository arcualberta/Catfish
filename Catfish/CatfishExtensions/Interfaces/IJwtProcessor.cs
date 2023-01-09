namespace CatfishExtensions.Interfaces
{
    public interface IJwtProcessor
    {
        public JwtSecurityToken ReadToken(string jwt);
        public JwtSecurityToken? ReadToken(string jwt, JsonWebKey publicKeyJWK, TokenValidationParameters tokenValidationparams);
        public string CreateUserToken(string username, IList<string> roles, string userData, DateTime expiresAt);
        public string CreateToken(List<Claim> claims, DateTime expiresAt);
    }
}
