using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Catfish.Core.Services;
using Catfish.Areas.Manager.Models.ViewModels;
using Piranha;

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
                        //ModelState.AddModelError("EntityGroupName", "Entity Group Name is required.");
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
           
            using (var db = new DataContext())
            {
                entityGrpVM.AllUsers2 = db.Users.Select(u => new { u.Id, u.Login }).ToDictionary(u => u.Id.ToString(), u => u.Login);
            }
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

        public string[] GetSuggestedNames(List<Piranha.Entities.User> users)
        {
            string[] loginNames = users.Select(u => u.Login).ToArray();

            return loginNames;
        }

       
        //// GET: Manager/EntityGroups/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Manager/EntityGroups/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        #region knockout MVC methods
        [HttpPost]
        public JsonResult AddUser(EntityGroupViewModel vm)
        {
            //vm.SelectedUsers.Add(vm.AllUsers.ElementAt(idx));
            vm.AddUser();
            return Json(vm);
        }

        //[HttpPost]
        //public JsonResult Move(EntityTypeViewModel vm, int idx, int step)
        //{
        //    int newIdx = KoBaseViewModel.GetBoundedArrayIndex(idx + step, vm.AssociatedMetadataSets.Count);
        //    if (idx != newIdx)
        //    {
        //        var ms = vm.AssociatedMetadataSets.ElementAt(idx);
        //        vm.AssociatedMetadataSets.RemoveAt(idx);
        //        vm.AssociatedMetadataSets.Insert(newIdx, ms);
        //    }
        //    return Json(vm);
        //}

        [HttpPost]
        public JsonResult RemoveSelected(EntityGroupViewModel vm)
        {
            //vm.SelectedUsers.RemoveAt(idx);
            vm.RemoveSelected();
            return Json(vm);
        }
        #endregion
    }
}
