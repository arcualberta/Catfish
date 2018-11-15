using Catfish.Core.Models;
using Catfish.Core.Models.Access;
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
    class AccessDefinitionsTests<TWebDriver> : BaseIntegrationTest<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        [Test]
        public void CanCreateAccessDefinitions()
        {
            //string accessDefinitionName = "Public read";
            //XXX Currently only checking AccessMode.Read manually
            AccessMode accessMode = AccessMode.Read;
            
            CreateAccessDefinition(AccessDefinitionName, accessMode);

            // Check name
            string pathToAccessDefinitionName = "(//table[contains(@class, 'list bs')]//td)[1]";
            string pathToAccessModeName = "(//table[contains(@class, 'list bs')]//td)[2]";
            string setName = Driver.FindElement(By.XPath(pathToAccessDefinitionName)).Text;
            string setAccessMode = Driver.FindElement(By.XPath(pathToAccessModeName)).Text;
            string stringAccessMode = AccessMode.Read.ToString();

            Assert.AreEqual(AccessDefinitionName, setName);
            Assert.AreEqual(stringAccessMode, setAccessMode);
            //XXX Check if accessMode is set
        }

        private void CreateAndAddEntityListToMain()
        {
            // create list entity region
            Driver.FindElement(By.LinkText(SettingsLinkText)).Click();
            Driver.FindElement(By.LinkText(PageTypesLinkText)).Click();
            Driver.FindElement(By.LinkText(StandardPageLinkText)).Click();

            // add region to main page
            string regionName = "newRegionName";
            Driver.FindElement(By.Id(RegionNameFieldId)).SendKeys(regionName);
            Driver.FindElement(By.Id(RegionInternalIdId)).SendKeys(regionName);

            IWebElement typeSelectorElement = Driver.FindElement(By.Id(RegionTypeSelectorId));
            SelectElement typeSelector = new SelectElement(typeSelectorElement);
            typeSelector.SelectByValue("Catfish.Models.Regions.ListEntitiesPanel");

            Driver.FindElement(By.Id(AddRegionButtonId)).Click();

            // Save button does not contain the id set on other views toolbar_save_button
            // Instead we will click on "Save"
            Driver.FindElement(By.LinkText(SaveLinkText)).Click();
            Driver.FindElement(By.LinkText(ContentLinkText)).Click();
            Driver.FindElement(By.LinkText(PagesLinkText)).Click();
            // Start is the link to the starting page
            // Send enter instead of clicking to get around element overlay
            Driver.FindElement(By.LinkText(StartLinkText), 10).SendKeys(Keys.Return);
            //Driver.FindElement(By.LinkText(regionName)).Click();
            Driver.FindElement(By.XPath($@"//button[contains(.,'{regionName}')]"), 10).Click();
            Driver.FindElement(By.Id("Regions_1__Body_ItemPerPage"), 10).SendKeys("10");
            Driver.FindElement(By.XPath("//span[contains(@class, 'glyphicon glyphicon-plus-sign')]")).Click();
            Driver.FindElement(By.ClassName(UpdateButtonClass)).Click();
        }

        private void AssignAccessDefinitionToLastItem()
        {
            Driver.FindElement(By.LinkText(ContentLinkText)).Click();
            Driver.FindElement(By.LinkText(ItemsLinkText)).Click();

            // Last item will be Item 2
            GetLastAccessGroupButton().Click();
            Driver.FindElement(By.Id(UserNameFieldId)).SendKeys(PublicGroupName);
            // First element in drop down will be our AccessDefinition previously 
            // created
            Driver.FindElement(By.ClassName(MenuItemWrapperClass), 10).Click();
            Driver.FindElement(By.Id(AddUserAccessButtonId), 10).Click();
            Driver.FindElement(By.Id(ToolBarSaveButtonId), 10).Click();
        }

        [Test]
        public void CanUseAccessDefinitions()
        {
            string item1Name = "Item 1";
            string item2Name = "Item 2";

            CreateBaseEntityType();
            CreateAccessDefinition(AccessDefinitionName, AccessDefinitionMode);
            CreateBaseItem(item1Name);
            CreateBaseItem(item2Name);

            // Set access definition for Item 2
            AssignAccessDefinitionToLastItem();

            CreateAndAddEntityListToMain();

            // Actual test goes here

            Driver.Navigate().GoToUrl(FrontEndUrl);
            
            string xPathToItems = $@"//tbody[contains(@id, '{ListEntitiesId}')]//tr";
            List<IWebElement> objectRows = Driver.FindElements(By.XPath(xPathToItems), 10).ToList();

            // Expecting the 2 created items to show up as administrator
            Assert.AreEqual(2, objectRows.Count);
            string firstName = objectRows[0].FindElement(By.XPath("(td)[2]")).Text;
            string secondName = objectRows[1].FindElement(By.XPath("(td)[2]")).Text;
            Assert.AreEqual(item1Name, firstName);
            Assert.AreEqual(item2Name, secondName);

            Driver.Navigate().GoToUrl(ManagerUrl);

            // logout and check only one item shows up on listing
            Driver.FindElement(By.LinkText(LogoutLinkText)).Click();
            Driver.Navigate().GoToUrl(FrontEndUrl);

            // ASSERT only one item shows up

            objectRows = Driver.FindElements(By.XPath(xPathToItems), 10).ToList();

            Assert.AreEqual(1, objectRows.Count);
            firstName = objectRows[0].FindElement(By.XPath("(td)[2]")).Text;                        
            Assert.AreEqual(item2Name, firstName);
        }
    }
}
