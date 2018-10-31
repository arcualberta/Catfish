using Catfish.Core.Models;
using Catfish.Core.Models.Attributes;
using Catfish.Core.Models.Forms;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catfish.Tests.Extensions;
using Catfish.Core.Services;
using SolrNet;

namespace Catfish.Tests.IntegrationTests.Helpers

{
    abstract class BaseIntegrationTest<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        protected IWebDriver Driver;
        protected string ManagerUrl;
        protected const string ContentLinkText = "CONTENT";
        protected const string SettingsLinkText = "SETTINGS";
        protected const string MetadataSetsLinkText = "Metadata Sets";
        protected const string EntityTypesLinkText = "Entity Types";
        protected const string ItemsLinkText = "Items";
        protected const string CollectionsLinkText = "Collections";
        protected const string FormsLinkText = "Forms";
        protected const string ToolBarAddButtonId = "toolbar_add_button";
        protected const string ToolBarSaveButtonId = "toolbar_save_button";
        protected const string NameId = "Name";
        protected const string DescriptionId = "Description";


        protected const string EntityTypeName = "Entity type name";
        protected const string EntityTypeDescription = "Entity type description";
        protected const string MetadataSetName = "Metadata set name";
        protected const string MetadataSetDescription = "Metadata set description";
        protected const string FieldName = "Field name";
        protected const string ItemValue = "Item Name";

        [SetUp]
        public void SetUp()
        {
            InitializeSolr();
            Driver = new TWebDriver();
            ManagerUrl = ConfigurationManager.AppSettings["ServerUrl"] + "manager";

            ClearDatabase();            
            ResetServerCache();
            
            SetupPiranha();
            RunMigrations();
            LoginAsAdmin();
            

            OnSetup();
        }

        [TearDown]
        public void TearDown()
        {
            OnTearDown();
            Driver.Close();
            ClearDatabase();
        }

        protected virtual void OnSetup() { }

        protected virtual void OnTearDown() { }

        private void InitializeSolr()
        {
            string solrString = System.Configuration.ConfigurationManager.AppSettings["SolrServer"];
            SolrService.Init(solrString);
        }

        private void ClearDatabase()
        {
            CatfishDbContext context = new CatfishDbContext();
            string query = @"DECLARE @Sql NVARCHAR(500) DECLARE @Cursor CURSOR
                SET @Cursor = CURSOR FAST_FORWARD FOR
                SELECT DISTINCT sql = 'ALTER TABLE [' + tc2.TABLE_NAME + '] DROP [' + rc1.CONSTRAINT_NAME + ']'
                FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc1
                LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc2 ON tc2.CONSTRAINT_NAME =rc1.CONSTRAINT_NAME

                OPEN @Cursor FETCH NEXT FROM @Cursor INTO @Sql

                WHILE (@@FETCH_STATUS = 0)
                BEGIN
                Exec sp_executesql @Sql
                FETCH NEXT FROM @Cursor INTO @Sql
                END

                CLOSE @Cursor DEALLOCATE @Cursor";
            int result = context.Database.ExecuteSqlCommand(query);

            context.Database.ExecuteSqlCommand(@"EXEC sp_MSforeachtable 'DROP TABLE ?'");
            
            result = context.Database.ExecuteSqlCommand(query);

            ISolrQuery allEntries = new SolrQuery("*:*");
            SolrService.solrOperations.Delete(allEntries);
            SolrService.solrOperations.Commit();
        }

        private void ResetServerCache()
        {
            string webConfig = ConfigurationManager.AppSettings["WebConfigLocation"];
            File.SetLastWriteTime(webConfig, DateTime.Now);
        }

        private void RunMigrations()
        {            
            Catfish.Core.Migrations.Configuration config = new Catfish.Core.Migrations.Configuration();
            var migrator = new DbMigrator(config);
            migrator.Update();
        }

