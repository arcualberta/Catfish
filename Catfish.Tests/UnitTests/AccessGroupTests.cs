using Catfish.Core.Models.Access;
using Catfish.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Catfish.Tests
{
    [TestFixture]
    public class AccessGroupTests : BaseUnitTest
    {

        CFAccessGroup AccessGroup;

        [Test]
        public void CanUseAccessDefinition()
        {
            AccessMode readWriteAccessMode = AccessMode.Read | AccessMode.Write;
            AccessGroup.AccessDefinition.AccessModes = readWriteAccessMode;
            Assert.IsTrue(AccessGroup.AccessDefinition.HasMode(readWriteAccessMode));
        }

        [Test]
        public void CanUseAccessGuid()
        {

            Guid guid1 = Guid.NewGuid();
            Guid guid2 = guid1;

            AccessGroup.AccessGuid = guid1;
            Assert.AreEqual(guid2, AccessGroup.AccessGuid);
        }

        [Test]
        public void CanCheckIsInheritable()
        {
            AccessGroup.IsInherited = false;
            Assert.IsFalse(AccessGroup.IsInherited);
        }

        protected override void OnSetup()
        {
            AccessGroup = new CFAccessGroup();
        }

        protected override void OnTearDown()
        {
        }
    }
}
