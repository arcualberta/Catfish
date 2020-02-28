using Catfish.Core.Models;
using NUnit.Framework;
using System;

namespace Catfish.UnitTests
{
    public class DataModelTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ItemCreateTest()
        {
            DateTime t1 = DateTime.Now;

            Item item = new Item();
            item.Initialize();

            Guid guid = item.Guid;
            Assert.AreNotEqual(null, guid);
            Assert.AreEqual(true, item.Created > t1);
            Assert.AreEqual(item.GetType().AssemblyQualifiedName, item.ModelType);
        }

        [Test]
        public void EntityTypeCloneTest()
        {
            EntityType template = new EntityType();
            template.Initialize();

            Item item = template.Clone<Item>();
            Assert.AreNotEqual(template.Guid, item.Guid);
            Assert.AreEqual(item.GetType().AssemblyQualifiedName, item.ModelType);

            Collection collection = template.Clone<Collection>();
            Assert.AreNotEqual(template.Guid, collection.Guid);
            Assert.AreEqual(collection.GetType().AssemblyQualifiedName, collection.ModelType);

        }

    }
}