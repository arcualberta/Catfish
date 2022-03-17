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
        public readonly IdentitySQLServerDb _piranhaDb;
        [BindProperty]
        public Group Group { get; set; }

        [BindProperty]
        public List<GroupRoleAssignmentVM> Roles { get; set; }

        [BindProperty]
        public List<GroupTemplateAssignmentVM> Templates { get; set; }

        [BindProperty]
        public List<TemplateCollectionVM> Collections { get; set; }

        [BindProperty]
        public List<UserGroupRole> Users { get; set; }


        public GroupModel(IGroupService srv, AppDbContext appDb, IdentitySQLServerDb pdb)
        {
            _srv = srv;
            _appDb = appDb;
            _piranhaDb = pdb;
        }
        public void OnGet(Guid? id)
        {
            //If the id is given, retrieving the group from the database. Otherwise, creating a new one.
            if (id == null || id == Guid.Empty)
            {
                Group = new Group()
                {
                    GroupStatus = Group.eGroupStatus.Inactive,
                    Id = Guid.NewGuid()

                };
                _appDb.Add(Group);
                _appDb.SaveChanges();
            }
            else
            {
                Group = _appDb.Groups.FirstOrDefault(g => g.Id == id);
            }


            if (Group == null)
                throw new Exception(NotFound);

            Templates = _srv.SetTemplateAttribute(Group.Id);

            Roles = _srv.SetRoleAttribute(Group.Id);

            Users = _srv.SetUserAttribute(Group.Id);

            Collections = _srv.SetCollectionAttribute(Group.Id);


        }

        public IActionResult OnPost()
        {
            Group group = _srv.SaveGroupRoles(Group, Roles);
            _srv.SaveGroupTemplates(group, Templates);
            _appDb.SaveChanges();

            return RedirectToPage("GroupEdit", "Manager", new { id = group.Id });
        }


        public void OnPostDelete(Guid id, Guid userGroupRoleId)
        {
            _srv.DeleteUserGroupRole(userGroupRoleId);

            _appDb.SaveChanges();
            _piranhaDb.SaveChanges();
            OnGet(id);
        }
        public void OnPostEdit(Guid id, Guid collectionId, Guid groupTemplateId)
        {
            _srv.DeleteGroupTemplateCollection(groupTemplateId, collectionId);
            _appDb.SaveChanges();
            OnGet(id);
        }

    }
}
