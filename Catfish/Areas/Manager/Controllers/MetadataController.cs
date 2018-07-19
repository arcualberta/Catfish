using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using Piranha.Areas.Manager.Controllers;
using System.Data.Entity;

namespace Catfish.Areas.Manager.Controllers
{
    public class MetadataController : FormBuilderController<CFMetadataSet>
    {
        public override CFMetadataSet CreateDataModel() { return new CFMetadataSet(); }

        public override FormBuilderViewModel CreateViewModel(CFMetadataSet model)
        {
            FormBuilderViewModel vm = new FormBuilderViewModel(model) { ShowFieldDescriptions = false };

            return vm;
        }

        public ActionResult Index()
        {
            return View(MetadataService.GetMetadataSets());
        }

    }
}