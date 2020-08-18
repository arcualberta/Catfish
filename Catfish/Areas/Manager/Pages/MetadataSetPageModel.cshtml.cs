using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Manager.Pages
{
    public class MetadataSetPageModel : FieldContainerPageModel
    {
        private readonly AppDbContext _db;
        public MetadataSetPageModel(AppDbContext db)
        {
            _db = db;
            ApiRoot = "/manager/api/metadatasets/";
            ModelLabel = "Metadata Sets";
        }

        public void OnGet(Guid id)
        {
            Model = _db.MetadataSets.Where(x => x.Id == id).FirstOrDefault();
        }

    }
}
