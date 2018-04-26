using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Controllers.Api
{
    public class StatsController : Controller
    {
        // GET: Stats
        public JsonResult Index()
        {
            CatfishDbContext db = new CatfishDbContext();
            int items = db.XmlModels.Where(x => x is CFItem).Count();
            int collections = db.XmlModels.Where(x => x is CFCollection).Count();
            Object stats = new { Items = items, Collections = collections };
            return Json(stats, JsonRequestBehavior.AllowGet);
        }
    }
}