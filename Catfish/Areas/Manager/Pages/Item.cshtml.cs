using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.ViewModels;
using Catfish.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Areas.Manager.Pages
{
    public class ItemModel : PageModel
    {
        DbEntityService _srv;
        public ItemModel(DbEntityService srv)
        {
            _srv = srv;
        }
        public Item Item { get; set; }
        public EntityVM ViewModel { get; set; }
        public void OnGet(Guid id)
        {
            Item = _srv.GetItem(id);
            ViewModel = Item.InstantiateViewModel<EntityVM>();
        }
    }
}
