using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.IO;
using System.Configuration;

namespace Catfish.Tests
{
    /// <summary>
    /// Summary description for IntegrationTests
    /// </summary>

    static class TestStrings
    {
        public static string MetadatasetName = "Metadataset name";
        public static string MetadatasetDescription = "Metadataset description";
    }

    [TestClass]
    public class ManagerIntegrationTests
    {

        static IWebDriver ChromeDriver;
        static IWebDriver FirefoxDriver;
        static IWebDriver Driver;
        static string ServerUrl;
        static string ManagerUrl;

        [AssemblyInitialize]
        public static void SetUp(TestContext context)
        {
            ServerUrl = ConfigurationManager.AppSettings["ServerUrl"];
            ManagerUrl = ServerUrl + "/manager";
            ChromeDriver = new ChromeDriver("./");
            //firefoxDriver = new FirefoxDriver();
        }

        //[TestMethod]
        //public void TestChromeDriver()
        //{
        //    Driver = ChromeDriver;
        //    this.TestDriver();
        //    Driver.Close();
        //}

        [TestMethod]
        public void TestChromeLogin()
        {
            Driver = ChromeDriver;
            this.TestLogin();
            Driver.Close();
        }

        private void Login()
        {
            Driver.Navigate().GoToUrl(ManagerUrl);
            Driver.FindElement(By.Id("login")).SendKeys("admin");
            Driver.FindElement(By.Name("password")).SendKeys("admin");
            Driver.FindElement(By.Name("password")).SendKeys(Keys.Enter);
        }

        private void TestLogin()
        {
            this.Login();
            bool isLogoutDisplayed = Driver.FindElement(By.ClassName("logout")).Displayed;

            Assert.IsTrue(isLogoutDisplayed);
        }



        

    }
}
