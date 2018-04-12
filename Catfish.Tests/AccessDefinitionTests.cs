using Catfish.Areas.Manager.Controllers;
using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Core.Models;
using Catfish.Core.Models.Access;
using Catfish.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Piranha.Areas.Manager.Views.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Catfish.Tests
{
    [TestClass]
    public class AccessDefinitionTests
    {
        AccessDefinition accessDefinition;

        [TestInitialize]
        public void Initialize()
        {
            accessDefinition = new AccessDefinition();
        }     

        [TestMethod]
        public void CanUseAccessModes()
        {
            accessDefinition.AccessModes = AccessMode.Append;
            Assert.IsTrue(accessDefinition.HasMode(AccessMode.Append));
        }

        [TestMethod]
        public void CanUseMixedAccessModes()
        {
            AccessMode mixedAccessMode = AccessMode.Append | AccessMode.Discover;
            accessDefinition.AccessModes = mixedAccessMode;
            Assert.IsTrue(accessDefinition.HasModes(mixedAccessMode));
        }

        [TestMethod]
        public void CanUseSingleAccessModeFromMixed()
        {
            AccessMode mixedAccessMode = AccessMode.Control
                | AccessMode.Read
                | AccessMode.Write;
            accessDefinition.AccessModes = mixedAccessMode;

            Assert.IsTrue(accessDefinition.HasMode(AccessMode.Control));
            Assert.IsTrue(accessDefinition.HasMode(AccessMode.Read));
            Assert.IsTrue(accessDefinition.HasMode(AccessMode.Write));
        }

        [TestMethod]
        public void CanFindMissingAccessMode()
        {
            AccessMode mixedAccessMode = AccessMode.Discover
                | AccessMode.Append;
            accessDefinition.AccessModes = mixedAccessMode;

            Assert.IsFalse(accessDefinition.HasMode(AccessMode.Read));

        }

        [TestMethod]
        public void CanFindMissingMixedAccessMode()
        {
            AccessMode mixedAccessMode = AccessMode.Discover
                | AccessMode.Append;

            AccessMode readWriteAccessMode = AccessMode.Read
                | AccessMode.Write;

            accessDefinition.AccessModes = mixedAccessMode;

            Assert.IsFalse(accessDefinition.HasModes(readWriteAccessMode));

        }
    }
}