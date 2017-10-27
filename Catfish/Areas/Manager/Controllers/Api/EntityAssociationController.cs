using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Core.Models;
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

        [HttpPost]
        public JsonResult Save(EntityContentViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Entity model;
                if (vm.Id > 0)
                {
                    model = Db.XmlModels.Where(x => x.Id == vm.Id).FirstOrDefault() as Entity;
                    if (model == null)
                        return Json(vm.Error("Specified entity not found"));
                    else
                    {
                        if (typeof(Collection).IsAssignableFrom(model.GetType()))
                        {
                            Collection parent = model as Collection;
                            foreach(var c in vm.ChildEntityList)
                            {
                                Aggregation c2 = Db.XmlModels.Where(x => x.Id == c.Id).FirstOrDefault() as Aggregation;
                                parent.ChildMembers.Add(c2);
                            }
                        }
                        ////vm.UpdateDataModel(model, Db);
                        Db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    }
                }

                Db.SaveChanges();
                vm.Status = KoBaseViewModel.eStatus.Success;
            }
            else
                return Json(vm.Error("Model validation failed"));

            return Json(vm);
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