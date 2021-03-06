﻿using Catfish.Areas.Manager.Models.ViewModels;
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
    public abstract class FormBuilderController<T> : CatfishController where T:AbstractForm
    {
        public abstract T CreateDataModel();
        public abstract FormBuilderViewModel CreateViewModel(T model);


        [HttpGet]
        public ActionResult Edit(int? id)
        {
            T model;

            if (id.HasValue && id.Value > 0)
                model = FormService.GetForm<T>(id.Value);
            else
                model = CreateDataModel();

            return View(CreateViewModel(model));
        }

        [HttpPost]
        public JsonResult AddField(FormBuilderViewModel vm)
        {
            int rank = vm.Fields.Count;
            int page = vm.Fields.Count > 0 ? vm.Fields[vm.Fields.Count - 1].Page : 1;
            foreach (FormFieldType t in vm.SelectedFieldTypes)
            {
                Type type = Type.GetType(t.FieldType, true);
                if (!typeof(FormField).IsAssignableFrom(type))
                    throw new InvalidOperationException("Bad Type");

                FormField field = Activator.CreateInstance(type) as FormField;
                field.Rank = ++rank;
                if (field.IsPageBreak())
                    page = page + 1;
                field.Page = page;
                vm.Fields.Add(new FormFieldViewModel(field, vm.Id));
            }
            vm.SelectedFieldTypes.Clear();
            return Json(vm);
        }

        [HttpPost]
        public JsonResult RemoveField(FormBuilderViewModel vm, int idx)
        {
            vm.Fields.RemoveAt(idx);
            vm.UpdateFieldRanks();
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
            vm.UpdateFieldRanks();
            return Json(vm);
        }

        [HttpPost]
        public ActionResult Save(int? id, FormBuilderViewModel vm)
        {
            if (ModelState.IsValid)
            {
                T model;

                if (vm.Id > 0)
                {
                    model = FormService.GetForm<T>(vm.Id);
                    if (model == null)
                        return HttpNotFound();
                }
                else
                {
                    model = CreateDataModel();
                }

                vm.UpdateDataModel(model, Db);
                FormService.SaveForm(model);
                Db.SaveChanges(User.Identity);
                vm.Status = KoBaseViewModel.eStatus.Success;

                if (vm.Id == 0)
                {
                    //This is a newly created object, so we ask knockout MVC to redirect it to the edit page
                    //so that the ID is added to the URL.
                    vm.redirect = true;
                    string controller = this.GetType().Name;
                    controller = controller.Substring(0, controller.Length - "Controller".Length);
                    vm.url = Url.Action("Edit", controller, new { id = model.Id });
                }
                
                SuccessMessage(Resources.Views.Form.Edit.SaveSuccess);
            }
            else
                ErrorMessage(Resources.Views.Form.Edit.SaveInvalid);

            return View("Edit", vm);
        }
    }
}