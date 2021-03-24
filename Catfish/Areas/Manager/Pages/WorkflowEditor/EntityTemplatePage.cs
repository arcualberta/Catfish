using Catfish.Core.Models;
using Catfish.Core.Services;
using Catfish.Services;
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
        public readonly IEntityTemplateService _srv;
        public EntityTemplate Template { get; set; }

        public EntityTemplatePage(IEntityTemplateService srv)
        {
            _srv = srv;
        }

        public async Task OnGetAsync(Guid id)
        {
            Template = await _srv.GetTemplateAsync(id)
                .ConfigureAwait(false); //The library call does not need access to things like 
                                        //the HTTP Context so it can run completely separately
        }
    }
}
