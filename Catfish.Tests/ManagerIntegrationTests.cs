using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Configuration;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace Catfish.Tests
{

    static class TestStrings
    {
        public static string MetadatasetName = "Metadataset Name";
        public static string MetadatasetDescription = "Metadataset Description";
        public static string PostfixEdit = " Edit";
    }

    [TestFixture(typeof(ChromeDriver))]
    public class ManagerIntegrationTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver Driver;        
        private string ManagerUrl;

        [SetUp]
        public void SetUp()
        {
            this.Driver = new TWebDriver();
            ManagerUrl = ConfigurationManager.AppSettings["ServerUrl"] + "/manager";
            this.Login();
        }

        [TearDown]
        public void TearDown()
        {
            this.Driver.Close();
        }

        [Test]
        public void CanLogin()
        {
            bool isLogoutDisplayed = Driver.FindElement(By.ClassName("logout")).Displayed;
            Assert.IsTrue(isLogoutDisplayed);
        }

        [Test]
        public void CanCreateMetadataset()
        {
            Driver.FindElement(By.LinkText("Metadata Sets")).Click();
            Driver.FindElement(By.ClassName("add")).Click();
            Driver.FindElement(By.Id("Name")).SendKeys(TestStrings.MetadatasetName);
            Driver.FindElement(By.Id("Description")).SendKeys(TestStrings.MetadatasetDescription);

            IWebElement selectWebElement = Driver.FindElement(By.Id("field-type-selector"));
            SelectElement select = new SelectElement(selectWebElement);

            IEnumerable<IWebElement> test = select.Options;

               
            Driver.FindElement(By.ClassName("save")).Click();
        }

        private void Login()
        {
            Driver.Navigate().GoToUrl(ManagerUrl);
            Driver.FindElement(By.Id("login")).SendKeys("admin");
            Driver.FindElement(By.Name("password")).SendKeys("admin");
            Driver.FindElement(By.Name("password")).SendKeys(Keys.Enter);
        }

        //private AddSimpleMetadataSetData()
        //{

        //}

    }

}



//using System;
//using System.Text;
//using System.Collections.Generic;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.Firefox;
//using System.IO;
//using System.Configuration;

//namespace Catfish.Tests
//{
//    /// <summary>
//    /// Summary description for IntegrationTests
//    /// </summary>



//    static class TestStrings
//    {

//        public static string MetadatasetName = "Metadataset Name";
//        public static string MetadatasetDescription = "Metadataset Description";
//        public static string PostfixEdit = " Edit";

//    }

//    [TestClass]
//    public class ManagerIntegrationTests
//    {

//        static IWebDriver ChromeDriver;
//        static IWebDriver FirefoxDriver;
//        static IWebDriver Driver;
//        static string ServerUrl;
//        static string ManagerUrl;

//        [AssemblyInitialize]
//        public static void SetUp(TestContext context)
//        {
//            ServerUrl = ConfigurationManager.AppSettings["ServerUrl"];
//            ManagerUrl = ServerUrl + "/manager";
//            ChromeDriver = new ChromeDriver("./");
//            //firefoxDriver = new FirefoxDriver();
//        }

//        //[TestMethod]
//        //public void TestChromeDriver()
//        //{
//        //    Driver = ChromeDriver;
//        //    this.TestDriver();
//        //    Driver.Close();
//        //}

//        [TestMethod]
//        public void TestChromeLogin()
//        {
//            Driver = ChromeDriver;
//            this.TestLogin();
//            Driver.Close();
//        }


//        private void Login()
//        {
//            Driver.Navigate().GoToUrl(ManagerUrl);
//            Driver.FindElement(By.Id("login")).SendKeys("admin");
//            Driver.FindElement(By.Name("password")).SendKeys("admin");
//            Driver.FindElement(By.Name("password")).SendKeys(Keys.Enter);
//        }

//        // Test if we can login to the manager area
//        private void TestLogin()
//        {
//            this.Login();
//            bool isLogoutDisplayed = Driver.FindElement(By.ClassName("logout")).Displayed;

//            Assert.IsTrue(isLogoutDisplayed);
//        }

//        // Test creating a metadata set with all available fields

//        private void TestCreateMetadataSet()
//        {
//            this.Login();
//            Driver.FindElement(By.LinkText("Metadata Sets")).Click();
//            Driver.FindElement(By.ClassName("add")).Click();
//            Driver.FindElement(By.Id("Name")).SendKeys(TestStrings.MetadatasetName);
//            Driver.FindElement(By.Id("Description")).SendKeys(TestStrings.MetadatasetDescription);

//            Driver.FindElement(By.Id("field-type-selector"));

//            // Add and remove 

//            // save metadataset
//            Driver.FindElement(By.ClassName("save")).Click();
//        }

//        private void TestEditMetadataSet()
//        {

//        }

//        //private void TestDriver()
//        //{
//        //    Driver.Navigate().GoToUrl("http://www.google.com");
//        //    Driver.FindElement(By.Id("lst-ib")).SendKeys("Test");
//        //    Driver.FindElement(By.Id("lst-ib")).SendKeys(Keys.Enter);
//        //    Assert.AreEqual(1, 1);
//        //}
//    }
//}
