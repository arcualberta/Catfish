using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Manager.Pages
{
    public class FormPageModel : FieldContainerPageModel
    {
        private readonly AppDbContext _db;
        public FormPageModel(AppDbContext db)
        {
            _db = db;
            ApiRoot = "/manager/api/forms/";
            ModelLabel = "Forms";
        }

        public void OnGet(Guid id)
        {
            Model = _db.Forms.Where(x => x.Id == id).FirstOrDefault();
            if (Model != null)
                Model.Initialize(XmlModel.eGuidOption.Ignore);
        }
    }
}
