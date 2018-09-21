using Catfish.Core.Models.Forms;
using Catfish.Tests.IntegrationTests.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.IntegrationTests.Manager
{
    [TestFixture(typeof(ChromeDriver))]
    class EntityTypeTests<TWebDriver> : BaseIntegrationTest<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        protected override void OnSetup()
        {
            
        }

        protected override void OnTearDown()
        {
            
        }

        [Test]
        public void CanCreateNewEntityType()
        {
            CreateMetadataSet("name 1", "desc 1", new FormField[0]);
        }
    }
}
