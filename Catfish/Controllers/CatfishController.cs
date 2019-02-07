using Catfish.Core.Models;
using Catfish.Core.Services;
using Catfish.Services;
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

        private SubmissionService mSubmissionService;
        public SubmissionService SubmissionService { get { if (mSubmissionService == null) mSubmissionService = new SubmissionService(Db); return mSubmissionService; } }

        private ItemService mItemService;
        public ItemService ItemService { get { if (mItemService == null) mItemService = new ItemService(Db); return mItemService; } }

        private DataService mDataService;
        public DataService DataService { get { if (mDataService == null) mDataService = new DataService(Db); return mDataService; } }

        private EntityTypeService mEntityTypeService;
        public EntityTypeService EntityTypeService { get { if (mEntityTypeService == null) mEntityTypeService = new EntityTypeService(Db); return mEntityTypeService; } }

        private ItemQueryService mItemQueryService;
        public ItemQueryService ItemQueryService { get { if (mItemQueryService == null) mItemQueryService = new ItemQueryService(Db); return mItemQueryService; } }

        private SecurityService mSecurityService;
        public SecurityService SecurityService
        {
            get
            {
                if (mSecurityService == null)
                {
                    mSecurityService = new SecurityService(Db);
                }
                return mSecurityService;
            }
        }

        private AggregationService mAggregationService;
        public AggregationService AggregationService
        {
            get
            {
                if (mAggregationService == null)
                    mAggregationService = new AggregationService(Db);
                return mAggregationService;
            }
        }
    }
}