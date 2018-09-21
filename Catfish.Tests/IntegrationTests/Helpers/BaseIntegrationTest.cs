using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.IntegrationTests.Helpers

{
    abstract class BaseIntegrationTest<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        protected IWebDriver Driver;
        protected string ManagerUrl;

        [SetUp]
        public void SetUp()
        {
            this.Driver = new TWebDriver();
            this.ManagerUrl = ConfigurationManager.AppSettings["ServerUrl"] + "manager";

            ClearDatabase();

            ResetServerCahce();

            SetupPiranha();

            OnSetup();
        }

        [TearDown]
        public void TearDown()
        {
            ClearDatabase();
            OnTearDown();

            Driver.Close();
        }

        protected abstract void OnSetup();

        protected abstract void OnTearDown();

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
        }

        private void ResetServerCahce()
        {
            string webConfig = ConfigurationManager.AppSettings["WebConfigLocation"];
            File.SetLastWriteTime(webConfig, DateTime.Now);
        }

        private void SetupPiranha()
        {

            this.Driver.Navigate().GoToUrl(ManagerUrl);
            this.Driver.FindElement(By.Name("UserLogin")).SendKeys(ConfigurationManager.AppSettings["AdminLogin"]);
            this.Driver.FindElement(By.Name("Password")).SendKeys(ConfigurationManager.AppSettings["AdminPassword"]);
            this.Driver.FindElement(By.Name("PasswordConfirm")).SendKeys(ConfigurationManager.AppSettings["AdminPassword"]);
            this.Driver.FindElement(By.Name("UserEmail")).SendKeys(ConfigurationManager.AppSettings["AdminEmail"]);

            this.Driver.FindElement(By.Id("full")).Click();
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

        public int CreateMetadataSet(FormField[] fields)
        {
            throw new NotImplementedException();
        }

        public int CreateEntityType(int[] metadataSetIds, CFEntityType.eTarget[] targetTypes)
        {
            throw new NotImplementedException();
        }

        public int CreateItem(int entityTypeId)
        {
            throw new NotImplementedException();
        }

        public int CreateCollection(int entityTypeId)
        {
            throw new NotImplementedException();
        }

        public int CreateForm(int entityTypeId)
        {
            throw new NotImplementedException();
        }

        public string CreateUser(string userName, string password, string email)
        {
            throw new NotImplementedException();
        }
    }
}
