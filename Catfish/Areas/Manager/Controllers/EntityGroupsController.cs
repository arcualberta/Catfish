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
    public class EntityGroupsController : CatfishController
    {
       

        // GET: Manager/EntityGroups
        public ActionResult Index()
        {
            return View(EntityGroupService.GetEntityGroups());
        }

        
        // GET: Manager/EntityGroups/Edit/5
        public ActionResult Edit(string id)
        {
            EntityGroup entityGroup = EntityGroupService.GetEntityGroup(id);
            EntityGroupViewModel entityGroupVM = PopulateEntityGroupViewModel(entityGroup);

            ViewBag.SugestedNames = entityGroupVM.AllUsers2.Values.ToArray();
            return View(entityGroupVM);
        }

        // POST: Manager/EntityGroups/Edit/5
        [HttpPost]
        public JsonResult Edit(EntityGroupViewModel entGrpVM)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    EntityGroup entGrp = EntityGroupService.GetEntityGroup(entGrpVM.Id);

                    List<EntityGroupUser> oldUsers = new List<EntityGroupUser>();
                    if(entGrp != null)
                        oldUsers = entGrp.EntityGroupUsers.ToList();

                    entGrp = entGrpVM.UpdateModel(entGrp); 
                    entGrp = EntityGroupService.EditEntityGroup(entGrp, oldUsers);
                  
                    Db.SaveChanges();
                    entGrpVM.ErrorMessage = string.Empty;
                }
                else
                {
                    if(string.IsNullOrEmpty(entGrpVM.EntityGroupName))
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

        public EntityGroupViewModel PopulateEntityGroupViewModel(EntityGroup entityGrp)
        {
            EntityGroupViewModel entityGrpVM = new EntityGroupViewModel();
            UserService userService = new UserService();
          
            entityGrpVM.AllUsers2 = userService.GetUserIdAndLoginName();
            entityGrpVM.Id = entityGrp.Id.ToString();
            entityGrpVM.EntityGroupName = entityGrp.Name;
            if(entityGrp.EntityGroupUsers.Count > 0)
            {
                foreach (EntityGroupUser egu in entityGrp.EntityGroupUsers)
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
        public JsonResult AddUser(EntityGroupViewModel vm)
        {            
            vm.AddUser();
            return Json(vm);
        }

        [HttpPost]
        public JsonResult RemoveSelected(EntityGroupViewModel vm)
        {
            vm.RemoveSelected();
            return Json(vm);
        }
        #endregion
    }
}
