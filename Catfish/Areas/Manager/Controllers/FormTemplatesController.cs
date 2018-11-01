using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Catfish.Areas.Manager.Models.ViewModels;

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
            return View(model);
        }

    }
}