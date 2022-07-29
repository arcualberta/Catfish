
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
                //var handler = new JwtSecurityTokenHandler();
                //var token = handler.ReadJwtToken(jwt);
                //var issuer = token.Issuer;

                ////var publicKeyAPI = "https://www.googleapis.com/oauth2/v1/certs"; //PEM
                //var publicKeyAPI = "https://www.googleapis.com/oauth2/v3/certs"; //JWK
                //var response = await _catfishWebClient.Get(publicKeyAPI);
                //var key = await response.Content.ReadAsStringAsync();

                //var status = _jwtProcessor.ReadToken(jwt, out JwtSecurityToken? token, key);

                //if(token != null)
                //{
                //    var issuer = token.Issuer;
                //}

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
