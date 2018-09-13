using System;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Configuration;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Interactions.Internal;
using System.Collections.Generic;

namespace Catfish.Tests.Views
{
    static class TestValues
    { 
        public static string EntityGroupName = "Selenium User List";
        public static string UserLogin = "admin";
    }
    [TestFixture(typeof(ChromeDriver))]
    public class EntityGroupViewTests<TWebDriver> : BaseIntegration<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private void Login()
        {
            this.Driver.Navigate().GoToUrl(ManagerUrl);
            this.Driver.FindElement(By.Id("login")).SendKeys(ConfigurationManager.AppSettings["AdminLogin"]);
            this.Driver.FindElement(By.Name("password")).SendKeys(ConfigurationManager.AppSettings["AdminPassword"]);
            this.Driver.FindElement(By.TagName("button")).Click();
        }

        [Test]
        public void CanCreateEntityGroup()
        {
            
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/CFUserLists");
          
            this.Driver.FindElement(By.LinkText("Add new")).Click();
            this.Driver.FindElement(By.Name("CFUserListName")).SendKeys(TestValues.EntityGroupName);
            this.Driver.FindElement(By.Name("userName")).SendKeys(TestValues.UserLogin);
            this.Driver.FindElement(By.Id("addUser")).Click();
            this.Driver.FindElement(By.Id("btnSave")).Click();
           
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/CFUserLists");
            Assert.AreEqual(this.FindEntityGroupName(), TestValues.EntityGroupName);
          
        }

        private string FindEntityGroupName()
        {
            string grpName = "";
            var cols = this.Driver.FindElements(By.TagName("td"));

            foreach (var col in cols)
            {
                if (col.Text == TestValues.EntityGroupName)
                {
                    grpName = col.Text;
                    break;
                }
            }
  
            return grpName;
        }
    }
}
