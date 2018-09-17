using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Configuration;
using OpenQA.Selenium.Support.UI;
using Catfish.Core.Models;
using System.Data;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Catfish.Core.Models.Access;

namespace Catfish.Tests.Views
{

    [TestFixture(typeof(ChromeDriver))]
    public class AccessIntegration<TWebDriver> : BaseIntegration<TWebDriver> where TWebDriver : IWebDriver, new()
    {

        // values
        const string readWriteModeLabel = "Read Write mode";

        private string GetAccessModesLabel(AccessMode accessModes)
        {
            return String.Join(" - ", accessModes.AsStringList());
        }

        [Test]
        public void CanCreateAccessDefinition()
        {
            AccessMode modes = AccessMode.Write | AccessMode.Read;

            AccessDefinitionParameters parameters = new AccessDefinitionParameters
            {
                Modes = modes,
                AccessModesLabel = GetAccessModesLabel(modes)
            };

            CreateAccessDefinition(parameters);

            // Assertions for creation
            // Check for last table row which contains latest access definition
            IReadOnlyList<IWebElement> result = Driver.FindElements(By.CssSelector("tr:last-child td"));
            Assert.GreaterOrEqual(result.Count, 2);
            Assert.AreEqual(parameters.AccessModesLabel, result[0].Text);

            // second td element has list of access modes
            List<string> retreivedModes = result[1].Text.Split(',').Select(x => x.Trim()).ToList();
            List<string> allModes = modes.AsStringList();

            foreach (string currentMode in retreivedModes)
            {
                Assert.Contains(currentMode, allModes);
            }

            // Destroy access definition
            int rowsBeforeDeletion = Driver.FindElements(By.TagName("tr")).Count();
            // result[4] has the delete button
            result[4].Click();
            Driver.SwitchTo().Alert().Accept();
            WaitPageSourceChange(5, 500);
            int rowsAfterDeletion = Driver.FindElements(By.TagName("tr")).Count();
            
            // Check if it was detroyed
            Assert.AreEqual(rowsBeforeDeletion - 1, rowsAfterDeletion);            

        }

        [Ignore("Test needs to be corrected")]
        [Test]
        public void CanFilterAccessibleEntities()
        {
            /*
             * admin create user
             * admin create accessdefinition
             * admin create item
             * admin create collection
             * admin add permission to view item
             * can admin see item ?
             * can admin see collection ?
             * can user can see item ?
             * can user not see collection ?
             * delete user
             * delete item
             * delete collection
             */

            string userName1 = "user read permission";
            string userName2 = "user with user list read permission";
            string userListName = "userlist read permission";
            string accessDefinitionName = "access modes for integration tests";
            string password = "password";

            UserParameters user1Parameters = new UserParameters
            {
                UserName = userName1,
                Password = password
            };

            UserParameters user2Parameters = new UserParameters
            {
                UserName = userName2,
                Password = password
            };

            ItemParameters itemParameters = new ItemParameters
            {
                ItemName = "Item for access integration tests"
            };

            CollectionParameters collectionParameters = new CollectionParameters
            {
                CollectionName = "Collection for access integration tests"
            };

            AccessDefinitionParameters accessDefinitionParameters = new AccessDefinitionParameters
            {
                Modes = AccessMode.Read,
                AccessModesLabel = accessDefinitionName
            };

            UserListParameters userListParameters = new UserListParameters
            {
                ListName = userListName,
                Users = new string[] { userName2 }
            };


            //CreateUser(user1Parameters);
            //CreateUser(user2Parameters);
            ////CreateUserList(userListParameters);
            // Create metadata set
            // Create entityh type
            CreateItem(itemParameters);
            ////CreateCollection(collectionParameters);
            ////CreateAccessDefinition(accessDefinitionParameters);

            //// navigate to newly created item
            //// click on its button with class glyphicon-eye-close
            WaitPageSourceChange(5, 500);

            //Navigate(new string[] { ContentLabel, ItemsLabel });

            //// click on last item
            ////Driver.FindElement(By.XPath("//button[@class='glyphicon-eye-close']")).Click();
            ////Driver.FindElement(By.XPath("//table/tbody/tr[last()]/tr[last()]/form[last()]/button[@class='glyphicon glyphicon-eye-close']")).Click();
            //Driver.FindElement(By.XPath("(//button[@class[contains(.,'glyphicon-eye-close')]])[last()]")).Click();
            //Driver.FindElement(By.Id("usrName")).SendKeys(userName1);
            //WaitPageSourceChange(5, 500);
            //Driver.FindElement(By.XPath("//li/div[contains(string(), '"+userName1+"')]")).Click();

            //IWebElement groupElement = Driver.FindElement(By.Id("SelectedAccessDefinition"));
            //SelectElement selectElement = new SelectElement(groupElement);
            //// assuming there are at least 2 groups to avoid creating 
            //// administrators
            //selectElement.SelectByText(accessDefinitionName + " - Read");
            //Driver.FindElement(By.Id("btnAddUserAccess")).Click();
            //Driver.FindElement(By.LinkText(SaveLabel)).Click();

            //Logout();
            //Login(userName1, password);

            //// check if user 1 can see what it needs

            //Logout();
            //Login(userName2, password);
            //// check if user 2 can see what it needs


            //Logout();

            //LoginAsAdmin();
            // Cleanup


            // delete item
            Navigate(new string[] { ContentLabel, ItemsLabel });
            
            string pathToRemoveButton = "//td[text()='"
                + itemParameters.ItemName
                + "']/../td[contains[@class='action-panel']]"
                + "//button[contains[@class='glyphicon-remove']]";

            Driver.FindElement(By.XPath(pathToRemoveButton)).Click();


            // delete collection
            //Navigate(new string[] { ContentLabel, CollectionsLabel });

            // delete access definition
            //Navigate(new string[] { SystemLabel, AccessDefinitionLabel });
            // delete user1


            // delete user2

            // delete entity type
            // delete metadata set

        }

        [Ignore("Test needs to be corrected")]
        [Test]
        public void CanRemoveUserList()
        {
            Assert.Fail();
        }

        //[Test]
        //public void TestMethods()
        //{
        //    //CreateItem("item name now this is an itemr");
        //    AccessMode modes = AccessMode.Write;
        //    AccessDefinitionParameters accessDef = new AccessDefinitionParameters()
        //    {
        //        Modes = modes,
        //        AccessModesLabel = GetAccessModesLabel(modes)
        //    };
        //    CreateAccessDefinition(accessDef);

        //    Assert.IsTrue(true);
        //}
    }
}
