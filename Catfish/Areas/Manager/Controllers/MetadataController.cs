using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Catfish.Core.Models.Metadata;
using Piranha.Areas.Manager.Controllers;

namespace Catfish.Areas.Manager.Controllers
{
    public class MetadataController : CatfishController
    {
        public ActionResult Index()
        {
            return View(MetadataService.GetMetadataSets());
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            MetadataSet model = id.HasValue ? MetadataService.GetMetadataSet(id.Value) : new MetadataSet();

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(MetadataSet model)
        {
            if(ModelState.IsValid)
            {
                if (model.Id > 0)
                    Db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                else
                    Db.MetadataSets.Add(model);

                Db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }

    }
}