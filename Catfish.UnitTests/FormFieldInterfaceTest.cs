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
using Catfish.UnitTests.Helpers;

namespace Catfish.UnitTests
{
    class FormFieldInterfaceTest
    {
        private string _dataRoot = "..\\..\\..\\Data\\Schemas\\";
        private AppDbContext _db;
        private TestHelper _testHelper;
        private SeleniumHelper _seleniumHelper;
        private IAuthorizationService _auth;
        private string siteUrl = ConfigurationManager.AppSettings["SiteUrl"];
        private string itemAtrib;

        public FormFieldInterfaceTest()
        {

        }

        [SetUp]
        public void Setup()
        {
            _testHelper = new TestHelper();
            _seleniumHelper = new SeleniumHelper(_testHelper.Configuration);
            _db = _testHelper.Db;
            IAuthorizationService _auth = _testHelper.AuthorizationService;

            //_seleniumHelper.SetDriver(SeleniumHelper.eDriverType.Chrome);
            _seleniumHelper.SetDriver(SeleniumHelper.eDriverType.Firefox);
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

        //public void LoginTest()
        //{
        //    IWebElement element = driver.FindElement(By.Name("pageGroup"));
        //    Assert.NotNull(element);
        //}

        [Test]    
        public void Login()
        {
            _seleniumHelper.LoginLocal("admin", "passwd");
        }

        [Test]
        public void SimpleFormSubmissionTest()
        {
            RefreshData();
            _seleniumHelper.LoginLocal("admin", "passwd");

            //Navigating to the test page
            _seleniumHelper.GoToUrl("simple-form");

            //Selecting Option2 for DD1
            var ddId = "48cd8112-beea-4664-b5a9-739a79e652bc";
            var opOptId = "b8068a1b-a184-47f5-9da1-625e3eb4a2f4";
            _seleniumHelper.SelectDropdownOption(ddId, opOptId);

            //Selecting Option 3 for RB1
            var rbId = "ef1c777b-6e80-48f6-b742-548f5226239c";
            var rbOptId = "cd99d343-7901-4a1e-a3a3-052a53d737b7";
            _seleniumHelper.SelectRadioOption(rbId, rbOptId);

            //Selecting Option 3 and Option 4 for the CB1
            var chkId = "f69a2661-a375-47ea-a46c-9009a76c08eb";
            var chkOptIds = new string[] { "391d611d-d7f5-48a6-b8ae-e52053294616", "c320b902-21f4-4bf1-ba7e-67890f7a0849" };
            _seleniumHelper.SelectCheckOptions(chkId, chkOptIds);

            //Setting value of TF1 to Hello World
            var tfId = "50850ee4-2686-4f84-b8b7-5c2b66d70185";
            var tfVal = "Hello World";
            _seleniumHelper.SetTextFieldValue(tfId, tfVal);

            //Setting value of TA1 to Hello World
            var taId = "8016425b-c632-49fc-8d72-32d294ee429b";
            var taVal = "Hello World 123";
            _seleniumHelper.SetTextAreaValue(taId, taVal);

            //Setting value of INT1 to 250
            var intId = "2ce85370-5277-4d5a-baea-ac5a2ea435f1";
            var intVal = "250";
            _seleniumHelper.SetNumberValue(intId, intVal);

            //Setting value of DEC1 to 12.50
            var decId = "8628d84f-d209-4440-9fed-0e1ecef17c54";
            var decVal = "1250";
            _seleniumHelper.SetNumberValue(decId, decVal);

            //Setting value of DATE1 to 2021-03-05
            var dateId = "7d5f4efc-624b-4ba7-998b-6ec7fe48745d";
            var dateVal = new DateTime(2021, 3, 25);
            _seleniumHelper.SetDateValue(dateId, dateVal);

            //Clicking on the submit button
            var dataItemTemplateId = "b8ca1de3-a6ce-4693-aadc-9e32a322b6ba";
            _seleniumHelper.ClickSubmitButton(dataItemTemplateId, "Submit");

            _seleniumHelper.ModalSubmit(dataItemTemplateId);

        }

    }
}
