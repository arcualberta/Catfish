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
        private ISolrIndexService<SolrItemModel> solrIndexService;
        private IEntityService entityService;
        private readonly ISolrReadOnlyOperations<SolrItemModel> _solr;
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
            item.MetadataSets[0].SetFieldValue("Subject", "en", "Add Documents", "en");
            string desc = @"Solr is built to find documents that match queries. Solr’s schema provides an idea of how content is structured (more on the schema later), but without documents there is nothing to find. Solr needs input before it can do much.

You may want to add a few sample documents before trying to index your own content. The Solr installation comes with different types of example documents located under the sub-directories of the example/ directory of your installation.

In the bin/ directory is the post script, a command line tool which can be used to index different types of documents. Do not worry too much about the details for now. The Indexing and Basic Data Operations section has all the details on indexing.";

            item.MetadataSets[0].SetFieldValue("Description", "en", desc, "en");
            item.Name.SetContent("Solr documents");
            item.Description.SetContent("Solr documents Content");
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

    }
}