using Catfish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Controllers
{
    public class PingController : Controller
    {
        //
        // GET: /Ping/
        public ActionResult Index()
        {
            //CatfishDbContext db = new CatfishDbContext();
            //var count = db.DigitalEntities.Count();
            return Json(new { time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt")}, JsonRequestBehavior.AllowGet);
        }
	}
}