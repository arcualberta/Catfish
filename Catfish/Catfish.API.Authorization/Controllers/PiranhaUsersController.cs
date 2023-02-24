using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CatfishExtensions.DTO;
using Catfish.API.Authorization.Interfaces;

namespace Catfish.API.Authorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(CorsPolicyNames.General)]
    public class PiranhaUsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public PiranhaUsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        //[Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult<IEnumerable<UserInfo>>> Get()
        {
            return await _userService.GetUsers();
        }
    }
}
