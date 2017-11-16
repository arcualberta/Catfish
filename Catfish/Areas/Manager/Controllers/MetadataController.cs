using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using Piranha.Areas.Manager.Controllers;
using System.Data.Entity;

namespace Catfish.Areas.Manager.Controllers
{
    public class MetadataController : CatfishController
    {
        public ActionResult Index()
        {
            return View(MetadataService.GetMetadataSets());
        }

        ////[HttpGet]
        ////public ActionResult Details(int id)
        ////{
        ////    MetadataSet model = MetadataService.GetMetadataSet(id);
        ////    if (model == null)
        ////        return HttpNotFound();

        ////    return View(new MetadataDefinition(model, model.Id));
        ////}

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            MetadataSet model;
            if (id.HasValue && id > 0)
                model = MetadataService.GetMetadataSet(id.Value);
            else
                model = new MetadataSet();

            var viewModel = new FormBuilderViewModel(model);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            MetadataSet model = null;
            if (id.HasValue && id.Value > 0)
            {
                model = Db.MetadataSets.Where(et => et.Id == id).FirstOrDefault();
                if (model != null)
                    Db.Entry(model).State = EntityState.Deleted;
            }

            Db.SaveChanges();

            return RedirectToAction("index");
        }

        [HttpPost]
        public JsonResult Save(FormBuilderViewModel vm)
        {
            if (ModelState.IsValid)
            {
                MetadataSet model;

                if (vm.Id > 0)
                {
                    model = Db.MetadataSets.Where(x => x.Id == vm.Id).FirstOrDefault();
                    if (model == null)
                        return Json(vm.Error("Specified metadata set not found"));
                    else
                    {
                        vm.UpdateDataModel(model, Db);
                        Db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                else
                {
                    model = new MetadataSet();
                    vm.UpdateDataModel(model, Db);
                    Db.MetadataSets.Add(model);
                }

                Db.SaveChanges();
                vm.Status = KoBaseViewModel.eStatus.Success;

                if (vm.Id == 0)
                {
                    //This is a newly created object, so we ask knockout MVC to redirect it to the edit page
                    //so that the ID is added to the URL.
                    vm.redirect = true;
                    vm.url = Url.Action("Edit", "Metadata", new { id = model.Id });
                }
            }
            else
                return Json(vm.Error("Model validation failed"));

            return Json(vm);
        }

    }
}