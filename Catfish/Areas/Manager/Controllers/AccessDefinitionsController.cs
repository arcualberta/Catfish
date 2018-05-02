using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Core.Models.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Areas.Manager.Controllers
{
    public class AccessDefinitionsController : CatfishController
    {
        // GET: Manager/AccessDefinition
        public ActionResult Index()
        {
            //AccessDefinitionsViewModel adVM = new AccessDefinitionsViewModel();
            var acessDefs = (AccessDefinitionService.GetAccessDefinitions()).Select(a=>a as CFAccessDefinition);
           // adVM = UpdateAccessDefinitionViewModel(accessDef, adVM);
            return View(acessDefs);
        }

        // GET: Manager/AccessDefinition/Edit/5
        public ActionResult Edit(int? id)
        {
            AccessDefinitionsViewModel adVM = new AccessDefinitionsViewModel();
            CFAccessDefinition access = new CFAccessDefinition();

            if (id != null)
            {
                CFAccessDefinition accessDef = AccessDefinitionService.GetAccessDefinitionById(id.Value);

                adVM = UpdateAccessDefinitionViewModel(accessDef, adVM);
            }
            else
            {
                //get all available AccessMode enum
                int i = 0;
                foreach (AccessMode am in Enum.GetValues(typeof(AccessMode)))
                {
                    if (i > 0) //skip the first one -- None
                    {
                        adVM.AccessModes.Add(new AccessCheckBox { Text = am.ToString(), Value = ((int)am) });
                    }
                    i++;
                }
            }
           
            return View(adVM);
        }

        // POST: Manager/AccessDefinition/Edit/5
        [HttpPost]
        public ActionResult Edit(AccessDefinitionsViewModel accessDefVM)
        {
            try
            {
               if(ModelState.IsValid)
                {
                    CFAccessDefinition accessDefinition = new CFAccessDefinition();
                    string[] selectedModes = Request.Form["SelectedAccessModes"].Split(',');
                    accessDefinition = UpdateAccessDefinition(accessDefVM, selectedModes);
                    accessDefinition = AccessDefinitionService.EditAccessDefinition(accessDefinition);
                    accessDefinition.Serialize();
                    Db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                throw ex;
                
            }
            return View();
        }

        // GET: Manager/AccessDefinition/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Manager/AccessDefinition/Delete/5
        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool success = false;
            try
            {
                AccessDefinitionService.DeleteAccessDefinition(id);
                Db.SaveChanges(User.Identity);
                success = true;
            }catch(Exception ex)
            {
                success = false;
            }
           
                return Json(success, JsonRequestBehavior.AllowGet);
          
        }

        public CFAccessDefinition UpdateAccessDefinition(AccessDefinitionsViewModel accessDefVM, string[] selectedModes)
        {
            CFAccessDefinition accessDef = new CFAccessDefinition();
            if(accessDefVM.Id != null)
            {
                accessDef = AccessDefinitionService.GetAccessDefinitionById(accessDefVM.Id.Value);

                foreach (string am in selectedModes.ToList())
                {

                    AccessMode selectedMode = (AccessMode)Enum.Parse(typeof(AccessMode), am);
                    accessDef.AccessModes |= selectedMode;

                }
                

            }
            else
            {
                foreach (string am in selectedModes.ToList())
                {   
                   AccessMode selectedMode = (AccessMode)Enum.Parse(typeof(AccessMode), am);
                   accessDef.AccessModes |= selectedMode;
                   
                }
               
            }
            accessDef.Name = accessDefVM.Name;
            return accessDef;
        }

        public AccessDefinitionsViewModel UpdateAccessDefinitionViewModel(CFAccessDefinition accessDef, AccessDefinitionsViewModel adVM)
        {
            adVM.Name = accessDef.Name;
          //  List<AccessMode> selectedModes = accessDef.AccessModesList;

            adVM.AccessModes = PopulateAccessModesView(accessDef);
            return adVM;
        }

        private List<AccessCheckBox> PopulateAccessModesView(CFAccessDefinition accessDef)
        {
            List<AccessCheckBox> AccessModes = new List<AccessCheckBox>();
            int i = 0;
            foreach (AccessMode am in Enum.GetValues(typeof(AccessMode)))
            {
                if (i > 0) //skip the first one -- None
                {
                    if (accessDef.HasMode(am))
                    {
                        AccessModes.Add(new AccessCheckBox { Text = am.ToString(), Value = ((int)am), Checked=true });
                    }
                    else
                    {
                        AccessModes.Add(new AccessCheckBox { Text = am.ToString(), Value = ((int)am) });
                    }
                }
                i++;
            }
            return AccessModes;
        }
    }
}
