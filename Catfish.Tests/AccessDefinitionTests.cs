using Catfish.Core.Models.Access;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Catfish.Tests
{
    [TestClass]
    public class AccessDefinitionTests
    {
        CFAccessDefinition AccessDefinition;

        [TestInitialize]
        public void Initialize()
        {
            AccessDefinition = new CFAccessDefinition();
        }     

        [TestMethod]
        public void CanUseAccessModes()
        {
            AccessDefinition.AccessModes = AccessMode.Append;
            Assert.IsTrue(AccessDefinition.HasMode(AccessMode.Append));
        }

        [TestMethod]
        public void CanUseMixedAccessModes()
        {
            AccessMode mixedAccessMode = AccessMode.Append | AccessMode.Control;
            AccessDefinition.AccessModes = mixedAccessMode;
            Assert.IsTrue(AccessDefinition.HasModes(mixedAccessMode));
        }

        [TestMethod]
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

        [TestMethod]
        public void CanFindMissingAccessMode()
        {
            AccessMode mixedAccessMode = AccessMode.Control
                | AccessMode.Append;
            AccessDefinition.AccessModes = mixedAccessMode;

            // Test used to check agains AccessMode.Read but now AccessMode.Read
            // is always present for all modes
            Assert.IsFalse(AccessDefinition.HasMode(AccessMode.Write));

        }

        [TestMethod]
        public void CanFindMissingMixedAccessMode()
        {
            AccessMode mixedAccessMode = AccessMode.Control
                | AccessMode.Append;

            AccessMode readWriteAccessMode = AccessMode.Read
                | AccessMode.Write;

            AccessDefinition.AccessModes = mixedAccessMode;

            Assert.IsFalse(AccessDefinition.HasModes(readWriteAccessMode));

        }

        [TestMethod]
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