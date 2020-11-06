using Catfish.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Manager.Pages
{
    public class Processes : PageModel
    {
        private readonly IPageIndexingService _srv;
        public List<Site> Sites { get; set; }

        public Processes(IPageIndexingService service)
        {
            _srv = service;
        }

        public async Task OnGetAsync()
        {
            Sites = await _srv.GetSitesList().ConfigureAwait(false);
        }

     }
}
