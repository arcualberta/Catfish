using Catfish.Core.Models;
using Catfish.Core.Models.Data;
using Catfish.Core.Services;
using Catfish.Helpers;
using Catfish.Models.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Controllers.Api
{
    public class ItemsController : CatfishController
    {     
        // GET: Items
        public JsonResult Index(string fields, int offset = 0, int limit = 25, bool randomize = false, int entityTypeId = 0, int collectionId = 0)
        {
            try
            {
                IEnumerable<CFItem> items;
                if (collectionId > 0)
                {
                    CFCollection collection = CollectionService.GetCollection(collectionId);
                    items = collection.ChildItems.Select(it => it as CFItem);
                }
                else
                {
                    
                    items = ItemService.GetItems(User.Identity);
                }

                if (entityTypeId > 0)
                    items = items.Where(it => it.EntityTypeId == entityTypeId);

                items = randomize ? items.OrderBy(item => Guid.NewGuid()) : items.OrderBy(item => item.Id);

                if (offset > 0)
                    items = items.Skip(offset);

                if (limit > 0)
                    items = items.Take(limit);
                else
                    items = items.Take(25);

                List<BulletinBoardItem> ret = items.Select(it => new BulletinBoardItem(it, Request.RequestContext, fields)).ToList();
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
                Response.StatusDescription = "An error occurred.";
                return Json(new List<BulletinBoardItem>(), JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Thumbnail(int id, string name)
        {
            CFDataFile file = DataService.GetFile(id, name);
            if (file == null)
                return HttpNotFound("File not found");

            string path_name = file.ThumbnailType == CFDataFile.eThumbnailTypes.Shared
                ? Path.Combine(FileHelper.GetThumbnailRoot(Request), file.Thumbnail)
                : Path.Combine(file.Path, file.Thumbnail);

            return new FilePathResult(path_name, file.ContentType);
        }

        public ActionResult File(int id, string guid)
        {
            CFDataFile file = DataService.GetFile(id, guid);
            if (file == null)
                return HttpNotFound("File not found");

            string path_name = Path.Combine(file.Path, file.LocalFileName);
            return new FilePathResult(path_name, file.ContentType);
        }


    }
}