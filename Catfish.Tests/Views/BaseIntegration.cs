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

    public interface IIntegrationParameters { }

    public class AccessDefinitionParameters : IIntegrationParameters {
        public AccessMode Modes;
        public string AccessModesLabel;
    }

    public class GroupParameters : IIntegrationParameters
    {
        public string GroupName;
    }

    public class UserParameters : IIntegrationParameters
    {
        public string UserName;
        public string Password;
    }

    public class UserListParameters : IIntegrationParameters
    {
        public string ListName;
        public string[] Users;
    }

    public class CollectionParameters : IIntegrationParameters
    {
        public string CollectionName;
    }

    public class ItemParameters : IIntegrationParameters
    {
        public string ItemName;
    }

    public class BaseIntegration<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        protected IWebDriver Driver;
        protected string ManagerUrl;
        protected const string AddLabel = "Add new";
        protected const string SaveLabel = "Save";
        protected const string SystemLabel = "SYSTEM";
        protected const string ContentLabel = "CONTENT";
        protected const string LogoutLabel = "LOGOUT";
        protected const string UsersLabel = "Users";
        protected const string GroupsLabel = "Groups";
        protected const string CollectionsLabel = "Collections";
        protected const string ItemsLabel = "Items";
        protected const string UserListLabel = "User List";
        protected const string AccessDefinitionLabel = "Access Definitions";

        [SetUp]
        public void SetUp()
        {
            this.Driver = new TWebDriver();
            this.ManagerUrl = ConfigurationManager.AppSettings["ServerUrl"] + "manager";
            this.LoginAsAdmin();
        }

        [TearDown]
        public void TearDown()
        {
            //this.Driver.Close();
        }

        protected void LoginAsAdmin()
        {
            Login(ConfigurationManager.AppSettings["AdminLogin"],
                ConfigurationManager.AppSettings["AdminPassword"]);
        }

        protected void Login(string user, string password)
        {
            this.Driver.Navigate().GoToUrl(ManagerUrl);
            this.Driver.FindElement(By.Id("login")).SendKeys(user);
            this.Driver.FindElement(By.Name("password")).SendKeys(password);
            this.Driver.FindElement(By.TagName("button")).Click();
        }

        protected void Logout()
        {
            this.Driver.FindElement(By.LinkText(LogoutLabel)).Click();
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

        // Methods to create things on webpage

        private void NavigateToCreate(
            string mainMenu, 
            string submenu,
            IIntegrationParameters parameters,
            Action<IIntegrationParameters> fillValues 
            )
        {
            Navigate(new string[] { mainMenu, submenu, AddLabel});

            fillValues(parameters);

            Driver.FindElement(By.LinkText(SaveLabel)).Click();
        }

        public void Navigate(IEnumerable<string> links)
        {
            foreach (string link in links)
            {
                Driver.FindElement(By.LinkText(link)).Click();
            }
        }

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
        
        protected void CreateAccessDefinition(IIntegrationParameters parameters, 
            Action<IIntegrationParameters> fillValues)
        {
            NavigateToCreate(SystemLabel, AccessDefinitionLabel, parameters, fillValues);
        }

        protected void CreateAccessDefinition(IIntegrationParameters parameters)
        {
            CreateAccessDefinition(parameters, FillAccessDefinition);
        }

        protected void FillAccessDefinition(IIntegrationParameters parameters)
        {
            AccessDefinitionParameters accessDefinitionParameters = (AccessDefinitionParameters)parameters;
            AccessMode modes = accessDefinitionParameters.Modes;
            string accessModesLabel = accessDefinitionParameters.AccessModesLabel;
            List<AccessMode> modesList = modes.AsList();
            Driver.FindElement(By.Id("Name")).SendKeys(accessModesLabel);
            SelectAllAccessModes(modesList);
        }

        protected void CreateGroup(IIntegrationParameters parameters, 
            Action<IIntegrationParameters> fillValues)
        {
            NavigateToCreate(SystemLabel, GroupsLabel, parameters, fillValues);
        }

        protected void CreateGroup(IIntegrationParameters parameters)
        {
            CreateGroup(parameters, FillGroup);
        }

        protected void FillGroup(IIntegrationParameters parameters)
        {
            GroupParameters groupParameters = (GroupParameters)parameters;
            Driver.FindElement(By.Id("Group_Name")).SendKeys(groupParameters.GroupName);
            IWebElement groupElement = Driver.FindElement(By.Id("Group_ParentId"));
            SelectElement selectElement = new SelectElement(groupElement);
            // assuming there is already a first group
            selectElement.SelectByIndex(1);
        }

        // W
        protected void CreateUser(IIntegrationParameters parameters, 
            Action<IIntegrationParameters> fillValues)
        {
            NavigateToCreate(SystemLabel, UsersLabel, parameters, fillValues);
        }

        protected void CreateUser(IIntegrationParameters parameters)
        {
            CreateUser(parameters, FillUser);
        }

        protected void FillUser(IIntegrationParameters parameters)
        {
            UserParameters userParameters = (UserParameters)parameters;
            string userName = userParameters.UserName;
            string password = userParameters.Password;

            Driver.FindElement(By.Id("User_Login")).SendKeys(userName);
            Driver.FindElement(By.Id("User_Firstname")).SendKeys(userName + " User_Firstname");
            Driver.FindElement(By.Id("User_Surname")).SendKeys(userName + " User_Surname");
            Driver.FindElement(By.Id("User_Email")).SendKeys(userName + "@none.com");
            Driver.FindElement(By.Id("Password_Password")).SendKeys(password);
            Driver.FindElement(By.Id("Password_PasswordConfirm")).SendKeys(password);

            IWebElement groupElement = Driver.FindElement(By.Id("User_GroupId"));
            SelectElement selectElement = new SelectElement(groupElement);
            // assuming there are at least 2 groups to avoid creating 
            // administrators
            selectElement.SelectByIndex(2);
        }

        // XXX System breaks when creating user lists 20180613
        protected void CreateUserList(IIntegrationParameters parameters, 
            Action<IIntegrationParameters> fillValues)
        {
            NavigateToCreate(SystemLabel, UserListLabel, parameters, fillValues);
        }

        protected void CreateUserList(IIntegrationParameters parameters)
        {
            CreateUserList(parameters, FillUserList);
        }

        protected void FillUserList(IIntegrationParameters parameters)
        {

            UserListParameters userListParameters = (UserListParameters)parameters;
            string listName = userListParameters.ListName;
            string[] users = userListParameters.Users;

            Driver.FindElement(By.Id("txtGrpName")).SendKeys(listName);
            foreach (string user in users)
            {
                Driver.FindElement(By.Id("usrName")).SendKeys(user);
                Driver.FindElement(By.Id("btnSave")).Click();
            }
        }

        protected void CreateItem(IIntegrationParameters parameters, 
            Action<IIntegrationParameters> fillValues)
        {
            NavigateToCreate(ContentLabel, ItemsLabel, parameters, fillValues);
        }

        protected void CreateItem(IIntegrationParameters parameters)
        {
            CreateItem(parameters, FillItem);
        }

        protected void FillItem(IIntegrationParameters parameters)
        {
            ItemParameters itemParameters = (ItemParameters)parameters;
            string name = itemParameters.ItemName;
            Driver.FindElement(By.Id("add-field")).Click();
            WaitPageSourceChange(5, 500);
            Driver.FindElement(By.Id("MetadataSets_0__Fields_0__Values_0__Value"))
                .SendKeys(name);
        }

        protected void CreateCollection(IIntegrationParameters parameters,
            Action<IIntegrationParameters> fillValues)
        {
            NavigateToCreate(ContentLabel, CollectionsLabel, parameters, fillValues);
        }

        protected void CreateCollection(IIntegrationParameters parameters)
        {
            CreateCollection(parameters, FillCollection);
        }

        protected void FillCollection(IIntegrationParameters parameters)
        {
            CollectionParameters collectionParameters = (CollectionParameters)parameters;
            string name = collectionParameters.CollectionName;
            Driver.FindElement(By.Id("add-field")).Click();
            WaitPageSourceChange(5, 500);
            Driver.FindElement(By.Id("MetadataSets_0__Fields_0__Values_0__Value"))
                .SendKeys(name);
        }
    }    
}
