using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Catfish.Core.Models;
using System.Web.Script.Serialization;

namespace Catfish.Areas.Manager.Controllers
{
    public class ItemsController : Controller
    {
        private CatfishDbContext db = new CatfishDbContext();

        // GET: Manager/Items
        public ActionResult Index()
        {

            var entities = db.Entities.Include(e => e.EntityType);
            return View(entities);
        }

        // GET: Manager/Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entity entity = db.Entities.Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        /*
        // GET: Manager/Items/Create
        public ActionResult Create()
        {
            ViewBag.EntityTypeId = new SelectLi st(db.EntityTypes, "Id", "Name");
            return View();
        }

        // POST: Manager/Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Created,Updated,EntityTypeId")] Entity entity)
        {
            if (ModelState.IsValid)
            {
                db.Entities.Add(entity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EntityTypeId = new SelectList(db.EntityTypes, "Id", "Name", entity.EntityTypeId);
            return View(entity);
        }
        */
        // GET: Manager/Items/Edit/5
        public ActionResult Edit(int? id)
        {
            Item model;
            if (id.HasValue)
                model = db.Entities.Find(id) as Item;
            else
                model = new Item();

            // Rendering these as json objects in view result in circular references 
            var metadataSets = db.MetadataSets.ToList();
            var entityTypes = db.EntityTypes.ToList();
            ViewBag.EntityTypes = new JavaScriptSerializer().Serialize(entityTypes);//Json(db.EntityTypes.ToList());
            ViewBag.MetadataSets = new JavaScriptSerializer().Serialize(metadataSets);//Json(db.MetadataSets.ToList());

            return View(model);
        }

        // POST: Manager/Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EntityTypeId = new SelectList(db.EntityTypes, "Id", "Name", model.EntityTypeId);
            return View(model);
        }

        // GET: Manager/Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entity entity = db.Entities.Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        // POST: Manager/Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Entity entity = db.Entities.Find(id);
            db.Entities.Remove(entity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
