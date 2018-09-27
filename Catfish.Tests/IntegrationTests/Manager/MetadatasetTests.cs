using Catfish.Core.Models.Forms;
using Catfish.Tests.Extensions;
using Catfish.Tests.IntegrationTests.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.IntegrationTests.Manager
{
    [TestFixture(typeof(ChromeDriver))]
    class MetadataSetTests<TWebDriver> : BaseIntegrationTest<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        [Test]
        public void CanCreateMetadataSet()
        {
            string MetadataSetName = "Metadataset name";
            string MetadataSetDescription = "Metadataset description";

            FormField field1 = new FormField();
            field1.Name = "Test";

            List<FormField> formFields = new List<FormField>();
            formFields.Add(field1);
            CreateMetadataSet(MetadataSetName, MetadataSetDescription, formFields.ToArray());

            Driver.FindElement(By.LinkText(SettingsLinkText), 10).Click();
            Driver.FindElement(By.LinkText(MetadataSetsLinkText), 10).Click();
            
            // Last edit button belongs to our newly created metadata set
            IWebElement lastEditButton = GetLastEditButton();
            lastEditButton.Click();
            string nameText = Driver.FindElement(By.Id("Name")).GetAttribute("value");
            string descriptionText = Driver.FindElement(By.Id("Description")).GetAttribute("value");

            Assert.AreEqual(MetadataSetName, nameText);
            Assert.AreEqual(MetadataSetDescription, descriptionText);            
        }
    }
}
