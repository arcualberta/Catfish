using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Catfish.Core.Models;
using Catfish.Core.Services;
using Piranha.Areas.Manager.Controllers;

namespace Catfish.Areas.Manager.Controllers
{
    public class CatfishController : ManagerController
    {
        private CatfishDbContext mDb;
        public CatfishDbContext Db { get { if (mDb == null) mDb = new CatfishDbContext(); return mDb; } }

        private MetadataService mMetadataService;
        public MetadataService MetadataService { get { if (mMetadataService == null) mMetadataService = new MetadataService(Db); return mMetadataService; } }
    }
}