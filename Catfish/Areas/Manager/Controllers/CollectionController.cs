using Catfish.Models.Entities;
using Piranha;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Areas.Manager.Controllers
{
    public class CollectionController : CatfishManagerController
    {
        [Access(Function = "ADMIN_CONTENT")]
        public ActionResult Index()
        {
            var collections = Db.Collections.ToList();
            return View(collections);
        }

        [Access(Function = "ADMIN_CONTENT")]
        [HttpGet]
        public ActionResult Edit(int? id = null)
        {
            var collection = id == null ? new Collection() : Db.Collections.Where(c => c.Id == id).First();
            return View(collection);
        }

        [Access(Function = "ADMIN_CONTENT")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Collection collection)
        {
            if (ModelState.IsValid)
            {
                if (collection.Id == 0)
                {
                    collection.Created = DateTime.Now;
                    Db.Collections.Add(collection);
                }
                else
                {
                    collection.Updated = DateTime.Now;
                    Db.Entry(collection).State = EntityState.Modified;
                }
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(collection);
        }

    }
}