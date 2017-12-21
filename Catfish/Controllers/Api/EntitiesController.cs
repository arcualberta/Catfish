using Catfish.Core.Models;
using Catfish.Models.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Controllers.Api
{
    public class EntitiesController : Controller
    {
        // GET: Entities
        public JsonResult Items(int? offset, int? limit, bool? randomize, int? entityTypeId, int?collectionId)
        {
            try
            {
                CatfishDbContext db = new CatfishDbContext();
                IEnumerable<Item> items;
                if (collectionId.HasValue)
                {
                    Collection collection = db.Collections.Where(c => c.Id == collectionId).FirstOrDefault();
                    items = collection.ChildItems.Select(it => it as Item);
                }
                else
                {
                    items = db.Items;
                }
                if (entityTypeId.HasValue)
                    items = items.Where(it => it.EntityTypeId == entityTypeId.Value);

                items = randomize.HasValue && randomize.Value ? items.OrderBy(item => Guid.NewGuid()) : items.OrderBy(item => item.Id);

                if (offset.HasValue)
                    items = items.Skip(offset.Value);

                if (limit.HasValue)
                    items = items.Take(limit.Value);

                List<BulletinBoardItem> ret = items.Select(it => new BulletinBoardItem(it, Request.RequestContext)).ToList();
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
                Response.StatusDescription = "An error occurred.";
                return Json(new List<BulletinBoardItem>(), JsonRequestBehavior.AllowGet);

            }

        }
    }
}