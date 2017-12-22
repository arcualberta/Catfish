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
    public class ItemsController : Controller
    {
        private CatfishDbContext mDb;
        public CatfishDbContext Db { get { if (mDb == null) mDb = new CatfishDbContext(); return mDb; } }
       
        // GET: Items
        public JsonResult Index(int? offset, int? limit, bool? randomize, int? entityTypeId, string fields, int?collectionId)
        {
            try
            {
                IEnumerable<Item> items;
                if (collectionId.HasValue && collectionId.Value > 0)
                {
                    Collection collection = Db.Collections.Where(c => c.Id == collectionId).FirstOrDefault();
                    items = collection.ChildItems.Select(it => it as Item);
                }
                else
                {
                    items = Db.Items;
                }
                if (entityTypeId.HasValue && entityTypeId.Value > 0)
                    items = items.Where(it => it.EntityTypeId == entityTypeId.Value);

                items = randomize.HasValue && randomize.Value ? items.OrderBy(item => Guid.NewGuid()) : items.OrderBy(item => item.Id);

                if (offset.HasValue)
                    items = items.Skip(offset.Value);

                if (limit.HasValue & limit.Value > 0)
                    items = items.Take(limit.Value);

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
            ItemService srv = new ItemService(Db);
            DataFile file = srv.GetFile(id, name);
            if (file == null)
                return HttpNotFound("File not found");

            string path_name = file.ThumbnailType == DataFile.eThumbnailTypes.Shared
                ? Path.Combine(FileHelper.GetThumbnailRoot(Request), file.Thumbnail)
                : Path.Combine(file.Path, file.Thumbnail);

            return new FilePathResult(path_name, file.ContentType);
        }

        public ActionResult File(int id, string guid)
        {
            ItemService srv = new ItemService(Db);
            DataFile file = srv.GetFile(id, guid);
            if (file == null)
                return HttpNotFound("File not found");

            string path_name = Path.Combine(file.Path, file.LocalFileName);
            return new FilePathResult(path_name, file.ContentType);
        }


    }
}