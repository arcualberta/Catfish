using Catfish.Core.Models.Access;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Catfish.Tests
{
    [TestClass]
    public class AccessGroupTests
    {

        CFAccessGroup AccessGroup;

        [TestInitialize]
        public void Initialize()
        {
            AccessGroup = new CFAccessGroup();
        }

        [TestMethod]
        public void CanUseAccessDefinition()
        {
            AccessMode readWriteAccessMode = AccessMode.Read | AccessMode.Write;
            AccessGroup.AccessDefinition.AccessModes = readWriteAccessMode;
            Assert.IsTrue(AccessGroup.AccessDefinition.HasMode(readWriteAccessMode));
        }

        [TestMethod]
        public void CanUseAccessGuid()
        {

            Guid guid1 = Guid.NewGuid();
            Guid guid2 = guid1;

            AccessGroup.AccessGuid = guid1;
            Assert.AreEqual(guid2, AccessGroup.AccessGuid);
        }

        [TestMethod]
        public void CanCheckIsInheritable()
        {
            AccessGroup.IsInherited = false;
            Assert.IsFalse(AccessGroup.IsInherited);
        }
    }
}
