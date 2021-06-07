using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Models.ViewModels;
using Catfish.Core.Services;
using Catfish.Helper;
using Catfish.Models.ViewModels;
using Catfish.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha;
using Piranha.AspNetCore.Identity.Data;
using Piranha.AspNetCore.Identity.Models;
using Piranha.AspNetCore.Identity.SQLServer;

namespace Catfish.Areas.Manager.Pages
{
    public class GroupUserAddModel : PageModel
    {
        private IGroupService _srv;
        public readonly AppDbContext _appDb;
        public readonly IdentitySQLServerDb _piranhaDb;
        private readonly UserManager<User> _userManager;
        private readonly ICatfishAppConfiguration _catfishConfig;
       

        [BindProperty]
        public GroupRole GroupRole { get; set; }

        [BindProperty]
        public List<GroupRoleUserAssignmentVM> Users { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Searching { get; set; }

        [BindProperty]
        public string AdditinalUsers { get; set; }

        public GroupUserAddModel(IGroupService srv, AppDbContext appDb, IdentitySQLServerDb pdb, UserManager<User> userManager, ICatfishAppConfiguration catfishConfig)
        {
            _srv = srv;
            _appDb = appDb;
            _piranhaDb = pdb;
            _userManager = userManager;
            _catfishConfig = catfishConfig;
           
        }

        public async Task OnGetAsync(Guid id)
        {
            //get selected group role details
            GroupRole = _srv.GetGroupRoleDetails(id);
            Users = _srv.SetUserAttribute(GroupRole.Id, Searching);
        }
        public async Task<IActionResult> OnPost()
        {
            GroupRole = _srv.GetGroupRoleDetails(GroupRole.Id);
            foreach (var userGroupRole in Users.Where(ugr => ugr.Assigned))
                _srv.AddUserGroupRole(userGroupRole.UserId, userGroupRole.RoleGroupId);


            if (!string.IsNullOrWhiteSpace(AdditinalUsers))
            {
                string[] newUsers = AdditinalUsers.Split("\r\n");

                foreach (string uemail in newUsers)
                {
                    if (await _userManager.FindByNameAsync(uemail) == null)
                    {

                        User _user = new Piranha.AspNetCore.Identity.Data.User();

                        UserEditModel userEditModel = new UserEditModel();
                        userEditModel.User = _user;


                        userEditModel.Password = new string(uemail.Reverse().ToArray()); 
                        userEditModel.PasswordConfirm = new string(uemail.Reverse().ToArray());
                        await _userManager.SetUserNameAsync(_user,uemail);
                        await _userManager.SetEmailAsync(_user, uemail);
                        await _userManager.CreateAsync(_user, userEditModel.Password); 
                        string roleName = _catfishConfig.GetDefaultUserRole();
                        Role role = _piranhaDb.Roles.Where(r => r.Name == roleName).FirstOrDefault();

                        userEditModel.Roles.Add(role);
                        var result = userEditModel.Save(_userManager);

                        if (result.Result.Succeeded)
                        {
                            _srv.AddUserGroupRole(_user.Id, GroupRole.Id);

                        }

                    }
                    else
                    {
                        if (!_srv.CheckUserGroupRole(GroupRole.Id))
                        {
                            User _user = await _userManager.FindByNameAsync(uemail);
                            _srv.AddUserGroupRole(_user.Id, GroupRole.Id);
                        }

                    }
                   

                }
            }

         
            _appDb.SaveChanges();
            return RedirectToPage("GroupEdit", "Manager", new { id=GroupRole.GroupId });
            
        }
    }
}
