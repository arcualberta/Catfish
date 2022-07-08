using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Composition.Convention;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Manager.Pages
{
    public class ListPageModel : PageModel
    {
        public string ApiRoot { get; set; }
        public string EditPage { get; set; }
        public string DetailsPage { get; set; }
        public string ModelLabel { get; set; }

        public List<ListEntry> Entries { get; set; } = new List<ListEntry>();
    }
}
