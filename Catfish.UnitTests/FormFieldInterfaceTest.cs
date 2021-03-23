using Catfish.Core.Models;
using Catfish.Core.Services;
using Catfish.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Interactions;

namespace Catfish.UnitTests
{
    class FormFieldInterfaceTest
    {
        private string _dataRoot = "..\\..\\..\\Data\\Schemas\\";
        private AppDbContext _db;
        private TestHelper _testHelper;
        private IAuthorizationService _auth;
        private string siteUrl = ConfigurationManager.AppSettings["SiteUrl"];
        //IWebDriver driver = new ChromeDriver(".");
        IWebDriver driver = new ChromeDriver();
        private string itemAtrib;

        [SetUp]
        public void Setup()
        {
            _testHelper = new TestHelper();
            _db = _testHelper.Db;
            IAuthorizationService _auth = _testHelper.AuthorizationService;
        }

        public XElement LoadXml(string fileName)
        {
            var path = Path.Combine(_dataRoot, fileName);
            if (!File.Exists(path))
                Assert.Fail("File not found at " + path);

            XElement xml = XElement.Parse(File.ReadAllText(path));
            return xml;
        }

        public void RefreshDatabase()
        {
            //Deleting all entities in the Entity table
            var entities = _db.Entities.ToList();
            _db.Entities.RemoveRange(entities);
            _db.SaveChanges();

            //Reloading default collection
            _db.Collections.Add(Collection.Parse(LoadXml("collection_1.xml")) as Collection);

            //Reloading Item Templates
            _db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("simple_form.xml")) as ItemTemplate);
            _db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("visibleIf_requiredIf.xml")) as ItemTemplate);
            _db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("visibleIf_options.xml")) as ItemTemplate);
            _db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("composite_field.xml")) as ItemTemplate);
            _db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("table_field.xml")) as ItemTemplate);
            _db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("table_field2.xml")) as ItemTemplate);
            _db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("grid_table.xml")) as ItemTemplate);

            _db.SaveChanges();
        }

        [Test]
        public void RefreshData()
        {
            RefreshDatabase();
        }

        public void LoginTest()
        {
            IWebElement element = driver.FindElement(By.Name("pageGroup"));
            Assert.NotNull(element);
        }

        [Test]    
        public void Login()
        {
            //driver.Navigate().GoToUrl(siteUrl + "/manager/");
            driver.Navigate().GoToUrl("https://localhost:44385/manager/"); 
            driver.FindElement(By.Name("username")).SendKeys("admin");
            IWebElement element = driver.FindElement(By.Name("password"));
            element.SendKeys("passwd");
            element.Submit();

        }

        [Test]
        public void SimpleFormSubmissionTest()
        {
  
            RefreshData();
            // got to home page first to check if logged in (as otherwise forms to be tested not assessable)
            driver.Navigate().GoToUrl("https://localhost:44385/");


            List<IWebElement> elms = new List<IWebElement>();
            elms.AddRange(driver.FindElements(By.Id("btnSignIn")));


            if (elms.Count > 0)  // sign in element found -  so sign-in but via /manager/pages URL instead of where link goes
            {
                Login();
            }

            //else can assume loggged it



            //driver.Navigate().GoToUrl(siteUrl + "/simple-form/");
            driver.Navigate().GoToUrl("https://localhost:44385/simple-form/");
          

            //Dropdown test

            
            // IWebElement DDelement = driver.FindElement(By.Id("48cd8112-beea-4664-b5a9-739a79e652bc"));


            //SelectElement selectDD1 = new SelectElement(DDelement);
            // select dropdowm option 2
           // selectDD1.SelectByIndex(1);

            //Radio button test
            // 
            //IWebElement RBElement = driver.FindElement(By.Id("ef1c777b-6e80-48f6-b742-548f5226239c"));

            // first just select option 2 byID - could use index 1 after finding element.
            IWebElement RBElement = driver.FindElement(By.Id("9e45b902-dba8-4828-b65d-9b8ac47a0954"));
            RBElement.Click();

            // test if radio button 2 selected 
            //Boolean itemSlected = false;
            //itemSlected = RBElement.isSelected();
            
            itemAtrib = RBElement.GetAttribute("checked");



            //checkbox test
            // test if check boxes 3,4 are selected 
            IWebElement CBElement = driver.FindElement(By.Id("391d611d-d7f5-48a6-b8ae-e52053294616"));
            CBElement.Click();
            IWebElement CBElement2 = driver.FindElement(By.Id("c320b902-21f4-4bf1-ba7e-67890f7a0849"));
            CBElement2.Click();

            //test if selected



            itemAtrib = CBElement.GetAttribute("checked");

            itemAtrib = CBElement2.GetAttribute("checked");


        }

    }
}
