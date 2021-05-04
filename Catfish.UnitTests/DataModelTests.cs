using Catfish.Core.Models;
using Catfish.Core.Models.Solr;
using Catfish.Core.Services;
using Catfish.Core.Services.Solr;
using Catfish.Models.ViewModels;
using Catfish.Test.Helpers;
using NUnit.Framework;
using Piranha.AspNetCore.Identity.SQLServer;
using SolrNet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Catfish.UnitTests
{
    public class DataModelTests
    {
        protected AppDbContext _db;
        protected TestHelper _testHelper;
        private ISolrIndexService<SolrEntry> solrIndexService;
        private IEntityIndexService entityService;
        private readonly ISolrReadOnlyOperations<SolrEntry> _solr;
        [SetUp]
        public void Setup()
        {
            _testHelper = new TestHelper();
            solrIndexService = _testHelper.Seviceprovider.GetService(typeof(ISolrIndexService<SolrEntry>)) as ISolrIndexService<SolrEntry>;
            entityService = _testHelper.Seviceprovider.GetService(typeof(IEntityIndexService)) as IEntityIndexService;
            _db = _testHelper.Db;
        }

        //[Test]
        //public void CreateItemTest()
        //{
        //    //Crreating an empty item
        //    Item item = new Item();
        //    ////item.Initialize();

        //    //Creating an entity view model from the item to manipulate the item
        //    EntityVM vm = item.InstantiateViewModel<EntityVM>();

        //    //Creating and adding new metadata sets to the item
        //    SeedingService srv = _testHelper.Seviceprovider.GetService(typeof(SeedingService)) as SeedingService;
        //    MetadataSet ms = srv.NewDublinCoreMetadataSet();

        //    vm.MetadataSets.Add(ms);

        //    _db.Items.Add(item);
        //    _db.SaveChanges();

        //    Assert.AreNotEqual(null, item.Id);
        //    Assert.AreEqual(true, item.Created > DateTime.MinValue);
        //    Assert.AreEqual(item.GetType().AssemblyQualifiedName, item.ModelType);

        //    // Loading back the saved item and verifying
        //    Item reloaded = _db.Entities.Where(e => e.Id == item.Id).FirstOrDefault() as Item;
        //    Assert.AreNotEqual(null, reloaded);
        //    EntityVM reloadedVM = reloaded.InstantiateViewModel<EntityVM>();

        //    reloadedVM.Data.Save("item.xml");


        //    _db.Items.Remove(item);
        //    _db.SaveChanges();
        //}

        //[Test]
        //public void CloneEntityTypeTest()
        //{
        //    EntityTemplate template = new EntityTemplate();
        //    ////template.Initialize();

        //    Item item = template.Clone<Item>();
        //    Assert.AreNotEqual(template.Id, item.Id);
        //    Assert.AreEqual(item.GetType().AssemblyQualifiedName, item.ModelType);

        //    Collection collection = template.Clone<Collection>();
        //    Assert.AreNotEqual(template.Id, collection.Id);
        //    Assert.AreEqual(collection.GetType().AssemblyQualifiedName, collection.ModelType);
        //}

        //[Test]
        //public void CreateEntityTypeTest()
        //{
        //    SeedingService srv = _testHelper.Seviceprovider.GetService(typeof(SeedingService)) as SeedingService;

        //    MetadataSet ms = srv.NewDublinCoreMetadataSet();

        //    EntityTemplate entityType = new EntityTemplate()
        //    {
        //        TemplateName = "Defauly Item"
        //    };

        //    ms.Data.Save("metadata.xml");
        //}


        [Test]
        public void SeedData()
        {
            SeedingService srv = _testHelper.Seviceprovider.GetService(typeof(SeedingService)) as SeedingService;

            srv.SeedDefaults(true);

            _testHelper.Db.SaveChanges();

        }

        [Test]
        public void ConfigureTestGroups()
        {
            AppDbContext appdb = _testHelper.Db;
            IdentitySQLServerDb pirdb = _testHelper.PiranhaDb;

            List<string> testGroupNames = new List<string>() { "ARC", "Music", "Drama", "History and Classics", "English and Film Studies" };
            foreach (var groupName in testGroupNames)
                if (!appdb.Groups.Where(gr => gr.Name == groupName).Any())
                    appdb.Groups.Add(new Group() { Name = groupName, GroupStatus = Group.eGroupStatus.Active, Id = Guid.NewGuid() });
            //db.SaveChanges();

            //Adding all roles to all groups
            var roles = pirdb.Roles.Except(pirdb.Roles.Where(r => r.NormalizedName == "SYSADMIN")).ToList();
            foreach(var role in roles)
                foreach (var group in appdb.Groups)
                    if (appdb.GroupRoles.Where(gr => gr.GroupId == group.Id && gr.RoleId == role.Id).Any() == false)
                        appdb.GroupRoles.Add(new GroupRole() { RoleId = role.Id, GroupId = group.Id, Id = Guid.NewGuid() });


            //Associating all item templates with a workflow with all test groups
            var templates = appdb.ItemTemplates.ToList();
            foreach(var template in templates)
            {
                if (template.Workflow != null)
                {
                    foreach (var group in appdb.Groups.ToList())
                    {
                        if (appdb.GroupTemplates.Where(gt => gt.GroupId == group.Id && gt.EntityTemplateId == template.Id).Any() == false)
                            appdb.GroupTemplates.Add(new GroupTemplate() { GroupId = group.Id, EntityTemplateId = template.Id, Id = Guid.NewGuid() });
                        //db.SaveChanges();
                    }
                }
            }

            //Adding the "admin" user to all roles under the each group
            var adminUserId = pirdb.Users.Where(u => u.NormalizedUserName == "ADMIN").Select(u => u.Id).FirstOrDefault();
            foreach(var groupRole in appdb.GroupRoles)
            {
                var role = pirdb.Roles.Where(r => r.Id == groupRole.RoleId).FirstOrDefault();
                if (appdb.UserGroupRoles.Where(ugr => ugr.GroupRoleId == groupRole.Id && ugr.UserId == adminUserId).Any() == false)
                    appdb.UserGroupRoles.Add(new UserGroupRole() { GroupRoleId = groupRole.Id, UserId = adminUserId, Id = Guid.NewGuid() });
                //db.SaveChanges();
            }

            appdb.SaveChanges();
        }

        ////        [Test]
        ////        public void SolrNewItemTest()
        ////        {
        ////            //private ISolrIndexService<SolrItemModel> solrIndexService;
        ////            SeedingService srv = _testHelper.Seviceprovider.GetService(typeof(SeedingService)) as SeedingService;

        ////            ItemTemplate template = SeedingService.NewDublinCoreItem();
        ////            Item item = template.Instantiate<Item>();
        ////            item.MetadataSets[0].SetFieldValue<TextField>("Subject", "en", "New restaurants are mad crazy to be opening right now -- or are they?", "en");
        ////            string desc = @"On Wednesdays, Palmetto, which opened for the first time on May 11 in Oakland, California, serves a full prime rib dinner -- to go.
        ////It's 'enough to feed two people, or one really hungry person,' Christ Aivaliotis, one of the new restaurant's owners, tells CNN Travel.
        ////But this isn't how Palmetto or a smattering of other restaurants around the world expected to be operating in the spring of 2020.
        ////Modified menus, a bare - bones staff and the seemingly gargantuan task of attracting business in a time of such grave uncertainty are all factors in a new food and beverage operation.
        ////'It may not be ideal,' says Lilly W.Jan, a lecturer in food and beverage management at Cornell's School of Hotel Administration, but she wouldn't call it 'crazy.'";

        ////            item.MetadataSets[0].SetFieldValue<TextArea>("Description", "en", desc, "en");
        ////            item.Name.SetContent("Solr documents");
        ////            item.Description.SetContent("Solr documents Content");
        ////            _testHelper.Db.Items.Add(item);
        ////            _testHelper.Db.SaveChanges();

        ////            entityService.AddUpdateEntity(item);

        ////        }


        //[Test]
        ////public void Query()
        ////{
        ////   // var solr = ServiceLocator.current.getinstance<ISolrOperations<SolrItemModel>>();
        ////    var results = _solr.Query(new SimpleQueryByField("id", "sp2514n"));
        ////    Assert.AreEqual(1, results.Count);
        ////    Console.WriteLine(Results[0].manufacturer);
        ////}


        //[Test]
        //public void EntityListEntryViewModelTest()
        //{
        //    Item item = _testHelper.Db.Items.FirstOrDefault();
        //    Assert.IsNotNull(item);
        //    string idFromXml = item.Data.Attribute("id").Value;
        //    Assert.AreEqual(idFromXml, item.Id.ToString());

        //    EntityListEntry entry = new EntityListEntry(item);
        //    Assert.AreEqual(entry.Id.ToString(), item.Id.ToString());
        //}

        //[Test]
        //public void ItemListEntryRetieveTest()
        //{
        //    DbEntityService srv = _testHelper.Seviceprovider.GetService(typeof(DbEntityService)) as DbEntityService;
        //    Assert.IsNotNull(srv);

        //    var itemListEntries = srv.GetEntityList<Item>();
        //    //foreach(var entry in itemListEntries)
        //    //{
        //    //    Item item = _testHelper.Db.Items.Where(it => it.Id == entry.Id).FirstOrDefault();
        //    //    Assert.IsNotNull(item);
        //    //    Assert.AreEqual(entry.Id.ToString(), item.Id.ToString());
        //    //}

        //    //Assert.IsNotNull(item);
        //    //string idFromXml = item.Data.Attribute("id").Value;
        //    //Assert.AreEqual(idFromXml, item.Id.ToString());

        //    //EntityListEntry entry = new EntityListEntry(item);
        //    //Assert.AreEqual(entry.Id.ToString(), item.Id.ToString());

        //}

        [Test]
        public void AddUserGroupTest()
        {
            Guid userId = Guid.Parse("40134C7B-0DBC-4A29-9E8B-BB4D74308944");
            Guid groupRoleId = Guid.Parse("4717DF86-AA34-4686-B4F7-C2153C6EFFF2");

            var srv = _testHelper.Seviceprovider.GetService(typeof(IGroupService)) as IGroupService;
            srv.AddUserGroupRole(userId, groupRoleId);
       
        }

    }
}