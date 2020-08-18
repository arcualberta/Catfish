using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Areas.Manager.Pages
{
    public class MetadataSetListPageModel : ListPageModel
    {
        private readonly AppDbContext _db;
        public MetadataSetListPageModel(AppDbContext db)
        {
            ApiRoot = "/manager/api/metadatasets/";
            DetailsPage = "";
            EditPage = "edit/";
            ModelLabel = "Metadata Sets";
            _db = db;
        }
        public void OnGet()
        {
            ViewData["data"] = _db.MetadataSets.Select(x => x as FieldContainer).ToList();
        }
    }
}
