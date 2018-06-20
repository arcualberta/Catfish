using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Areas.Manager.Services;
using Catfish.Core.Models;
using Catfish.Core.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Catfish.Core.Models.Access;
using Catfish.Services;

namespace Catfish.Areas.Manager.Controllers
{
    public class CollectionsController : CatfishController
    {
        // GET: Manager/Collections
        public ActionResult Index()
        {
            SecurityService.CreateAccessContext();
            var entities = CollectionService.GetCollections()
                .Select(e => e as CFEntity);
            return View(entities);
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            SecurityService.CreateAccessContext();
            if (id.HasValue)
            {
                CollectionService.DeleteCollection(id.Value);
                Db.SaveChanges(User.Identity);
            }
            
            return RedirectToAction("index");
        }


        // GET: Manager/Collections/children/5
        public ActionResult Associations(int id)
        {
            SecurityService.CreateAccessContext();
            CFCollection model = CollectionService.GetCollection(id);
            if (model == null)
                return HttpNotFound("Collection was not found");
            
            EntityContentViewModel childCollections = new EntityContentViewModel();
            childCollections.Id = model.Id;
            childCollections.LoadNextChildrenSet(model.ChildCollections);
            childCollections.LoadNextMasterSet(CollectionService.GetCollections());
            ViewBag.ChildCollections = childCollections;

            EntityContentViewModel childItems = new EntityContentViewModel();
            childItems.Id = model.Id;
            childItems.LoadNextChildrenSet(model.ChildItems);
            childItems.LoadNextMasterSet(ItemService.GetItems());
            ViewBag.ChildItems = childItems;

            EntityContentViewModel relatedItems = new EntityContentViewModel();
            relatedItems.Id = model.Id;
            relatedItems.LoadNextChildrenSet(model.ChildRelations);
            relatedItems.LoadNextMasterSet(ItemService.GetItems());
            ViewBag.RelatedItems = relatedItems;

            return View(model);
        }

        // GET: Manager/Collections/Edit/5
        public ActionResult Edit(int? id, int? entityTypeId)
        {
            SecurityService.CreateAccessContext();
            CFCollection model;

            if (id.HasValue && id.Value > 0)
            {
                model = CollectionService.GetCollection(id.Value);
                if (model == null)
                    return HttpNotFound("Collection was not found");
            }
            else
            {
                if (entityTypeId.HasValue)
                {
                    model = CollectionService.CreateCollection(entityTypeId.Value);
                }
                else
                {
                    List<CFEntityType> entityTypes = EntityTypeService.GetEntityTypes(CFEntityType.eTarget.Collections).ToList();
                    ViewBag.SelectEntityViewModel = new SelectEntityTypeViewModel()
                    {
                        EntityTypes = entityTypes
                    };

                    model = new CFCollection();
                }
            }

            return View(model);
        }

        // POST: Manager/Collections/Edit/5
        [HttpPost]
        public ActionResult Edit(CFCollection model)
        {
            if (ModelState.IsValid)
            {
                CFCollection dbModel = CollectionService.UpdateStoredCollection(model);
                Db.SaveChanges(User.Identity);

                if (model.Id == 0)
                    return RedirectToAction("Edit", new { id = dbModel.Id });
                else
                    return View(dbModel);

            }
            return View(model);
        }


        [HttpGet]
        public ActionResult AccessGroup(int id)
        {
            var entity = CollectionService.GetCollection(id); //ItemService.GetAnEntity(id);
            EntityAccessDefinitionsViewModel entityAccessVM = new EntityAccessDefinitionsViewModel();
            AccessGroupService accessGroupService = new AccessGroupService(Db);
            entityAccessVM = accessGroupService.UpdateViewModel(entity);// UpdateViewModel(entity);
            ViewBag.SugestedUsers = entityAccessVM.AvailableUsers2.ToArray();

            var accessList = accessGroupService.GetAccessCodesList();
            accessList.Remove(accessList.First()); //remove "None"
            accessList.Remove(accessList.Last()); //remove all
            ViewBag.AccessCodesList = accessList;

            return View(entityAccessVM);
        }



        public ActionResult AddUserAccessDefinition(EntityAccessDefinitionsViewModel entityAccessVM)
        {
            SecurityService.CreateAccessContext();
            CFCollection collection = CollectionService.GetCollection(entityAccessVM.Id);//ItemService.GetItem(entityAccessVM.Id);

            AccessGroupService accessGroupService = new AccessGroupService(Db);
            collection = accessGroupService.UpdateEntityAccessGroups(collection, entityAccessVM) as CFCollection;
            collection = EntityService.UpdateEntity(collection) as CFCollection;

            collection.Serialize();
            Db.SaveChanges();


            return RedirectToAction("AccessGroup", new { id = entityAccessVM.Id });
        }

        public JsonResult GetuserPermissions(string userGuid, int entityId)
        {
            CFAggregation entity = EntityService.GetAnEntity(entityId) as CFAggregation;
            //SecurityService securityService = new SecurityService(Db);
            AccessMode accessMode = SecurityService.GetAggregationPermissions(userGuid, entity);

            CFAccessDefinition cFAccessDefinition = new CFAccessDefinition();
            cFAccessDefinition.AccessModes = (AccessMode)accessMode;



            return Json(cFAccessDefinition.AccessModes.AsStringList(), JsonRequestBehavior.AllowGet);
        }
    }
}
