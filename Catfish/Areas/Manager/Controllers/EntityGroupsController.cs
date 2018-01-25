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
        private CatfishDbContext db = new CatfishDbContext();
       

        // GET: Manager/EntityGroups
        public ActionResult Index()
        {
            return View();
        }

        
        // GET: Manager/EntityGroups/Edit/5
        public ActionResult Edit(string id)
        {
            EntityGroup entityGroup = EntityGroupService.GetEntityGroup(id);
            EntityGroupViewModel entityGroupVM = PopulateEntityGroupViewModel(entityGroup);
            ViewBag.SugestedNames = GetSuggestedNames(entityGroupVM.AllUsers);
            return View(entityGroupVM);
        }

        // POST: Manager/EntityGroups/Edit/5
        [HttpPost]
        public ActionResult Edit(EntityGroupViewModel entytiGrpVM)//(int id, FormCollection collection)
        {
            try
            {
                if(ModelState.IsValid)
                {

                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public EntityGroupViewModel PopulateEntityGroupViewModel(EntityGroup entityGrp)
        {
            EntityGroupViewModel entityGrpVM = new EntityGroupViewModel();
            List<Piranha.Entities.User> AllUsers;
            using (var db = new DataContext())
            {
                AllUsers = db.Users.ToList(); 
            }
            entityGrpVM.Id = entityGrp.Id.ToString();
            entityGrpVM.EntityGroupName = entityGrp.Name;
            
            entityGrpVM.AllUsers = AllUsers;

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
