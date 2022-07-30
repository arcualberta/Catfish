
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


namespace Catfish.API.Authorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(CorsPolicyNames.General)]
    public class GoogleIdentityController : ControllerBase
    {
        private readonly IGoogleIdentity _googleIdentity;
        public GoogleIdentityController(IGoogleIdentity googleIdentity)
        {
            _googleIdentity = googleIdentity;
        }

        // POST api/<GoogleIdentityController>
        [HttpPost]
        public async Task<LoginResult> Post([FromBody] string jwt)
        {
            try
            {
                var result = await _googleIdentity.GetUserLoginResult(jwt);
                return result;
            }
            catch(Exception ex)
            {
                return new LoginResult();
            }
        }
    }
}
