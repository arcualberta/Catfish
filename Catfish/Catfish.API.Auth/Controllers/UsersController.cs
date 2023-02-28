using Catfish.API.Auth.Interfaces;
using CatfishExtensions.DTO;
using CatfishExtensions.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Catfish.API.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public UsersController(IAccountService accountService)
        {
            _accountService = accountService;
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
                //
                //int offset = 0; int max = int.MaxValue;
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostUser(RegistrationModel model)
        {
            try
            {
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
        [Authorize(Roles = "Admin")]
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
        [HttpDelete]
        [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> Login([FromBody] LoginModel model)
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
