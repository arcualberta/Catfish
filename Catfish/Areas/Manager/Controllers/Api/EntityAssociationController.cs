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
        public JsonResult AddChildren(EntityContentViewModel model)
        {
            model.Associate();
            return Json(model);
        }

        public JsonResult RemoveChildren(EntityContentViewModel model)
        {
            model.Disassociate();
            return Json(model);
        }

        ////public JsonResult AddSelection(EntityContentViewModel entityVM)
        ////{
        ////    entityVM.Associate();
        ////    //ViewBag.ChildItems = entityVM;
        ////    return Json(entityVM);
        ////}

        ////public JsonResult RemoveItem(EntityContentViewModel entityVM, int itemIndex)
        ////{
        ////    entityVM.ChildEntityList.RemoveAt(itemIndex);
        ////    //ViewBag.ChildItems = entityVM;
        ////    return Json(entityVM);
        ////}

    }
}