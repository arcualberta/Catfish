using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Areas.Manager.Controllers
{
    public class FormTemplatesController : FormBuilderController
    {
        public override AbstractForm CreateDataModel() { return new CFForm(); }

        // GET: Manager/SubmissionTemplate
        public ActionResult Index()
        {
            return View(FormService.GetSubmissionTemplates());
        }
    }
}