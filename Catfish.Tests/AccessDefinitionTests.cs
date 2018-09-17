using Catfish.Core.Models.Access;
using Catfish.Tests.Helpers;
using NUnit.Framework;
using System.Collections.Generic;

namespace Catfish.Tests
{
    [TestFixture]
    public class AccessDefinitionTests : BaseUnitTest
    {
        CFAccessDefinition AccessDefinition;
        
        protected override void OnSetup()
        {
            AccessDefinition = new CFAccessDefinition();
        }

        protected override void OnTearDown()
        {
        }

        [Test]
        public void CanUseAccessModes()
        {
            AccessDefinition.AccessModes = AccessMode.Append;
            Assert.IsTrue(AccessDefinition.HasMode(AccessMode.Append));
        }

        [Test]
        public void CanUseMixedAccessModes()
        {
            AccessMode mixedAccessMode = AccessMode.Append | AccessMode.Control;
            AccessDefinition.AccessModes = mixedAccessMode;
            Assert.IsTrue(AccessDefinition.HasModes(mixedAccessMode));
        }

        [Test]
        public void CanUseSingleAccessModeFromMixed()
        {
            AccessMode mixedAccessMode = AccessMode.Control
                | AccessMode.Read
                | AccessMode.Write;
            AccessDefinition.AccessModes = mixedAccessMode;

            Assert.IsTrue(AccessDefinition.HasMode(AccessMode.Control));
            Assert.IsTrue(AccessDefinition.HasMode(AccessMode.Read));
            Assert.IsTrue(AccessDefinition.HasMode(AccessMode.Write));
        }

        [Test]
        public void CanFindMissingAccessMode()
        {
            AccessMode mixedAccessMode = AccessMode.Control
                | AccessMode.Append;
            AccessDefinition.AccessModes = mixedAccessMode;

            // Test used to check agains AccessMode.Read but now AccessMode.Read
            // is always present for all modes
            Assert.IsFalse(AccessDefinition.HasMode(AccessMode.Write));

        }

        [Test]
        public void CanFindMissingMixedAccessMode()
        {
            AccessMode mixedAccessMode = AccessMode.Control
                | AccessMode.Append;

            AccessMode readWriteAccessMode = AccessMode.Read
                | AccessMode.Write;

            AccessDefinition.AccessModes = mixedAccessMode;

            Assert.IsFalse(AccessDefinition.HasModes(readWriteAccessMode));

        }

        [Test]
        public void CanListAccessModes()
        {
            AccessMode controlWriteAccessMode = AccessMode.Control | AccessMode.Write;
            AccessDefinition.AccessModes = controlWriteAccessMode;
            List<AccessMode> accessModes = AccessDefinition.AccessModesList;
            Assert.IsTrue(accessModes.Contains(AccessMode.Control));
            Assert.IsTrue(accessModes.Contains(AccessMode.Write));
            Assert.IsFalse(accessModes.Contains(AccessMode.Append));
        }
    }
}