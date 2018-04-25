﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Catfish.Core.Models;
using Catfish.Core.Services;
using Piranha.Areas.Manager.Controllers;
using Catfish.Areas.Manager.Models.ViewModels;

namespace Catfish.Areas.Manager.Controllers
{
    public class CatfishController : ManagerController
    {
        private CatfishDbContext mDb;
        public CatfishDbContext Db { get { if (mDb == null) mDb = new CatfishDbContext(); return mDb; } }

        private SubmissionService mFormService;
        public SubmissionService FormService { get { if (mFormService == null) mFormService = new SubmissionService(Db); return mFormService; } }

        private MetadataService mMetadataService;
        public MetadataService MetadataService { get { if (mMetadataService == null) mMetadataService = new MetadataService(Db); return mMetadataService; } }

        private EntityService mEntityService;
        public EntityService EntityService { get { if (mEntityService == null) mEntityService = new EntityService(Db); return mEntityService; } }

        private ItemService mItemService;
        public ItemService ItemService { get { if (mItemService == null) mItemService = new ItemService(Db); return mItemService; } }

        private CollectionService mCollectionService;
        public CollectionService CollectionService { get { if (mCollectionService == null) mCollectionService = new CollectionService(Db); return mCollectionService; } }

        private CFUserListService mEntityGroupService;

        public CFUserListService EntityGroupService { get { if (mEntityGroupService == null) mEntityGroupService = new CFUserListService(Db); return mEntityGroupService; } }

        private EntityTypeService mEntityTypeService;

        public EntityTypeService EntityTypeService { get { if (mEntityTypeService == null) mEntityTypeService = new EntityTypeService(Db); return mEntityTypeService; } }

        private DataService mDataService;
        public DataService DataService { get { if (mDataService == null) mDataService = new DataService(Db); return mDataService; } }


        [HttpPost]
        public JsonResult SelectEntity(SelectEntityTypeViewModel vm)
        {
            var id = vm.SelectedEntityType.Id;
            if(id > 0)
            {
                vm.redirect = true;
                string controller = this.GetType().Name;
                controller = controller.Substring(0, controller.Length - "Controller".Length);
                vm.url = Url.Action("edit", controller, new { entityTypeId = id });
            }

            return Json(vm);
        }

    }
}