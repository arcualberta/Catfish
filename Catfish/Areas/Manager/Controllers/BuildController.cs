using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Areas.Manager.Controllers
{
    public class BuildController : Controller
    {
        // GET: Build
        public ActionResult Index()
        {
            ViewBag.GitCommmitSha = System.Configuration.ConfigurationManager.AppSettings["GitCommitSha"];
            ViewBag.GitUrl = System.Configuration.ConfigurationManager.AppSettings["GitUrl"];
            ViewBag.GitBranch = System.Configuration.ConfigurationManager.AppSettings["GitBranch"];

            string githubUrl = Regex.Replace(ViewBag.GitUrl, @"\.git$", "");
            ViewBag.CommitUrl = githubUrl + "/commit/" + ViewBag.GitCommmitSha;

            return View();
        }
    }
}