

using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace CatfishExtensions.Services
{
    public class JwtProcessor : IJwtProcessor
    {
        private readonly IConfiguration _configuration;
        private readonly string _privateKey;
        private readonly string _issuer;
        private readonly string _audience;
        public JwtProcessor(IConfiguration configuration)
        {
            _configuration = configuration;
            _privateKey = _configuration.GetSection("JwtConfig:RsaPrivateKey").Value;
            _issuer = _configuration.GetSection("JwtConfig:Issuer").Value;
            _audience = _configuration.GetSection("JwtConfig:Audience").Value;

        }

        public JwtSecurityToken ReadToken(string jwt) 
            => (new JwtSecurityTokenHandler()).ReadJwtToken(jwt);
        
        public JwtSecurityToken? ReadToken(string jwt, JsonWebKey publicKeyJWK, TokenValidationParameters tokenValidationparams)
        {
            var handler = new JwtSecurityTokenHandler();
            handler.ValidateToken(jwt, tokenValidationparams, out SecurityToken validatedToken);
            return validatedToken as JwtSecurityToken;
        }

        //public string CreateUserToken(string username, IList<string> roles, string userData, DateTime expiresAt)
        //{
        //    throw new NotImplementedException();
        //}

        //public string CreateToken(List<Claim> claims, DateTime expiresAt)
        //{
        //    throw new NotImplementedException();
        //}
        public string CreateUserToken(string userName, IList<string> userRoles, string userData, DateTime expiresAt)
        {
            var authClaims = new List<Claim>
            {
                new Claim("username", userName),
                new Claim("userdata", userData),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
                authClaims.Add(new Claim("roles", userRole));


            return CreateToken(authClaims, expiresAt);
        }
        public string GenerateJSonWebToken(LoginResult userInfo)
        {
            if (userInfo == null)
            {
                return string.Empty;
            }

            string userData = string.Empty; //???
            DateTime expiredAt = DateTime.Now.AddDays(1);
            return CreateUserToken(userInfo.Name, userInfo.GlobalRoles, userData, expiredAt);



        }
        public string CreateToken(List<Claim> authClaims, DateTime expiresAt)
        {
            RSA rsa = RSA.Create();

            rsa.ImportRSAPrivateKey( // Convert the loaded key from base64 to bytes.
                source: Convert.FromBase64String(_privateKey), // Use the private key to sign tokens
                bytesRead: out int _); // Discard the out variable 

            var signingCredentials = new SigningCredentials(
                key: new RsaSecurityKey(rsa),
                algorithm: SecurityAlgorithms.RsaSha256 // Important to use RSA version of the SHA algo 
            );

            DateTime jwtDate = DateTime.Now;
           // var issuer = _configuration.GetSection("Google:Identity:Issuer").Value;
            //var audience = _configuration.GetSection("Google:ClientId").Value;

            var token = new JwtSecurityToken(
                audience: _audience,
                issuer: _issuer,
                claims: authClaims,
                notBefore: DateTime.Now,
                expires: expiresAt,
                signingCredentials: signingCredentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }


    }
}
