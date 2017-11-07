using Catfish.Core.Helpers;
using Catfish.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language
        public ActionResult Switch(string lang)
        {
            ViewHelper.SetActiveLanguage(ConfigHelper.Languages.Where(x => x.Code == lang).FirstOrDefault(), Session);
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}