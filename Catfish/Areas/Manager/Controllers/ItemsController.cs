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
using Catfish.Core.Models.Forms;
using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Core.Models.Data;
using Catfish.Areas.Manager.Helpers;
using Catfish.Helpers;
using Catfish.Core.Helpers;
using Catfish.Areas.Manager.Services;
using Catfish.Core.Models.Access;
using Catfish.Services;

namespace Catfish.Areas.Manager.Controllers
{
    public class ItemsController : CatfishController
    {
 
        // GET: Manager/Items
        public ActionResult Index(int offset=0, int limit=int.MaxValue)
        {
            if(limit == int.MaxValue)
                limit = ConfigHelper.PageSize;

            // XXX What happens if user is admin ?
            // XXX How to check for admin ?

            //UserService.GetUserById(User.Identity.Name.ToString());

            //SecurityService ss = new SecurityService(Db);

            //if (ss.IsAdmin(User.Identity.Name))
            //{
            //    return ItemService.
            //}

            var itemQuery = ItemService.GetItems(User.Identity);
            var entities = itemQuery.OrderBy(e => e.Id)
                .Skip(offset)
                .Take(limit)
                .Include(e => (e as CFEntity).EntityType)
                .Select(e => e as CFEntity);
            var total = itemQuery.Count();

            ViewBag.TotalItems = total;
            ViewBag.Limit = limit;
            ViewBag.Offset = offset;
                       
            if (entities != null)
                return View(entities);

            return View();
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            CFItem model = null;
            if (id.HasValue && id.Value > 0)
            {
                model = Db.Items.Where(et => et.Id == id).FirstOrDefault();
                if (model != null)
                {
                    Db.Entry(model).State = EntityState.Deleted;
                    Db.SaveChanges(User.Identity);
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

            CFItem item = ItemService.GetItem(id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Manager/Items/Edit/5
        public ActionResult Edit(int? id, int? entityTypeId)
        {
            CFItem model;
          
            if (id.HasValue && id.Value > 0)
            {
                model = ItemService.GetItem(id.Value);
                if (model == null)
                    return HttpNotFound("Item was not found");
            }
            else
            {
                if(entityTypeId.HasValue)
                {
                    model = ItemService.CreateItem(entityTypeId.Value);
                }
                else
                {
                    List<CFEntityType> entityTypes = EntityTypeService.GetEntityTypes(CFEntityType.eTarget.Items).ToList(); //srv.GetEntityTypes(EntityType.eTarget.Items).ToList();
                    ViewBag.SelectEntityViewModel = new SelectEntityTypeViewModel()
                    {
                        EntityTypes = entityTypes
                    };

                    model = new CFItem();
                }
            }

            model.AttachmentField = new Attachment() { FileGuids = string.Join(Attachment.FileGuidSeparator.ToString(), model.Files.Select(f => f.Guid)) };
            return View(model);
        }

        // POST: Manager/Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CFItem model)
        {
            if (ModelState.IsValid)
            {
                CFItem dbModel = ItemService.UpdateStoredItem(model);
                Db.SaveChanges(User.Identity);

                if (model.Id == 0)
                    return RedirectToAction("Edit", new { id = dbModel.Id });
                else
                    return View(dbModel);
            }
            return View(model);
        }

        public ActionResult Associations(int id)
        {
            CFItem model = ItemService.GetItem(id);
            if (model == null)
                throw new Exception("Item not found");

            EntityContentViewModel childItems = new EntityContentViewModel();
            childItems.Id = model.Id;
            childItems.LoadNextChildrenSet(model.ChildItems);
            childItems.LoadNextMasterSet(ItemService.GetItems(User.Identity));
            ViewBag.ChildItems = childItems;

           

            EntityContentViewModel relatedItems = new EntityContentViewModel();
            relatedItems.Id = model.Id;
            relatedItems.LoadNextChildrenSet(model.ChildRelations);
            relatedItems.LoadNextMasterSet(ItemService.GetItems(User.Identity));
            ViewBag.RelatedItems = relatedItems;

            return View(model);
        }

        //XXX This method should be moved to a file controller
        [HttpPost]
        public JsonResult Upload()
        {
            try
            {
                List<CFDataFile> files = DataService.UploadTempFiles(Request);
                Db.SaveChanges(User.Identity);

                //Saving ids  of uploaded files in the session because these files and thumbnails
                //needs to be accessible by the user who is uploading them without restriction of any security rules.
                //This is because these files are stored in the temporary area without associating to any items.
                FileHelper.CacheGuids(Session, files);

                var ret = files.Select(f => new FileViewModel(f, f.Id, ControllerContext.RequestContext, "items"));
                return Json(ret);
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
                return Json(string.Empty);
            }
        }

        [HttpPost]
        public JsonResult DeleteCashedFile(string guid)
        {
            try
            {
                //Makes sure that the requested file is in the cache
                if(!FileHelper.CheckGuidCache(Session, guid))
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    Response.StatusDescription = "BadRequest: the file cannot be deleted -  NOT IN CACHE.";
                    return Json(string.Empty);
                }
                
                if (!DataService.DeleteStandaloneFile(guid))
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    Response.StatusDescription = "The file not found";
                    return Json(string.Empty);
                }

                Db.SaveChanges(User.Identity);
                return Json(new List<string>() { guid });
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                Response.StatusDescription = "BadRequest: an unknown error occurred.";
                return Json(string.Empty);
            }
        }

        public ActionResult File(int id, string guid)
        {
            CFDataFile file = DataService.GetFile(id, guid);
            if (file == null)
                return HttpNotFound("File not found");

            string path_name = Path.Combine(file.Path, file.LocalFileName);
            return new FilePathResult(path_name, file.ContentType);
        }

        public ActionResult Thumbnail(int id, string name)
        {
            CFDataFile file = DataService.GetFile(id, name);
            if (file == null)
                return HttpNotFound("File not found");
            var test = file.ThumbnailType;
            string path_name = file.ThumbnailType == CFDataFile.eThumbnailTypes.Shared 
                ? Path.Combine(FileHelper.GetThumbnailRoot(Request), file.Thumbnail)
                : Path.Combine(file.Path, file.Thumbnail);

            return new FilePathResult(path_name, file.ContentType);
        }

        [HttpGet]
        public ActionResult AccessGroup(int id)
        {
            var entity = ItemService.GetAnEntity(id);
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

            CFItem item = ItemService.GetItem(entityAccessVM.Id);
           
            AccessGroupService accessGroupService = new AccessGroupService(Db);
            item = accessGroupService.UpdateEntityAccessGroups(item, entityAccessVM) as CFItem;
            item = EntityService.UpdateEntity(item) as CFItem;
           
            item.Serialize();
            Db.SaveChanges();


            return RedirectToAction("AccessGroup", new { id = entityAccessVM.Id });
        }

        public JsonResult GetuserPermissions(string userGuid, int entityId)
        {
            CFAggregation entity = EntityService.GetAnEntity(entityId) as CFAggregation;
            SecurityService securityService = new SecurityService(Db);
            AccessMode accessMode = securityService.GetAggregationPermissions(userGuid, entity);

            CFAccessDefinition cFAccessDefinition = new CFAccessDefinition();
            cFAccessDefinition.AccessModes = (AccessMode)accessMode;

            

            return Json(cFAccessDefinition.AccessModes.AsStringList(), JsonRequestBehavior.AllowGet);
        }
    }
}
