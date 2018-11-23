using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Core.Services;

namespace Catfish.Areas.Manager.Controllers
{
    public class FormTemplatesController : FormBuilderController<Form>
    {
        public override Form CreateDataModel() { return new Form(); }

        public override FormBuilderViewModel CreateViewModel(Form model)
        {
            FormBuilderViewModel vm = new FormBuilderViewModel(model) { ShowFieldDescriptions = true };

            return vm;
        }

        // GET: Manager/SubmissionTemplate
        public ActionResult Index()
        {
            return View(FormService.GetSubmissionTemplates());
        }

        [HttpGet]
        public ActionResult Ingest()
        {
            FormIngestionViewModel model = new FormIngestionViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Ingest(FormIngestionViewModel model)
        {
            GoogleSheetService srv = new GoogleSheetService(model.SpreadSheetId, Db);

            if (model.Button == "Ingest")
            {
                //All settings have been specified. Ready to create the form.

                Form form = srv.CreateForm(model);
                if (form != null)
                {
                    Db.FormTemplates.Add(form);
                    Db.SaveChanges();
                    return RedirectToAction("Edit", new { id = form.Id });
                }

                model.ColumnHeadings = srv.GetColumnHeadings(model.DataSheet);
                return View(model);
            }
            else
            {
                //Settings are being specified

                if (string.IsNullOrEmpty(model.SpreadSheetId))
                    return View(model);

                if (string.IsNullOrEmpty(model.DataSheet))
                {
                    ViewBag.SheetList = srv.GetSheetNames();
                    return View(model);
                }

                model.ColumnHeadings = srv.GetColumnHeadings(model.DataSheet);

                if (model.PreContextColumns.Count < model.PreContextColumnCount)
                {
                    for (int i = model.PreContextColumns.Count; i < model.PreContextColumnCount; ++i)
                        model.PreContextColumns.Add("");
                }

                return View(model);
            }
        }

        [HttpGet]
        public ActionResult CheckMedia(int id)
        {
            SecurityService.CreateAccessContext();
            SurveyService srv = new SurveyService(Db);
            var form = FormService.GetSubmissionTemplates().Where(f => f.Id == id).FirstOrDefault();
            if (form == null)
                return HttpNotFound("Not found");
            var errors = srv.CheckMedia(form);
            
            return File(new System.Text.UTF8Encoding().GetBytes(string.Join("\n", errors)), "text/txt", "errors.txt");
        }

    }
}