using Catfish.Core.Models;
using Catfish.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Controllers
{
    public abstract class CatfishController : Controller
    {
        private CatfishDbContext mDb;
        public CatfishDbContext Db { get { if (mDb == null) mDb = new CatfishDbContext(); return mDb; } }

        private CollectionService mCollectionService;
        public CollectionService CollectionService { get { if (mCollectionService == null) mCollectionService = new CollectionService(Db); return mCollectionService; } }
    }
}