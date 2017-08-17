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
using System.IO;
using System.Xml.Linq;
using System.Web.Configuration;

namespace Catfish.Areas.Manager.Controllers
{
    public class ItemsController : Controller
    {
        private CatfishDbContext db = new CatfishDbContext();

        // GET: Manager/Items
        public ActionResult Index()
        {
            var entities = db.XmlModels.Where(m => m is Item).Include(e => (e as Entity).EntityType).Select(e => e as Entity);
            //var entities = db.XmlModels.Where(m => m is Item).Select(e => e as Item);
            ////foreach (var e in entities)
            ////    e.Deserialize();
            if (entities != null)
            {
                //foreach(var e in entities)
                //{
                //    e.Data = XElement.Parse(e.Content);
                //}
                return View(entities);
            }

            return View();
        }

        // GET: Manager/Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entity entity = db.XmlModels.Find(id) as Entity;
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
            {
                model = db.XmlModels.Find(id) as Item;
                model.Deserialize();
            }
            else
            {
                string filename = "Item.xml";
                var path = WebConfigurationManager.AppSettings["TestDataFolder"];
                path = Path.Combine(path, filename);
                model = XmlModel.Load(path) as Item;

            }

            ViewBag.JSONModel = Newtonsoft.Json.JsonConvert.SerializeObject(model.Data);
            ////// Rendering these as json objects in view result in circular references 
            ////var metadataSets = db.MetadataSets.ToList();
            ////var entityTypes = db.EntityTypes.ToList();
            ////ViewBag.EntityTypes = new JavaScriptSerializer().Serialize(entityTypes);//Json(db.EntityTypes.ToList());
            ////ViewBag.MetadataSets = new JavaScriptSerializer().Serialize(metadataSets);//Json(db.MetadataSets.ToList());

            return View("EditItem", model);
        }

        // POST: Manager/Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item model)
        {
            HttpContextBase ctx = this.HttpContext;

            if (ModelState.IsValid)
            {
                ViewBag.Status = "Validation Passed";

                if(model.Id > 0)
                {
                    Item dbModel = db.XmlModels.Find(model.Id) as Item;
                    dbModel.Deserialize();


                    dbModel.UpdateValues(model);
                    db.Entry(dbModel).State = EntityState.Modified;
                    db.SaveChanges();

                    return View("EditItem", dbModel);
                }

                //model.Data.SetValue(ctx.Request["Data"]);

                //db.Entry(model).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }
            //ViewBag.EntityTypeId = new SelectList(db.EntityTypes, "Id", "Name", model.EntityTypeId);
            return View("EditItem", model);
        }

        // GET: Manager/Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entity entity = db.XmlModels.Find(id) as Entity;
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
            Entity entity = db.XmlModels.Find(id) as Entity;
            db.XmlModels.Remove(entity);
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
