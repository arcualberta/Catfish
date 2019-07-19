using Catfish.Core.Models;
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
    class CollectionTests<TWebDriver> : AggregationTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {


        

        [Test]
        public void CanCreateCollection()
        {

            CreateBaseEntityType();
            // FormFields is instantiated in CreateBaseEntityType
            CreateCollection(EntityTypeName, FormFields[0]);
            NavigateToCollections();
         
            string savedValue = FindTestValue(ItemValue);
            Assert.AreEqual(ItemValue, savedValue);
            //edit
            // GetLastEditButton().Click();
            IWebElement editBtn = Driver.FindElement(By.XPath($".//span[contains(@class,'object-edit')]/ancestor::button"),15);
            editBtn.Click();
            string EditedValue = "Edited Text";
            IWebElement inputBox = Driver.FindElement(By.Id("MetadataSets_0__Fields_0__Values_0__Value"), 15);
            inputBox.Clear();
            inputBox.SendKeys(EditedValue);
            Driver.FindElement(By.Id(ToolBarSaveButtonId), 10).Click();
            NavigateToCollections();
            savedValue = FindTestValue(EditedValue);
            Assert.AreEqual(EditedValue, savedValue);
        }

        [Test]
        public void CanAssociateCollectionParents()
        {

            SetUpAssociationTest();
            CreateCollections(5);
            // Create simple entity type
            NavigateToCollections();
            //object-associations
            Driver.FindElement(By.XPath($".//span[contains(@class,'object-associations')]/ancestor::button"),15).Click();
           // GetFirstAssociationsButton().Click();
            AssertAssociateParents();

        }

        [Test]
        public void CanAssociateCollectionChildren()
        {
            SetUpAssociationTest();            
            CreateCollections(1);
            CreateItems(1);
            // Create simple entity type
            NavigateToCollections();
            Driver.FindElement(By.XPath($".//span[contains(@class,'object-associations')]/ancestor::button"),15).Click();
            // GetFirstAssociationsButton().Click();
            AssertAssociateChildren();

        }

        [Test]
        public void CanAssociateCollectionRelated()
        {
            SetUpAssociationTest();
            CreateItems(1);
            CreateCollections(1);
            NavigateToCollections();
            Driver.FindElement(By.XPath($".//span[contains(@class,'object-associations')]/ancestor::button"),15).Click();
            //GetFirstAssociationsButton().Click();
            AssertAssociateRelated();

        }

        [Test]
        public void CanRemoveCollectionParents()
        {
            SetUpAssociationTest();
            CreateCollections(1);
            NavigateToCollections();
            // GetFirstAssociationsButton().Click();
            Driver.FindElement(By.XPath($".//span[contains(@class,'object-associations')]/ancestor::button"),15).Click();
            TestForRemoval("parents");

        }

        [Test]
        public void CanRemoveCollectionChildren()
        {
            SetUpAssociationTest();
            CreateCollections(1);
            NavigateToCollections();
            Driver.FindElement(By.XPath($".//span[contains(@class,'object-associations')]/ancestor::button"),15).Click();
            //GetFirstAssociationsButton().Click();
            TestForRemoval("children");
        }

        [Test]
        public void CanRemoveCollectionRelated()
        {
            SetUpAssociationTest();
            CreateItems(1);
            CreateCollections(1);
            NavigateToCollections();
            Driver.FindElement(By.XPath($".//span[contains(@class,'object-associations')]/ancestor::button"),15).Click();
            //GetFirstAssociationsButton().Click();
            TestForRemoval("related");
        }

        //private void CheckNamesInPage()
        //{
        //    string aggregationNamesXpath =
        //        "//div[@id='all-actionable-table']//tr[@class='data-row']//td[2]";

        //    TimeSpan timeout = new System.TimeSpan(500);
        //    WebDriverWait wait = new WebDriverWait(Driver, timeout);
                   
        //    IList<IWebElement> names = Driver.FindElements(By.XPath(aggregationNamesXpath), 10);
        //    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(names[0]));


        //    foreach (IWebElement element in names)
        //    {
        //        Assert.IsNotEmpty(element.Text);
        //    }
        //}

        [Test]
        public void CanPaginateCollection()
        {
            SetUpAssociationTest();
            CreateCollections(20);
            NavigateToCollections();
            Driver.FindElement(By.XPath($".//span[contains(@class,'object-associations')]/ancestor::button"),15).Click();
           // GetFirstAssociationsButton().Click();

            // id all-pagination
            // check drop down has two values

            //SelectElement paginationSelect = new SelectElement(
            //    Driver.FindElement(By.Id("all-pagination"), 10)
            //    );

            AssertPagination();
            
        }

        [Test]
        public void CanSearchInCollectionActionableTable()
        {
            SetUpAssociationTest();
            CreateCollections(2);
            NavigateToCollections();
            Driver.FindElement(By.XPath($".//span[contains(@class,'object-associations')]/ancestor::button"),15).Click();
           // GetFirstAssociationsButton().Click();
            AssertSearchInActionableTable("1", "Collection 1");            
        }

        //private string FindTestValue(string expectedValue)
        //{
        //    string val = "";
        //    IWebElement tbody = Driver.FindElement(By.ClassName("object-list"), 10);
        //    var cols = tbody.FindElements(By.TagName("td"));

        //    foreach (var col in cols)
        //    {
        //        if (!string.IsNullOrEmpty(col.Text) && col.Text.Contains(expectedValue)) //Item's name will display in all 3 lang separated by '/' -- eng is the first one
        //        {
        //            val = col.Text.Split('/')[0].Trim();
        //            break;
        //        }
        //    }
        //    return val;
        //}

    }
}