        private void SetupPiranha()
        {

            Driver.Navigate().GoToUrl(ManagerUrl);
            Driver.FindElement(By.Name("UserLogin")).SendKeys(ConfigurationManager.AppSettings["AdminLogin"]);
            Driver.FindElement(By.Name("Password")).SendKeys(ConfigurationManager.AppSettings["AdminPassword"]);
            Driver.FindElement(By.Name("PasswordConfirm")).SendKeys(ConfigurationManager.AppSettings["AdminPassword"]);
            Driver.FindElement(By.Name("UserEmail")).SendKeys(ConfigurationManager.AppSettings["AdminEmail"]);

            Driver.FindElement(By.Id("full")).Click();
        }

        protected void LoginAsAdmin()
        {
            Login(ConfigurationManager.AppSettings["AdminLogin"],
                ConfigurationManager.AppSettings["AdminPassword"]);
        }

        protected void Login(string user, string password)
        {
            Driver.Navigate().GoToUrl(ManagerUrl);
            Driver.FindElement(By.Id("login")).SendKeys(user);
            Driver.FindElement(By.Name("password")).SendKeys(password);
            Driver.FindElement(By.TagName("button")).Click();
        }
       
        protected IWebElement GetLastObjectRow()
        {
            return Driver.FindElement(By.XPath("(//tbody[contains(@class, 'object-list')]/tr)[last()]"));
        }

        protected IWebElement GetLastActionPanel()
        {
            return GetLastObjectRow().FindElement(By.XPath("//td[contains(@class, 'action-panel')]"));
        }

        public IWebElement GetLastButtonByClass(string cssClass)
        {
            return GetLastObjectRow().FindElement(By.XPath($"//button[contains(@class, '{cssClass}')]"));
        }

        protected IWebElement GetLastEditButton()
        {
            return GetLastButtonByClass("object-edit");
        }

        protected IWebElement GetLastAssociationsButton()
        {
            return GetLastButtonByClass("object-associations");
        }

        protected IWebElement GetLastDeleteButton()
        {
            return GetLastButtonByClass("object-delete");
        }

        protected IWebElement GetLastAccessGroupButton()
        {
            return GetLastButtonByClass("object-accessgroup");
        }

        public void CreateMetadataSet(string name, string description)
        {
            CreateMetadataSet(name, description, new FormField[0]);
        }

        public void CreateMetadataSet(string name, string description, FormField[] fields)
        {
            Driver.Navigate().GoToUrl(ManagerUrl);
            Driver.FindElement(By.LinkText(SettingsLinkText)).Click();
            Driver.FindElement(By.LinkText(MetadataSetsLinkText)).Click();
            Driver.FindElement(By.Id(ToolBarAddButtonId)).Click();
            Driver.FindElement(By.Id(NameId)).SendKeys(name);
            Driver.FindElement(By.Id(DescriptionId)).SendKeys(description);

            // Add metadata set fields

            // id field-type-selector
            IWebElement fieldTypeSelectorElement = Driver.FindElement(By.Id("field-type-selector"));
            SelectElement fieldTypeSelector = new SelectElement(fieldTypeSelectorElement);

            foreach (FormField field in fields)
            {
                //field.GetType();
                CFTypeLabelAttribute typeLabelAttribute = (CFTypeLabelAttribute)field.GetType()
                    .GetCustomAttributes(typeof(CFTypeLabelAttribute), true)
                    .First();

                // select option by label
                fieldTypeSelector.SelectByText(typeLabelAttribute.Name);
                // click on id add-field

                Driver.FindElement(By.Id("add-field")).Click();

                // fill values on last field-entry
                IWebElement lastFieldEntry = Driver.FindElement(By.XPath("(//div[contains(@class, 'field-entry')])[last()]"), 10);

                // fill class field-is-required with fields[0].IsRequired

                // XXX generalize to use all language codes Name_sp, Name_fr
                lastFieldEntry.FindElement(By.Name("Name_en")).SendKeys(field.Name);

                // XXX need to add options ?

                if (field.IsRequired)
                {
                    lastFieldEntry.FindElement(By.ClassName("field-is-required")).Click();
                }


            }            

            Driver.FindElement(By.Id(ToolBarSaveButtonId)).Click();          
        }

