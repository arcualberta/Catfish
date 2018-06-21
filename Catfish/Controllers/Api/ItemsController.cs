using Catfish.Core.Models;
using Catfish.Core.Models.Data;
using Catfish.Core.Services;
using Catfish.Helpers;
using Catfish.Models.Regions;
using Catfish.Services;
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
        public JsonResult GetGraphData(string xMetadataSet, string xField, string yMetadataSet, string yField, string catMetadataSet, string catField,int xmin = 0, int xmax = 0)
        {
             xmin = xmin == 0 ? DateTime.MinValue.Year : xmin;
            xmax = xmax == 0 ? DateTime.Now.Year : xmax;
            string xQuerySelect = "SELECT a.Year as YValue, SUM(a.Amount) AS XValue, COUNT(*) as 'Count', a.Category" + 
                                   " FROM(" +
                                   " SELECT  Content.value('(/item/metadata/metadata-set[@guid=\"" + xMetadataSet + "\"]/fields/field[@guid=\"" + xField + "\"]/value/text/text())[1]', 'INT') AS Year ," +
                                    " Content.value('(/item/metadata/metadata-set[@guid=\"" + yMetadataSet + "\"]/fields/field[@guid=\"" + yField + "\"]/value/text/text())[1]', 'DECIMAL') AS Amount," +
                                    " Content.value('(/item/metadata/metadata-set[@guid=\"" + catMetadataSet + "\"]/fields/Field[@guid=\"" +catField + "\"]/options/option[@selected=\"true\"]/text/text())[1]', 'VARCHAR(25)') AS Category" +
                                    " FROM[dbo].[CFXmlModels]" +
                                    " WHERE Discriminator = 'CFItem' AND Content.exist('/item/metadata/metadata-set[@guid=\"" + xMetadataSet + "\"]') = 1" +
                                    " ) as a" +
                                     " WHERE a.Year >= " + xmin + " AND a.Year <= " +  xmax  +
                                     " GROUP BY a.Year, a.Category" +
                                     " ORDER BY a.Year";

            
            var result = Db.Database.SqlQuery<GraphQueryObject>(xQuerySelect, new object[] { xMetadataSet, xField, yMetadataSet, yField,catMetadataSet, catField, xMetadataSet });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: Items
        public JsonResult Index(string fields, int offset = 0, int limit = 25, bool randomize = false, int entityTypeId = 0, int collectionId = 0)
        {
            try
            {
                //SecurityService
                IEnumerable<CFItem> items;
                if (collectionId > 0)
                {
                    CFCollection collection = CollectionService.GetCollection(collectionId);
                    items = collection.ChildItems.Select(it => it as CFItem);
                }
                else
                {
                    
                    items = ItemService.GetItems();
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