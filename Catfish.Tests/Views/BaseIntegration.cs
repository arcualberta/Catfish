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
using System.Data.Common;

namespace Catfish.Tests.Views
{

    public interface IIntegrationParameters { }

    public class AccessDefinitionParameters : IIntegrationParameters
    {
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

    public class MetadataSetParameters : IIntegrationParameters
    {
        public string MetadataSetName;
    }

    [Flags]
    public enum ApplicabilityValues
    {
        Collections,
        Items,
        Files,
        Forms
    }

    public class EntityTypeParameters : IIntegrationParameters
    {
        public string EntityTypeName;
        // Possible values for Applicability
        // chk_Collections
        // chk_Items
        // chk_Files
        // chk_Forms
        public ApplicabilityValues ApplicabilityFlags;


        public List<string> Applicability
        {
            get
            {
                List<string> result = new List<string>();

                foreach (ApplicabilityValues value in Enum.GetValues(typeof(ApplicabilityValues)))
                {
                    if ((ApplicabilityFlags & value) != 0)
                    {
                        result.Add(ApplicabilityStrings[value]);
                    } 
                }

                return result;
            }
        }
        
        private Dictionary<ApplicabilityValues, string> ApplicabilityStrings = new Dictionary<ApplicabilityValues, string>
        {
            { ApplicabilityValues.Collections, "chk_Collections"},
            { ApplicabilityValues.Items, "chk_Items"},
            { ApplicabilityValues.Files, "chk_Files"},
            { ApplicabilityValues.Forms, "chk_Forms"}
        };

        public string[] MetadataSets;
    }

    public abstract class BaseIntegration
    {
        protected const string AddId = "toolbar_add_button";
        protected const string SaveId = "toolbar_save_button";
        
        protected const string SystemLabel = "SYSTEM";
        protected const string ContentLabel = "CONTENT";
        protected const string SettingsLabel = "SETTINGS";
        protected const string LogoutLabel = "LOGOUT";
        protected const string UsersLabel = "Users";
        protected const string GroupsLabel = "Groups";
        protected const string CollectionsLabel = "Collections";
        protected const string ItemsLabel = "Items";
        protected const string UserListLabel = "User List";
        protected const string AccessDefinitionLabel = "Access Definitions";
        protected const string MetadataSetsLabel = "Metadata Sets";
        protected const string EntityTypesLabel = "Entity Types";
    }

    public class BaseIntegration<TWebDriver> : BaseIntegration where TWebDriver : IWebDriver, new()
    {
        protected IWebDriver Driver;
        protected string ManagerUrl;

        protected IJavaScriptExecutor JsExecutor
        {
            get
            {
                return (IJavaScriptExecutor)Driver;
            }
        }

        [SetUp]
        public virtual void SetUp()
        {
            ResetDatabase();

            this.Driver = new TWebDriver();
            this.ManagerUrl = ConfigurationManager.AppSettings["ServerUrl"] + "manager";
            this.LoginAsAdmin();
        }

        [TearDown]
        public virtual void TearDown()
        {
            this.Driver.Close();
        }

        protected void ResetDatabase()
        {
            //ClearDatabase();
            //ClearSolr();

        }

        private void ClearDatabase()
        {
            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;
            ConnectionStringSettings connectionString = null;

            if(settings != null)
            {
                foreach(ConnectionStringSettings s in settings)
                {
                    if(s.Name == "pirahna")
                    {
                        connectionString = s;
                    }
                }
            }

            Assert.NotNull(connectionString);

            try
            {
                DbProviderFactory factory = DbProviderFactories.GetFactory(connectionString.ProviderName);
                DbConnection connection = factory.CreateConnection();
                connection.ConnectionString = connectionString.ConnectionString;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            //TODO: Clear the database
        }

        private void ClearSolr()
        {

        }

        private void CreateAdmin()
        {

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

        protected void WaitUnitVisibleById(int milliseconds, string id)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromMilliseconds(milliseconds));
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.Id(id)));
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
            Navigate(new string[] { mainMenu, submenu });

            Click(AddId);

            fillValues(parameters);

            Click(SaveId);
        }

        public void Navigate(params string[] links)
        {
            foreach (string link in links)
            {
                Driver.FindElement(By.LinkText(link)).Click();
            }
        }

        public void Click(params string[] ids)
        {
            foreach(string id in ids)
            {
                IWebElement element = Driver.FindElement(By.Id(id));

                Click(element);
            }
        }

        public void Click(params IWebElement[] elements)
        {
            foreach(var element in elements)
            {
                IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver;
                ex.ExecuteScript("arguments[0].focus(); arguments[0].click()", element);
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
            //Driver.FindElement(By.Id("add-field")).Click();
            //WaitPageSourceChange(5, 500);
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
            //Driver.FindElement(By.Id("add-field")).Click();
            //WaitPageSourceChange(5, 500);
            Driver.FindElement(By.Id("MetadataSets_0__Fields_0__Values_0__Value"))
                .SendKeys(name);
        }

        protected void CreateMetadataSet(IIntegrationParameters parameters,
            Action<IIntegrationParameters> fillValues)
        {
            NavigateToCreate(SettingsLabel, MetadataSetsLabel, parameters, fillValues);
        }

        protected void CreateMetadataSet(IIntegrationParameters parameters)
        {
            CreateMetadataSet(parameters, FillMetadataSet);
        }

        protected void FillMetadataSet(IIntegrationParameters parameters)
        {
            MetadataSetParameters metadataSetParameters = (MetadataSetParameters)parameters;

            Driver.FindElement(By.Id("Name")).SendKeys(metadataSetParameters.MetadataSetName);            

            IWebElement fieldTypeElement = Driver.FindElement(By.Id("field-type-selector"));
            SelectElement selectElement = new SelectElement(fieldTypeElement);
            selectElement.SelectByText("Short text");
            Driver.FindElement(By.Id("add-field")).Click();
            WaitPageSourceChange(5, 500);
            //"//button[contains[@class='glyphicon-remove']]
            Driver.FindElement(By.XPath("(//div[contains(@class, 'field-entry')][1]//input[contains(@class, 'input-field')])[1]"))
                .SendKeys("test");

            
            // add at least one field
        }

        protected void CreateEntityType(IIntegrationParameters parameters,
            Action<IIntegrationParameters> fillValues)
        {
            NavigateToCreate(SettingsLabel, EntityTypesLabel, parameters, fillValues);
        }

        protected void CreateEntitype(IIntegrationParameters parameters)
        {
            CreateEntityType(parameters, FillEntityType);
        }

        protected void FillEntityType(IIntegrationParameters parameters)
        {
            EntityTypeParameters entityTypeParameters = (EntityTypeParameters)parameters;
            Driver.FindElement(By.Id("Name")).SendKeys(entityTypeParameters.EntityTypeName);
            
            // fill in Applicability based on applicability flags
            foreach (string id in entityTypeParameters.Applicability)
            {
                Driver.FindElement(By.Id(id)).Click();
            }

            IWebElement metadataSetsElement = Driver.FindElement(By.Id("dd_MetadataSets"));

            SelectElement selectElement = new SelectElement(metadataSetsElement);

            selectElement.SelectByText("test");
            Driver.FindElement(By.Id("btnAddMetadataSet")).Click();


            // Add name mapping



        }

        protected IWebElement GetField(string cssPath)
        {
            return Driver.FindElement(By.CssSelector(cssPath));
        }

        protected string GetTextFieldValue(string cssPath)
        {
            IWebElement element = GetField(cssPath);

            if(element != null)
            {
                return element.Text;
            }

            return null;
        }

    }
}




