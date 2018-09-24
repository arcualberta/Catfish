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
            CreateMetadataSet(MetadataSetName, MetadataSetDescription, formFields.ToArray());
            //Driver.FindElement(By.LinkText(EntityTypesLinkText)).Click();
        }
    }
}
