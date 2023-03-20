using Catfish.API.Auth.Interfaces;
using CatfishExtensions.Constants;
using CatfishExtensions.DTO;
using CatfishExtensions.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Catfish.API.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(CorsPolicyNames.General)]
    public class UsersController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAuthService _authService;

        public UsersController(IAccountService accountService, IAuthService authService)
        {
            _accountService = accountService;
            _authService = authService;
        }

        [HttpPost("seed")]
        public async Task<ActionResult> Seed(RegistrationModel model)
        {
            try
            {
                await _accountService.Seed(model);
                return NoContent();
            }
            catch(CatfishException ex)
            {
                return StatusCode((int)ex.HttpStatusCode, ex.Message);
            }
            catch(Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetUsers(int offset = 0, int max = int.MaxValue)
        {
            try
            {
                var ret = await _accountService.GetUsers(offset, max);
                return Ok(ret);
            }
            catch (CatfishException ex)
            {
                return StatusCode((int)ex.HttpStatusCode, ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("membership/{username}")]
        public async Task<ActionResult<UserMembership>> GeMembership(string username)
        {
            try
            {
                var user = await _accountService.GetUser(username);
                var membership = await _authService.GetMembership(user);

                return Ok(membership);
            }
            catch (CatfishException ex)
            {
                return StatusCode((int)ex.HttpStatusCode, ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostUser(RegistrationModel model)
        {
            try
            {
                if (User?.IsInRole("SysAdmin") != true && model.SystemRoles.Any())
                    return Unauthorized("System roles can only be specified by Authorization Service Administrators.");
                
                await _accountService.CreateUser(model);
                return Ok();
            }
            catch (CatfishException ex)
            {
                return StatusCode((int)ex.HttpStatusCode, ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        [Authorize(Roles = "SysAdmin")]
        public async Task<IActionResult> PutUser(ChangePasswordModel model)
        {
            try
            {
                var roles = User.Identities.FirstOrDefault()?.Claims.Where(x => x.Type.EndsWith("role")).Select(x => x.Value).ToList();
                bool isAdmin = roles?.Contains("Admin") == true;

                await _accountService.ChangePassword(model, isAdmin);
                return Ok();
            }
            catch (CatfishException ex)
            {
                return StatusCode((int)ex.HttpStatusCode, ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{username}")]
        [Authorize(Roles = "SysAdmin")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            try
            {
                await _accountService.DeleteUser(username);
                return Ok();
            }
            catch (CatfishException ex)
            {
                return StatusCode((int)ex.HttpStatusCode, ex.Message);
            }
            catch (Exception)
            {
                throw;
            }

        }
       
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginModel model)
        {
            try
            {
                string token = await _accountService.Login(model);
                return Ok(token);
            }
            catch (CatfishException ex)
            {
                return StatusCode((int)ex.HttpStatusCode, ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
