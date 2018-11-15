using Catfish.Core.Models;
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
    class ItemTests<TWebDriver> : BaseIntegrationTest<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        [Test]
        public void CanCreateItem()
        {
            CreateBaseEntityType();
            // FormFields is instantiated in CreateBaseEntityType
            CreateItem(EntityTypeName, FormFields[0]);

            Driver.FindElement(By.LinkText(ContentLinkText), 10).Click();
            Driver.FindElement(By.LinkText(ItemsLinkText), 10).Click();

            GetLastEditButton().Click();
            
            string savedValue = Driver.FindElement(By.Id("MetadataSets_0__Fields_0__Values_0__Value")).GetAttribute("value");

            Assert.AreEqual(ItemValue, savedValue);
        }
    }
}
