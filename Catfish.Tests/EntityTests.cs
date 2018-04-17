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
            AccessGroup accessGroup1 = new AccessGroup();
            AccessGroup accessGroup2 = new AccessGroup();
            List<AccessGroup> accessGroups = new List<AccessGroup>()
            {
                accessGroup1,
                accessGroup2
            };
            MockEntity.AccessGroups = accessGroups;
            //Assert.AreEqual(accessGroups, MockEntity.AccessGroups);

            accessGroups.Clear();
            MockEntity.AccessGroups = accessGroups;
            Assert.AreEqual(0, MockEntity.AccessGroups.Count);
            
        }

        //[TestMethod]
        //public void CanUseAccessDefinition()
        //{
        //    AccessMode readWriteAccessMode = AccessMode.Read | AccessMode.Write;
        //    AccessGroup.AccessDefinition.AccessModes = readWriteAccessMode;
        //    Assert.IsTrue(AccessGroup.AccessDefinition.HasMode(readWriteAccessMode));
        //}

        //[TestMethod]
        //public void CanUseAccessGuids()
        //{
        //    int guidCount = 5;
        //    List<Guid> guidList = new List<Guid>();

        //    for (int i = 0; i < guidCount; ++i)
        //    {
        //        guidList.Add(Guid.NewGuid());
        //    }

        //    AccessGroup.AccessGuids = guidList;

        //    for (int i = 0; i < guidCount; ++i)
        //    {
        //        Assert.AreEqual(guidList[i], AccessGroup.AccessGuids[i]);
        //    }
        //}
    }
}
