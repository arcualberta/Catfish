using Catfish.Core.Models;
using Catfish.Core.Models.Access;
using Catfish.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Catfish.Tests
{
    [TestClass]
    public class EntityTests
    {

        MockEntity MockEntity;

        [TestInitialize]
        public void Initialize()
        {
            MockEntity = new MockEntity();
        }

        [TestMethod]
        public void CanUseAccessGroups()
        {
            int count = 3;
            List<AccessGroup> accessGroups = new List<AccessGroup>();

            for (int i = 0; i < count; ++i)
            {
                accessGroups.Add(new AccessGroup());
            }

            MockEntity.AccessGroups = accessGroups;
            Assert.AreEqual(accessGroups.Count, MockEntity.AccessGroups.Count);

            for (int i = 0; i < count; ++i)
            {
                Assert.AreEqual(accessGroups[i].Data, MockEntity.AccessGroups[i].Data);
            }

            accessGroups.Clear();
            MockEntity.AccessGroups = accessGroups;
            Assert.AreEqual(0, MockEntity.AccessGroups.Count);
            
        }
    }
}
