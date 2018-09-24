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
        [Ignore("Needs to be completed")]
        [Test]
        public void CanCreateNewEntityType()
        {
            string EntityTypeName = "Entity type name";
            string EntityTypeDescription = "Entity type description";

            CreateMetadataSet(EntityTypeName, EntityTypeDescription, new FormField[1]);
            Driver.FindElement(By.LinkText(EntityTypesLinkText)).Click();
        }
    }
}
