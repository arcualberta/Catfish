using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Models.Blocks.TileGrid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Pages
{
    public class DiscussionModel : PageModel
    {
        [BindProperty]
        public Tile Tile { get; set; }

        public void OnGet(Guid id)
        {

        }
    }
}