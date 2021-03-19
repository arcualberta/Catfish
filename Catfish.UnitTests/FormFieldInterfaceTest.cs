﻿using Catfish.Core.Models;
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
            //driver.Navigate().GoToUrl(siteUrl + "/simple-form/");
            driver.Navigate().GoToUrl("https://localhost:44385/simple-form/");
            
            IWebElement element = driver.FindElement(By.Id("48cd8112-beea-4664-b5a9-739a79e652bc"));


            //SelectElement selectDD1 = new SelectElement(element);
            //element.SelectByIndex(1);
        }

    }
}
