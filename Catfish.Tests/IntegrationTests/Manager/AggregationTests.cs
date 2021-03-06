﻿using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using Catfish.Tests.Extensions;
using Catfish.Tests.IntegrationTests.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.IntegrationTests.Manager
{

    [TestFixture(typeof(ChromeDriver))]
    class AggregationTests<TWebDriver> : BaseIntegrationTest<TWebDriver> where TWebDriver : IWebDriver, new()
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

        protected void SetUpAssociationTest()
        {
            CreateMetadataSet(MetadataSetNames[0],
                "Simple metadata",
                MetadataFields);

            CreateEntityType(EntityTypeName, "Simple entity type", MetadataSetNames, TargetTypes,
                new Tuple<string, FormField>[] {
                    new Tuple<string, FormField>(MetadataSetNames[0], MetadataFields[0]),
                });
        }

        protected void AssertAssociateParents()
        {
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

            List<string> parents = new List<string>
            {
                parentName1,
                parentName2
            };

            Assert.IsTrue(parents.Contains(aggregationName1));
            Assert.IsTrue(parents.Contains(aggregationName2));
            //Assert.AreEqual(aggregationName1, parentName1);
            //Assert.AreEqual(aggregationName2, parentName2);
        }

        // This test has a problem where the test may fail due to the order of the child names.
        // To fix this the names were reversed but a better solution is needed.
        protected void AssertAssociateChildren()
        {
            
            // first checkbox
            string allDataTable = "(//div[@id='all-actionable-table']//table)[2]";

            Driver.FindElement(By.XPath($@"({allDataTable}//input[1])[1]"), 10).Click();
            string aggregationName1 = Driver.FindElement(By.XPath($@"{allDataTable}//tbody//tr[1]//td[2]"), 10).Text;

            // first aggregation name
            Driver.FindElement(By.XPath($@"({allDataTable}//input[1])[2]"), 10).Click();
            string aggregationName2 = Driver.FindElement(By.XPath($@"{allDataTable}//tbody//tr[2]//td[2]"), 10).Text;

            Driver.FindElement(By.Id("add-children-action"), 10).Click();

            string chidlrenDataTable = "(//div[@id='children-actionable-table']//table)[2]";
            // Sometimes the order that shows up on the list does not match
            string childName1 = Driver.FindElement(By.XPath($@"{chidlrenDataTable}//tbody//tr[1]//td[2]"), 10).Text;
            string childName2 = Driver.FindElement(By.XPath($@"{chidlrenDataTable}//tbody//tr[2]//td[2]"), 10).Text;

            List<string> children = new List<string>
            {
                childName1,
                childName2
            };

            Assert.IsTrue(children.Contains(aggregationName1));
            Assert.IsTrue(children.Contains(aggregationName2));

            // If the test fails childName2 and childName1 may need to be reversed
            //Assert.AreEqual(aggregationName1, childName1);
            //Assert.AreEqual(aggregationName2, childName2);
        }

        protected void AssertAssociateRelated()
        {

            // first checkbox
            string allDataTable = "(//div[@id='all-actionable-table']//table)[2]";

            Driver.FindElement(By.XPath($@"({allDataTable}//input[1])[1]"), 10, 3000).Click();
            string aggregationName1 = Driver.FindElement(By.XPath($@"{allDataTable}//tbody//tr[1]//td[2]"), 10, 3000).Text;
            if(!aggregationName1.Contains("Item"))
            {
                //unchecked it -- can't add a collection as a 'related' item
                Driver.FindElement(By.XPath($@"({allDataTable}//input[1])[1]"), 10, 3000).Click();
            }
            // first aggregation name
            Driver.FindElement(By.XPath($@"({allDataTable}//input[1])[2]"), 10).Click();
            string aggregationName2 = Driver.FindElement(By.XPath($@"{allDataTable}//tbody//tr[2]//td[2]"), 10).Text;
            if(!aggregationName2.Contains("Item"))
            {
                Driver.FindElement(By.XPath($@"({allDataTable}//input[1])[2]"), 10).Click();
            }
            Driver.FindElement(By.Id("add-related-action"), 10).Click();

            string relatedDataTable = "(//div[@id='related-actionable-table']//table)[2]";
            string relatedName1 = Driver.FindElement(By.XPath($@"{relatedDataTable}//tbody//tr[1]//td[2]"), 10, 3000).Text;
            string relatedName2 = Driver.FindElement(By.XPath($@"{relatedDataTable}//tbody//tr[2]//td[2]"), 10, 3000).Text;

            // there should be only one item related, the second value should be blank
            if(aggregationName1.Contains("Item"))
                Assert.AreEqual(aggregationName1, relatedName1);
            else if(aggregationName2.Contains("Item"))
                Assert.AreEqual(aggregationName2, relatedName1);
            //Assert.AreEqual(aggregationName2, relatedName2);
            Assert.IsEmpty(relatedName2);
        }

        protected void TestForRemoval(string testName)
        {
            // first checkbox
            string allDataTable = "(//div[@id='all-actionable-table']//table)[2]";

            string aggregationName = Driver.FindElement(By.XPath($@"{allDataTable}//tbody//tr[1]//td[2]"), 10).Text;
            Driver.FindElement(By.XPath($@"({allDataTable}//input[1])[1]"), 10, 1500).Click();
            if(testName == "related")
            {
                //string aggregationName1 = Driver.FindElement(By.XPath($@"{allDataTable}//tbody//tr[1]//td[2]"), 10, 3000).Text;
                if (!aggregationName.Contains("Item"))
                {
                    //unchecked it -- can't add a collection as a 'related' item
                    Driver.FindElement(By.XPath($@"({allDataTable}//input[1])[1]"), 10, 3000).Click();
                }
                Driver.FindElement(By.XPath($@"({allDataTable}//input[1])[2]"), 10).Click();
                aggregationName = Driver.FindElement(By.XPath($@"{allDataTable}//tbody//tr[2]//td[2]"), 10).Text;
                if (!aggregationName.Contains("Item"))
                {
                    Driver.FindElement(By.XPath($@"({allDataTable}//input[1])[2]"), 10).Click();
                }
            }

            //// first aggregation name

            Driver.FindElement(By.Id("add-" + testName + "-action"), 10, 1500).Click();
            //string omar = "add-parents-action";
            //Driver.FindElement(By.Id(omar)).Click();

            string dataTable = "(//div[@id='" + testName + "-actionable-table']//table)[2]";

            string aggregationNameAdded = Driver.FindElement(By.XPath($@"{dataTable}//tbody//tr[1]//td[2]"), 10).Text;
            Assert.AreEqual(aggregationName, aggregationNameAdded);
            // XXX remove action

            Driver.FindElement(By.XPath($@"({dataTable}//input[1])[1]"), 10).Click();
            Driver.FindElement(By.Id("remove-" + testName + "-action"), 10).Click();


            // Make surte it is not empty

            // there should be only one item related, the second value should be blank
            string aggregationNameRemoved = Driver.FindElement(By.XPath($@"{dataTable}//tbody//tr[1]//td[2]"), 10).Text;

            //Assert.AreEqual(aggregationName2, relatedName2);
            Assert.IsEmpty(aggregationNameRemoved);
        }

        protected void CreateCollections(int count)
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

        protected void CreateItems(int count)
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

        protected void AssertPagination()
        {
            SelectElement paginationSelect = new SelectElement(
                Driver.FindElement(By.XPath("//div[@id='all-pagination']//select"), 10, 2500)
                );

            //"(//div[@id='all-actionable-table']//table)[2]";

            IList<IWebElement> pagesList = paginationSelect.Options;
            Assert.AreEqual(2, pagesList.Count);

            string aggregationNamesXpath =
               "//div[@id='all-actionable-table']//tr[@class='data-row']//td[2]";

            IList<IWebElement> names = Driver.FindElements(By.XPath(aggregationNamesXpath), 10);

            foreach (IWebElement element in names)
            {
                Assert.IsNotEmpty(element.Text);
            }

            paginationSelect.SelectByIndex(1);
            TimeSpan timeout = TimeSpan.FromSeconds(5);
            WebDriverWait wait = new WebDriverWait(Driver, timeout);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(names[0]));
            names = Driver.FindElements(By.XPath(aggregationNamesXpath), 10);

            foreach (IWebElement element in names)
            {
                Assert.IsNotEmpty(element.Text);
            }
        }

        protected void AssertSearchInActionableTable(string searchTerm, string expectedTerm)
        {
            TimeSpan timeout = TimeSpan.FromSeconds(5);
            WebDriverWait wait = new WebDriverWait(Driver, timeout);
            // CreateCollections would have created 'Collection 0' and 
            // 'Collection 1', filter out by typing 1
            string dataXpath = "(//div[@id='all-actionable-table']//table)[2]//tbody//tr//td[2]";
            By dataBy = By.XPath(dataXpath);


            IWebElement allTable = Driver.FindElement(dataBy, 10, 1500);
            Driver.FindElement(By.Id("all-search"), 10).SendKeys(searchTerm);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(allTable));
            IList<IWebElement> data = Driver.FindElements(dataBy, 10);
            Assert.AreEqual(expectedTerm, data[0].Text);
            Assert.IsEmpty(data[1].Text);
        }
    }
}
