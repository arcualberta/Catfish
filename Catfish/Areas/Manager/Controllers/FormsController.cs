using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Core.Models;
using Catfish.Core.Models.Data;
using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Areas.Manager.Controllers
{
    public class FormsController : CatfishController
    {
        // GET: Manager/Forms
        public ActionResult Index()
        {
            return View(FormService.GetSubmissionTemplates());
        }

        [HttpGet]
        public ActionResult Edit(int? id, int? entityTypeId)
        {
            Submission model;

            if (id.HasValue && id.Value > 0)
            {
                model = Db.XmlModels.Find(id) as Submission;
            }
            else
            {
                if (entityTypeId.HasValue)
                {
                    model = FormService.CreateEntity<Submission>(entityTypeId.Value);
                }
                else
                {
                    List<EntityType> entityTypes = FormService.GetEntityTypes(EntityType.eTarget.Forms).ToList();
                    ViewBag.SelectEntityViewModel = new SelectEntityTypeViewModel()
                    {
                        EntityTypes = entityTypes
                    };

                    model = new Submission();
                }
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult AddField(FormBuilderViewModel vm)
        {
            foreach (FormFieldType t in vm.SelectedFieldTypes)
            {
                Type type = Type.GetType(t.FieldType, true);
                if (!typeof(FormField).IsAssignableFrom(type))
                    throw new InvalidOperationException("Bad Type");

                FormField field = Activator.CreateInstance(type) as FormField;
                vm.Fields.Add(new FormFieldViewModel(field));
            }
            vm.SelectedFieldTypes.Clear();
            return Json(vm);
        }

        [HttpPost]
        public JsonResult RemoveField(FormBuilderViewModel vm, int idx)
        {
            vm.Fields.RemoveAt(idx);
            return Json(vm);
        }

        [HttpPost]
        public JsonResult Move(FormBuilderViewModel vm, int idx, int step)
        {
            int newIdx = KoBaseViewModel.GetBoundedArrayIndex(idx + step, vm.Fields.Count);
            if (idx != newIdx)
            {
                var field = vm.Fields.ElementAt(idx);
                vm.Fields.RemoveAt(idx);
                vm.Fields.Insert(newIdx, field);
            }
            return Json(vm);
        }

    }
}