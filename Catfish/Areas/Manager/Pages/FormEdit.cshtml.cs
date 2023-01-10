using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Manager.Pages
{
    public class FormEditPageModel : FieldContainerPageModel
    {
        private readonly AppDbContext _db;
        public FormEditPageModel(AppDbContext db)
        {
            _db = db;
            ApiRoot = "/manager/api/forms/";
            ModelLabel = "Forms";
        }

        public void OnGet(Guid id)
        {
            FieldContainerId = id;
        }
    }
}
