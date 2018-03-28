using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Catfish.Core.Models;
using Catfish.Core.Services;
using Catfish.Core.Models.Forms;
using Catfish.Areas.Manager.Models.ViewModels;
using System.Collections.Generic;

namespace Catfish.Areas.Manager.Controllers
{
    public class EntityTypesController : CatfishController
    {
        // GET: Manager/EntityTypes
        public ActionResult Index()
        {
            return View(EntityTypeService.GetEntityTypes());
        }


        // GET: Manager/EntityTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            EntityType model = null;
            if (id.HasValue && id.Value > 0)
            {
                model = EntityTypeService.GetEntityTypeById(id.Value); 
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
                model = EntityTypeService.GetEntityTypeById(id.Value); //Db.EntityTypes.Where(et => et.Id == id).FirstOrDefault();
                if (model != null)
                {
                    EntityTypeService.DeleteEntityType(model);//Db.Entry(model).State = EntityState.Deleted;
                    Db.SaveChanges(User.Identity);
                }
            }
            return RedirectToAction("index");
        }

        [HttpPost]
        public JsonResult AddMetadataSet(EntityTypeViewModel vm)
        {
            vm.AssociatedMetadataSets.Add(vm.SelectedMetadataSets);
          //  vm.MetadataSetMappingSrc.Add(vm.SelectedMetadataSets);

            MetadataSet metadataSet = MetadataService.GetMetadataSet(vm.SelectedMetadataSets.Id);
            //update attribute mapping
            if (metadataSet != null)
            {
                if (!vm.MetadataSetFields.ContainsKey(metadataSet.Id.ToString()))
                {
                    List<string> addList = new List<string>();
                    addList.Add("");
                    addList = addList.Concat((metadataSet.Fields.Select(f => f.Name).ToList())).ToList();
                    vm.MetadataSetFields.Add(metadataSet.Id.ToString(), addList);
                }
            }
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

        public JsonResult AddAttributeMapping(EntityTypeViewModel vm)
        {
            
            vm.AttributeMappings.Add(new AttributeMapping {Name = "Custom Mapping", Field = "Field Name", Deletable = true
               
            });
            return Json(vm);
        }

        public JsonResult RemoveAttributeMapping(EntityTypeViewModel vm, int idx)
        {
            vm.AttributeMappings.RemoveAt(idx);

            return Json(vm);
        }

        //[HttpPost]
        //public JsonResult UpdateMappingMetadataSet(EntityTypeViewModel vm, EntityTypeViewModel.eMappingType type)
        //{
        //    if(type == EntityTypeViewModel.eMappingType.NameMapping)
        //    {
        //        vm.NameMapping.MetadataSet = vm.SelectedNameMappingMetadataSet.Name;
        //        vm.NameMapping.MetadataSetId = vm.SelectedNameMappingMetadataSet.Id;

        //        MetadataSet ms = MetadataService.GetMetadataSet(vm.NameMapping.MetadataSetId);//Db.MetadataSets.Where(m => m.Id == vm.NameMapping.MetadataSetId).FirstOrDefault();
        //        //ms.Deserialize();
        //        vm.NameMapping.Field = "Not specified";
        //        vm.SelectedNameMappingField = "";
        //        vm.SelectedNameMappingFieldSrc = ms.Fields.Select(f => f.Name).ToList();
        //        vm.SelectedNameMappingFieldSrc.Sort();
        //        vm.SelectedNameMappingFieldSrc.Insert(0, "");

        //        vm.SelectedNameMappingMetadataSet = new MetadataSetListItem(0, "", new System.Collections.Generic.List<string>());
        //    }
        //    else if(type == EntityTypeViewModel.eMappingType.DescriptionMapping)
        //    {
        //        vm.DescriptionMapping.MetadataSet = vm.SelectedDescriptionMappingMetadataSet.Name;
        //        vm.DescriptionMapping.MetadataSetId = vm.SelectedDescriptionMappingMetadataSet.Id;

        //        MetadataSet ms = MetadataService.GetMetadataSet(vm.DescriptionMapping.MetadataSetId);// Db.MetadataSets.Where(m => m.Id == vm.DescriptionMapping.MetadataSetId).FirstOrDefault();
        //        //ms.Deserialize();
        //        vm.DescriptionMapping.Field = "Not specified";
        //        vm.SelectedDescriptionMappingField = "";
        //        vm.SelectedDescriptionMappingFieldSrc = ms.Fields.Select(f => f.Name).ToList();
        //        vm.SelectedDescriptionMappingFieldSrc.Sort();
        //        vm.SelectedDescriptionMappingFieldSrc.Insert(0, "");

        //        vm.SelectedDescriptionMappingMetadataSet = new MetadataSetListItem(0, "", new System.Collections.Generic.List<string>());
        //    }
        //    return Json(vm);
        //}

        //[HttpPost]
        //public JsonResult UpdateMappingField(EntityTypeViewModel vm, EntityTypeViewModel.eMappingType type)
        //{
        //    if (type == EntityTypeViewModel.eMappingType.NameMapping)
        //    {
        //        vm.NameMapping.Field = vm.SelectedNameMappingField;
        //        vm.SelectedNameMappingField = "";
        //    }
        //    else if (type == EntityTypeViewModel.eMappingType.DescriptionMapping)
        //    {
        //        vm.DescriptionMapping.Field = vm.SelectedDescriptionMappingField;
        //        vm.SelectedDescriptionMappingField = "";
        //    }
        //    return Json(vm);
        //}
        [HttpPost]
        public JsonResult Save(EntityTypeViewModel vm)
        {
            if (ModelState.IsValid)
            {
                EntityType model;
                if (vm.Id > 0)
                {
                    model = EntityTypeService.GetEntityTypeById(vm.Id);//Db.EntityTypes.Where(x => x.Id == vm.Id).FirstOrDefault();
                    if (model == null)
                        return Json(vm.Error("Specified entity type not found"));
                    else
                    {
                        vm.UpdateDataModel(model, Db);
                        //Db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        EntityTypeService.UpdateEntityType(model);
                    }
                }
                else
                {
                    model = new EntityType();
                    vm.UpdateDataModel(model, Db);
                    // Db.EntityTypes.Add(model);
                    EntityTypeService.UpdateEntityType(model);
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
            {
                if(string.IsNullOrEmpty(vm.Name))
                {
                    vm.ErrorMessage = "*";
                }
                foreach(var att in vm.AttributeMappings)
                {
                    if(string.IsNullOrEmpty(att.Name) || string.IsNullOrEmpty(att.Field))
                    {
                        att.ErrorMessage = "*";
                    }
                }
                return Json(vm.Error("Model validation failed"));
            }

            return Json(vm);
        }


       




        // POST: Manager/EntityTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(EntityType entityType)
        //{
        //    var test = Request.Params;
        //    if (ModelState.IsValid)
        //    {
        //        entityType.TargetTypes = entityType.TargetTypesList.Count > 0 ? string.Join(",", entityType.TargetTypesList.ToArray()) : string.Empty;
        //        if (entityType.Id > 0)
        //        {
        //            EntityService.UpdateEntityType(entityType);
        //        }
        //        else
        //        {
        //            EntityService.CreateEntityType(entityType);
        //        }
        //        Db.SaveChanges(User.Identity);
        //        return RedirectToAction("Index");
        //    }
        //    return View(entityType);
        //}


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
