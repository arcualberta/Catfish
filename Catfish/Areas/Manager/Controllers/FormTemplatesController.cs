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
            if (model.Button == "Ingest")
                return PopulateForm(model);

            if(string.IsNullOrEmpty(model.SpreadSheetId))
                return View(model);

            GoogleSheetService srv = new GoogleSheetService(model.SpreadSheetId, Db);
            if (string.IsNullOrEmpty(model.DataSheet))
            {
                ViewBag.SheetList = srv.GetSheetNames();
                return View(model);
            }

            ViewBag.columnHeadings = srv.GetColumnHeadings(model.DataSheet);

            if(model.PreContextColumns.Count < model.PreContextColumnCount)
            {
                for (int i = model.PreContextColumns.Count; i < model.PreContextColumnCount; ++i)
                    model.PreContextColumns.Add("");
            }


            return View(model);
        }

        protected ActionResult PopulateForm(FormIngestionViewModel model)
        {

            return RedirectToAction("index");
        }

    }
}