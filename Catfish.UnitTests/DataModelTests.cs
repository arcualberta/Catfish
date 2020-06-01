using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.ViewModels;
using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using Catfish.Core.Models.Solr;
using Catfish.Core.Services;
using Catfish.Core.Services.Solr;
using Catfish.Tests.Helpers;
using Microsoft.Data.SqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Catfish.UnitTests
{
    public class DataModelTests
    {
        protected AppDbContext _db;
        protected TestHelper _testHelper;
        private ISolrIndexService<SolrItemModel> solrIndexService;
        private IEntityService entityService;
        [SetUp]
        public void Setup()
        {
            _testHelper = new TestHelper();
            solrIndexService = _testHelper.Seviceprovider.GetService(typeof(ISolrIndexService<SolrItemModel>)) as ISolrIndexService<SolrItemModel>;
            entityService = _testHelper.Seviceprovider.GetService(typeof(IEntityService)) as IEntityService;
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
        public void SolrNewItemTest()
        {
            //private ISolrIndexService<SolrItemModel> solrIndexService;
            SeedingService srv = _testHelper.Seviceprovider.GetService(typeof(SeedingService)) as SeedingService;

            ItemTemplate template = srv.NewDublinCoreItem();
            Item item = template.Clone<Item>();
            item.MetadataSets[0].SetFieldValue("Subject", "en", "With tourists gone, Amsterdam locals reclaim their city", "en");
            string desc = @"A couple of weeks after the first coronavirus case arrived in the Netherlands, we were told to stay inside. Bars and schools closed down and my hometown of Amsterdam came to a halt.
After the first feelings of confusion and uncertainty, I slowly got used to the idea. There was a calmness in the streets I hadn't experienced in years.
In the past decade, Amsterdam has become a hasty and chaotic place, its occupants increasingly short-tempered. The city's population of 863,000 was annually swollen by nine million tourists.
The shops in the city center were given over to cater to them, selling waffles, souvenirs and cannabis seeds. Stores catering to residents closed down because of extreme hikes in rent and the lack of customers.
More and more, locals have started to avoid the most beautiful part of their city, as its houses were rented out to tourists and expats.";

            item.MetadataSets[0].SetFieldValue("Description", "en", desc, "en");
            item.Name.SetContent("Test Name");
            item.Description.SetContent("Test Description");
            _testHelper.Db.Items.Add(item);
            _testHelper.Db.SaveChanges();

            entityService.AddUpdateEntity(item);

        }


        public bool AddUpdate(Entity entity)
        {
            List<SolrItemModel> entries = ExtractSolrEntries(entity);
            foreach (var entry in entries)
                solrIndexService.AddUpdate(entry);

            return true;
        }

        public List<SolrItemModel> ExtractSolrEntries(Entity entity)
        {
            List<SolrItemModel> entries = new List<SolrItemModel>();
            entries.Add(new SolrItemModel());

            return entries;
        }


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

    }
}