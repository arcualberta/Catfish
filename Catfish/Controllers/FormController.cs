using Catfish.Core.Models;
using Catfish.Core.Services;
using Catfish.Models.Regions;
using Piranha.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Controllers
{
    public class FormController : SinglePageController
    {
        // GET: Forms
        public ActionResult Index()
        {
            var model = GetModel();

            Form form = model.Regions.Form as Form;
            int entityTypeId = form.EntityTypeId;

            CatfishDbContext db = new CatfishDbContext();
            ItemService srv = new ItemService(db);
            Item item = srv.CreateEntity<Item>(entityTypeId);
            ViewBag.Form = item;

            return View(model.GetView(), model);
        }

        [HttpPost]
        public ActionResult Edit(Item item)
        {
            ViewBag.Form = item;
            if (ModelState.IsValid)
            {

            }

            var model = GetModel();
            return View(model.GetView(), model);
        }
    }
}