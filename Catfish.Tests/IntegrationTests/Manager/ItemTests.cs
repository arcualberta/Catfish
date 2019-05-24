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
    class ItemTests<TWebDriver> : AggregationTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        [Test]
        public void CanCreateItem()
        {
            bool multipleField = true;
            bool requiredField = true;
            CreateBaseEntityType(multipleField, requiredField);
            // FormFields is instantiated in CreateBaseEntityType
            CreateItem(EntityTypeName, FormFields[0], multipleField);
           

            Driver.FindElement(By.LinkText(ContentLinkText), 10).Click();
            Driver.FindElement(By.LinkText(ItemsLinkText), 10).Click();
            string savedValue = FindTestValue(ItemValue);
            Assert.AreEqual(ItemValue, savedValue);

            IWebElement editBtn = Driver.FindElement(By.XPath($".//span[contains(@class,'object-edit')]/ancestor::button"), 15);
            //GetLastEditButton().Click();
            editBtn.Click();
            string EditedValue = "Edited Text";
            IWebElement inputBox = Driver.FindElement(By.Id("MetadataSets_0__Fields_0__Values_0__Value"));
            inputBox.Clear();
            inputBox.SendKeys(EditedValue);
            Driver.FindElement(By.Id(ToolBarSaveButtonId), 10).Click();
            NavigateToItems();
            savedValue = FindTestValue(EditedValue);
            Assert.AreEqual(EditedValue, savedValue);
           
            //Assert.AreEqual(ItemValue, savedValue);
            if (multipleField)
            {
                string secondValue = FindTestValue(MultipleField, multipleField);
               
                //string secondValue = Driver.FindElement(By.Id("MetadataSets_0__Fields_0__Values_3__Value")).GetAttribute("value");
                Assert.AreEqual(MultipleField, secondValue);
            }
                   
            AssertMatchesSolrInformationFromUrl();
        }

        [Test]
        public void CanFilterItemsByEntityType()
        {
            string itemA = "Item A";
            string itemB = "Item B";
            string entityTypeA = "Entity type A";
            string entityTypeB = "Entity type B";            

            CreateBaseMetadataSet();

            CreateEntityType(entityTypeA, EntityTypeDescription, new[] {
                MetadataSetName
                }, new CFEntityType.eTarget[0]);

            CreateEntityType(entityTypeB, EntityTypeDescription, new[] {
                MetadataSetName
                }, new CFEntityType.eTarget[0]);

            CreateBaseItem(itemA, entityTypeA);
            CreateBaseItem(itemB, entityTypeB);

            // Now create the listing page 
            CreateAndAddEntityListToMain();
            // No filter is set. Check both appear
            AssertItemsNameShows(new string[] {
                itemA,
                itemB
                });

            // Check filter for A
            SetEnityFilterTo(entityTypeA);
            AssertItemsNameShows(itemA);


            // Check filter for B
            SetEnityFilterTo(entityTypeB);
            AssertItemsNameShows(itemB);
        }

        private void AssertItemsNameShows(string itemName)
        {
            AssertItemsNameShows(new string[] { itemName });
        }

        // names need to be in specific order
        private void AssertItemsNameShows(string[] itemNames)
        {
            string tableRowsXpath = "//tbody[@id = 'ListEntitiesPanelTableBody']/tr";
            Driver.Navigate().GoToUrl(FrontEndUrl);
            List<IWebElement> rows = Driver.FindElements(By.XPath(tableRowsXpath), 10).ToList();
            Assert.AreEqual(itemNames.Length, rows.Count);
            
            for (int i = 0; i < itemNames.Length; ++i)
            {
                string name = rows[i].FindElement(By.ClassName("column-1")).Text;                
                Assert.AreEqual(itemNames[i], name);
            }            
        }

        //private void AssertOnlyItemNameShows(string itemName)
        //{
        //    string tableRowsXpath = "//tbody[@id = 'ListEntitiesPanelTableBody']/tr";
        //    Driver.Navigate().GoToUrl(FrontEndUrl);
        //    List<IWebElement> rows = Driver.FindElements(By.XPath(tableRowsXpath), 10).ToList();
        //    Assert.AreEqual(1, rows.Count);
        //    string nameA = rows[0].FindElement(By.ClassName("column-1")).Text;
        //    Assert.AreEqual(itemName, nameA);
        //}

        private void NavigateToEntitiyListRegionEditor()
        {
            Driver.Navigate().GoToUrl(ManagerUrl);
            Driver.FindElement(By.LinkText(ContentLinkText), 10).Click();
            Driver.FindElement(By.LinkText(PagesLinkText), 10).Click();
            Driver.FindElement(By.LinkText(StartLinkText), 10).SendKeys(Keys.Return);
            Driver.FindElement(By.Id("btn_EntityListRegionName"), 10).Click();
        }

        private void SetEnityFilterTo(string entityTypeFilterText)
        {
            NavigateToEntitiyListRegionEditor();
            IWebElement entityTypeFilter = Driver.FindElement(By.Id("Regions_1__Body_EntityTypeFilter"), 10);
            SelectElement entityTypeFilterSelector = new SelectElement(entityTypeFilter);
            entityTypeFilterSelector.SelectByText(entityTypeFilterText);
            Driver.FindElement(By.ClassName(UpdateButtonClass), 10).Click();
        }

        [Test]
        public void CanAssociateItemParents()
        {

            SetUpAssociationTest();
            CreateCollections(2);
            CreateItems(1);
            // Create simple entity type
            NavigateToItems();
            Driver.FindElement(By.XPath($".//span[contains(@class,'object-associations')]/ancestor::button"), 15).Click();
            //GetFirstAssociationsButton().Click();
            AssertAssociateParents();

        }

        [Test]
        public void CanAssociateItemChildren()
        {

            SetUpAssociationTest();
            CreateCollections(1);
            CreateItems(1);
            // Create simple entity type
            NavigateToItems();
            Driver.FindElement(By.XPath($".//span[contains(@class,'object-associations')]/ancestor::button"), 15).Click();
            //GetFirstAssociationsButton().Click();
            AssertAssociateChildren();

        }

        [Test]
        public void CanAssociateItemRelated()
        {
            SetUpAssociationTest();
            CreateItems(1);
            CreateCollections(1);
            NavigateToItems();
            Driver.FindElement(By.XPath($".//span[contains(@class,'object-associations')]/ancestor::button"), 15).Click();
           // GetFirstAssociationsButton().Click();
            AssertAssociateRelated();

        }

        [Test]
        public void CanRemoveItemParents()
        {
            SetUpAssociationTest();
            CreateItems(1);
            CreateCollections(1);
            NavigateToItems();
            Driver.FindElement(By.XPath($".//span[contains(@class,'object-associations')]/ancestor::button"), 15).Click();
           // GetFirstAssociationsButton().Click();
            TestForRemoval("parents");

        }

        [Test]
        public void CanRemoveItemChildren()
        {
            SetUpAssociationTest();
            CreateCollections(1);
            CreateItems(1);
            NavigateToItems();
            Driver.FindElement(By.XPath($".//span[contains(@class,'object-associations')]/ancestor::button"), 15).Click();
           // GetFirstAssociationsButton().Click();
            TestForRemoval("children");
        }

        [Test]
        public void CanRemoveItemRelated()
        {
            SetUpAssociationTest();
            CreateItems(1);
            CreateCollections(1);                        
            NavigateToItems();
            Driver.FindElement(By.XPath($".//span[contains(@class,'object-associations')]/ancestor::button"), 15).Click();
            //GetFirstAssociationsButton().Click();
            TestForRemoval("related");
        }

       
        [Test]
        public void CanPaginateItem()
        {
            SetUpAssociationTest();
            CreateItems(20);
            NavigateToItems();
            Driver.FindElement(By.XPath($".//span[contains(@class,'object-associations')]/ancestor::button"), 15).Click();
            //GetFirstAssociationsButton().Click();
            AssertPagination();

        }

        [Test]
        public void CanSearchInItemActionableTable()
        {
            SetUpAssociationTest();
            CreateItems(2);
            NavigateToItems();
            Driver.FindElement(By.XPath($".//span[contains(@class,'object-associations')]/ancestor::button"), 15).Click();
            //GetFirstAssociationsButton().Click();
            AssertSearchInActionableTable("1", "Item 1");
        }

    }
}
