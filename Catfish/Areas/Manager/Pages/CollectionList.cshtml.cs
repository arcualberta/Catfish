using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using Catfish.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Areas.Manager.Pages
{
    public class CollectionListModel : PageModel
    {
        private DbEntityService _srv;
        public IList<EntityListEntry> Children { get; set; }
        public CollectionListModel(DbEntityService srv)
        {
            _srv = srv;
        }
        public void OnGet()
        {
            Children = _srv.GetEntityList<Collection>()
                .ToList()
                .OrderBy(entry => entry.ConcatenatedName)
                .ToList();
        }
    }
}
