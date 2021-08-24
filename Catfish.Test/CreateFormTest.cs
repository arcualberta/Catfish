using Catfish.Core.Models;
using Catfish.Core.Services;
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
using System.Reflection;
using Catfish.Test.Helpers;
using System.Threading;

namespace Catfish.Test
{
    public class CreateFormTest
    {
        private AppDbContext _db;
        private TestHelper _testHelper;
        private SeleniumHelper _seleniumHelper;
        private IAuthorizationService _auth;
        private string siteUrl = ConfigurationManager.AppSettings["SiteUrl"];


      
        public CreateFormTest()
        {

        }

        [SetUp]
        public void Setup()
        {
            _testHelper = new TestHelper();
            _seleniumHelper = new SeleniumHelper(_testHelper.Configuration);
            _db = _testHelper.Db;
            IAuthorizationService _auth = _testHelper.AuthorizationService;

            _seleniumHelper.SetDriver(SeleniumHelper.eDriverType.Chrome);
            //_seleniumHelper.SetDriver(SeleniumHelper.eDriverType.Firefox);
            //_seleniumHelper.SetDriver(SeleniumHelper.eDriverType.Edge);
        }

        public void RefreshDatabase()
        {
            _testHelper.RefreshDatabase();
        }


        [Test]
        public void Login()
        {
            _seleniumHelper.LoginLocal();
            _seleniumHelper.Driver.Close();
        }

        [Test]
        public void NewFormOne()
        {

            string testSuffix = " C";
            string testTitle = "This is test" + testSuffix;
            string testDecs = "This test is about test" + testSuffix;
            string testLinkText = "Link to test" + testSuffix;
            //RefreshDatabase();
            _seleniumHelper.LoginLocal();

            //Navigating to the test page
            _seleniumHelper.GoToUrl("manager/forms/edit");

            _seleniumHelper.SetFormTextFromPlaceHolderText("Enter form title", testTitle);
            _seleniumHelper.SetFormTextAreaFromPlaceHolderText("Enter form description here.", testDecs);
            _seleniumHelper.SetFormTextFromPlaceHolderText("Enter link text", testLinkText);

            // save
            _seleniumHelper.ClickFormSaveButton();

            Thread.Sleep(1000);
            // get guid
            string ourID = _seleniumHelper.GetIDfromUrl();

            _seleniumHelper.GoToUrl("manager/forms/");
            Thread.Sleep(2000);

            _seleniumHelper.ClickViewForm(ourID);

            Thread.Sleep(5000);
            _seleniumHelper.Driver.Close();
        }

     }
}
