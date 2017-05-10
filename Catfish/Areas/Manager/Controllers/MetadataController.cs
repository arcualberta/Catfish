using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            {
                model = new MetadataSet();
                model.Name = "Sample Form";
                model.Description = "Sample form description";

                model.Fields.Add(new TextField()
                {
                    Name = "Text Field 1",
                    Description = "Text filed 1 description"
                });

                model.Fields.Add(new TextArea()
                {
                    Name = "Text Area 1",
                    Description = "Text area 1 description"
                });

                model.Fields.Add(new RadioButtonSet()
                {
                    Name = "Radio Button Set 1",
                    Description = "Radio button set 1 description",
                    Options = "radio-option 1\nradio-option 2\nradio-option 3"
                });

                model.Fields.Add(new CheckBoxSet()
                {
                    Name = "Check Box Set 1",
                    Description = "Check box set 1 description",
                    Options = "check 1\ncheck 2\ncheck 3"
                });

                model.Fields.Add(new DropDownMenu()
                {
                    Name = "Drop Down Menu 1",
                    Description = "Drop down menu 1 description",
                    Options = "menu 1\nmenu 2\nmenu 3"
                });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            MetadataSet model;
            if (id.HasValue)
                model = MetadataService.GetMetadataSet(id.Value);
            else
            {
                model = new MetadataSet();
                model.Name = "Sample Form";
                model.Description = "Sample form description";

                model.Fields.Add(new TextField()
                {
                    Name = "Text Field 1",
                    Description = "Text filed 1 description"
                });

                model.Fields.Add(new TextArea()
                {
                    Name = "Text Area 1",
                    Description = "Text area 1 description"
                });

                model.Fields.Add(new RadioButtonSet()
                {
                    Name = "Radio Button Set 1",
                    Description = "Radio button set 1 description",
                    Options = "radio-option 1\nradio-option 2\nradio-option 3"
                });

                model.Fields.Add(new CheckBoxSet()
                {
                    Name = "Check Box Set 1",
                    Description = "Check box set 1 description",
                    Options = "check 1\ncheck 2\ncheck 3"
                });

                model.Fields.Add(new DropDownMenu()
                {
                    Name = "Drop Down Menu 1",
                    Description = "Drop down menu 1 description",
                    Options = "menu 1\nmenu 2\nmenu 3"
                });
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(MetadataSet model)
        {
            if(ModelState.IsValid)
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
            var filedTypes = typeof(MetadataField).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(MetadataField))).ToList();
            return Json(filedTypes.Select(t => t.ToString()).ToList(), JsonRequestBehavior.AllowGet);
        }


    }
}