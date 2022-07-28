
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using Microsoft.IdentityModel.Tokens;

namespace Catfish.API.Authorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(CorsPolicyNames.General)]
    public class GoogleIdentityController : ControllerBase
    {
        // POST api/<GoogleIdentityController>
        [HttpPost]
        public void Post([FromBody] string jwt)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwt);
                var issuer = token.Issuer;

                var parameters = new TokenValidationParameters();
                handler.ValidateToken(jwt, parameters, out SecurityToken validatedToken);

            }
            catch(Exception ex)
            {

            }
        }
    }
}
