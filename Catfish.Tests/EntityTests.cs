using Catfish.Core.Models;
using Catfish.Core.Models.Access;
using Catfish.Tests.Helpers;
using Catfish.Tests.Mocks;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Catfish.Tests
{
    [TestFixture]
    public class EntityTests : BaseUnitTest
    {

        MockEntity MockEntity;
        
        protected override void OnSetup()
        {
            MockEntity = new MockEntity();
        }

        protected override void OnTearDown()
        {
        }

        [Test]
        public void CanUseAccessGroups()
        {
            int count = 3;
            List<CFAccessGroup> accessGroups = new List<CFAccessGroup>();

            for (int i = 0; i < count; ++i)
            {
                accessGroups.Add(new CFAccessGroup());
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
