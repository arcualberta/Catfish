using Catfish.Areas.Manager.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Areas.Manager.Controllers
{
    public class EntityAssociationController : CatfishController
    {
        public JsonResult AddSelection(EntityAssociationViewModel entityVM)
        {
            entityVM.Associate();
            //ViewBag.ChildItems = entityVM;
            return Json(entityVM);
        }

        public JsonResult RemoveItem(EntityAssociationViewModel entityVM, int itemIndex)
        {
            entityVM.AssociatedEntities.RemoveAt(itemIndex);
            //ViewBag.ChildItems = entityVM;
            return Json(entityVM);
        }

    }
}