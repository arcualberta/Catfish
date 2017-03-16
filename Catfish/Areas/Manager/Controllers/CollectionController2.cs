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
    public class CollectionController2 : CatfishManagerController
    {
        [Access(Function = "ADMIN_CONTENT")]
        public ActionResult Index()
        {
            var collections = Db.Collections.ToList();
            return View(collections);
        }

        [Access(Function = "ADMIN_CONTENT")]
        [HttpGet]
        public ActionResult Edit(string id = "")
        {
            var collection = new Collection();
            return View(collection);
        }

        //[Access(Function = "ADMIN_CONTENT")]
        //[HttpPost]
        //public ActionResult Edit(string id = "")
        //{
        //    var collection = new Collection();
        //    return View(collection);
        //}

    }
}