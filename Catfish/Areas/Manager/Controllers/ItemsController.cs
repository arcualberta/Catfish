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
using Catfish.Areas.Manager.Models.ViewModels;

namespace Catfish.Areas.Manager.Controllers
{
    public class ItemsController : CatfishController
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

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            Item model = null;
            if (id.HasValue && id.Value > 0)
            {
                model = Db.Items.Where(et => et.Id == id).FirstOrDefault();
                if (model != null)
                {
                    Db.Entry(model).State = EntityState.Deleted;
                    Db.SaveChanges();
                }
            }
            return RedirectToAction("index");
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
        public ActionResult Edit(int? id, int? entityTypeId)
        {
            Item model;
            ItemService srv = new ItemService(db);

            ViewBag.FileList = "[]";
            ViewBag.UploadAction = Url.Action("Upload", "Items");
            ViewBag.OtherPngUrl = Url.Content("~/content/thumbnails/other.png");

            if (id.HasValue && id.Value > 0)
            {
                model = db.XmlModels.Find(id) as Item;
                model.Deserialize();

                if(model.Files.Any()) //MR Sept 5 2017---chek if model has any file associated before pulling it
                    ViewBag.FileList = new JavaScriptSerializer().Serialize(Json(this.GetFileArray(model.Files, model.Id)).Data);

            }
            else
            {
                if(entityTypeId.HasValue)
                {
                    model = srv.CreateEntity<Item>(entityTypeId.Value);
                }
                else
                {
                    List<EntityType> entityTypes = srv.GetEntityTypes(EntityType.eTarget.Items).ToList();
                    ViewBag.SelectEntityViewModel = new SelectEntityTypeViewModel()
                    {
                        EntityTypes = entityTypes
                    };

                    model = new Item();
                }
            }

            return View(model);
        }

        // POST: Manager/Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item model)
        {

            HttpContextBase ctx = this.HttpContext;
                ViewBag.UploadAction = Url.Action("Upload", "Items");
                ViewBag.OtherPngUrl = Url.Content("~/content/thumbnails/other.png");
            if (ModelState.IsValid)
            {
                ViewBag.Status = "Validation Passed";

                ItemService srv = new ItemService(db);

                Item dbModel = srv.UpdateStoredItem(model);
                db.SaveChanges();
                ViewBag.FileList = new JavaScriptSerializer().Serialize(Json(this.GetFileArray(model.Files, model.Id)).Data);

                if (model.Id == 0)
                    return RedirectToAction("Edit", new { id = dbModel.Id });
                else
                    return View(dbModel);
                
            }
            //ViewBag.EntityTypeId = new SelectList(db.EntityTypes, "Id", "Name", model.EntityTypeId);
            return View(model);
        }

        [HttpGet]
        public ActionResult UploadTest()
        {
            return View();
        }

        private IEnumerable<Object> GetFileArray(List<DataFile> files, int? itemId = null)
        {

            UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);

            IEnumerable<Object> result = files.Select(f => new
            {
                Id = f.Id,
                Guid = f.GuidName,
                FileName = f.FileName,
                Thumbnail = u.Action("Thumbnail", "Items", new { id = itemId.HasValue ? itemId.Value : f.Id, name = f.GuidName }),
                Url = u.Action("File", "Items", new { id = itemId.HasValue ? itemId.Value : f.Id, name = f.GuidName })
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
