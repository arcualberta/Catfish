using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Catfish.Core.Services;
using Catfish.Areas.Manager.Models.ViewModels;
using Piranha;
using Catfish.Areas.Manager.Services;

namespace Catfish.Areas.Manager.Controllers
{
    public class CFUserListsController : CatfishController
    {
       

        // GET: Manager/EntityGroups
        public ActionResult Index()
        {
            return View(EntityGroupService.GetEntityGroups());
        }

        
        // GET: Manager/EntityGroups/Edit/5
        public ActionResult Edit(string id)
        {
            CFUserList entityGroup = EntityGroupService.GetEntityGroup(id);
            CFUserListViewModel entityGroupVM = PopulateEntityGroupViewModel(entityGroup);

            ViewBag.SugestedNames = entityGroupVM.AllUsers2.Values.ToArray();
            return View(entityGroupVM);
        }

        // POST: Manager/EntityGroups/Edit/5
        [HttpPost]
        public JsonResult Edit(CFUserListViewModel entGrpVM)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    CFUserList entGrp = EntityGroupService.GetEntityGroup(entGrpVM.Id);

                    List<CFUserListEntry> oldUsers = new List<CFUserListEntry>();
                    if(entGrp != null)
                        oldUsers = entGrp.CFUserListEntries.ToList();

                    entGrp = entGrpVM.UpdateModel(entGrp); 
                    entGrp = EntityGroupService.EditEntityGroup(entGrp, oldUsers);
                  
                    Db.SaveChanges();
                    entGrpVM.ErrorMessage = string.Empty;
                }
                else
                {
                    if(string.IsNullOrEmpty(entGrpVM.CFUserListName))
                    {
                        entGrpVM.ErrorMessage = "*";
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return Json(entGrpVM);
        }

        public CFUserListViewModel PopulateEntityGroupViewModel(CFUserList entityGrp)
        {
            CFUserListViewModel entityGrpVM = new CFUserListViewModel();
            UserService userService = new UserService();
          
            entityGrpVM.AllUsers2 = userService.GetUserIdAndLoginName();
            entityGrpVM.Id = entityGrp.Id.ToString();
            entityGrpVM.CFUserListName = entityGrp.Name;
            if(entityGrp.CFUserListEntries.Count > 0)
            {
                foreach (CFUserListEntry egu in entityGrp.CFUserListEntries)
                {
                   
                    var _usr = entityGrpVM.AllUsers2.FirstOrDefault(u => u.Key == egu.UserId.ToString());
                    if(_usr.Key != null)
                    {
                        var usrNameTemp = _usr.Value;
                        entityGrpVM.SelectedUsers.Add(_usr.Value);
                    }       
                }          
            }        
            return entityGrpVM;            
        }

        #region knockout MVC methods
        [HttpPost]
        public JsonResult AddUser(CFUserListViewModel vm)
        {            
            vm.AddUser();
            return Json(vm);
        }

        [HttpPost]
        public JsonResult RemoveSelected(CFUserListViewModel vm)
        {
            vm.RemoveSelected();
            return Json(vm);
        }
        #endregion
    }
}
