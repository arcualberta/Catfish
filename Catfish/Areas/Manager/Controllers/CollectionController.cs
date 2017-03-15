using Catfish.Models.Entities;
using Piranha;
using Piranha.Areas.Manager.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//namespace Catfish.Areas.Manager.Controllers
namespace Catfish.Areas.Manager.Controllers
{
    public class CollectionController : ManagerController
    {
        //[Access(Function = "ADMIN_COLLECTION")]
        public ActionResult Index()
        {
            var collections = new List<Collection>();
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