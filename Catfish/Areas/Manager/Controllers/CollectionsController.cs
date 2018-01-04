using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Core.Models;
using Catfish.Core.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Catfish.Areas.Manager.Controllers
{
    public class CollectionsController : CatfishController
    {
        private CatfishDbContext db = new CatfishDbContext();

        // GET: Manager/Collections
        public ActionResult Index()
        {
            var entities = db.XmlModels.Where(m => m is Collection).Include(e => (e as Entity).EntityType).Select(e => e as Entity);
            return View(entities);
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            Collection model = null;
            if (id.HasValue && id.Value > 0)
            {
                model = Db.Collections.Where(et => et.Id == id).FirstOrDefault();
                if (model != null)
                {
                    Db.Entry(model).State = EntityState.Deleted;
                    Db.SaveChanges(User.Identity);
                }
            }
            return RedirectToAction("index");
        }


        // GET: Manager/Collections/children/5
        public ActionResult Associations(int id)
        {
            Collection model = Db.Collections.Where(et => et.Id == id).FirstOrDefault();
            if (model == null)
                throw new Exception("Collection not found");

            CollectionService srv = new CollectionService(Db);

            EntityContentViewModel childCollections = new EntityContentViewModel();
            childCollections.Id = model.Id;
            childCollections.LoadNextChildrenSet(model.ChildCollections);
            childCollections.LoadNextMasterSet(db.Collections);
            ViewBag.ChildCollections = childCollections;

            EntityContentViewModel childItems = new EntityContentViewModel();
            childItems.Id = model.Id;
            childItems.LoadNextChildrenSet(model.ChildItems);
            childItems.LoadNextMasterSet(db.Items);
            ViewBag.ChildItems = childItems;

            EntityContentViewModel relatedItems = new EntityContentViewModel();
            relatedItems.Id = model.Id;
            relatedItems.LoadNextChildrenSet(model.ChildRelations);
            relatedItems.LoadNextMasterSet(db.Items);
            ViewBag.RelatedItems = relatedItems;

            return View(model);
        }

        // GET: Manager/Collections/Edit/5
        public ActionResult Edit(int? id, int? entityTypeId)
        {
            Collection model;
            CollectionService srv = new CollectionService(db);

            if (id.HasValue && id.Value > 0)
            {
                model = db.XmlModels.Find(id) as Collection;
                //model.Deserialize();
            }
            else
            {
                if (entityTypeId.HasValue)
                {
                    model = srv.CreateEntity<Collection>(entityTypeId.Value);
                }
                else
                {
                    List<EntityType> entityTypes = srv.GetEntityTypes(EntityType.eTarget.Collections).ToList();
                    ViewBag.SelectEntityViewModel = new SelectEntityTypeViewModel()
                    {
                        EntityTypes = entityTypes
                    };

                    model = new Collection();
                }
            }

            return View(model);
        }

        // POST: Manager/Collections/Edit/5
        [HttpPost]
        public ActionResult Edit(Collection model)
        {
            if (ModelState.IsValid)
            {
                CollectionService srv = new CollectionService(db);

                Collection dbModel = srv.UpdateStoredCollection(model);
                db.SaveChanges(User.Identity);

                if (model.Id == 0)
                    return RedirectToAction("Edit", new { id = dbModel.Id });
                else
                    return View(dbModel);

            }
            return View(model);
        }

        ////public Collection UpdateStoredItem(Collection changedItem)
        ////{ 
        ////    Collection dbModel = db.XmlModels.Find(changedItem.Id) as Collection;
        ////    dbModel.Deserialize();

        ////    //updating the "value" text elements
        ////    dbModel.UpdateValues(changedItem);
        ////    db.Entry(dbModel).State = EntityState.Modified;
            
        ////    return dbModel;
        ////}

        /////// <summary>
        /////// 
        /////// </summary>
        /////// <param name="id">Item.Id or Collection.Id</param>
        /////// <param name="offset"></param>
        /////// <param name="maxPage"></param>
        /////// <returns></returns>
        ////public JsonResult GetChildItems(int id, int offset=0, int maxPage=100)
        ////{
        ////    List<EntityViewModel> lEntyties = new List<EntityViewModel>();
        ////    var collection = db.XmlModels.Find(id) as Collection;

        ////    int numItem = offset * maxPage;
        ////    if(numItem <= 0)
        ////    {
        ////        numItem = collection.ChildItems.Count();
        ////    }
        ////    else
        ////    {
        ////        if (numItem > collection.ChildItems.Count())
        ////            numItem = collection.ChildItems.Count(); 
        ////    }

        ////    int i = 0;
        ////    foreach(var item in collection.ChildItems)
        ////    {
        ////        EntityViewModel eVM = new EntityViewModel(item);
        ////        lEntyties.Add(eVM);
        ////        i++;
        ////        if (i == numItem) //only retrieve as many item as indicate from offset and maxPage
        ////            break;
        ////    }
        ////    return Json(lEntyties);
        ////}

        ////public JsonResult GetChildCollections(int id, int offset = 0, int maxPage = 100)
        ////{
        ////    List<EntityViewModel> lEntyties = new List<EntityViewModel>();
        ////    var collection = db.XmlModels.Find(id) as Collection;

        ////    int numItem = offset * maxPage;
        ////    if (numItem <= 0)
        ////    {
        ////        numItem = collection.ChildItems.Count();
        ////    }
        ////    else
        ////    {
        ////        if (numItem > collection.ChildItems.Count())
        ////            numItem = collection.ChildItems.Count();
        ////    }

        ////    int i = 0;
        ////    foreach (var col in collection.ChildCollections)
        ////    {
        ////        EntityViewModel eVM = new EntityViewModel(col);
        ////        lEntyties.Add(eVM);
        ////        i++;
        ////        if (i == numItem)
        ////            break;
        ////    }
        ////    return Json(lEntyties);
        ////}
    }
}
