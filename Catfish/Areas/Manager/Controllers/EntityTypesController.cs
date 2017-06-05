using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Catfish.Core.Models;
using Catfish.Core.Services;

namespace Catfish.Areas.Manager.Controllers
{
    public class EntityTypesController : CatfishController
    {
        private CatfishDbContext db = new CatfishDbContext();

        // GET: Manager/EntityTypes
        public ActionResult Index()
        {
            return View(EntityService.GetEntityTypes());
        }


        // GET: Manager/EntityTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            EntityType model = null;
            if (id.HasValue)
                model = EntityService.GetEntityType(id.Value);
            else
                model = new EntityType();
            
            return View(model);
        }

        // POST: Manager/EntityTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EntityType entityType)
        {
            if (ModelState.IsValid)
            {
                if(entityType.Id > 0)
                {
                    db.Entry(entityType).State = EntityState.Modified;
                }
                else
                {
                    db.EntityTypes.Add(entityType);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(entityType);
        }

        ////// GET: Manager/EntityTypes/Delete/5
        ////public ActionResult Delete(int? id)
        ////{
        ////    if (id == null)
        ////    {
        ////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        ////    }
        ////    EntityType entityType = db.EntityTypes.Find(id);
        ////    if (entityType == null)
        ////    {
        ////        return HttpNotFound();
        ////    }
        ////    return View(entityType);
        ////}

        ////// POST: Manager/EntityTypes/Delete/5
        ////[HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        ////public ActionResult DeleteConfirmed(int id)
        ////{
        ////    EntityType entityType = db.EntityTypes.Find(id);
        ////    db.EntityTypes.Remove(entityType);
        ////    db.SaveChanges();
        ////    return RedirectToAction("Index");
        ////}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
