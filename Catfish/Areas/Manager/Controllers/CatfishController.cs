using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Catfish.Core.Models;
using Catfish.Core.Services;
using Piranha.Areas.Manager.Controllers;
using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Services;

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
          
        private CollectionService mCollectionService;
        public CollectionService CollectionService { get { if (mCollectionService == null) mCollectionService = new CollectionService(Db); return mCollectionService; } }

        private UserListService mEntityGroupService;

        public UserListService EntityGroupService { get { if (mEntityGroupService == null) mEntityGroupService = new UserListService(Db); return mEntityGroupService; } }

        private EntityTypeService mEntityTypeService;

        public EntityTypeService EntityTypeService { get { if (mEntityTypeService == null) mEntityTypeService = new EntityTypeService(Db); return mEntityTypeService; } }

        private DataService mDataService;
        public DataService DataService { get { if (mDataService == null) mDataService = new DataService(Db); return mDataService; } }

        private AccessDefinitionService mAccessDefinitionService;
        public AccessDefinitionService AccessDefinitionService { get { if (mAccessDefinitionService == null) mAccessDefinitionService = new AccessDefinitionService(Db); return mAccessDefinitionService; } }

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
        
        private ItemService mItemService;
        public ItemService ItemService {
            get {
                if (mItemService == null)
                    mItemService = new ItemService(Db);
                return mItemService;
            }
        }

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