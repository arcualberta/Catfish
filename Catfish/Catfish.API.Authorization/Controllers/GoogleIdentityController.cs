
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
        private readonly IJwtProcessor _jwtProcessor;
        public GoogleIdentityController(IGoogleIdentity googleIdentity, ICatfishUserManager catfishUserManager, IJwtProcessor jwtProcessor)
        {
            _googleIdentity = googleIdentity;
            _catfishUserManager = catfishUserManager;
            _jwtProcessor = jwtProcessor;
        }

        // POST api/<GoogleIdentityController>
        [HttpPost]
        //public async Task<LoginResult> Post([FromBody] string jwt) //
        public async Task<string> Post([FromBody] string jwt)
        {
            try
            {
                var externalLoginResult = await _googleIdentity.GetUserLoginResult(jwt);
                // var globalUserRoles=null;
                string jwtToken = "";
                if (externalLoginResult.Success)
                {
                    var user = await _catfishUserManager.GetUser(externalLoginResult);
                    var globalUserRoles = await _catfishUserManager.GetGlobalRoles(user);
                    externalLoginResult.Id = user.Id;
                    externalLoginResult.GlobalRoles = globalUserRoles;

                    string userData = string.Empty; //???
                    DateTime expiredAt = DateTime.Now.AddDays(1);
                    jwtToken = _jwtProcessor.CreateUserToken(externalLoginResult.Name!, globalUserRoles, userData,expiredAt);
                }

                 
                 return jwtToken;
               // return externalLoginResult;
            }
            catch(Exception ex)
            {
                return String.Empty; // new LoginResult();
            }
        }
    }
}
