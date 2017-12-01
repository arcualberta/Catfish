using Catfish.Core.Models;
using Catfish.Core.Services;
using Piranha.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Controllers
{
    public class CatfishSinglePageController : SinglePageController
    {
        private CatfishDbContext mDb;
        public CatfishDbContext Db { get { if (mDb == null) mDb = new CatfishDbContext(); return mDb; } }

        private SubmissionService mSubmissionService;
        public SubmissionService SubmissionService { get { if (mSubmissionService == null) mSubmissionService = new SubmissionService(Db); return mSubmissionService; } }

        public CatfishSinglePageController()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = Catfish.Helpers.ViewHelper.GetActiveLanguage();
        }
    }
}