        private void FillEntityTypeNameMapping()
        {
            string fieldElementsXpath = "//div[@id = 'fieldmappings-container']//div[contains(@class, 'fieldElement')]";
            List<IWebElement> fieldElements = Driver.FindElements(By.XPath(fieldElementsXpath), 10).ToList();

            // for simplicity sake link name and description to first element
            
            string mapMetadataXpath = $".//select[contains(@class, 'mapMetadata')]";
            string mapFieldXpath = $".//select[contains(@class, 'mapField')]";

            for (int i = 0; i < 2; ++i)
            {             
                IWebElement mapMetadataElement = fieldElements[i]
                    .FindElement(By.XPath(mapMetadataXpath));
                SelectElement mapMetadataSelector = new SelectElement(mapMetadataElement);
                mapMetadataSelector.SelectByIndex(1);

                IWebElement mapFieldElement = fieldElements[i]
                    .FindElement(By.XPath(mapFieldXpath));
                SelectElement mapFieldSelector = new SelectElement(mapFieldElement);
                mapFieldSelector.SelectByIndex(1);                      
            }
        }

        public void CreateEntityType(string name, string description, 
            string[] metadataSetNames, CFEntityType.eTarget[] targetTypes)
        {
            Driver.Navigate().GoToUrl(ManagerUrl);
            Driver.FindElement(By.LinkText(SettingsLinkText)).Click();
            Driver.FindElement(By.LinkText(EntityTypesLinkText)).Click();
            Driver.FindElement(By.Id(ToolBarAddButtonId)).Click();
            Driver.FindElement(By.Id(NameId)).SendKeys(name);
            Driver.FindElement(By.Id(DescriptionId)).SendKeys(description);

            //XXX for now make it applicable to all

            Driver.FindElement(By.Id("chk_Collections")).Click();
            Driver.FindElement(By.Id("chk_Items")).Click();
            Driver.FindElement(By.Id("chk_Files")).Click();
            Driver.FindElement(By.Id("chk_Forms")).Click();



            // Need to add field mappings

            // use first metadataset and fields for name and description

            IWebElement metadataSetSelectorElement = Driver.FindElement(By.Id("dd_MetadataSets"));
            SelectElement metadatasetSelector = new SelectElement(metadataSetSelectorElement);           

            foreach (string metadataSetName in metadataSetNames)
            {
                metadatasetSelector.SelectByText(metadataSetName);
                Driver.FindElement(By.Id("btnAddMetadataSet")).Click();
            }

            FillEntityTypeNameMapping();

            Driver.FindElement(By.Id(ToolBarSaveButtonId)).Click();
        }

        public void CreateItem(string entityTypeName, FormField[] metadatasetValues) { 

            Driver.Navigate().GoToUrl(ManagerUrl);
            Driver.FindElement(By.LinkText(ContentLinkText)).Click();
            Driver.FindElement(By.LinkText(ItemsLinkText)).Click();
            Driver.FindElement(By.Id(ToolBarAddButtonId)).Click();


            // entity type name ?

            // selector by id field-type-selector

            IWebElement fieldTypeSelectorElement = Driver.FindElement(By.Id("field-type-selector"));
            SelectElement fieldTypeSelector = new SelectElement(fieldTypeSelectorElement);
            fieldTypeSelector.SelectByText(entityTypeName);
            Driver.FindElement(By.Id("add-field")).Click();

            // XXX For now fill first input with field name
            Driver.FindElement(By.XPath("//input[contains(@class, 'text-box single-line')][1]"), 10).SendKeys(metadatasetValues[0].Values[0].Value);
            Driver.FindElement(By.Id(ToolBarSaveButtonId)).Click();
            
            // fill metadata set values
            // save

            //throw new NotImplementedException();
        }

        public void CreateCollection(int entityTypeId)
        {
            throw new NotImplementedException();
        }

        public void CreateForm(int entityTypeId)
        {
            throw new NotImplementedException();
        }

        public string CreateUser(string userName, string password, string email)
        {
            throw new NotImplementedException();
        }

        public void CreateAccessDefinition()
        {
            throw new NotImplementedException();
        }
    }
}
