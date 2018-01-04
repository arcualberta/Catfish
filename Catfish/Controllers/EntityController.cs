using Catfish.Models.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Controllers.Api
{
    public class EntityController : Controller
    {
        // GET: Entity
        public ActionResult Index(int? id)
        {
            var model = new EntityTree();
            //var model = GetModel();

            return View(model);
            //return View();
        }

        public ActionResult Details (int? id)
        {
            var model = new EntityTree();

            return View("Index", model);
            //return View();
        }
    }
}