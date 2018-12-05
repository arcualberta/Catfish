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
