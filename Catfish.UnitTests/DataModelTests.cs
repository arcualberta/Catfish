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
            item.MetadataSets[0].SetFieldValue("Subject", "en", "Is Covid-19 changing our relationships?", "en");
            string desc = @"The Covid-19 pandemic has reshaped our personal relationships in unprecedented ways, forcing us to live closer together with some people and further apart from others. Life in lockdown has necessitated close, constant contact with our families and partners, but social distancing measures have isolated us from our friends and wider communities.

                    Both in China, which was the first country in the world to go into full lockdown when the virus emerged there, and in Hong Kong – where schools closed, shops were shuttered, and employees sent home – the virus has been brought under control and life has returned to some semblance of normality. But the pandemic has left some cracks in family relationships.

                    Most notably the high-pressure environment of confinement, combined with the financial stress brought about by a Covid-19 burdened economy, has led to a rise in marital conflict, according to Susanne Choi, a sociologist at the Chinese University of Hong Kong.
                            ";

            item.MetadataSets[0].SetFieldValue("Description", "en", desc, "en");
            item.Name.SetContent("BBC News");
            item.Description.SetContent("BBC News Content");
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