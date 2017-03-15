using Catfish.Models;
using Piranha.Areas.Manager.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Areas.Manager.Controllers
{
    public class CatfishManagerController : ManagerController
    {
        public CatfishDbContext Db { get { if (mDb == null) mDb = new CatfishDbContext(); return mDb; } }
        private CatfishDbContext mDb;

    }
}