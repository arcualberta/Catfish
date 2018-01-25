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
            ViewBag.SugestedNames = entityGroupVM.AllUsers.ToArray();
            return View(entityGroupVM);
        }

        // POST: Manager/EntityGroups/Edit/5
        [HttpPost]
        public ActionResult Edit(EntityGroupViewModel entGrpVM)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    EntityGroup entGrp = EntityGroupService.GetEntityGroup(entGrpVM.Id);

                    List<EntityGroupUser> oldUsers = new List<EntityGroupUser>();
                    if(entGrp != null)
                        oldUsers = entGrp.EntityGroupUsers.ToList();

                    entGrp = UpdateModel(entGrp, entGrpVM);//entGrpVM.UpdateModel(entGrp);
                    entGrp = EntityGroupService.EditEntityGroup(entGrp, oldUsers);
                  
                    Db.SaveChanges();
                   
                }

            }
            catch(Exception ex)
            {
                throw ex;
            }
           // return RedirectToAction("Edit", new { id = entGrpVM.Id });
            return View(entGrpVM);
        }

        public EntityGroupViewModel PopulateEntityGroupViewModel(EntityGroup entityGrp)
        {
            EntityGroupViewModel entityGrpVM = new EntityGroupViewModel();
            List<Piranha.Entities.User> AllUsers;
            using (var db = new DataContext())
            {
                AllUsers = db.Users.ToList(); //(from u in db.Users select u.Login).ToList();
            }
            entityGrpVM.Id = entityGrp.Id.ToString();
            entityGrpVM.EntityGroupName = entityGrp.Name;
            if(entityGrp.EntityGroupUsers.Count > 0)
            {
                foreach (EntityGroupUser egu in entityGrp.EntityGroupUsers)
                {
                    Piranha.Entities.User user = AllUsers.Where(u => u.Id == egu.UserId).FirstOrDefault();
                    
                    entityGrpVM.SelectedUsers.Add(user.Login); 
                }
                    
            }
            entityGrpVM.AllUsers = AllUsers.Select(u=>u.Login).ToList();

            return entityGrpVM;
            
        }

        public string[] GetSuggestedNames(List<Piranha.Entities.User> users)
        {
            string[] loginNames = users.Select(u => u.Login).ToArray();

            return loginNames;
        }

        public EntityGroup UpdateModel(EntityGroup entityGroup,EntityGroupViewModel entityGrpVM)
        {
            if (entityGroup == null)
            {
                entityGroup = new EntityGroup();
                entityGroup.Id = Guid.NewGuid();
            }

            entityGroup.Name = entityGrpVM.EntityGroupName;
            if (entityGroup.EntityGroupUsers.Count > 0)
                entityGroup.EntityGroupUsers.Clear();

            foreach (string usr in entityGrpVM.SelectedUsers)
            {
                using (var db = new DataContext())
                {
                    Piranha.Entities.User user = db.Users.Where(u => u.Login == usr).FirstOrDefault();
                    EntityGroupUser egUser = new EntityGroupUser()
                    {
                        EntityGroupId = entityGroup.Id,
                        UserId = user.Id
                    };
                    entityGroup.EntityGroupUsers.Add(egUser);
                }
            }

            return entityGroup;
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
