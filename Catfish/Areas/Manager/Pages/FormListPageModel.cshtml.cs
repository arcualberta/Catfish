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
            EditPage = "forms/edit/";
            ModelLabel = "Forms";
            _db = db;
        }
        public void OnGet()
        {
            ViewData["data"] = _db.Forms.Select(x => x as FieldContainer).ToList();
        }
    }
}
