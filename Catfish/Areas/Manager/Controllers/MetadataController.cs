using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Piranha.Areas.Manager.Controllers;

namespace Catfish.Areas.Manager.Controllers
{
    public class MetadataController : CatfishController
    {
        public ActionResult Index()
        {
            return View(MetadataService.GetMetadataSets());
        }

        public ActionResult Edit(string id = "")
        {
            return View();
        }

    }
}