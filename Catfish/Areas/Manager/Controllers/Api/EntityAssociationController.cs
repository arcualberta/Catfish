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
        public JsonResult UpdateChildren(EntityContentViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("Model validation failed");

                if (vm.Id == 0)
                    throw new Exception("Parent model ID mnot specified");

                CFAggregation model = Db.XmlModels.Where(x => x.Id == vm.Id).FirstOrDefault() as CFAggregation;
                if (model == null)
                    throw new Exception("Specified parent entity of type Aggregation not found");

                //Associating children
                foreach (var c in vm.ChildEntityList)
                {
                    CFAggregation child = Db.XmlModels.Where(x => x.Id == c.Id).FirstOrDefault() as CFAggregation;
                    if (child == null)
                        throw new Exception("Id=" + c.Id + ": Specified child entity of type Aggregation not found");

                    model.ChildMembers.Add(child);
                }

                //Removing deleted children
                foreach (var c in vm.RemovalPendingChildEntities)
                {
                    CFAggregation child = Db.XmlModels.Where(x => x.Id == c.Id).FirstOrDefault() as CFAggregation;
                    model.ChildMembers.Remove(child);
                }

                Db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                Db.SaveChanges(User.Identity);
                vm.Status = KoBaseViewModel.eStatus.Success;
                return Json(vm);
            }
            catch (Exception ex)
            {
                return Json(vm.Error("UpdateChildren Error: " + ex.Message));
            }
        }

        [HttpPost]
        public JsonResult UpdateRelatedItems(EntityContentViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("Model validation failed");

                if (vm.Id == 0)
                    throw new Exception("Parent model ID mnot specified");

                CFAggregation model = Db.XmlModels.Where(x => x.Id == vm.Id).FirstOrDefault() as CFAggregation;
                if (model == null)
                    throw new Exception("Specified parent entity of type Aggregation not found");

                //Associating children
                foreach (var c in vm.ChildEntityList)
                {
                    CFItem child = Db.XmlModels.Where(x => x.Id == c.Id).FirstOrDefault() as CFItem;
                    if (child == null)
                        throw new Exception("Id=" + c.Id + ": Specified related item not found");

                    model.ChildRelations.Add(child);
                }

                //Removing deleted children
                foreach (var c in vm.RemovalPendingChildEntities)
                {
                    CFItem child = Db.XmlModels.Where(x => x.Id == c.Id).FirstOrDefault() as CFItem;
                    model.ChildRelations.Remove(child);
                }

                Db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                Db.SaveChanges(User.Identity);
                vm.Status = KoBaseViewModel.eStatus.Success;
                return Json(vm);
            }
            catch (Exception ex)
            {
                return Json(vm.Error("UpdateRelatedItems Error: " + ex.Message));
            }
        }
    }
}