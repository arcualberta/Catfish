using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Core.Models;
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
    public class FormController : SinglePageController
    {
        // GET: Forms
        public ActionResult Index()
        {
            var model = GetModel();
            ViewBag.PageModel = model;

            Form form = model.Regions.Form as Form;
            int entityTypeId = form.EntityTypeId;

            CatfishDbContext db = new CatfishDbContext();
            ItemService srv = new ItemService(db);
            Item item = srv.CreateEntity<Item>(entityTypeId);

            return View(model.GetView(), item);
        }

        [HttpPost]
        public ActionResult Edit(Item submission)
        {
            CatfishDbContext db = new CatfishDbContext();
            var model = GetModel();

            if (ModelState.IsValid)
            {
                FormService srv = new FormService(db);
                Form form = model.Region<Form>("Form");

                Item savedItem = null;
                savedItem = srv.SaveFormSubmission(form.CollectionId, submission);

                db.SaveChanges();

                string confirmLink = "confirmation";
                return Redirect(confirmLink);
            }

            ViewBag.PageModel = model;
            return View(model.GetView(), submission);
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
                FormService srv = new FormService(db);

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