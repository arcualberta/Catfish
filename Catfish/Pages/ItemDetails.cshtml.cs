using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Pages
{
    public class ItemDetailsModel : PageModel
    {
        private readonly AppDbContext _db;

        public Item Item { get; set; }
        public ItemDetailsModel(AppDbContext db)
        {
            _db = db;
        }
        public void OnGet(Guid id)
        {
            //TODO: retrieve the item using a service call that handles the security, meaning that it should
            //verify that the current user has access to the item

            Item = _db.Items.Where(it => it.Id == id).FirstOrDefault()l

        }
    }
}
