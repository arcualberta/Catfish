using Catfish.Core.Models;
using Catfish.Tests.Mocks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.Models
{
    class CFXmlModelTests
    {
        [Test]
        public void CanUseCreatedBy()
        {               
            CFXmlModel.InitializeExternally = (CFXmlModel model) =>
            {
                model.CreatedByGuid = "created-by-guid";
                model.CreatedByName = "created-by-name";
            };

            MockCFXmlModel mockCFXmlModel = new MockCFXmlModel();

            string createdByGuid = "created-by-guid";
            string createdByName = "created-by-name";

            Assert.AreEqual(createdByName, mockCFXmlModel.CreatedByName);
            Assert.AreEqual(createdByGuid, mockCFXmlModel.CreatedByGuid);
        }
    }
}
