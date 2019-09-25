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
                IDictionary<string, string> attributeMappings = new Dictionary<string, string>();
                foreach (var map in formContainer.FieldMappings)
                {
                    attributeMappings.Add(map.AttributeName, map.FieldName);
                }
                CFItem submission = subSrv.SaveSubmission(
                    vm.Form,
                    vm.FormSubmissionRef,
                    vm.ItemId,
                    formContainer.EntityTypeId,
                    formContainer.FormId,
                    formContainer.CollectionId, attributeMappings);

                //Sept 16 2019 -- if formContainer.AttachItemToUser = true, throw exception if current user is not authenticate
                if (formContainer.AttachItemToUser)
                {
                    if (!User.Identity.IsAuthenticated)
                    {
                        //throw new HttpException("You have to authenticate to save this page.");
                        vm.Errors = new Dictionary<string, string[]>();
                        vm.Errors.Add("Message", (new string[] { "You are not authorized." }));
                        return Json(vm);
                    }
                }


                // Set's the audit log value when saving.
                // TODO: this should be more automated.
                CFAuditEntry.eAction action = submission.Id == 0 ? CFAuditEntry.eAction.Create : CFAuditEntry.eAction.Update;
                string actor = User.Identity.IsAuthenticated ? User.Identity.Name : "Annonymous";



                Db.SaveChanges(User.Identity);
            }
            else
            {
                vm.Errors = new Dictionary<string, string[]>();

                IEnumerable<KeyValuePair<string, System.Web.Mvc.ModelState>> errors = ModelState.Where(m => m.Value.Errors.Count > 0);
                List<string> errorList = new List<string>();
                foreach (var error in errors)
                {
                    //vm.Errors.Add(error.Key, error.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                    errorList.AddRange(error.Value.Errors.Select(e => e.ErrorMessage));
                }

                vm.Errors.Add("Message", errorList.ToArray());
            }

            return Json(vm);
        }



    }


}