using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Core.Models;
using Catfish.Core.Models.Data;
using Catfish.Core.Services;
using Catfish.Models.Regions;
using Catfish.Models.ViewModels;
using Piranha.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Controllers
{
    public class FormSubmissionController : CatfishSinglePageController
    {
        // GET: Forms
        public ActionResult Index()
        {
            var model = GetModel();
            ViewBag.PageModel = model;

            FormContainer formContainer = model.Region<FormContainer>("FormContainer");

            Form form = SubmissionService.CreateSubmissionForm(formContainer.FormId);
            FormViewModel vm = new FormViewModel()
            {
                Form = form,
                ItemId = 0
            };

            return View(model.GetView(), vm);
        }

        [HttpPost]
        public ActionResult Edit(FormViewModel vm)
        {
            var model = GetModel();

            if (ModelState.IsValid)
            {
                FormContainer formContainer = model.Region<FormContainer>("FormContainer");
                Item submission = SubmissionService.SaveSubmission(
                    vm.Form,
                    vm.FormSubmissionRef,
                    vm.ItemId,
                    formContainer.EntityTypeId,
                    formContainer.FormId,
                    formContainer.CollectionId);

                Db.SaveChanges();

                string confirmLink = "confirmation";
                return Redirect(confirmLink);
            }

            ViewBag.PageModel = model;
            return View(model.GetView(), vm);
        }

        public ActionResult Confirmation()
        {
            var model = GetModel();
            return View(model);
        }

        [HttpPost]
        public JsonResult Upload()
        {
            try
            {
                CatfishDbContext db = new CatfishDbContext();
                SubmissionService srv = new SubmissionService(db);

                List<DataFile> files = srv.UploadTempFiles(Request);
                db.SaveChanges();

                var ret = files.Select(f => new FileViewModel(f, 0, ControllerContext.RequestContext));
                return Json(ret);
            }
            catch (Exception)
            {
                //return 500 or something appropriate to show that an error occured.
                return Json(string.Empty);
            }
        }

    }
}