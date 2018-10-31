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

            // Create metadata set
            // create entity type

            TextField fieldName = new TextField();
            fieldName.Name = FieldName;

            TextValue textValue = new TextValue("en", "English", ItemValue);
            fieldName.SetTextValues(new List<TextValue> { textValue });

            TextArea fieldDescription = new TextArea();
            fieldDescription.Name = "Description";
            List<FormField> formFields = new List<FormField>();
            formFields.Add(fieldName);
            formFields.Add(fieldDescription);

            CreateMetadataSet(MetadataSetName, MetadataSetDescription, formFields.ToArray());

            CreateEntityType(EntityTypeName, EntityTypeDescription, new[] {
                MetadataSetName
                }, new CFEntityType.eTarget[0]);

            // Finally create and check item

            CreateItem(EntityTypeName, formFields.ToArray());

            Driver.FindElement(By.LinkText(ContentLinkText), 10).Click();
            Driver.FindElement(By.LinkText(ItemsLinkText), 10).Click();

            GetLastEditButton().Click();

            string savedValue = Driver.FindElement(By.XPath("//input[contains(@class, 'text-box single-line')][1]")).GetAttribute("value");

            Assert.AreEqual(ItemValue, savedValue);
        }
    }
}
