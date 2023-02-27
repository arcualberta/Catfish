
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860



using Catfish.API.Auth.Interfaces;
using Catfish.API.Auth.Models;
using CatfishExtensions.Constants;
using CatfishExtensions.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Catfish.API.Authorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(CorsPolicyNames.General)]
    public class GoogleIdentityController : ControllerBase
    {
        private readonly IGoogleIdentity _googleIdentity;
        private readonly IJwtProcessor _jwtProcessor;
        private readonly AuthDbContext _authDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAccountService _accountService;

        public GoogleIdentityController(IGoogleIdentity googleIdentity, IJwtProcessor jwtProcessor, AuthDbContext authDbContext, UserManager<IdentityUser> userManager, IAccountService accountService)
        {
            _googleIdentity = googleIdentity;
            _jwtProcessor = jwtProcessor;
            _authDbContext = authDbContext;
            _userManager = userManager;
            _accountService = accountService;
        }

        // POST api/<GoogleIdentityController>
        [HttpPost]
        //public async Task<LoginResult> Post([FromBody] string jwt) //
        public async Task<string> Post([FromBody] string jwt)
        {
            try
            {
                var externalLoginResult = await _googleIdentity.GetUserLoginResult(jwt);
                var jwtToken = await _accountService.GetSignedToken(externalLoginResult);                
                return jwtToken;
            }
            catch(Exception)
            {
                return String.Empty; // new LoginResult();
            }
        }
    }
}
