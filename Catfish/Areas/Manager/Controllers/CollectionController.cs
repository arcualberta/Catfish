using Catfish.Models.Entities;
using Piranha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//namespace Catfish.Areas.Manager.Controllers
namespace Catfish.Areas.Manager.Controllers
{
    public class CollectionController : CatfishManagerController
    {
        //[Access(Function = "ADMIN_COLLECTION")]
        public ActionResult Index()
        {
            var collections = Db.Collections.ToList();
            return View(collections);
        }

        //[Access(Function = "ADMIN_COLLECTION")]
        public ActionResult Edit(string id = "")
        {
            var collection = new Collection();
            return View(collection);
        }
    }
}