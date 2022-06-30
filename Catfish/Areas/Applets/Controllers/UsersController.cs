using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Areas.Applets.Models.User;
using Microsoft.AspNetCore.Identity;
using Piranha.AspNetCore.Identity.Data;
using Catfish.Core.Authorization.Handlers;
using Microsoft.AspNetCore.Cors;

namespace Catfish.Areas.Applets.Controllers
{
	[Route("applets/api/[controller]")]
	[ApiController]
	[EnableCors("CatfishApiPolicy")]
	public class UsersController : ControllerBase
	{
        private readonly SignInManager<User> _signInManager;
		public UsersController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
		[Route("current")]
		public UserInfo CurrentUser()
		{
			UserInfo info = new UserInfo()
			{
				UserName = User.Identity?.Name
			};

			if (User.IsInRole("SysAdmin"))
				info.Roles = new string[] { "SysAdmin" };

			return info;
		}

		[HttpPost]
		[Route("logout")]
		public async Task<UserInfo> Logout()
		{
			await _signInManager.SignOutAsync();
			return new UserInfo();
		}

	}
}
