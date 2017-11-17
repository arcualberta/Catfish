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
    public class MetadataController : FormBuilderController
    {
        public override Form CreateDataModel() { return new MetadataSet(); }

        public ActionResult Index()
        {
            return View(MetadataService.GetMetadataSets());
        }

    }
}