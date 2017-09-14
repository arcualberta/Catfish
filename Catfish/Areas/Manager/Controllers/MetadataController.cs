using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Core.Models.Metadata;
using Piranha.Areas.Manager.Controllers;

namespace Catfish.Areas.Manager.Controllers
{
    public class MetadataController : CatfishController
    {
        public ActionResult Index()
        {
            return View(MetadataService.GetMetadataSets());
        }

        [HttpGet]
        public ActionResult GetModel(int? id)
        {
            MetadataSet model;
            if (id.HasValue)
                model = MetadataService.GetMetadataSet(id.Value);
            else
                model = new MetadataSet();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            MetadataSet model = MetadataService.GetMetadataSet(id);
            if (model == null)
                return HttpNotFound();

            return View(new MetadataDefinition(model, model.Id));
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            MetadataSet model;
            if (id.HasValue)
                model = MetadataService.GetMetadataSet(id.Value);
            else
                model = new MetadataSet();

            ViewBag.FieldTypes = GetSerializedMetadataFieldTypes();
            //ViewBag.FileList = new JavaScriptSerializer().Serialize(Json(this.GetFileArray(model.Files)).Data);
            ViewBag.FieldList = GetSerializedMetadataFieldList(model);
            //XXX Creates circular reference
            //ViewBag.FieldList = new JavaScriptSerializer().Serialize(Json(model.Fields));
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MetadataSet model)
        {
            //var deletedFields = test["deletedFields"];
            var test = Request.Params;
            if (ModelState.IsValid)
            {
                MetadataSet ms = MetadataService.UpdateMetadataSet(model);
                if (ms == null)
                    return HttpNotFound();
                ////if (id > 0)
                ////{
                ////    Db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                ////    foreach (var field in model.Fields)
                ////    {
                ////        if (field.Id > 0)
                ////            Db.Entry(field).State = System.Data.Entity.EntityState.Modified;
                ////        else
                ////            Db.MetadataFields.Add(field);
                ////    }
                ////}
                ////else
                ////{
                ////    Db.MetadataSets.Add(model);
                ////}
                // Remove deletedFields

                ////////////////if (deletedFields != null && deletedFields != "" )
                ////////////////{
                ////////////////    string[] toDelete = deletedFields.Trim().Split(' ');
                ////////////////    foreach (string id in toDelete)
                ////////////////    {
                ////////////////        var field = Db.MetadataFields.Find(Int32.Parse(id));
                ////////////////        Db.MetadataFields.Remove(field);
                ////////////////        //var entry = Db.Entry(id);
                ////////////////        //entry.State = System.Data.Entity.EntityState.Deleted;
                ////////////////    }
                ////////////////}
                

                Db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.FieldTypes = GetSerializedMetadataFieldTypes();
            ViewBag.FieldList = GetSerializedMetadataFieldList(model);

            return View(model);
        }

        [HttpGet]
        public ActionResult EditTest(int? id) //notes: Kamal use this one
        {
            MetadataSet model;
            if (id.HasValue)
                model = MetadataService.GetMetadataSet(id.Value);
            else
            {
                model = new MetadataSet();
                ////model.Name = "Sample Form";
                ////model.Description = "Sample form description";

                ////model.Fields.Add(new TextField()
                ////{
                ////    Name = "Text Field 1",
                ////    Description = "Text filed 1 description"
                ////});

                ////model.Fields.Add(new TextArea()
                ////{
                ////    Name = "Text Area 1",
                ////    Description = "Text area 1 description"
                ////});

                ////model.Fields.Add(new RadioButtonSet()
                ////{
                ////    Name = "Radio Button Set 1",
                ////    Description = "Radio button set 1 description",
                ////    Options = "radio-option 1\nradio-option 2\nradio-option 3"
                ////});

                ////model.Fields.Add(new CheckBoxSet()
                ////{
                ////    Name = "Check Box Set 1",
                ////    Description = "Check box set 1 description",
                ////    Options = "check 1\ncheck 2\ncheck 3"
                ////});

                ////model.Fields.Add(new DropDownMenu()
                ////{
                ////    Name = "Drop Down Menu 1",
                ////    Description = "Drop down menu 1 description",
                ////    Options = "menu 1\nmenu 2\nmenu 3"
                ////});
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EditTest(MetadataSet model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                    Db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                else
                    Db.MetadataSets.Add(model);

                Db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }


        [HttpGet]
        public ActionResult FieldTypes()
        {
            var fieldTypes = this.MetadataService.GetMetadataFieldTypes();
            var fieldTypeViewModels = fieldTypes.Select(ft => new FieldDefinitionViewModel(ft)).ToList();

            //Dictionary<string, object> dummy = new Dictionary<string, object>();
            //dummy.Add("Simple", new SimpleField());
            //dummy.Add("Options", new OptionsField());


            //HtmlHelper<Dictionary<string, object>> html = new HtmlHelper<Dictionary<string, object>>();


            return Json(fieldTypeViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult MetadataSets()
        {
            var metadatasets = Db.MetadataSets.Select(ms => ms.Content).ToList();

            return Json(metadatasets, JsonRequestBehavior.AllowGet);
        }

        private string GetSerializedMetadataFieldTypes()
        {
            var fieldTypes = this.MetadataService.GetMetadataFieldTypes();
            var fieldTypeViewModels = fieldTypes.Select(ft => new FieldDefinitionViewModel(ft)).ToArray();
            return new JavaScriptSerializer().Serialize(fieldTypeViewModels);
        }

        private string GetSerializedMetadataFieldList(MetadataSet model)
        {
            List<Object> fieldList = new List<Object>();

            foreach (var field in model.Fields)
            {
                Type t = field.GetType();
                bool IsOption = typeof(OptionsField).IsAssignableFrom(t);

                var entry = new
                {
                    Name = field.Name,
                    Description = field.Description,
                    IsRequired = field.IsRequired,
                    FieldType = t.AssemblyQualifiedName,
                    Options = IsOption ? (field as OptionsField).Options : new List<Option>(),
                    ParentType = IsOption ? typeof(OptionsField).ToString() : typeof(MetadataField).ToString()
                };

                fieldList.Add(entry);
            }

            return new JavaScriptSerializer().Serialize(fieldList);
        }

    }
}