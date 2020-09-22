using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace Catfish.Areas.Manager.Pages
{
    public class GroupListModel : PageModel
    {
        public IList<Group> Groups { get; protected set; }
        public UserGroupService _srv { get; private set; }

        public GroupListModel(UserGroupService srv)
        {
            _srv = srv;
        }
        public void OnGet()
        {
            Groups  = _srv.GetGroupList();
        }
    }
}
