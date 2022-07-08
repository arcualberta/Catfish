using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Catfish.Areas.Applets.Authorization
{
	public class ItemAuthorizationHelper : IItemAuthorizationHelper
	{
		private readonly AppDbContext _appDb;
        private readonly Piranha.AspNetCore.Identity.IDb _piranhaDb;
		public ItemAuthorizationHelper(AppDbContext appDb, Piranha.AspNetCore.Identity.IDb piranhaDb)
		{
			_appDb = appDb;
			_piranhaDb = piranhaDb;
		}
		public bool AuthorizebyRole(Item item, ClaimsPrincipal user, string actionFunction)
		{
			if (item == null)
				return false;

			var action = item.Template.Workflow.Actions.FirstOrDefault(ac => ac.Function == actionFunction);

			//No read action ground
			if (action == null)
				return false;

			//Publicly readable
			if (action.Access == GetAction.eAccess.Public)
				return true;

			//Not publicly readable, so we need the user to be logged in
			if (!user.Identity.IsAuthenticated)
				return false;

			//Authorize system admins
			if (user.IsInRole("SysAdmin"))
				return true;

			//Get the state-reference definition that is corresponding to the item state from the read action
			var stateRef = action.States.FirstOrDefault(st => st.RefId == item.StatusId);

			//Cannot proceed without having a matching state
			if (stateRef == null)
				return false;

			var authorizedRoleIds = stateRef.AuthorizedRoles.Where(ar => !ar.Owner).Select(ar => ar.RefId).ToList();
			if (authorizedRoleIds.Count == 0)
				return false;

			var loginUser = _piranhaDb.Users.Where(u => u.UserName == user.Identity.Name).FirstOrDefault();
			Guid userId = loginUser.Id;

			List<Guid> userRoleIds;
			if (item.GroupId.HasValue) //The item belongs to a group
			{
				//Select the Role IDs of the roles where the user is assicated within the specified group.
				userRoleIds = _appDb.UserGroupRoles
					.Where(ugr => ugr.GroupRole.GroupId == item.GroupId && ugr.UserId == userId)
					.Select(ugr => ugr.GroupRole.RoleId)
					.ToList();

			}
			else //The item does not belong to a group.
			{
				//Select the Role IDs of the roles where the user is assicated globally
				userRoleIds = _piranhaDb.UserRoles
					.Where(ur => ur.UserId == userId)
					.Select(ur => ur.RoleId).ToList();
			}

			//The user is authorized if the user holds at least one authorized role ID
			return authorizedRoleIds.Intersect(userRoleIds).Any();
		}
	}
}
