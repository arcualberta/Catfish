using CatfishExtensions.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Piranha.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Services
{
    public class CatfishSecurity : ISecurity, ICatfishSecurity
    {
        private readonly ICatfishWebClient _catfishWebClient = new CatfishWebClient();
        private string _authUrlRoot;
        private IJwtProcessor _jwtProcessor;
        public CatfishSecurity()
        {
        }

        public void SetAuthApiRoot(string url) => _authUrlRoot = url.TrimEnd('/');
        public void SetJwtprocessor(IJwtProcessor processor) => _jwtProcessor = processor;

        public async Task<bool> SignIn(object context, string username, string password)
        {
            var httpContext =  context as HttpContext;
            if (httpContext == null) return false;

            var loginUrl = $"{_authUrlRoot}/api/users/login";
            var response = await _catfishWebClient.PostJson(loginUrl, new LoginModel { UserName= username, Password = password });
            if (response.IsSuccessStatusCode)
            {
                var jwt = await response.Content.ReadAsStringAsync();

                var jwtSecurityToken = _jwtProcessor.ReadToken(jwt);

                if (jwtSecurityToken == null) return false;

                User user = new User() { UserName = username };
                var claims = new List<Claim>();
                foreach (var claim in jwtSecurityToken.Claims)
                {
                    claims.Add(new Claim(claim.Type, claim.Value));
                }
                claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                claims.Add(new Claim("role", "Admin"));
                //claims.Add(new Claim(ClaimTypes.Sid, user.Id));

                var identity = new ClaimsIdentity(claims, "Catfish.Custom");              

                var principle = new ClaimsPrincipal(identity);
                try
                {
                    await ((HttpContext)context).SignInAsync("Catfish.Custom", principle);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }


                /*
                var user = Users
                                .Single(u => u.UserName == username && u.Password == password);

                var claims = new List<Claim>();
                foreach (var claim in user.Claims)
                {
                    claims.Add(new Claim(claim, claim));
                }
                claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                claims.Add(new Claim(ClaimTypes.Sid, user.Id));

                var identity = new ClaimsIdentity(claims, user.Password);
                var principle = new ClaimsPrincipal(identity);

                await ((HttpContext)context)
                    .SignInAsync("Piranha.SimpleSecurity", principle);

                return true;
                */
            }
            else
                return false;
        }

        public async Task SignOut(object context)
        {
            throw new NotImplementedException();
        }

    }
}
