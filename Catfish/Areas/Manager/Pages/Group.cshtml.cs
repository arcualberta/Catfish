using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha.AspNetCore.Identity.Data;
using Piranha.Extend.Fields;
using Microsoft.EntityFrameworkCore;
using Piranha.AspNetCore.Identity.SQLServer;
using Catfish.Core.Models.ViewModels;
using Catfish.Core.Services;

namespace Catfish.Areas.Manager.Pages
{
    public class GroupModel : PageModel
    {
        private const string NotFound = "Group not found.";

        private IGroupService _srv;
        public readonly AppDbContext _appDb;

        [BindProperty]
        public Group Group { get; set; }

        [BindProperty]
        public List<GroupRoleAssignmentVM> Roles { get; set; }

        [BindProperty]
        public List<GroupTemplateAssignmentVM> Templates { get; set; }

        [BindProperty]
        public List<UserGroupRole> Users { get; set; }

        
        public GroupModel(IGroupService srv, AppDbContext appDb)
        {
            _srv = srv;
            _appDb = appDb;
        }
        public void OnGet(Guid? id)
        {
            //If the id is given, retrieving the group from the database. Otherwise, creating a new one.
            if (id.HasValue)
                Group = _appDb.Groups.FirstOrDefault(g => g.Id == id);
            else
                Group = new Group()
                {
                    GroupStatus = Group.eGroupStatus.Inactive,
                    Id = Guid.NewGuid()
                };

            if (Group == null)
                throw new Exception(NotFound);

            Templates = _srv.SetTemplateAttribute(Group.Id);

            Roles = _srv.SetRoleAttribute(Group.Id);

            Users = _srv.SetUserAttribute(Group.Id);
            

        }

        public IActionResult OnPost()
        {
            Group group = _srv.SaveGroupRoles(Group, Roles);
            _srv.SaveGroupTemplates(group, Templates);
            _appDb.SaveChanges();
           
            return RedirectToPage("GroupEdit","Manager", new { id = group.Id });
        }


        public void OnPostDelete(Guid id, Guid userGroupRoleId)
        {
            _srv.DeleteUserGroupRole(userGroupRoleId);
            _appDb.SaveChanges();
            OnGet(id);
        }
        

    }
}
