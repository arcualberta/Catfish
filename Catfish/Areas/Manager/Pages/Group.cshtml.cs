using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Areas.Manager.Pages
{
    public class GroupModel : PageModel
    {
        private IAuthorizationService _srv;
        public Group Group { get; set; }


        public GroupModel(IAuthorizationService srv)
        {
            _srv = srv;
        }
        public void OnGet(Guid id)
        {
            Group = _srv.GetGroupDetails(id);
        }
    }
}
