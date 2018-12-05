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
            Assert.IsTrue(MatchesSolrInformationFromUrl(), "Solr information does not match to CFEntity");

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
                itemB,
                itemA
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
            Driver.FindElement(By.Id("btn_RegionName"), 10).Click();
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
        public void CanCreateItemWithAttachment()
        {
            Driver.Manage().Window.Maximize();
            string EntityTypeName = "Entity type name";
            string EntityTypeDescription = "Entity type description";
            string MetadataSetName = "Metadata set name";
            string MetadataSetDescription = "Metadata set description";
            string ItemName = "Item name";
            bool withAttachment = true;
            TextField fieldName = new TextField();
            fieldName.Name = "Name";
            CFEntityType.eTarget[] eTarget = new CFEntityType.eTarget[] { CFEntityType.eTarget.Items };



            TextArea fieldDescription = new TextArea();
            fieldDescription.Name = "Description";
            List<FormField> formFields = new List<FormField>();
            formFields.Add(fieldName);
            formFields.Add(fieldDescription);

            CreateMetadataSet(MetadataSetName, MetadataSetDescription, formFields.ToArray());



            CreateEntityType(EntityTypeName, EntityTypeDescription, new[] {
                MetadataSetName
                }, eTarget);

            CreateItem(EntityTypeName, ItemName, withAttachment);

            Assert.IsTrue(Driver.FindElement(By.XPath("//div[@class='img']"), 10).Displayed);

        }

    }
}
