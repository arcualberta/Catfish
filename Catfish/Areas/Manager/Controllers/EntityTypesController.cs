using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Catfish.Core.Models;
using Catfish.Core.Services;
using Catfish.Core.Models.Forms;
using Catfish.Areas.Manager.Models.ViewModels;

namespace Catfish.Areas.Manager.Controllers
{
    public class EntityTypesController : CatfishController
    {
        // GET: Manager/EntityTypes
        public ActionResult Index()
        {
            return View(EntityService.GetEntityTypes());
        }


        // GET: Manager/EntityTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            EntityType model = null;
            if (id.HasValue && id.Value > 0)
            {
                model = Db.EntityTypes.Where(et => et.Id == id).FirstOrDefault();
            }
            else
            {
                model = new EntityType();
            }

            EntityTypeViewModel vm = new EntityTypeViewModel();
            vm.UpdateViewModel(model, Db);
            return View(vm);
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            EntityType model = null;
            if (id.HasValue && id.Value > 0)
            {
                model = Db.EntityTypes.Where(et => et.Id == id).FirstOrDefault();
                if (model != null)
                {
                    Db.Entry(model).State = EntityState.Deleted;
                    Db.SaveChanges(User.Identity);
                }
            }
            return RedirectToAction("index");
        }

        [HttpPost]
        public JsonResult AddMetadataSet(EntityTypeViewModel vm)
        {
            vm.AssociatedMetadataSets.Add(vm.SelectedMetadataSets);
            vm.MetadataSetMappingSrc.Add(vm.SelectedMetadataSets);
            vm.SelectedMetadataSets = new MetadataSetListItem();
            return Json(vm);
        }

        [HttpPost]
        public JsonResult Move(EntityTypeViewModel vm, int idx, int step)
        {
            int newIdx = KoBaseViewModel.GetBoundedArrayIndex(idx + step, vm.AssociatedMetadataSets.Count);
            if (idx != newIdx)
            {
                var ms = vm.AssociatedMetadataSets.ElementAt(idx);
                vm.AssociatedMetadataSets.RemoveAt(idx);
                vm.AssociatedMetadataSets.Insert(newIdx, ms);
            }
            return Json(vm);
        }

        [HttpPost]
        public JsonResult RemoveMetadataSet(EntityTypeViewModel vm, int idx)
        {
            vm.AssociatedMetadataSets.RemoveAt(idx);
            return Json(vm);
        }

        [HttpPost]
        public JsonResult UpdateMappingMetadataSet(EntityTypeViewModel vm, EntityTypeViewModel.eMappingType type)
        {
            if(type == EntityTypeViewModel.eMappingType.NameMapping)
            {
                vm.NameMapping.MetadataSet = vm.SelectedNameMappingMetadataSet.Name;
                vm.NameMapping.MetadataSetId = vm.SelectedNameMappingMetadataSet.Id;

                MetadataSet ms = Db.MetadataSets.Where(m => m.Id == vm.NameMapping.MetadataSetId).FirstOrDefault();
                //ms.Deserialize();
                vm.NameMapping.Field = "Not specified";
                vm.SelectedNameMappingField = "";
                vm.SelectedNameMappingFieldSrc = ms.Fields.Select(f => f.Name).ToList();
                vm.SelectedNameMappingFieldSrc.Sort();
                vm.SelectedNameMappingFieldSrc.Insert(0, "");

                vm.SelectedNameMappingMetadataSet = new MetadataSetListItem(0, "");
            }
            else if(type == EntityTypeViewModel.eMappingType.DescriptionMapping)
            {
                vm.DescriptionMapping.MetadataSet = vm.SelectedDescriptionMappingMetadataSet.Name;
                vm.DescriptionMapping.MetadataSetId = vm.SelectedDescriptionMappingMetadataSet.Id;

                MetadataSet ms = Db.MetadataSets.Where(m => m.Id == vm.DescriptionMapping.MetadataSetId).FirstOrDefault();
                //ms.Deserialize();
                vm.DescriptionMapping.Field = "Not specified";
                vm.SelectedDescriptionMappingField = "";
                vm.SelectedDescriptionMappingFieldSrc = ms.Fields.Select(f => f.Name).ToList();
                vm.SelectedDescriptionMappingFieldSrc.Sort();
                vm.SelectedDescriptionMappingFieldSrc.Insert(0, "");

                vm.SelectedDescriptionMappingMetadataSet = new MetadataSetListItem(0, "");
            }
            return Json(vm);
        }

        [HttpPost]
        public JsonResult UpdateMappingField(EntityTypeViewModel vm, EntityTypeViewModel.eMappingType type)
        {
            if (type == EntityTypeViewModel.eMappingType.NameMapping)
            {
                vm.NameMapping.Field = vm.SelectedNameMappingField;
                vm.SelectedNameMappingField = "";
            }
            else if (type == EntityTypeViewModel.eMappingType.DescriptionMapping)
            {
                vm.DescriptionMapping.Field = vm.SelectedDescriptionMappingField;
                vm.SelectedDescriptionMappingField = "";
            }
            return Json(vm);
        }
        [HttpPost]
        public JsonResult Save(EntityTypeViewModel vm)
        {
            if (ModelState.IsValid)
            {
                EntityType model;
                if (vm.Id > 0)
                {
                    model = Db.EntityTypes.Where(x => x.Id == vm.Id).FirstOrDefault();
                    if (model == null)
                        return Json(vm.Error("Specified entity type not found"));
                    else
                    {
                        vm.UpdateDataModel(model, Db);
                        Db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                else
                {
                    model = new EntityType();
                    vm.UpdateDataModel(model, Db);
                    Db.EntityTypes.Add(model);
                }

                Db.SaveChanges(User.Identity);
                vm.Status = KoBaseViewModel.eStatus.Success;

                if (vm.Id == 0)
                {
                    //This is a newly created object, so we ask knockout MVC to redirect it to the edit page
                    //so that the ID is added to the URL.
                    vm.redirect = true;
                    vm.url = Url.Action("Edit", "EntityTypes", new { id = model.Id });
                }
            }
            else
                return Json(vm.Error("Model validation failed"));

            return Json(vm);
        }








        // POST: Manager/EntityTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EntityType entityType)
        {
            var test = Request.Params;
            if (ModelState.IsValid)
            {
                if(entityType.Id > 0)
                {
                    EntityService.UpdateEntityType(entityType);
                }
                else
                {
                    EntityService.CreateEntityType(entityType);
                }
                Db.SaveChanges(User.Identity);
                return RedirectToAction("Index");
            }
            return View(entityType);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
