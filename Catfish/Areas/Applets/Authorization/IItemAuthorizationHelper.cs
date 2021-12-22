using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Catfish.Areas.Applets.Authorization
{
	public interface IItemAuthorizationHelper
	{
		public bool AuthorizebyRole(Item item, ClaimsPrincipal user, string actionFunction);

	}
}
