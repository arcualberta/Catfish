using Catfish.Core.Models;
using Catfish.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Manager.Pages.WorkflowEditor
{
    public class EntityTemplatePage : PageModel
    {
        public readonly IEntityTypeService _srv;
        public EntityTemplate Template { get; set; }

        public EntityTemplatePage(IEntityTypeService srv)
        {
            _srv = srv;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            //Template = _srv
            throw new NotImplementedException();
        }
    }
}
