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
        public JsonResult Index(int? offset, int? limit, bool? randomize, int? entityTypeId, string fields, int?collectionId)
        {
            try
            {
                IEnumerable<Item> items;
                if (collectionId.HasValue && collectionId.Value > 0)
                {
                    Collection collection = CollectionService.GetCollection(collectionId.Value);
                    items = collection.ChildItems.Select(it => it as Item);
                }
                else
                {
                    items = ItemService.GetItems();
                }
                if (entityTypeId.HasValue && entityTypeId.Value > 0)
                    items = items.Where(it => it.EntityTypeId == entityTypeId.Value);

                items = randomize.HasValue && randomize.Value ? items.OrderBy(item => Guid.NewGuid()) : items.OrderBy(item => item.Id);

                if (offset.HasValue)
                    items = items.Skip(offset.Value);

                if (limit.HasValue & limit.Value > 0)
                    items = items.Take(limit.Value);
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
            DataFile file = DataService.GetFile(id, name);
            if (file == null)
                return HttpNotFound("File not found");

            string path_name = file.ThumbnailType == DataFile.eThumbnailTypes.Shared
                ? Path.Combine(FileHelper.GetThumbnailRoot(Request), file.Thumbnail)
                : Path.Combine(file.Path, file.Thumbnail);

            return new FilePathResult(path_name, file.ContentType);
        }

        public ActionResult File(int id, string guid)
        {
            DataFile file = DataService.GetFile(id, guid);
            if (file == null)
                return HttpNotFound("File not found");

            string path_name = Path.Combine(file.Path, file.LocalFileName);
            return new FilePathResult(path_name, file.ContentType);
        }


    }
}