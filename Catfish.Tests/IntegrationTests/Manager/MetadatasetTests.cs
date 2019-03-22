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

            TextField field1 = new TextField();
            field1.Name = "Test";
            List<FormField> formFields = new List<FormField>();
            formFields.Add(field1);
            bool isMultiple = true;
            CreateMetadataSet(MetadataSetName, MetadataSetDescription, formFields.ToArray(), isMultiple);

            Driver.FindElement(By.LinkText(SettingsLinkText), 10, 1000).Click();
            Driver.FindElement(By.LinkText(MetadataSetsLinkText), 10, 1000).Click();
            
            // Last edit button belongs to our newly created metadata set
            IWebElement lastEditButton = GetLastEditButton();
            lastEditButton.Click();
            string nameText = Driver.FindElement(By.Id("Name")).GetAttribute("value");
            string descriptionText = Driver.FindElement(By.Id("Description")).GetAttribute("value");

            Assert.AreEqual(MetadataSetName, nameText);
            Assert.AreEqual(MetadataSetDescription, descriptionText);

            // XXX Check metadataset field valiues

            List<IWebElement> fieldEntries = Driver.FindElements(By.ClassName("field-entry"), 10, 1000).ToList();
            Assert.AreEqual(formFields.Count(), fieldEntries.Count(), "formFields and IWebelements count dont match");
            
            for(int i=0; i < formFields.Count; ++i)
            {
                CompareFormField(formFields[i], fieldEntries[i]);
            }            
        }

        private void CompareFormField(FormField formField, IWebElement fieldEntry)
        {
            // XXX For now just check name and required

            string expectedNameEn = formField.Name;
            string realNameEn = fieldEntry.FindElement(By.Name("Name_en")).GetAttribute("value");

            Assert.AreEqual(expectedNameEn, realNameEn, "Name mismatch");

            bool expectedIsRequired = formField.IsRequired;
            bool realIsRequired = fieldEntry.FindElement(By.ClassName("field-is-required")).Selected;

            Assert.AreEqual(expectedIsRequired, realIsRequired, "Is required mismatch");

        }
    }
}
