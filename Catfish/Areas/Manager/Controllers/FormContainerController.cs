using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
//using System.Net.Http;
//using System.Web.Http;
using System.Web.Mvc;
using Catfish.Core.Models;
using Catfish.Core.Services;
using Catfish.Models.Regions;
//using Catfish.Models.ViewModels;

namespace Catfish.Areas.Manager.Controllers
{
    public class FormContainerController : Controller
    {
        private CatfishDbContext mDb;
        public CatfishDbContext Db { get { if (mDb == null) mDb = new CatfishDbContext(); return mDb; } }

        [HttpGet]
        public JsonResult UpdateFieldsMapping(int formId)
        {
            try
            {
                EntityTypeService entityTypeService = new EntityTypeService(Db);
                // use these past fetches to show list on vies using SelectList ?
                Form form = Db.FormTemplates.Where(f => f.Id == formId).FirstOrDefault();
                SelectList FormFields = new SelectList(new string[0]);
                if (form != null)
                {
                    FormFields = new SelectList(form.Fields, "Name", "Name");
                }

                return Json(FormFields, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult UpdateAttributesMapping(int entityTypeId)
        {
            EntityTypeService entityTypeService = new EntityTypeService(Db);
            SelectList AttributesFields = new SelectList(new string[0]);
            if (entityTypeId > 0)
            {
                AttributesFields = new SelectList(entityTypeService.GetEntityTypeById(entityTypeId).AttributeMappings, "Name", "Name");
            }

            return Json(AttributesFields, JsonRequestBehavior.AllowGet);
        }
    }
}
