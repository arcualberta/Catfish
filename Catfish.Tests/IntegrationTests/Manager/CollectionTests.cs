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
    class CollectionTests<TWebDriver> : BaseIntegrationTest<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        public static string[] MetadataSetNames =
        {
            "Simple metadata"
        };

        private static FormField[] MetadataFields =
        {
            new TextField() {Name = "Simple field"}
        };

        private static CFEntityType.eTarget[] TargetTypes =
        {
            CFEntityType.eTarget.Items,
            CFEntityType.eTarget.Collections
        };

        [Test]
        public void CanCreateCollection()
        {

            CreateBaseEntityType();
            // FormFields is instantiated in CreateBaseEntityType
            CreateCollection(EntityTypeName, FormFields[0]);

            Driver.FindElement(By.LinkText(ContentLinkText), 10).Click();
            Driver.FindElement(By.LinkText(CollectionsLinkText), 10).Click();

            GetLastEditButton().Click();

            string savedValue = Driver.FindElement(By.Id("MetadataSets_0__Fields_0__Values_0__Value")).GetAttribute("value");

            Assert.AreEqual(ItemValue, savedValue);
        }

        private void SetUpAssociationTest()
        {
            CreateMetadataSet(MetadataSetNames[0],
                "Simple metadata",
                MetadataFields);

            CreateEntityType(EntityTypeName, "Simple entity type", MetadataSetNames, TargetTypes,
                new Tuple<string, FormField>[] {
                    new Tuple<string, FormField>(MetadataSetNames[0], MetadataFields[0]),
                });            
        }

        [Test]
        public void CanAssociateParents()
        {

            SetUpAssociationTest();
            CreateCollections(5);
            // Create simple entity type
            NavigateToCollections();
            GetFirstAssociationsButton().Click();

            // select aggregation to add
            //IWebElement allTable = Driver.FindElement(By.Id("all-actionable-table"));

            // first checkbox
            string allDataTable = "(//div[@id='all-actionable-table']//table)[2]";

            Driver.FindElement(By.XPath($@"({allDataTable}//input[1])[1]"), 10).Click();
            string aggregationName1 = Driver.FindElement(By.XPath($@"{allDataTable}//tbody//tr[1]//td[2]"), 10).Text;

            // first aggregation name
            Driver.FindElement(By.XPath($@"({allDataTable}//input[1])[2]"), 10).Click();
            string aggregationName2 = Driver.FindElement(By.XPath($@"{allDataTable}//tbody//tr[2]//td[2]"), 10).Text;

            Driver.FindElement(By.Id("add-parents-action"), 10).Click();

            string parentsDataTable = "(//div[@id='parents-actionable-table']//table)[2]";
            string parentName1 = Driver.FindElement(By.XPath($@"{parentsDataTable}//tbody//tr[1]//td[2]"), 10).Text;
            string parentName2 = Driver.FindElement(By.XPath($@"{parentsDataTable}//tbody//tr[2]//td[2]"), 10).Text;

            Assert.AreEqual(aggregationName1, parentName1);
            Assert.AreEqual(aggregationName2, parentName2);

            int i = 0;

        }

        [Test]
        public void CanAssociateChildren()
        {

            SetUpAssociationTest();
            CreateCollections(1);
            CreateItems(1);
            // Create simple entity type
            NavigateToCollections();
            GetFirstAssociationsButton().Click();

            // select aggregation to add
            //IWebElement allTable = Driver.FindElement(By.Id("all-actionable-table"));

            // first checkbox
            string allDataTable = "(//div[@id='all-actionable-table']//table)[2]";

            Driver.FindElement(By.XPath($@"({allDataTable}//input[1])[1]"), 10).Click();
            string aggregationName1 = Driver.FindElement(By.XPath($@"{allDataTable}//tbody//tr[1]//td[2]"), 10).Text;

            // first aggregation name
            Driver.FindElement(By.XPath($@"({allDataTable}//input[1])[2]"), 10).Click();
            string aggregationName2 = Driver.FindElement(By.XPath($@"{allDataTable}//tbody//tr[2]//td[2]"), 10).Text;

            Driver.FindElement(By.Id("add-children-action"), 10).Click();

            string chidlrenDataTable = "(//div[@id='children-actionable-table']//table)[2]";
            string childName1 = Driver.FindElement(By.XPath($@"{chidlrenDataTable}//tbody//tr[1]//td[2]"), 10).Text;
            string childName2 = Driver.FindElement(By.XPath($@"{chidlrenDataTable}//tbody//tr[2]//td[2]"), 10).Text;

            Assert.AreEqual(aggregationName1, childName1);
            Assert.AreEqual(aggregationName2, childName2);

            int i = 0;
        }

        [Test]
        public void CanAssociateRelated()
        {
            SetUpAssociationTest();
            CreateItems(5);
        }


        private void CreateCollections(int count)
        {
            FormField[] fields = new FormField[1];

            MetadataFields[0].Serialize();

            for (int i = 0; i < count; ++i)
            {
                fields[0] = new TextField() { Content = MetadataFields[0].Content };
                fields[0].SetValues(new string[] { "Collection " + i });

                CreateCollection(EntityTypeName, fields);
            }
        }

        private void CreateItems(int count)
        {
            FormField[] fields = new FormField[1];

            MetadataFields[0].Serialize();

            for (int i = 0; i < count; ++i)
            {
                fields[0] = new TextField() { Content = MetadataFields[0].Content };
                fields[0].SetValues(new string[] { "Item " + i });

                CreateItem(EntityTypeName, fields, false);
            }
        }
    }
}
