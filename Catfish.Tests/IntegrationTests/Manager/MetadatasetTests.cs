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
            int metadataSetId = CreateMetadataSet(MetadataSetName, MetadataSetDescription, formFields.ToArray());

            Driver.FindElement(By.LinkText(SettingsLinkText)).Click();
            Driver.FindElement(By.LinkText(MetadataSetsLinkText)).Click();

            // find element in list
            Driver.FindElement(By.ClassName($"object-edit-{metadataSetId.ToString()}")).Click();

            string nameText = Driver.FindElement(By.Id("Name")).Text;
            string descriptionText = Driver.FindElement(By.Id("Description")).Text;

            Assert.AreEqual(MetadataSetName, nameText);
            Assert.AreEqual(MetadataSetDescription, descriptionText);
            //Driver.FindElement(By.LinkText(EntityTypesLinkText)).Click();
        }
    }
}
