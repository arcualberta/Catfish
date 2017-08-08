using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Catfish.Core.Models;
using Catfish.Core.Services;
using Catfish.Core.Models.Metadata;
using System.Web.Script.Serialization;
using System.Web.Helpers;
using Newtonsoft.Json;

namespace Catfish.Areas.Manager.Controllers
{
    public class EntityTypesController : CatfishController
    {
        //private CatfishDbContext db = new CatfishDbContext();

        // GET: Manager/EntityTypes
        public ActionResult Index()
        {
            return View(EntityService.GetEntityTypes());
        }


        // GET: Manager/EntityTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            EntityType model = null;
            if (id.HasValue)
            {
                model = EntityService.GetEntityType(id.Value);
                /*if (model.MetadataSets.Count > 1)
                    model.MetadataSets.Remove(model.MetadataSets.Last());*/
            }
            else
            {
                model = new EntityType();

                //////TODO:Remove the following testing code
                ////List<MetadataSet> metadata = MetadataService.GetMetadataSets().ToList();
                ////int i = 0;
                ////foreach (var s in metadata)
                ////{
                ////    model.MetadataSets.Add(s);
                ////    if (++i >= 3)
                ////        break;
                ////}
            }

            var metadataSets = MetadataService.GetMetadataSets().ToList();
            ViewBag.MetadataSets = new JavaScriptSerializer().Serialize(metadataSets); //Json(MetadataService.GetMetadataSets().ToList());
            return View(model);
        }

        // POST: Manager/EntityTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EntityType entityType)
        {
            var test = Request.Params;
            if (ModelState.IsValid)
            {
                if(entityType.Id > 0)
                {
                    EntityService.UpdateEntityType(entityType);
                }
                else
                {
                    EntityService.CreateEntityType(entityType);
                }
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(entityType);
        }

        ////// GET: Manager/EntityTypes/Delete/5
        ////public ActionResult Delete(int? id)
        ////{
        ////    if (id == null)
        ////    {
        ////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        ////    }
        ////    EntityType entityType = db.EntityTypes.Find(id);
        ////    if (entityType == null)
        ////    {
        ////        return HttpNotFound();
        ////    }
        ////    return View(entityType);
        ////}

        ////// POST: Manager/EntityTypes/Delete/5
        ////[HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        ////public ActionResult DeleteConfirmed(int id)
        ////{
        ////    EntityType entityType = db.EntityTypes.Find(id);
        ////    db.EntityTypes.Remove(entityType);
        ////    db.SaveChanges();
        ////    return RedirectToAction("Index");
        ////}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }

        //private string GetSerializedMetadataSets()
        //{
        //    //var metadataSets = this.MetadataService.
        //    var metadataSets = MetadataService.GetMetadataSets().ToList();
        //    //var metadataSetViewModels = metadataSets.Select(ft => new MetadataSetDefinitionViewModel(ft));
        //    //return new JavaScriptSerializer().Serialize(metadataSets);
        //    //var test = Json(metadataSets).Data;
        //    //var test = Json(metadataSets);
        //    //test.RecursionLimit = 0;
        //    //return test.ToString();

        //    //var test = new JavaScriptSerializer();
        //    //test.RecursionLimit = 2;
        //    ////st.
        //    ////test.

        //    //return test.Serialize(metadataSets);

        //    //var test = Json(metadataSets);
        //    //test.Data(metadataSets
        //    //return test.

        //    //string result = JsonConvert.SerializeObject(metadataSets);

        //    //return result;
        //    //return metadataSets;

        //    string test = JsonConvert.SerializeObject(metadataSets, Formatting.Indented,
        //        new JsonSerializerSettings {
        //            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        //        });

        //    return test;

        //}
    }
}
