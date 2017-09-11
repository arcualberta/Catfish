using Catfish.Areas.Manager.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Areas.Manager.Controllers.Api
{
    public class EntityAssociationListController : Controller
    {
        public JsonResult AddSelection(EntityAssociationViewModel entityVM)
        {
            entityVM.Associate();
            //ViewBag.ChildItems = entityVM;
            return Json(entityVM);
        }

        public JsonResult RemoveItem(EntityAssociationViewModel entityVM, int itemId)
        {
            entityVM.Associate();
            //ViewBag.ChildItems = entityVM;
            return Json(entityVM);
        }

    }
}