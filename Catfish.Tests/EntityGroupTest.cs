using Catfish.Areas.Manager.Controllers;
using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Core.Models;
using Catfish.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using System.Data.Entity;
using System.Data;
using Piranha;
using System.Linq;



namespace Catfish.Tests
{
    [TestClass]
    public class EntityGroupTest
    {
        CatfishDbContext db = new CatfishDbContext();
        [TestMethod]
        public void CreateEntityGroup()
        { 
            if (db.Database.Connection.State == ConnectionState.Closed)
            {
                db.Database.Connection.Open();
            }
            EntityGroupViewModel egVM = new EntityGroupViewModel();

            EntityGroup entGrp = null;
            EntityGroupService srv = new EntityGroupService(db);

            //data from sysuser table
            egVM.AllUsers2.Add("4BC87457-77F5-4E3B-9B6E-12BE58F523E7", "ninja"); 
            egVM.AllUsers2.Add("018ED9DB-0AE0-4D38-BE21-515A024BBD5C", "melania");
            
            egVM.Id = Guid.NewGuid().ToString();
            egVM.EntityGroupName = TestData.LoremIpsum(5, 10);// "Test Creation of Entity Group";
            egVM.SelectedUsers.Add("ninja");



            //saving data
            List<EntityGroupUser> oldUsers = new List<EntityGroupUser>();
            if (entGrp != null)
                oldUsers = entGrp.EntityGroupUsers.ToList();

            entGrp = egVM.UpdateModel(entGrp);
            entGrp = srv.EditEntityGroup(entGrp, oldUsers);

            db.SaveChanges();

            //checking if saving succesfull
            EntityGroup newlyCreatedGrp = srv.GetEntityGroup(entGrp.Id.ToString());

            Assert.IsNotNull(newlyCreatedGrp);
            Assert.AreEqual(1, newlyCreatedGrp.EntityGroupUsers.Count);          
        }
        

       

    }
}
