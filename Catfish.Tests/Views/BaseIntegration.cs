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
    //static class ItemTestValues
    //{
    //    public static Random Rnd = new Random();
    //    public static string Name = "Item Name - Selenium" + Rnd.Next(1, 100);
    //    public static string Description = "EntityType Description";
    //    public static string EditedName = "Edited Item Name - Selenium" + Rnd.Next(1, 100);
    //}

    public interface IIntegrationParameters
    {

    }

    public class AccessDefinitionParameters : IIntegrationParameters {
        public AccessMode modes;
        public string accessModesLabel;
    }

    public class BaseIntegration<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        protected IWebDriver Driver;
        protected string ManagerUrl;
        protected string indexUrl = "";

        // values


        [SetUp]
        public void SetUp()
        {
            this.Driver = new TWebDriver();
            this.ManagerUrl = ConfigurationManager.AppSettings["ServerUrl"] + "manager";
            this.indexUrl = ManagerUrl + "/items";
            this.Login();
        }

        [TearDown]
        public void TearDown()
        {
            //this.Driver.Close();
        }

        protected void Login()
        {
            this.Driver.Navigate().GoToUrl(ManagerUrl);
            this.Driver.FindElement(By.Id("login")).SendKeys(ConfigurationManager.AppSettings["AdminLogin"]);
            this.Driver.FindElement(By.Name("password")).SendKeys(ConfigurationManager.AppSettings["AdminPassword"]);
            this.Driver.FindElement(By.TagName("button")).Click();
        }

        protected void WaitMethod(int tries, int milliseonds, Func<bool> myMethod)
        {
            for (int i = 0; i < tries; ++i)
            {
                while (!myMethod())
                {
                    Thread.Sleep(milliseonds);
                }
            }
        }

        protected void WaitPageSourceChange(int tries, int milliseconds)
        {
            string originalPageSource = Driver.PageSource;

            for (int i = 0; i < tries; ++i)
            {
                Thread.Sleep(milliseconds);
                string pageSource = Driver.PageSource;
                if (originalPageSource != pageSource)
                {
                    return;
                }
            }
        }

        // Methods to create items on webpage

        private string GetAccessModeSiblingInputXPath(AccessMode accessMode)
        {
            return "//label[text() = '" + accessMode.ToString() + "']/preceding-sibling::input";
        }

        private void SelectAllAccessModes(List<AccessMode> accessModes)
        {
            foreach (AccessMode mode in accessModes)
            {
                Driver.FindElement(By.XPath(GetAccessModeSiblingInputXPath(mode))).Click();
            }
        }
        
        protected void CreateAccessDefinition(IIntegrationParameters parameters, Action<IIntegrationParameters> fillValues)
        {
            Driver.FindElement(By.LinkText("SYSTEM")).Click();
            Driver.FindElement(By.LinkText("Access Definitions")).Click();
            Driver.FindElement(By.LinkText("Add new")).Click();

            fillValues(parameters);

            Driver.FindElement(By.LinkText("Save")).Click();

        }

        protected void CreateAccessDefinition(IIntegrationParameters parameters)
        {
            CreateAccessDefinition(parameters, FillAccessDefinition);
        }

        protected void FillAccessDefinition(IIntegrationParameters parameters)
        {
            AccessDefinitionParameters test = (AccessDefinitionParameters)parameters;
            AccessMode modes = test.modes;
            string accessModesLabel = test.accessModesLabel;
            List<AccessMode> modesList = modes.AsList();
            Driver.FindElement(By.Id("Name")).SendKeys(accessModesLabel);
            SelectAllAccessModes(modesList);
        }

        // W
        protected void CreateGroup(string groupName)
        {
            Driver.FindElement(By.LinkText("SYSTEM")).Click();
            Driver.FindElement(By.LinkText("Groups")).Click();
            Driver.FindElement(By.LinkText("Add new")).Click();

            // Fill in values
            Driver.FindElement(By.Id("Group_Name")).SendKeys(groupName);
            IWebElement groupElement = Driver.FindElement(By.Id("Group_ParentId"));
            SelectElement selectElement = new SelectElement(groupElement);
            // assuming there is already a first group
            selectElement.SelectByIndex(1);

            Driver.FindElement(By.LinkText("Save")).Click();
        }

        // W
        protected void CreateUser(string userName)
        {

            string password = "password";
            Driver.FindElement(By.LinkText("SYSTEM")).Click();
            Driver.FindElement(By.LinkText("Users")).Click();
            Driver.FindElement(By.LinkText("Add new")).Click();

            // Fill in values
            Driver.FindElement(By.Id("User_Login")).SendKeys(userName);
            Driver.FindElement(By.Id("User_Firstname")).SendKeys(userName + " User_Firstname");
            Driver.FindElement(By.Id("User_Surname")).SendKeys(userName + " User_Surname");
            Driver.FindElement(By.Id("User_Email")).SendKeys(userName + "@none.com");
            Driver.FindElement(By.Id("Password_Password")).SendKeys(password);
            Driver.FindElement(By.Id("Password_PasswordConfirm")).SendKeys(password);

            IWebElement groupElement = Driver.FindElement(By.Id("User_GroupId"));
            SelectElement selectElement = new SelectElement(groupElement);
            // assuming there is a first group
            selectElement.SelectByIndex(1);

            Driver.FindElement(By.LinkText("Save")).Click();
        }

        // XXX System breaks when creating user lists 20180613
        protected void CreateUserList(string listName, string[] users)
        {
            Driver.FindElement(By.LinkText("SYSTEM")).Click();
            Driver.FindElement(By.LinkText("User List")).Click();
            Driver.FindElement(By.LinkText("Add new")).Click();

            Driver.FindElement(By.Id("txtGrpName")).SendKeys(listName);

            foreach (string user in users)
            {
                Driver.FindElement(By.Id("usrName")).SendKeys(user);
                Driver.FindElement(By.Id("btnSave")).Click();
            }

            Driver.FindElement(By.LinkText("Save")).Click();
        }

        
        protected void CreateAggregation(string name, string aggregationDescriminator)
        {
            Driver.FindElement(By.LinkText("CONTENT")).Click();
            Driver.FindElement(By.LinkText(aggregationDescriminator)).Click();
            Driver.FindElement(By.LinkText("Add new")).Click();

            // Fill in values
            // Select entity type
            //XXX Assuming there is already an entity type

            Driver.FindElement(By.Id("add-field")).Click();
            Driver.FindElement(By.Id("MetadataSets_0__Fields_0__Values_0__Value"))
                .SendKeys(name);


            Driver.FindElement(By.LinkText("Save")).Click();
        }

        // F
        protected void CreateItem(string itemName)
        {
            CreateAggregation(itemName, "Items");
        }

        protected void CreateCollection(string collectionName)
        {
            CreateAggregation(collectionName, "Collections");
        }
    }
    
}
