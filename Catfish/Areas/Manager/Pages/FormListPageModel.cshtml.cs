using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha;

namespace Catfish.Areas.Manager.Pages
{
    public class FormListPageModel : ListPageModel
    {
        private readonly AppDbContext _db;
        public FormListPageModel(AppDbContext db)
        {
            ApiRoot = "/manager/api/forms/";
            DetailsPage = "";
            EditPage = "edit/";
            ModelLabel = "Forms";
            _db = db;
        }
        public void OnGet()
        {
            Entries = _db.Forms
                .Select(x => new ListEntry() { Id = x.Id, Name = x.FormName })
                .AsEnumerable()
                .OrderBy(entry => entry.Name)
                .ToList();
    }
    }

}
