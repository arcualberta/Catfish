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
using Catfish.Core.Services;
using Catfish.Core.Models.Metadata;

namespace Catfish.Areas.Manager.Controllers
{
    public class ItemsController : Controller
    {
        private CatfishDbContext db = new CatfishDbContext();

        public string ThumbnailRoot
        {
            get
            {
                return Request.RequestContext.HttpContext.Server.MapPath("~/Content/Thumbnails");
            }
        }

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
            ViewBag.FileList = "[]";
            if (id.HasValue)
            {
                model = db.XmlModels.Find(id) as Item;
                model.Deserialize();
                //ViewBag.
                if(model.Files.Any()) //MR Sept 5 2017---chek if model has any file associated before pulling it
                    ViewBag.FileList = new JavaScriptSerializer().Serialize(Json(this.GetFileArray(model.Files)).Data);
            }
            else
            {
                string filename = "Item.xml";
                var path = WebConfigurationManager.AppSettings["TestDataFolder"];
                path = Path.Combine(path, filename);
                model = XmlModel.Load(path) as Item;
            }
            //get entity type for testing-- this won't be needed if from edit interface there's a way to choose the EntityType
            string eTypeName = model.Data.Attribute("entity-type").Value;
            EntityType eType = db.EntityTypes.Where(e => e.Name == eTypeName).FirstOrDefault();
            if (eType != null)
            {
                model.EntityTypeId = eType.Id;
            }

            //ViewBag.JSONModel = Newtonsoft.Json.JsonConvert.SerializeObject(model.Data);
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
                ItemService srv = new ItemService(db);
                if (model.Id > 0)
                {
                    ////Item dbModel = db.XmlModels.Find(model.Id) as Item;
                    ////dbModel.Deserialize();
                    ////dbModel.UpdateValues(model);
                    ////db.Entry(dbModel).State = EntityState.Modified;

                    Item dbModel = srv.UpdateStoredItem(model);
                    db.SaveChanges();
                    ViewBag.FileList = new JavaScriptSerializer().Serialize(Json(this.GetFileArray(model.Files)).Data);
                    return View("EditItem", dbModel);
                }
                else
                {
                    // Item dbModel = srv.UpdateStoredItem(model); //this should be enough if follow current structure

                    //TODO: Create a new service method on ItemService for the folowings, which returns a new Item
                    // 1. Get the EntityType ID from the post call variable.
                    // 2. Load the item type from the database
                    // Item itm = db.XmlModels.Where(x => (x as Entity).EntityTypeId == model.EntityTypeId).FirstOrDefault() as Item;
                    Item itm = new Item();
                   
                    // 3. Create a new item. Add the list of metadata sets in the item type into the newly created model
                    // do we still need no 3, since Item retrieve from db contains all metadata already??
                    EntityType et = db.EntityTypes.Where(e => e.Id == model.EntityTypeId).FirstOrDefault();
                    XAttribute attribute = new XAttribute("entity-type", et.Name);
                    itm.Data.Add(attribute);
                    XElement meta = itm.Data.Element("metadata-sets");
                    foreach (MetadataSet ms in et.MetadataSets)
                    {
                        MetadataSet mSet = db.XmlModels.Find(ms.Id) as MetadataSet; 
                        meta.Add(mSet.Data);
                    }
                    // 4. Call srv.UpdateStoredItem(model); method to assign the values passed through the posted model into the newly created item

                    //updating the "value" text elements
                    itm.UpdateValues(model);
                    itm.EntityTypeId = model.EntityTypeId;
                    //save the item
                    db.XmlModels.Add(itm);
                     db.SaveChanges();
                     return View("EditItem", itm);
                   // return View();
                }
                
            }
            //ViewBag.EntityTypeId = new SelectList(db.EntityTypes, "Id", "Name", model.EntityTypeId);
            return View("EditItem", model);
        }

        [HttpGet]
        public ActionResult UploadTest()
        {
            return View();
        }

        private IEnumerable<Object> GetFileArray(List<DataFile> files)
        {

            UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);

            IEnumerable<Object> result = files.Select(f => new
            {
                Id = f.Id,
                Guid = f.GuidName,
                FileName = f.FileName,
                Thumbnail = u.Action("Thumbnail", "Items", new { id = f.Id, name = f.GuidName }),
                Url = u.Action("File", "Items", new { id = f.Id, name = f.GuidName })
            });
            return result;
        }

        [HttpPost]
        public JsonResult Upload()
        {
            try
            {
                ItemService srv = new ItemService(db);
                List<DataFile> files = srv.UploadFile(HttpContext, Request);
                db.SaveChanges();

                UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                string url = u.Action("About", "Home", null);

                var ret = this.GetFileArray(files);
                return Json(ret);
            }
            catch (Exception)
            {
                //return 500 or something appropriate to show that an error occured.
                return Json(string.Empty);
            }
        }

        public ActionResult File(int id, string name)
        {
            ItemService srv = new ItemService(db);
            DataFile file = srv.GetFile(id, name);
            if (file == null)
                return HttpNotFound("File not found");

            string path_name = Path.Combine(srv.UploadRoot, file.Path, file.GuidName);
            return new FilePathResult(path_name, file.ContentType);
        }

        public ActionResult Thumbnail(int id, string name)
        {
            ItemService srv = new ItemService(db);
            DataFile file = srv.GetFile(id, name);
            if (file == null)
                return HttpNotFound("File not found");

            string path_name = file.ThumbnailType == DataFile.eThumbnailTypes.Shared
                ? Path.Combine(ThumbnailRoot, file.Thumbnail)
                : Path.Combine(srv.UploadRoot, file.Path, file.Thumbnail);

            return new FilePathResult(path_name, file.ContentType);
        }

        // GET: Manager/Items/Delete/5
        public ActionResult Delete(int? id)
        {
            throw new NotImplementedException("This method is yet to be implemented.");
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
            throw new NotImplementedException("This method is yet to be implemented.");
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
