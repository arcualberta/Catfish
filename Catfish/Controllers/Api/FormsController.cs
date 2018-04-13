using Catfish.Core.Models;
using Catfish.Core.Services;
using Catfish.Models.Regions;
using Catfish.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Controllers.Api
{
    public class FormsController : Controller
    {
        private CatfishDbContext mDb;
        public CatfishDbContext Db { get { if (mDb == null) mDb = new CatfishDbContext(); return mDb; } }

        [HttpPost]
        public JsonResult Submit(FormViewModel vm, FormContainer formContainer)
        {
            if (ModelState.IsValid)
            {
                SubmissionService subSrv = new SubmissionService(Db);
                //add AttributeMappings -- Apr 10 2018
                IDictionary<string, string> attributeMappings = new Dictionary<string,string>();
                foreach(var map in formContainer.FieldMappings)
                {
                    attributeMappings.Add(map.AttributeName, map.FieldName);
                }
                Item submission = subSrv.SaveSubmission(
                    vm.Form,
                    vm.FormSubmissionRef,
                    vm.ItemId,
                    formContainer.EntityTypeId,
                    formContainer.FormId,
                    formContainer.CollectionId, attributeMappings);

                // Set's the audit log value when saving.
                // TODO: this should be more automated.
                AuditEntry.eAction action = submission.Id == 0 ? AuditEntry.eAction.Create : AuditEntry.eAction.Update;
                string actor = User.Identity.IsAuthenticated ? User.Identity.Name : "Annonymous";
                Db.SaveChanges(User.Identity);
            }
            else
            {
                vm.Errors = new Dictionary<string, string[]>();

                IEnumerable<KeyValuePair<string, System.Web.Mvc.ModelState>> errors = ModelState.Where(m => m.Value.Errors.Count > 0);
                foreach(var error in errors)
                {
                    vm.Errors.Add(error.Key, error.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                }
            }

            return Json(vm);
        }

        
      
    }

    
}