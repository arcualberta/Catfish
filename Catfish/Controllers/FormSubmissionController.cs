using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Core.Models;
using Catfish.Core.Models.Data;
using Catfish.Core.Services;
using Catfish.Models.Regions;
using Piranha.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Controllers
{
    public class FormSubmissionController : SinglePageController
    {
        // GET: Forms
        public ActionResult Index()
        {
            var model = GetModel();
            ViewBag.PageModel = model;

            FormContainer formContainer = model.Region<FormContainer>("FormContainer");

            SubmissionService srv = new SubmissionService(new CatfishDbContext());
            Form form = srv.CreateSubmissionForm(formContainer.FormId);

            return View(model.GetView(), form);
        }

        [HttpPost]
        public ActionResult Edit(Form form)
        {
            CatfishDbContext db = new CatfishDbContext();
            var model = GetModel();

            if (ModelState.IsValid)
            {
                SubmissionService srv = new SubmissionService(db);

                //retreaving the form container
                FormContainer formContainer = model.Region<FormContainer>("FormContainer");

                Item savedItem = null;
               // savedItem = srv.SaveFormSubmission(form.CollectionId, submission);

                db.SaveChanges();

                string confirmLink = "confirmation";
                return Redirect(confirmLink);
            }

            ViewBag.PageModel = model;
            return View(model.GetView(), form);
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