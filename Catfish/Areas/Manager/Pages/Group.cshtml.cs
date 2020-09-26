using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha.Extend.Fields;

namespace Catfish.Areas.Manager.Pages
{
    public class GroupModel : PageModel
    {
        private IAuthorizationService _srv;

        [BindProperty]
        public Group Group { get; set; }

        public GroupModel(IAuthorizationService srv)
        {
            _srv = srv;
        }
        public void OnGet(Guid id)
        {
            Group = _srv.GetGroupDetails(id);
        }

        public void OnPost()
        {
            var dbGroup = _srv.GetGroupDetails(Group.Id);
        }

    }
}
