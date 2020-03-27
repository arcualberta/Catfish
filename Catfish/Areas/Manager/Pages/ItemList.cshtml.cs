using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using Catfish.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Areas.Manager.Pages
{
    public class ItemListModel : PageModel
    {
        private DbEntityService _srv;
        public ItemListModel(DbEntityService srv)
        {
            _srv = srv;
        }
        public IQueryable<EntityListEntry> Children { get; set; }
        public void OnGet()
        {
            Children = _srv.GetEntityList<Item>();
        }
    }
}
