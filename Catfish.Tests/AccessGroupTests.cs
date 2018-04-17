using Catfish.Core.Models.Access;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Catfish.Tests
{
    [TestClass]
    public class AccessGroupTests
    {

        AccessGroup AccessGroup;

        [TestInitialize]
        public void Initialize()
        {
            AccessGroup = new AccessGroup();
        }

        [TestMethod]
        public void CanUseAccessDefinition()
        {
            AccessMode readWriteAccessMode = AccessMode.Read | AccessMode.Write;
            AccessGroup.AccessDefinition.AccessModes = readWriteAccessMode;
            Assert.IsTrue(AccessGroup.AccessDefinition.HasMode(readWriteAccessMode));
        }

        [TestMethod]
        public void CanUseAccessGuids()
        {
            int guidCount = 5;
            List<Guid> guidList = new List<Guid>();

            for (int i = 0; i < guidCount; ++i)
            {
                guidList.Add(Guid.NewGuid());
            }

            AccessGroup.AccessGuids = guidList;

            for (int i = 0; i < guidCount; ++i)
            {
                Assert.AreEqual(guidList[i], AccessGroup.AccessGuids[i]);
            }
        }
    }
}
