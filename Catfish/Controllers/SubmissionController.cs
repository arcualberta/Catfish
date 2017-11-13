using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Controllers
{
    public class SubmissionController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // GET: Submission
        //[HttpPost]
        public ActionResult Edit()
        {
            return View();
        }
    }
}