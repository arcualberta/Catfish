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
    public class FormFieldInterfaceTest
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

            _seleniumHelper.SetDriver(SeleniumHelper.eDriverType.Chrome);
            //_seleniumHelper.SetDriver(SeleniumHelper.eDriverType.Firefox);
            //_seleniumHelper.SetDriver(SeleniumHelper.eDriverType.Edge);
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
            _db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("SASform.xml")) as ItemTemplate);

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
            Login();

            //Navigating to the test page
            _seleniumHelper.GoToUrl("simple-form");

            //Selecting Option2 for DD1
            var ddId = "48cd8112-beea-4664-b5a9-739a79e652bc";
            var ddOptId = "b8068a1b-a184-47f5-9da1-625e3eb4a2f4";
            var dd1OptVal = "Option 2";
            _seleniumHelper.SelectDropdownOption(ddId, ddOptId);

            //Selecting Option 3 for RB1
            var rbId = "ef1c777b-6e80-48f6-b742-548f5226239c";
            var rbOptId = "cd99d343-7901-4a1e-a3a3-052a53d737b7";
            var rbOptVal = "Option 3";
            _seleniumHelper.SelectRadioOption(rbId, rbOptId);

            //Selecting Option 3 and Option 4 for the CB1
            var chkId = "f69a2661-a375-47ea-a46c-9009a76c08eb";
            var chkOptIds = new string[] { "391d611d-d7f5-48a6-b8ae-e52053294616", "c320b902-21f4-4bf1-ba7e-67890f7a0849" };
            var chkOptVals = new string[] { "Option 3", "Option 4"};
            _seleniumHelper.SelectCheckOptions(chkId, chkOptIds);

            //Setting value of TF1 to Hello World
            var tfId = "50850ee4-2686-4f84-b8b7-5c2b66d70185";
            var tfVal = "Hello World";
            _seleniumHelper.SetTextFieldValue(tfId, tfVal);

            //Setting value of TA1 to Hello World
            var taId = "8016425b-c632-49fc-8d72-32d294ee429b";
            var taVal = "Identity configuration in ASP.NET can be quite confusing, especially if you want to customize setup properties." +
                "When you use a code - first approach using Entity Framework, you have full control over your user identity options. However when " +
                "developers deal with bigger projects, they typically prefer to use a table - first approach in which they create the database, then consume " +
                "the information in the API, and lastly shape it in a way that it makes sense on the front end." +
                "So, configuring identity might work best with a mixed approach.";
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

            //Clicking on the modal confirmation button
            _seleniumHelper.CkickModalSubmit(dataItemTemplateId, "btn btn-success");

            //Clicking on the link to view detail view
            _seleniumHelper.ClickOnALink("click on here");


            //Validating DD1
            var ddDispOpVal = _seleniumHelper.GetSelectDisplayValue(ddId);
            Assert.AreEqual(dd1OptVal, ddDispOpVal, "DD1 value is not correctly saved");

            //Validating RB1
            var rbDispVal = _seleniumHelper.GetRadioDisplayValue(rbId);
            Assert.AreEqual(rbOptVal, rbDispVal, "RB1 value is not correctly saved");

            //Validating TF1
            var chkDispVal = _seleniumHelper.GetCheckboxDisplayValue(chkId);
            var valuesMissing = chkOptVals.Except(chkDispVal).Any();
            Assert.IsFalse(valuesMissing, "At least one value was not saved in CB1");
            var WrongValuesSaved = chkDispVal.Except(chkOptVals).Any();
            Assert.IsFalse(WrongValuesSaved, "Wrong values saved in CB1");

            //Validating TF1
            var tfDispVal = _seleniumHelper.GetTextFieldDisplayValue(tfId);
            Assert.AreEqual(tfVal, tfDispVal, "TF1 value is not correctly saved");

            //Validating TA1
            var taDispVal = _seleniumHelper.GetTextFieldDisplayValue(taId);
            Assert.AreEqual(taVal, taDispVal, "TA1 value is not correctly saved");

            //Validating INT1
            var intDispVal = _seleniumHelper.GetIntegerDisplayValue(intId);
            Assert.AreEqual(intVal, intDispVal, "INT1 value is not correctly saved");

            //Validating DEC1
            var decDispVal = _seleniumHelper.GetDecimalDisplayValue(decId);
            Assert.AreEqual(decVal, decDispVal, "DEC1 value is not correctly saved");

            //Validating DATE1
            var dateDispVal = _seleniumHelper.GetDateDisplayValue(dateId);
            Assert.AreEqual(dateVal.ToString("yyyy-MM-dd"), dateDispVal, "DATE1 value is not correctly saved");

             _seleniumHelper.Driver.Close();
        }

        [Test]
        public void TableFieldFormTest()
        {
            RefreshData();
            Login();

            //Navigating to the test page
            _seleniumHelper.GoToUrl("table-field-form");

            //Setting value of DEC1 to 12.50
            var decId1 = "b958ef1f-229f-4a60-a4b0-140e482938b1";
            var decVal1 = "200";
            _seleniumHelper.SetNumberValue(decId1, decVal1);

            //Setting value of DEC1 to 12.50
            var decId2 = "1e7ba58f-32b0-44b7-ae32-3f2365cceeb2";
            var decVal2 = "100";
            _seleniumHelper.SetNumberValue(decId2, decVal2);

            //Validating column sum
            var totalActualValue = Decimal.Parse(decVal1) + Decimal.Parse(decVal2);
            var totalId = "1fbff568-d9b3-4e1f-a51c-6b65800ec290";
            var totalDisplayValue = _seleniumHelper.GetTableSummaryColumnSum(totalId);
            Assert.AreEqual(totalActualValue.ToString(), totalDisplayValue, "Table feild column summation is wrong");
            
            //Clicking on sumbit button
            var dataItemTemplateId = "8cc8bb7f-9c50-41d6-b87f-cbf53889d44e";
            _seleniumHelper.ClickSubmitButton(dataItemTemplateId, "Submit");

            //Clicking on the modal confirmation button
            _seleniumHelper.CkickModalSubmit(dataItemTemplateId, "btn btn-success");

            //Clicking on the link to view detail view
            _seleniumHelper.ClickOnALink("click on here");

           //Valicating column 1 enterd value and saved value.
            var colVal1 = _seleniumHelper.GetTableColumnDisplayValue(decId1, "table");
            Assert.AreEqual(colVal1, decVal1, "table column 1 value is not correctly saved");

            //Valicating column 1 enterd value and saved value.
            var colVal2 = _seleniumHelper.GetTableColumnDisplayValue(decId2, "table");
            Assert.AreEqual(colVal2, decVal2, "table column 2 value is not correctly saved");

            //Valicating total value and saved value.
            var colTotal = _seleniumHelper.GetTableFooterColumnDisplayValue(totalId, "table");
            Assert.AreEqual(colTotal, totalActualValue.ToString(), "table total value is not correctly saved");

            _seleniumHelper.Driver.Close();
        }

        [Test]
        public void TableFieldFormOriginalTest()
        {
            RefreshData();
            Login();

            //Navigating to the test page
            _seleniumHelper.GoToUrl("table-field-form-original");

            //Setting value of DATE1 to 2021-03-31
            var dateVal = new DateTime(2021, 3, 31);
            _seleniumHelper.SetDateValue(dateVal, 0, 0);

            //Setting value of TF1 to Product 1
            var tfVal = "Product 1";
            _seleniumHelper.SetTextFieldValue(tfVal, 0, 1);
            
            //Setting value of TA1 to Description
            var taVal = "This is the product 1 description. If you can read this, selenium test is working.";
            _seleniumHelper.SetTextAreaValue(taVal, 0, 2);//Setting value of DEC1 to 12.50

            //Setting value of DEC1 to 25
            var decVal = "25";
            _seleniumHelper.SetNumberValue(decVal, 0, 3);

            //Setting value of INT1 to 5
            var int1Val = "5";
            _seleniumHelper.SetNumberValue(int1Val, 0, 4);

            //Setting value of INT1 to 3
            var int2Val = "3";
            _seleniumHelper.SetNumberValue(int2Val, 0, 5);

            //Validating column sum
            var totalQuantityActualValue = int.Parse(int1Val) + int.Parse(int2Val);
            var totalQuantityDisplayValue = _seleniumHelper.GetTableRowColumnSum(0, 6);
            Assert.AreEqual(totalQuantityActualValue.ToString(), totalQuantityDisplayValue, "Table quantity feild summation is wrong");

            //Validating column sum
            var totalActualValue = int.Parse(totalQuantityDisplayValue) * double.Parse(decVal);
            var totalDisplayValue = _seleniumHelper.GetTableRowColumnSum(0, 7);
            Assert.AreEqual(totalActualValue.ToString(), totalDisplayValue, "Table total feild summation is wrong");

            //Selecting Available for RB1
            var rbOptId = "bc1f70b3-92a4-4588-acad-f48f900690eb";
            var rbOptVal = "Available";
            _seleniumHelper.SelectRadioOption(rbOptId, 0, 8);

            //Selecting Option 3 and Option 4 for the CB1
            var chkOptIds = new string[] { "147ec889-948b-4cd8-9469-89a8d77ea4d8", "ed42c49e-bc8e-47ca-b2b2-732a673e36ce" };
            var chkOptVals = new string[] { "Health", "Beauty" };
            _seleniumHelper.SelectCheckOptions(chkOptIds, 0, 9);


            //Clicking on the add row button
            var dataItemTemplateId = "5ee03f43-8b8e-4853-b993-32de0a4d7da9";
            _seleniumHelper.ClickSubmitButton(dataItemTemplateId, "+ Row");
//=======================================================================================
//===================New Row=============================================================
//=======================================================================================
            //Setting value of DATE1 to 2021-03-31
            //var date2Id = "c95566ce-2e27-489f-86da-b309514e947b";
            var date2Val = new DateTime(2021, 4, 06);
            _seleniumHelper.SetDateValue(date2Val, 1, 0);

            //Setting value of TF1 to Product 2
            var tf2Val = "Product 2";
            _seleniumHelper.SetTextFieldValue(tf2Val, 1, 1);

            //Setting value of TA1 to Description
            var ta2Val = "This is the product 2 description. If you can read this, selenium test is working.";
            _seleniumHelper.SetTextAreaValue(ta2Val, 1, 2);//Setting value of DEC1 to 12.50

            //Setting value of DEC1 to 50
           var dec2Val = "50";
            _seleniumHelper.SetNumberValue(dec2Val, 1, 3);

            //Setting value of INT1 to 4
            var int1Val2 = "4";
            _seleniumHelper.SetNumberValue(int1Val2, 1, 4);

            //Setting value of INT1 to 3
            var int2Val2 = "5";
            _seleniumHelper.SetNumberValue(int2Val2, 1, 5);

            ////Validating column sum
            var totalQuantity2ActualValue = int.Parse(int1Val2) + int.Parse(int2Val2);
            //var totalQuantity2DisplayValue = _seleniumHelper.GetTableRowColumnSum(1, 6);
            //Assert.AreEqual(totalQuantity2ActualValue.ToString(), totalQuantity2DisplayValue.ToString(), "Table quantity feild summation is wrong");

            //Validating column sum
            var total2ActualValue = totalQuantity2ActualValue * double.Parse(dec2Val);
            var total2DisplayValue = _seleniumHelper.GetTableRowColumnSum(1, 7);
            Assert.AreEqual(total2ActualValue.ToString(), total2DisplayValue.ToString(), "Table total feild summation is wrong");

            //Selecting Available for RB1
            var rb2OptId = "7443bc06-1b20-4593-95b6-9f191faff0e8";
            var rb2OptVal = "Not available";
            _seleniumHelper.SelectRadioOption(rb2OptId, 1, 8);

            //Selecting Option 3 and Option 4 for the CB1
            var chk2OptIds = new string[] { "73ae522c-fd4f-4aeb-9cb7-af92b709646f", "9845cc34-7020-44a9-bf3d-98f075c36f41" };
            var chk2OptVals = new string[] { "Prescription", "Nutrition" };
            _seleniumHelper.SelectCheckOptions(chk2OptIds, 1, 9);
        }

        [Test]
        public void SASForm()
        {
            RefreshData();
            Login();
            _seleniumHelper.GoToUrl("sas-form");

            //Setting value of TF1 to Hello World
            var tfId = "8e33c004-1864-46ec-b279-b5541b7adffe";
            var tfVal = "Isuru Wickramasinghe";
            _seleniumHelper.SetTextFieldValue(tfId, tfVal);

            //Clicking on the submit button
            var dataItemTemplateId = "f31602f0-1002-4e35-8c2c-90a03a8d3a93";
            _seleniumHelper.ClickSaveButton(dataItemTemplateId, "Save");

            //Clicking on the link to view detail view
            _seleniumHelper.ClickOnALink("click on here");

            //Clicking on the link to view detail view
            _seleniumHelper.ClickOnALink("Edit");
        }
    }
}
