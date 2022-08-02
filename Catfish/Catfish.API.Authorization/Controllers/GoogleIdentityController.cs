
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860



namespace Catfish.API.Authorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(CorsPolicyNames.General)]
    public class GoogleIdentityController : ControllerBase
    {
        private readonly IGoogleIdentity _googleIdentity;
        private readonly ICatfishUserManager _catfishUserManager;
        public GoogleIdentityController(IGoogleIdentity googleIdentity, ICatfishUserManager catfishUserManager)
        {
            _googleIdentity = googleIdentity;
            _catfishUserManager = catfishUserManager;   
        }

        // POST api/<GoogleIdentityController>
        [HttpPost]
        public async Task<LoginResult> Post([FromBody] string jwt)
        {
            try
            {
                var externalLoginResult = await _googleIdentity.GetUserLoginResult(jwt);
                if (externalLoginResult.Success)
                {
                    var user = await _catfishUserManager.GetUser(externalLoginResult);
                    var globalUserRoles = await _catfishUserManager.GetGlobalRoles(user);
                    externalLoginResult.Id = user.Id;
                    externalLoginResult.GlobalRoles = globalUserRoles;
                }
                return externalLoginResult;
            }
            catch(Exception ex)
            {
                return new LoginResult();
            }
        }
    }
}
