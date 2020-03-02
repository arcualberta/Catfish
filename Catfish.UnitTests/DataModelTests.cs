using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.ViewModels;
using Catfish.Core.Models.ViewModels;
using Catfish.Core.Services;
using Catfish.Tests.Helpers;
using NUnit.Framework;
using System;

namespace Catfish.UnitTests
{
    public class DataModelTests
    {
        protected AppDbContext _db;
        protected TestHelper _testHelper;

        [SetUp]
        public void Setup()
        {
            _testHelper = new TestHelper();
            _db = _testHelper.Db;
        }

        [Test]
        public void CreateItemTest()
        {
            //Crreating an empty item
            Item item = new Item();
            item.Initialize();

            //Creating an entity view model from the item to manipulate the item
            EntityVM vm = item.InstantiateViewModel<EntityVM>();

            //Creating and adding new metadata sets to the item
            MetadataSet ms = new MetadataSet();

            vm.AppendMetadataSet(ms);


            _db.Items.Add(item);
            _db.SaveChanges();

            Assert.AreNotEqual(null, item.Id);
            Assert.AreEqual(true, item.Created > DateTime.MinValue);
            Assert.AreEqual(item.GetType().AssemblyQualifiedName, item.ModelType);

            _db.Items.Remove(item);
            _db.SaveChanges();
        }

        [Test]
        public void CloneEntityTypeTest()
        {
            EntityType template = new EntityType();
            template.Initialize();

            Item item = template.Clone<Item>();
            Assert.AreNotEqual(template.Id, item.Id);
            Assert.AreEqual(item.GetType().AssemblyQualifiedName, item.ModelType);

            Collection collection = template.Clone<Collection>();
            Assert.AreNotEqual(template.Id, collection.Id);
            Assert.AreEqual(collection.GetType().AssemblyQualifiedName, collection.ModelType);
        }

        [Test]
        public void CreateEntityTypeTest()
        {
            SeedingService srv = _testHelper.Seviceprovider.GetService(typeof(SeedingService)) as SeedingService;

            MetadataSet ms = srv.NewDublinCoreMetadataSet();

            EntityType entityType = new EntityType()
            {
                Name = "Defauly Item"
            };

            ms.Data.Save("metadata.xml");
        }



    }
}