using Catfish.Core.Models;
using Catfish.Core.Models.Access;
using Catfish.Core.Models.Forms;
using Catfish.Tests.Extensions;
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
    class AccessDefinitionsTests<TWebDriver> : BaseIntegrationTest<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        [Test]
        public void CanCreateAccessDefinitions()
        {
            string accessDefinitionName = "Public read";
            //XXX Currently only checking AccessMode.Read manually
            AccessMode accessMode = AccessMode.Read;
            
            CreateAccessDefinition(accessDefinitionName, accessMode);

            // Check name
            string pathToAccessDefinitionName = "(//table[contains(@class, 'list bs')]//td)[1]";
            string pathToAccessModeName = "(//table[contains(@class, 'list bs')]//td)[2]";
            string setName = Driver.FindElement(By.XPath(pathToAccessDefinitionName)).Text;
            string setAccessMode = Driver.FindElement(By.XPath(pathToAccessModeName)).Text;
            string stringAccessMode = AccessMode.Read.ToString();

            Assert.AreEqual(accessDefinitionName, setName);
            Assert.AreEqual(stringAccessMode, setAccessMode);
            //XXX Check if accessMode is set
        }

        [Test]
        public void CanUseAccessDefinitions()
        {

        }
    }
}
