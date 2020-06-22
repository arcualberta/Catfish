using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Manager.Pages
{
    public class ListPageModel : PageModel
    {
        public string ApiRoot { get; set; }
        public string EditPage { get; set; }
        public string DetailsPage { get; set; }
    }
}
