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
    public class FormFieldInterfaceTest
    {
        private string _dataRoot = "..\\..\\..\\Data\\Schemas\\";
        private AppDbContext _db;
        private TestHelper _testHelper;
        private SeleniumHelper _seleniumHelper;
        private IAuthorizationService _auth;
        private string siteUrl = ConfigurationManager.AppSettings["SiteUrl"];
        //private string attachmentPath = ConfigurationManager.AppSettings["AttachmentPath"];
        //private string _attachmentPath = "C:/CodeBase/CatfishTest/Catfish.Test/Data/Attachments/";
        private string _attachmentPath = @"..\\..\\..\\Data\\Attachments\\";
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

        public void RefreshDatabase()
        {
            _testHelper.RefreshDatabase();

            //////Deleting all entities in the Entity table
            ////var entities = _db.Entities.ToList();
            ////_db.Entities.RemoveRange(entities);
            ////_db.SaveChanges();

            //////Reloading default collection
            ////_db.Collections.Add(Collection.Parse(LoadXml("collection_1.xml")) as Collection);

            //////Reloading Item Templates
            ////_db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("simple_form.xml")) as ItemTemplate);
            ////_db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("visibleIf_requiredIf.xml")) as ItemTemplate);
            ////_db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("visibleIf_options.xml")) as ItemTemplate);
            ////_db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("composite_field.xml")) as ItemTemplate);
            ////_db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("table_field.xml")) as ItemTemplate);
            ////_db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("table_field2.xml")) as ItemTemplate);
            ////_db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("grid_table.xml")) as ItemTemplate);
            ////_db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("SASform.xml")) as ItemTemplate);

            ////_db.SaveChanges();
        }

        [Test]
        public void Login()
        {
            _seleniumHelper.LoginLocal();
            _seleniumHelper.Driver.Close();
        }

        [Test]
        public void SimpleFormSubmissionTest()
        {
            RefreshDatabase();
            _seleniumHelper.LoginLocal();

            //Navigating to the test page
            _seleniumHelper.GoToUrl("simple-form");

            //Selecting Option2 for DD1
            var ddId = "a7f5aa8f-b9a5-4619-8d33-eb5d232bfcf0";
            var ddOptId = "97f12c67-0f0e-4e9d-bad3-e5a4f425230e";
            var dd1OptVal = "Option 2";
            _seleniumHelper.SelectDropdownOption(ddId, ddOptId);

            //Selecting Option 3 for RB1
            var rbId = "f5860dc9-8d50-4332-980b-fc6e4f230332";
            var rbOptId = "451a4cea-0d30-4ce7-b6c2-6a6311351595";
            var rbOptVal = "Option 3";
            _seleniumHelper.SelectRadioOption(rbId, rbOptId);

            //Selecting Option 3 and Option 4 for the CB1
            var chkId = "cd53c8c7-3893-42e1-b8d6-4a7ce7899a84";
            var chkOptIds = new string[] { "0720136e-d8a4-4995-af87-575487310555", "dd2a3bb9-1dd9-4253-bf4d-19f53d1a18ed" };
            var chkOptVals = new string[] { "Option 3", "Option 4" };
            _seleniumHelper.SelectCheckOptions(chkId, chkOptIds);

            //Setting value of TF1 to Hello World
            var tfId = "d4165f46-1443-4dba-b10c-ce77ac7c1925";
            var tfVal = "Hello World";
            _seleniumHelper.SetTextFieldValue(tfId, tfVal);

            //Setting value of TA1 to Hello World
            var taId = "bbb9fb03-b5a1-4ba8-bfde-3f75d4130bc0";
            var taVal = "Identity configuration in ASP.NET can be quite confusing, especially if you want to customize setup properties." +
                "When you use a code - first approach using Entity Framework, you have full control over your user identity options. However when " +
                "developers deal with bigger projects, they typically prefer to use a table - first approach in which they create the database, then consume " +
                "the information in the API, and lastly shape it in a way that it makes sense on the front end." +
                "So, configuring identity might work best with a mixed approach.";
            _seleniumHelper.SetTextAreaValue(taId, taVal);

            //Setting value of INT1 to 250
            var intId = "bc9e041f-1948-407c-b5fa-db4106e47e2d";
            var intVal = "250";
            _seleniumHelper.SetNumberValue(intId, intVal);

            //Setting value of DEC1 to 12.50
            var decId = "715692a9-ae70-4746-87f2-76c3a81a397a";
            var decVal = "1250";
            _seleniumHelper.SetNumberValue(decId, decVal);

            //Setting value of DATE1 to 2021-03-05
            var dateId = "73a2258c-7430-405e-9f04-c9ddcf19f076";
            var dateVal = new DateTime(2021, 3, 25);
            _seleniumHelper.SetDateValue(dateId, dateVal);

            var fileName = "SAStestingdoc1.pdf";
            var fileId = "3cdb4841-b9c2-4614-b0d8-7b08adbd9e5a";
            _seleniumHelper.UpLoadFile(fileId, fileName);

            //Clicking on the submit button
            var dataItemTemplateId = "d46d777e-66aa-45d8-867a-c51d50c6fa7c";
            _seleniumHelper.ClickOnButtonType(dataItemTemplateId, "Submit");

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


            var attachmentFileDisplayValue = _seleniumHelper.GetAttachmentFieldDisplayValue("3cdb4841-b9c2-4614-b0d8-7b08adbd9e5a");
            Assert.AreEqual(fileName, attachmentFileDisplayValue, "Attachment is not correctly saved");
            _seleniumHelper.Driver.Close();
        }

        [Test]
        public void TableFieldFormTest()
        {
            RefreshDatabase();
            _seleniumHelper.LoginLocal();

            //Navigating to the test page
            _seleniumHelper.GoToUrl("table-field-form");

            //Setting value of DEC1 to 12.50
            var decId1 = "b958ef1f-229f-4a60-a4b0-140e482938b1";
            var decVal1 = "2.2";
            _seleniumHelper.SetNumberValue(decId1, decVal1);

            //Setting value of DEC1 to 12.50
            var decId2 = "1e7ba58f-32b0-44b7-ae32-3f2365cceeb2";
            var decVal2 = "2.36";
            _seleniumHelper.SetNumberValue(decId2, decVal2);

            //Validating column sum
            var totalActualValue = Decimal.Parse(decVal1) + Decimal.Parse(decVal2);
            var totalId = "1fbff568-d9b3-4e1f-a51c-6b65800ec290";
            var totalDisplayValue = _seleniumHelper.GetTableSummaryColumnSum(totalId);
            Assert.AreEqual(totalActualValue.ToString(), totalDisplayValue, "Table feild column summation is wrong");

            //Clicking on sumbit button
            var dataItemTemplateId = "8cc8bb7f-9c50-41d6-b87f-cbf53889d44e";
            _seleniumHelper.ClickOnButtonType(dataItemTemplateId, "Submit");

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
        public void TableFieldFormTestMultipleValues()
        {
            RefreshDatabase();
            _seleniumHelper.LoginLocal();

            //Navigating to the test page
            _seleniumHelper.GoToUrl("table-field-form");

            //Setting value of DEC1 to 12.50
            var decId1 = "b958ef1f-229f-4a60-a4b0-140e482938b1";
            var decVal1 = "2.17";
            _seleniumHelper.SetNumberValue(decId1, decVal1);

            string decVal2 = "";
            decimal totalActualValue = 0.0M;
            string totalDisplayValue = "";

            string decId2 = "1e7ba58f-32b0-44b7-ae32-3f2365cceeb2";
            string totalId = "1fbff568-d9b3-4e1f-a51c-6b65800ec290";

            //Setting value of DEC1 to 12.50
            for ( int iCent = 1; iCent <= 100; iCent += 1) {

                 decVal2 = (iCent/100.0).ToString();

                _seleniumHelper.SetNumberValue(decId2, decVal2);

                //Validating column sum
                 totalActualValue = Decimal.Parse(decVal1) + Decimal.Parse(decVal2);

                 totalDisplayValue = _seleniumHelper.GetTableSummaryColumnSum(totalId);

                
                Assert.AreEqual(totalActualValue.ToString(), totalDisplayValue, "Table feild column summation is wrong");
            }

                //Clicking on sumbit button
                var dataItemTemplateId = "8cc8bb7f-9c50-41d6-b87f-cbf53889d44e";
                _seleniumHelper.ClickOnButtonType(dataItemTemplateId, "Submit");

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
        ////////[Test]
        ////////public void TableFieldFormOriginalTest()
        ////////{
        ////////    RefreshDatabase();
        ////////    _seleniumHelper.LoginLocal();

        ////////    //Navigating to the test page
        ////////    _seleniumHelper.GoToUrl("table-field-form-original");

        ////////    //Setting value of DATE1 to 2021-03-31
        ////////    var dateVal = new DateTime(2021, 3, 31);
        ////////    _seleniumHelper.SetDateValue(dateVal, 0, 0);

        ////////    //Setting value of TF1 to Product 1
        ////////    var tfVal = "Product 1";
        ////////    _seleniumHelper.SetTextFieldValue(tfVal, 0, 1);

        ////////    //Setting value of TA1 to Description
        ////////    var taVal = "This is the product 1 description. If you can read this, selenium test is working.";
        ////////    _seleniumHelper.SetTextAreaValue(taVal, 0, 2);//Setting value of DEC1 to 12.50

        ////////    //Setting value of DEC1 to 25
        ////////    var decVal = "25";
        ////////    _seleniumHelper.SetNumberValue(decVal, 0, 3);

        ////////    //Setting value of INT1 to 5
        ////////    var int1Val = "5";
        ////////    _seleniumHelper.SetNumberValue(int1Val, 0, 4);

        ////////    //Setting value of INT1 to 3
        ////////    var int2Val = "3";
        ////////    _seleniumHelper.SetNumberValue(int2Val, 0, 5);

        ////////    //Validating column sum
        ////////    var totalQuantityActualValue = int.Parse(int1Val) + int.Parse(int2Val);
        ////////    var totalQuantityDisplayValue = _seleniumHelper.GetTableRowColumnSum(0, 6);
        ////////    Assert.AreEqual(totalQuantityActualValue.ToString(), totalQuantityDisplayValue, "Table quantity feild summation is wrong");

        ////////    //Validating column sum
        ////////    var totalActualValue = int.Parse(totalQuantityDisplayValue) * double.Parse(decVal);
        ////////    var totalDisplayValue = _seleniumHelper.GetTableRowColumnSum(0, 7);
        ////////    Assert.AreEqual(totalActualValue.ToString(), totalDisplayValue, "Table total feild summation is wrong");

        ////////    //Selecting Available for RB1
        ////////    var rbOptId = "bc1f70b3-92a4-4588-acad-f48f900690eb";
        ////////    var rbOptVal = "Available";
        ////////    _seleniumHelper.SelectRadioOption(rbOptId, 0, 8);

        ////////    //Selecting Option 3 and Option 4 for the CB1
        ////////    var chkOptIds = new string[] { "147ec889-948b-4cd8-9469-89a8d77ea4d8", "ed42c49e-bc8e-47ca-b2b2-732a673e36ce" };
        ////////    var chkOptVals = new string[] { "Health", "Beauty" };
        ////////    _seleniumHelper.SelectCheckOptions(chkOptIds, 0, 9);


        ////////    //Clicking on the add row button
        ////////    var dataItemTemplateId = "5ee03f43-8b8e-4853-b993-32de0a4d7da9";
        ////////    _seleniumHelper.ClickSubmitButton(dataItemTemplateId, "+ Row");

        ////////    //=======================================================================================
        ////////    //===================New Row=============================================================
        ////////    //=======================================================================================
        ////////    //Setting value of DATE1 to 2021-03-31
        ////////    //var date2Id = "c95566ce-2e27-489f-86da-b309514e947b";
        ////////    var date2Val = new DateTime(2021, 4, 06);
        ////////    _seleniumHelper.SetDateValue(date2Val, 1, 0);

        ////////    //Setting value of TF1 to Product 2
        ////////    var tf2Val = "Product 2";
        ////////    _seleniumHelper.SetTextFieldValue(tf2Val, 1, 1);

        ////////    //Setting value of TA1 to Description
        ////////    var ta2Val = "This is the product 2 description. If you can read this, selenium test is working.";
        ////////    _seleniumHelper.SetTextAreaValue(ta2Val, 1, 2);//Setting value of DEC1 to 12.50

        ////////    //Setting value of DEC1 to 50
        ////////   var dec2Val = "50";
        ////////    _seleniumHelper.SetNumberValue(dec2Val, 1, 3);

        ////////    //Setting value of INT1 to 4
        ////////    var int1Val2 = "4";
        ////////    _seleniumHelper.SetNumberValue(int1Val2, 1, 4);

        ////////    //Setting value of INT1 to 3
        ////////    var int2Val2 = "5";
        ////////    _seleniumHelper.SetNumberValue(int2Val2, 1, 5);

        ////////    ////Validating column sum
        ////////    var totalQuantity2ActualValue = int.Parse(int1Val2) + int.Parse(int2Val2);
        ////////    //var totalQuantity2DisplayValue = _seleniumHelper.GetTableRowColumnSum(1, 6);
        ////////    //Assert.AreEqual(totalQuantity2ActualValue.ToString(), totalQuantity2DisplayValue.ToString(), "Table quantity feild summation is wrong");

        ////////    //Validating column sum
        ////////    var total2ActualValue = totalQuantity2ActualValue * double.Parse(dec2Val);
        ////////    var total2DisplayValue = _seleniumHelper.GetTableRowColumnSum(1, 7);
        ////////    Assert.AreEqual(total2ActualValue.ToString(), total2DisplayValue.ToString(), "Table total feild summation is wrong");

        ////////    //Selecting Available for RB1
        ////////    var rb2OptId = "7443bc06-1b20-4593-95b6-9f191faff0e8";
        ////////    var rb2OptVal = "Not available";
        ////////    _seleniumHelper.SelectRadioOption(rb2OptId, 1, 8);

        ////////    //Selecting Option 3 and Option 4 for the CB1
        ////////    var chk2OptIds = new string[] { "73ae522c-fd4f-4aeb-9cb7-af92b709646f", "9845cc34-7020-44a9-bf3d-98f075c36f41" };
        ////////    var chk2OptVals = new string[] { "Prescription", "Nutrition" };
        ////////    _seleniumHelper.SelectCheckOptions(chk2OptIds, 1, 9);

        ////////    _seleniumHelper.Driver.Close();
        ////////}

        [Test]
        public void TableFieldFormOriginalTest()
        {
            RefreshDatabase();
            //Login();

            RefreshDatabase();
            _seleniumHelper.LoginLocal();

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
            _seleniumHelper.ClickOnButtonType(dataItemTemplateId, "+ Row");

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
            //var total2DisplayValue = _seleniumHelper.GetTableRowColumnSum(1, 7);
            //Assert.AreEqual(total2ActualValue.ToString(), total2DisplayValue.ToString(), "Table total feild summation is wrong");

            //Selecting Available for RB1
            var rb2OptId = "7443bc06-1b20-4593-95b6-9f191faff0e8";
            var rb2OptVal = "Not available";
            _seleniumHelper.SelectRadioOption(rb2OptId, 1, 8);

            //Selecting Option 3 and Option 4 for the CB1
            var chk2OptIds = new string[] { "73ae522c-fd4f-4aeb-9cb7-af92b709646f", "9845cc34-7020-44a9-bf3d-98f075c36f41" };
            var chk2OptVals = new string[] { "Prescription", "Nutrition" };
            _seleniumHelper.SelectCheckOptions(chk2OptIds, 1, 9);

            //Clicking on the add row button
            _seleniumHelper.ClickOnButtonType(dataItemTemplateId, "+ Row");

            //Deleting third row.
            _seleniumHelper.ClickTableRowDeleteButton(2);

            //Clicking on the submit button
            _seleniumHelper.ClickOnButtonType(dataItemTemplateId, "Submit");

            //Clicking on the modal confirmation button
            _seleniumHelper.CkickModalSubmit(dataItemTemplateId, "btn btn-success");

            //Clicking on the link to view detail view
            _seleniumHelper.ClickOnALink("click on here");

            //Validating first row DD1
            var date1DispOpVal = _seleniumHelper.GetDateDisplayValue(0, 0);
            Assert.AreEqual(dateVal.ToString("yyyy-MM-dd"), date1DispOpVal, "First row DD1 value is not correctly saved");

            //Validating first row TF1
            var tf1Display1 = _seleniumHelper.GetTextFieldDisplayValue(0, 1);
            Assert.AreEqual(tfVal, tf1Display1, "First row TF1 value is not correctly saved");

            //Validating first row TA1
            var ta1Display1 = _seleniumHelper.GetTextAreaDisplayValue(0, 2);
            Assert.AreEqual(taVal, ta1Display1, "First row TA1 value is not correctly saved");

            //Validating first row DEC1
            var dec1Display1 = _seleniumHelper.GetDecimalDisplayValue(0, 3);
            Assert.AreEqual(decVal, dec1Display1, "First row DEC1 value is not correctly saved");

            //Validating first row INT1
            var int1Display1 = _seleniumHelper.GetIntegerDisplayValue(0, 4);
            Assert.AreEqual(int1Val, int1Display1, "First row INT1 value is not correctly saved");

            //Validating first row INT2
            var int2Display1 = _seleniumHelper.GetIntegerDisplayValue(0, 5);
            Assert.AreEqual(int2Val, int2Display1, "First row INT2 value is not correctly saved");

            //Validating first row TQ1
            var totalQuantityDisplay1 = _seleniumHelper.GetIntegerDisplayValue(0, 6);
            Assert.AreEqual(totalQuantityActualValue, int.Parse(totalQuantityDisplay1), "First row TQ1 value is not correctly saved");

            //Validating first row TOTAL1
            var totalDisplay1 = _seleniumHelper.GetDecimalDisplayValue(0, 7);
            Assert.AreEqual(totalActualValue, Decimal.Parse(totalDisplay1), "First row TOTAL value is not correctly saved");

            _seleniumHelper.Driver.Close();
        }

        [Test]
        public void SASForm()
        {
            RefreshDatabase();
            _seleniumHelper.LoginLocal();
            _seleniumHelper.GoToUrl("sas-form");


            // first part - applicant - project identified
            #region applicant_part

            //Setting value applicant's name
            var applNameId = "8e33c004-1864-46ec-b279-b5541b7adffe";
            var applNameVal = "Arc Guy";
            _seleniumHelper.SetTextFieldValue(applNameId, applNameVal);

            //applicant's email
            var emailId = "d191085e-7ecf-4be6-8d69-56c3cfa9da51";
            var emailVal = "arcguya@ualberta.ca";
            _seleniumHelper.SetTextFieldValue(emailId, emailVal);

            //Selecting in  Dept dropdown
            var deptddId = "18133fb5-285d-47b3-8fa4-5a63cf37e723";
            var deptddOptId = "6a8689b8-3c17-4b31-8378-3584766f1940";
            //var deptdd1OptVal = "Arts Resources Centre";
            _seleniumHelper.SelectDropdownOption(deptddId, deptddOptId);

            //Selecting in  Rank dropdown
            var rankddId = "3cbfe358-8d4d-4f20-a84c-53ba4b450141";
            var rankddOptId = "6c0bc3c7-344f-46e1-8100-b18416ed228f";
            //var rankdd1OptVal = "Professor";
            _seleniumHelper.SelectDropdownOption(rankddId, rankddOptId);

            //Selecting "No" Option  for "are you a dept. chair" RB
            var chairrbId = "5ada22d2-2b7b-42aa-a449-79b04115af12";

            //var chairrbOptVal = "No";
            var chairrbOptNoId = "fc6bf9a5-b0c7-44de-8cff-6f2eee8b3f91";
            _seleniumHelper.SelectRadioOption(chairrbId, chairrbOptNoId);

            // opps - make that a yes -- leave back to no so  can select alt contact
            //var chairrbOptYesId = "799bb23c-7137-4b9a-a150-77359e8a3e90";
            //_seleniumHelper.SelectRadioOption(chairrbId, chairrbOptYesId);

            // enter chair info  36c2aabc-44b9-4a6c-8172-1bb9c199c276, d8afbf19-a9ec-4da2-9077-4a0c63effa66

            var chairId = "36c2aabc-44b9-4a6c-8172-1bb9c199c276";
            var chairOptId = "2e506b72-dd18-4fa4-9843-49a9c6657fa8";
            _seleniumHelper.SelectDropdownOption(chairId, chairOptId);

            //Setting project name
            var projNameId = "58b39ab2-fbf7-43df-ba76-59b7d04bc32f";
            var projNameVal = "Project Arc1";
            _seleniumHelper.SetTextFieldValue(projNameId, projNameVal);

            //Setting project description
            var projDescId = "f89e70fa-14c2-495b-89cc-e98307f05bf5";
            var projDescVal = "Officials with Alberta parks and environment sampled, or went fishing in the pond where the catfish was discovered and caught thirty catfish, from three generations — or catfish that have lived in the lake for about three years.";
            _seleniumHelper.SetTextAreaValue(projDescId, projDescVal);

            //Option  for involving subjects RB 575
            var involveSubjectsrbId = "bc5e74c6-3586-434e-aad4-e2bd92a2fabb";
            //var involveSubjectsnoOptId = "b26717d3-c8e2-49d1-bb27-e98542ce748b";
            var involveSubjectsyesOptId = "bcb08bd5-5e35-4139-abc6-5bda5264e490";
            //yes-option-id="bcb08bd5-5e35-4139-abc6-5bda5264e490"
            //var chairrbOptVal = "Yes";
            _seleniumHelper.SelectRadioOption(involveSubjectsrbId, involveSubjectsyesOptId);

            //Option approval Obtained
            var approvalObtainedrbId = "9a857bd0-369c-45e8-984e-ef1aefc9f368";
            var approvalObtainedYesOptId = "376bbad9-670d-483b-afff-1e825d25f2a1";
            _seleniumHelper.SelectRadioOption(approvalObtainedrbId, approvalObtainedYesOptId);

            //approval expiry date
            var approvalExpireDateId = "3b762808-b1b5-4d0a-9ed8-3c54e5510be3";
            var approvalExpireDateVal = new DateTime(2024, 7, 27);
            _seleniumHelper.SetDateValue(approvalExpireDateId, approvalExpireDateVal);



            #endregion applicant_part

            // Start of Budget details

            //Conference travel
            #region conference_part

            //Conference name
            var confNameId = "0eea8ef1-16d2-4ef3-be9c-65b947b329a9";
            var confNameVal = "Catfish Annual";
            _seleniumHelper.SetTextFieldValue(confNameId, confNameVal);



            //Setting conf Date 1 to 2021-05-05
            var confDate1Id = "5d0d1ee2-16a7-4fef-84d6-f0bc8ba26ad0";
            var confDate1Val = new DateTime(2021, 5, 5);
            _seleniumHelper.SetDateValue(confDate1Id, confDate1Val);


            //Setting conf Date 2 to 2021-05-25
            var confDate2Id = "2d5eab48-dd99-41ca-a8f1-cd29271d2779";
            var confDate2Val = new DateTime(2021, 5, 25);
            _seleniumHelper.SetDateValue(confDate2Id, confDate2Val);

            //Travel destination
            var destinationId = "22b3f496-c444-4ea2-8114-1d1feaff9813";
            var destinationVal = "Winnipeg";
            _seleniumHelper.SetTextFieldValue(destinationId, destinationVal);



            //Setting travel Date 1 to 2021-05-01
            var travDate1Id = "5fc6c8b7-9c8f-48d0-ab22-d0baa3db20c1";
            var travDate1Val = new DateTime(2021, 5, 1);
            _seleniumHelper.SetDateValue(travDate1Id, travDate1Val);

            //Setting travel Date 2 to 2021-05-30
            var travDate2Id = "cb9ae842-12b2-416d-9042-d6ed7420ac4e";
            var travDate2Val = new DateTime(2021, 5, 30);
            _seleniumHelper.SetDateValue(travDate2Id, travDate2Val);


            //Setting Participation checkbox

            var participationgroupId = "64678688-16bd-41a9-8738-3ded02bc374f";
            var participationIds = new string[] { "491e8b2f-4ddd-46ae-b589-f108cc80ac13", "f43a07df-9d58-4a02-bf30-2c9a2af9566e" };
            var participationVals = new string[] { "Invited Speaker", "Other" };
            _seleniumHelper.SelectCheckOptions(participationgroupId, participationIds);

            // input other option ...
            var participationOtherId = "47c956cc-2c9a-4ace-a6bc-d374b8d83566";
            var participationOtherVal = "Catfish Expert";
            _seleniumHelper.SetTextFieldValue(participationOtherId, participationOtherVal);


            //Setting airfare value 700.57
            var airfareId = "8f938a04-2900-4ea6-a361-b1534c5199ec";
            //var airfareVal = "700.57";  // unless round number, item flagged at sumbit stage.
            var airfareVal = "700";
            _seleniumHelper.SetNumberValue(airfareId, airfareVal);


            // accomadation
            var accomadationId = "8eb97013-f4a9-4dad-98c4-7903bd07344e";
            //var accomadationVal = "1050.55"; // unless round number, item flagged at sumbit stage.
            var accomadationVal = "1050";
            _seleniumHelper.SetNumberValue(accomadationId, accomadationVal);

            // per diem
            var perdiemId = "fbea4d01-a890-492d-b0b7-abb5af7f1c41";
            var perdiemVal = "80.00";
            _seleniumHelper.SetNumberValue(perdiemId, perdiemVal);

            // Confrenece Registration
            var confRegId = "a71a23c1-a58c-4e6e-b374-e4c782cf5371";
            var confRegVal = "200.00";
            _seleniumHelper.SetNumberValue(confRegId, confRegVal);

            // Ground transport
            var groundId = "1ebc5b43-fdef-44f4-bdfb-e533a5bb4f11";
            var groundVal = "50.99";
            _seleniumHelper.SetNumberValue(groundId, groundVal);

            //Additional expenses
            var addExpId = "32538c9b-09b2-409b-8593-9f59ed04668a";
            var addExpVal = "100.00";
            _seleniumHelper.SetNumberValue(addExpId, addExpVal);

            //Details Additional expenses
            var addExpDetailsId = "7b2c801b-25cb-48ed-9cb1-669d5067b360";
            // note space before \r may produce failed asserts
            //var addExpDetailsVal = "Catfish food: $90.00 \r\n" +
            //                        "dental floss: $0.99 \r\n" +
            //                        "Eyedrops: $9.00";

            var addExpDetailsVal = "Catfish food: $90.00\r\n" +
                                    "dental floss: $0.99\r\n" +
                                    "Eyedrops: $9.00";

            _seleniumHelper.SetTextAreaValue(addExpDetailsId, addExpDetailsVal);


            //Supporting Documentation - upload one pdf file

            var chooseButtonId_1 = "7c0578e5-948f-4eed-a2d3-ba1baf11edfa";

            //var uploadPath = Path.Combine(_attachmentPath, "SAStestingdoc1.pdf");
            _seleniumHelper.UpLoadFile(chooseButtonId_1, "SAStestingdoc1.pdf");

            //Justification test area
            //e929d9f2-4d0b-4771-8643-00138336a072
            var justificationId = "e929d9f2-4d0b-4771-8643-00138336a072";
            var justificationVal = "Based on observations, up to 60% of catfish farmers make little or no profit, " +
                                    "and a reasonable majority even run at loss. In order to encourage and protect the " +
                                    "passion of both existing and incoming catfish farmers, this series is necessary as an eye opener " +
                                    "to what catfish farmers do wrong in their day to day running of their businesses";
            _seleniumHelper.SetTextAreaValue(justificationId, justificationVal);


            #endregion conference_part




            //Research and Creative Activity
            #region research_part

            // input destination ...
            var destination2Id = "7afa5795-b4fa-4fc5-a442-e5e0676e3c3d";
            var destination2Val = "Quebec City";
            _seleniumHelper.SetTextFieldValue(destination2Id, destination2Val);


            //Setting travel Date 1-departure 
            var travDateQ1Id = "8fbe3b14-4243-445c-8b5c-ca5ecd30dd34";
            var travDateQ1Val = new DateTime(2021, 7, 1);
            _seleniumHelper.SetDateValue(travDateQ1Id, travDateQ1Val);

            //Setting travel Date 2-return 
            var travDateQ2Id = "cc98603e-fed2-4c24-8fb5-74a19bc91b1f";
            var travDateQ2Val = new DateTime(2021, 7, 29);
            _seleniumHelper.SetDateValue(travDateQ2Id, travDateQ2Val);

            //air fare
            //c949503a-4fda-4fa3-aefc-252aff92ab1d
            var qairfareId = "c949503a-4fda-4fa3-aefc-252aff92ab1d";
            //var qairfareVal = "707.50";// unless round number, item flagged at sumbit stage.
            var qairfareVal = "707";
            _seleniumHelper.SetNumberValue(qairfareId, qairfareVal);

            //accomadations
            //bcea11d7-17a8-44dd-aeed-e221041f5714
            var qaccomadationId = "bcea11d7-17a8-44dd-aeed-e221041f5714";
            //var qaccomadationVal = "1057.50";// unless round number, item flagged at sumbit stage.
            var qaccomadationVal = "1057";
            _seleniumHelper.SetNumberValue(qaccomadationId, qaccomadationVal);

            //perdiem
            //7bb1de5e-9cef-4318-a0fc-3d04afdb73b7
            var qperdiemId = "7bb1de5e-9cef-4318-a0fc-3d04afdb73b7";
            var qperdiemVal = "87.00";
            _seleniumHelper.SetNumberValue(qperdiemId, qperdiemVal);

            //ground
            //7f30da14-70df-4762-a0ca-aa78e78e1df7
            var qgroundId = "7f30da14-70df-4762-a0ca-aa78e78e1df7";
            var qgroundVal = "57.00";
            _seleniumHelper.SetNumberValue(qgroundId, qgroundVal);

            //addition expenses
            //02908600-b5e1-49ff-9890-434d9a8bc587
            var qaddExpId = "02908600-b5e1-49ff-9890-434d9a8bc587";
            var qaddExpVal = "107.00";
            _seleniumHelper.SetNumberValue(qaddExpId, qaddExpVal);



            //addition expenses details
            //9d45c24e-e28b-487b-ae9c-857fb192b547

            var qaddExpDetailsId = "9d45c24e-e28b-487b-ae9c-857fb192b547";
            var qaddExpDetailsVal = "Catfish pate: $90.00\r\n" +
                                    "Catfish eggs: $10.00\r\n" +
                                    "Catfish whiskers: $7.00";
            _seleniumHelper.SetTextAreaValue(qaddExpDetailsId, qaddExpDetailsVal);


            //file upload - supporting documents - upload one pdf file


            var chooseButtonId_2 = "641e57e0-e878-414c-ab48-fc4de14739cd";
            _seleniumHelper.UpLoadFile(chooseButtonId_2, "SAStestingdoc2.pdf");



            // q justifiation
            //a181bebd-8212-410c-8672-985a98d70831
            var qjustificationId = "a181bebd-8212-410c-8672-985a98d70831";
            var qjustificationVal = "Over the next few weeks, I will be revealing major reasons " +
                                    "for poor returns on catfish farming investment. Below is an outline of these " +
                                    "reasons, details to be posted in subsequent research activities. " +
                                    "I am not really surprised with the popularity of this research.";
            _seleniumHelper.SetTextAreaValue(qjustificationId, qjustificationVal);

            //---

            #endregion research_part

            // Personnel and Services ------------------------------------------------------------------------

            #region personal_part
            //description
            //e503d887-7115-4405-80e5-2a0b6437c728
            var descriptionId = "e503d887-7115-4405-80e5-2a0b6437c728";
            var descriptionVal = "One student will be helping us, teaching time release required " +
                                    "possible contracted work and necessary equipment will be needed\r\n" +
                                    "Finishing later this year." +
                                    "Or maybe beginning of following year.";
            _seleniumHelper.SetTextAreaValue(descriptionId, descriptionVal);

            //----------------------Estimate for Hiring Students
            // click add button - assume first button
            //#Blocks_0__Item_Fields_54__addChildButton > input:nth-child(1)


            var addBtnSelecton = "#Blocks_0__Item_Fields_54__addChildButton > input:nth-child(1)";
            _seleniumHelper.ClickAddButton(addBtnSelecton);

            // enter infomation in new area
            // b4e2939e-3504-421a-8e37-a37df7756c3f
            var studentDetailsId = "b4e2939e-3504-421a-8e37-a37df7756c3f";
            var studentDetailsVal = "One Undergraduate student,\r\n" +
                                 "10 hours * 55 days * 10.00\r\n" +
                                 "July, august of this year.";
            _seleniumHelper.SetTextAreaValue(studentDetailsId, studentDetailsVal);

            // cost of student
            //d1e6d321-18c8-476d-bf41-9376bbbe331d
            var studentcostId = "d1e6d321-18c8-476d-bf41-9376bbbe331d";
            var studentcostVal = "5500.00";
            _seleniumHelper.SetNumberValue(studentcostId, studentcostVal);


            //-------------------Estimates for Contracted Services
            //#Blocks_0__Item_Fields_57__addChildButton > input

            var addContractedServicesSelecton = "#Blocks_0__Item_Fields_57__addChildButton > input";
            _seleniumHelper.ClickAddButton(addContractedServicesSelecton);

            // contract servisces description
            //ca9d88ba-9e18-4aa9-b21f-3c52b4b51111
            var contractDetailsId = "ca9d88ba-9e18-4aa9-b21f-3c52b4b51111";
            var contractDetailsVal = "Drain ponds, Fill up ponds " +
                                 "reroute river.\r\n" +
                                 "Restock catfish population.";
            _seleniumHelper.SetTextAreaValue(contractDetailsId, contractDetailsVal);


            // contract prices
            //45de8e34-9ced-4ae2-b2b8-e7135559bd01
            var contractedcostId = "45de8e34-9ced-4ae2-b2b8-e7135559bd01";
            var contractedcostVal = "4400.00";
            _seleniumHelper.SetNumberValue(contractedcostId, contractedcostVal);


            //file upload - supporting documents - upload one pdf file
            var chooseButtonId_3 = "b768fde3-3565-41fb-b0e3-59e63ced439e";
            // for now reuse file 2
            _seleniumHelper.UpLoadFile(chooseButtonId_3, "SAStestingdoc3.pdf");





            // p justifiation
            //74edda56-f5d5-46c5-b9af-249e6b9a928a
            var pjustificationId = "74edda56-f5d5-46c5-b9af-249e6b9a928a";
            var pjustificationVal = "Only ARC personnel can restock the ponds professionally " +
                                    "They keep the water clean and the fish safe " +
                                    "for any subsequent research activities." +
                                    "I am not going to let just anyone restock the fish.";
            _seleniumHelper.SetTextAreaValue(pjustificationId, pjustificationVal);

            // p equipment and materials
            //11705d20-5203-44ae-af0f-68459355e4dd
            var equipId = "11705d20-5203-44ae-af0f-68459355e4dd";
            var equipVal = "fishing rods - $500.00 " +
                           "gloves - $50.00 " +
                           "A good pair of gloves needed. " +
                           "to handle the catfish.";
            _seleniumHelper.SetTextAreaValue(equipId, equipVal);




            //-------------------Estimates for Contracted Services

            //add button for Estimates for Equipment and Material
            //#Blocks_0__Item_Fields_64__addChildButton > input

            var addEstimatesSelecton = "#Blocks_0__Item_Fields_64__addChildButton > input";
            _seleniumHelper.ClickAddButton(addEstimatesSelecton);



            // estimate description
            //fd24add9-d593-4120-aa0d-8341a964ddf7
            var estDetailsId = "fd24add9-d593-4120-aa0d-8341a964ddf7";
            var estDetailsVal = "time it takes to fix ponds " +
                                 "fix river.\r\n" +
                                 "fix catfish population.";
            _seleniumHelper.SetTextAreaValue(estDetailsId, estDetailsVal);


            // estimate prices
            //7a5467df-7326-48ae-99ff-58bf1751f3eb
            var estCostId = "7a5467df-7326-48ae-99ff-58bf1751f3eb";
            //var estCostVal = "550.55";  // unless round number, item flagged at sumbit stage.
            //To see how to resolve above  issue look at adding step attribute in Catfish.Core.Models.Contents.Fields.DecimalField.cshtml
            var estCostVal = "550.55";
            _seleniumHelper.SetNumberValue(estCostId, estCostVal);


            //file upload - supporting documents - upload one pdf file
            var chooseButtonId_4 = "8a868079-ac08-4dbf-a649-493edda47c96";
            // for now reuse file 2
            _seleniumHelper.UpLoadFile(chooseButtonId_4, "SAStestingdoc4.pdf");




            // 1st term release

            // click  add button  for course release in first term - assume first button
            //# Blocks_0__Item_Fields_68__addChildButton > input
            //#Blocks_0__Item_Fields_68__addChildButton > input:nth-child(1)


            var addBtnFirstTerm = "#Blocks_0__Item_Fields_68__addChildButton > input:nth-child(1)";
            _seleniumHelper.ClickAddButton(addBtnFirstTerm);


            //cousre name
            var course_1_Id = "a8c263b1-f098-4fb9-a266-1b4213cc8548";
            var course_1_Val = "Fish 201";
            _seleniumHelper.SetTextFieldValue(course_1_Id, course_1_Val);


            //Selecting "Yes" Option  for "Release Required?" RB
            // 46e8482f-f1d4-4d95-a8e8-907fac9e84f0 for set. 

            var relasese_1_Id = "46e8482f-f1d4-4d95-a8e8-907fac9e84f0";
            var relasese_1_OptId = "e96bfb6e-2bda-4e35-b12a-7debdf17154e";

            _seleniumHelper.SelectRadioOption(relasese_1_Id, relasese_1_OptId);

            // release amount.  f512e48c-6073-49ef-95b0-ec88576d01ef
            var releaseAmount_1_Id = "f512e48c-6073-49ef-95b0-ec88576d01ef";
            var releaseAmount_1_Val = "7700.15";
            _seleniumHelper.SetNumberValue(releaseAmount_1_Id, releaseAmount_1_Val);

            // release amount, 2nd term, justification

            // 2nd term release 940

            // click  add button  for course release in second term - assume Second button
            //#Blocks_0__Item_Fields_70__addChildButton > input:nth-child(1)


            var addBtnSecondTerm = "#Blocks_0__Item_Fields_70__addChildButton > input:nth-child(1)";
            _seleniumHelper.ClickAddButton(addBtnSecondTerm);


            //cousre name
            var course_2_Id = "dcda46dd-ef40-4aac-8d45-210ae2a14636";
            var course_2_Val = "Squid 409";
            _seleniumHelper.SetTextFieldValue(course_2_Id, course_2_Val);


            //Selecting "No" Option  for "Release Required?" RB
            // f3f58a5b-34ee-4a4c-82b7-9dbf5255de1f for set. 

            var relasese_2_Id = "f3f58a5b-34ee-4a4c-82b7-9dbf5255de1f";
            var relasese_2_OptId = "1cc57361-7fc8-4485-94c9-c0879f2bc33b";

            _seleniumHelper.SelectRadioOption(relasese_2_Id, relasese_2_OptId);

            // release amount.  9e97372e-76f8-45ba-bc2e-0cc2a69c13c1
            var releaseAmount_2_Id = "9e97372e-76f8-45ba-bc2e-0cc2a69c13c1";
            var releaseAmount_2_Val = "0.00";
            _seleniumHelper.SetNumberValue(releaseAmount_2_Id, releaseAmount_2_Val);



            // release amount, both possible  terms, justification
            //24625214-3d47-45ae-a7fa-017d60f1e608
            var releasePleaId = "24625214-3d47-45ae-a7fa-017d60f1e608";
            var releasePleaVal = "I need release time " +
                                 "because,\r\n" +
                                 "I'm a very busy person.";
            _seleniumHelper.SetTextAreaValue(releasePleaId, releasePleaVal);

            #endregion personal_part


            // ------------------------ checkout results overview funds ..
            #region summary_part

            //Conference Travel Amount Requested
            var ctar_id = "7c623048-859a-438e-9ace-3d11517cd38f";
            //var ctar_OnForm = _seleniumHelper.GetDecimalDisplayValue(ctar_id);
            //var ctar_required = airfareVal + accomadationVal + perdiemVal + confRegVal + groundVal + addExpVal;
            //Assert.AreEqual(ctar_OnForm, ctar_required, "Conference Travel Amount Requested value is not correctly indicated");


            var ctar_OnForm = _seleniumHelper.GetSummaryFieldValue(ctar_id);
            var ctar_required = decimal.Parse(airfareVal) + decimal.Parse(accomadationVal) + decimal.Parse(perdiemVal) +
                decimal.Parse(confRegVal) + decimal.Parse(groundVal) + decimal.Parse(addExpVal);
            Assert.AreEqual(decimal.Parse(ctar_OnForm), ctar_required, "Conference Travel Amount Requested value is not correctly indicated");



            //Research / Creativity Activity Travel Amount Requested 38bde356-dd5c-47b4-b0ba-97dd86cadf58
            var rcatr_id = "38bde356-dd5c-47b4-b0ba-97dd86cadf58";
            var rcatr_onForm = _seleniumHelper.GetSummaryFieldValue(rcatr_id);
            var rcatr_required = decimal.Parse(qairfareVal) + decimal.Parse(qaccomadationVal) + decimal.Parse(qperdiemVal) +
                 decimal.Parse(qgroundVal) + decimal.Parse(qaddExpVal);
            Assert.AreEqual(decimal.Parse(rcatr_onForm), rcatr_required, "Research - Creativity Activity Travel Amount Requested is not correctly indicated");

            //Support for Research and Creative Activity Equipment and Materials - 2ebe044b-ed64-4ceb-b7b2-0f37a5851c69
            var rcaem_id = "2ebe044b-ed64-4ceb-b7b2-0f37a5851c69";
            var rcaem_onForm = _seleniumHelper.GetSummaryFieldValue(rcaem_id);
            var rcaem_required = decimal.Parse(estCostVal);
            Assert.AreEqual(decimal.Parse(rcaem_onForm), rcaem_required, "Support for Research and Creative Activity Equipment and Materials is not correctly indicated");

            //Teaching release time  5e71a356-cd02-4bd3-8ab1-629c2b81c45a
            //var releaseAmount_2_Val = "0.0";
            var ttr_id = "5e71a356-cd02-4bd3-8ab1-629c2b81c45a";
            var ttr_onForm = _seleniumHelper.GetSummaryFieldValue(ttr_id);
            var ttr_required = decimal.Parse(releaseAmount_1_Val) + decimal.Parse(releaseAmount_2_Val);
            Assert.AreEqual(decimal.Parse(ttr_onForm), ttr_required, "Teaching release time  is not correctly indicated");

            //Personnel and Services
            var ps_id = "38ddbde7-e34a-432c-a4c8-18714115f6b2";
            var ps_onForm = _seleniumHelper.GetSummaryFieldValue(ps_id);
            var ps_required = decimal.Parse(studentcostVal) + decimal.Parse(contractedcostVal);
            Assert.AreEqual(decimal.Parse(ps_onForm), ps_required, "Personnel and Services costs  are not correctly indicated");

            //TOTAL ASK OF GRANT   7890332e-bc9b-4365-8402-299ac7645e69

            var total_id = "7890332e-bc9b-4365-8402-299ac7645e69";
            var total_onForm = _seleniumHelper.GetSummaryFieldValue(total_id);
            var total_required = ctar_required + rcatr_required + rcaem_required +
                 ttr_required + ps_required;
            Assert.AreEqual(decimal.Parse(total_onForm), total_required, "TOTAL ASK OF GRANT is not correctly indicated");


            #endregion summary_part

            //---------------------------------------------------------------------------------
            //Other and Previous Funding
            //   
            //Selecting "No" Option  for "previous SAS funding 5 years" 
            // no option var prevSAS5rbOptId = "46dc91e8-895a-41ef-be78-a1eff9fe6257";
            var prevSAS5Id = "471e3fbb-2034-447c-80ad-9d61f33829f9";
            var prevSAS5rbOptId = "8fd8c6b1-73a7-4cd7-91a0-a336753215eb";

            _seleniumHelper.SelectRadioOption(prevSAS5Id, prevSAS5rbOptId);

            // if yes add button
            var addprevSASBtnSelecton = "#Blocks_0__Item_Fields_81__addChildButton > input:nth-child(1)";
            _seleniumHelper.ClickAddButton(addprevSASBtnSelecton);

            // desc SAS previous 5 years

            //name of type of funding
            var pSASnameId = "a1cb9808-ab28-4d8f-aefe-a36df9ef40d6";
            var pSASnameVal = "SAS TypeOne";
            _seleniumHelper.SetTextFieldValue(pSASnameId, pSASnameVal);

            // value of award
            var awardValueId = "47c6f57e-e547-4e32-bc9a-7ff39a1e42bf";
            var awardValueVal = "4000.00";
            _seleniumHelper.SetNumberValue(awardValueId, awardValueVal);

            //Year of application
            var applYearId = "717e09ed-928d-4e28-9389-b90dad48da60";
            var applYearVal = "2018";
            _seleniumHelper.SetNumberValue(applYearId, applYearVal);

            // for Competition Applied To -- choose Spring
            var compSeasonId = "a08b7227-848c-4b16-b26d-c8cc94ffbbca";
            var compSeasonSpringOptId = "8890dff5-3cb3-4b5f-964e-75c673ff0a32";
            var compSeasonVal = "Spring";
            _seleniumHelper.SelectRadioOption(compSeasonId, compSeasonSpringOptId);

            //Setting Title of Project  
            var PATitleId = "384e010a-c819-4639-9cd9-7305053b91cd";
            var PATitleVal = "Fresh Water Catfish";
            _seleniumHelper.SetTextFieldValue(PATitleId, PATitleVal);

            //Setting description of previous Award
            var PAdescId = "f9284ed5-9811-4532-b4be-03b2c799f75b";
            var PAdescVal = "We looked at Fresh water Fish. " +
                "In particular Catfish";
            _seleniumHelper.SetTextAreaValue(PAdescId, PAdescVal);

            //--
            //Selecting "Yes" Option  for "previous SAS funding this project" 
            //8247d88d-01dc-42ea-b726-4f851dca888d,e2e5a69e-8f54-47cd-af1e-f425e75be726
            var prevthisSASId = "8247d88d-01dc-42ea-b726-4f851dca888d";
            var prevthisSASrbOptId = "e2e5a69e-8f54-47cd-af1e-f425e75be726";
            var prevthisSASVal = "Yes";
            _seleniumHelper.SelectRadioOption(prevthisSASId, prevthisSASrbOptId);

            //Setting how this relates
            var howRelatesId = "9f1175ff-9761-4a8a-b604-98d6743f9e8d";
            var howRelatesVal = "Study of similar aquatic life " +
                "like Catfish etc.";
            _seleniumHelper.SetTextAreaValue(howRelatesId, howRelatesVal);


            //Selecting "Yes" Option  for "previous SAS funding this project" 
            //1f3631c4-14e3-4c45-b268-203c3bd2d65c, 9327cd92-94d7-40df-a2ec-15b5fd693099
            var otherFundingId = "1f3631c4-14e3-4c45-b268-203c3bd2d65c";
            var otherFundingOptId = "9327cd92-94d7-40df-a2ec-15b5fd693099";
            var otherFundingOptVal = "Yes";
            _seleniumHelper.SelectRadioOption(otherFundingId, otherFundingOptId);

            // reason fork
            //Setting funding other sources 44b89364-87fa-4a3b-87af-cfb116d53214
            var otherFundingdescId = "44b89364-87fa-4a3b-87af-cfb116d53214";
            var otherFundingdescVal = "There is other Funding from: " +
                "Alberta";
            _seleniumHelper.SetTextAreaValue(otherFundingdescId, otherFundingdescVal);

            //Setting funding why not 2e4b3a6a-9031-45d8-aae5-ec0eeff3f1ef
            //           
            //           var reasonFunding2Id = "2e4b3a6a-9031-45d8-aae5-ec0eeff3f1ef";
            //          var reasonFunding2Val = "why not, Other Funding will be " +
            //             "applied.";
            //          _seleniumHelper.SetTextAreaValue(reasonFunding2Id, reasonFunding2Val);

            // other funding fab1e156-776c-4616-aaad-789419065ee6
            var otherSupport2Id = "fab1e156-776c-4616-aaad-789419065ee6";
            var otherSupport2Val = "Funds from catfish sales to local groceries";
            _seleniumHelper.SetTextAreaValue(otherSupport2Id, otherSupport2Val);


            //---------
            // Scholarly Publications b9cf535d-5445-4a94-8e35-c132afd26579
            var scholarlyPubId = "b9cf535d-5445-4a94-8e35-c132afd26579";
            var scholarlyPubVal = "Arcguy, Main. “Are Catfish good? Something about taste.” Catfish & Fresh Water Fish 21.4 (2016): 280-295\r\n" +
                                    "Arcguy, Main, Arcperson, Other. “Are Catfish edible? Maybe.” Catfish & Commonsense 10.12 (2008): 400-462.";
            _seleniumHelper.SetTextAreaValue(scholarlyPubId, scholarlyPubVal);


            // Collaborators if F of Arts
            //add button
            var addCollaboratorsBtnID = "#Blocks_0__Item_Fields_91__addChildButton > input:nth-child(1)";
            _seleniumHelper.ClickAddButton(addCollaboratorsBtnID);

            //Setting value of Collaborator name
            var CollaboratorsName_Id = "30065150-d3c5-4894-8f3e-12b2cff943a6";
            var CollaboratorsName_Val = "Anotherarc Guy";
            _seleniumHelper.SetTextFieldValue(CollaboratorsName_Id, CollaboratorsName_Val);

            //Setting value of Collaborator email
            var CollaboratorsEmail_Id = "9dba4c6e-12c1-4d83-8953-5b96ebf642e6";
            var CollaboratorsEmail_Val = "arcguyb@ualberta.ca";
            _seleniumHelper.SetTextFieldValue(CollaboratorsEmail_Id, CollaboratorsEmail_Val);

            // ---------------------submit section
            // --------------------------

            //Clicking on the submit button



            //var dataItemTemplateId = "button_wrapper_d5d50e2c-726e-4ba4-a512-635ad66ba8ea";
            //_seleniumHelper.ClickSubmitButton(dataItemTemplateId, "Submit");

            _seleniumHelper.ClickSimpleSubmitButton();

            //Clicking on the modal confirmation button
            // modalTemplateId can be found as the "data-template-id" as an attribute of the form - look at 
            // beginning of form after the id.
            var modalTemplateId = "f31602f0-1002-4e35-8c2c-90a03a8d3a93";
            _seleniumHelper.CkickModalSubmit(modalTemplateId, "btn btn-success");

            //_seleniumHelper.ClickModalSubmitButton();

            //Clicking on the link to view detail view
            _seleniumHelper.ClickOnALink("click on here.");



            //--------------------------validation section 2
            // --------------------------

            //  --- name  8e33c004-1864-46ec-b279-b5541b7adffe   applNameId applNameVal = "Arc Guy";
            var dispAppName = _seleniumHelper.GetSelectDisplayValue(applNameId);
            Assert.AreEqual(applNameVal, dispAppName, "application name is not correctly saved");

            //  --- email address ---    emailVal = "arcguya@ualberta.ca" emailId
            //var emailVal = "arcguya@ualberta.ca";
            var dispEmailVal = _seleniumHelper.GetSelectDisplayValue(emailId);
            Assert.AreEqual(emailVal, dispEmailVal, "Applicants email name is not correctly saved");


            //  --- dept  18133fb5-285d-47b3-8fa4-5a63cf37e723  deptdd1OptVal = "Arts Resources Centre";
            // var deptddOptId = "6a8689b8-3c17-4b31-8378-3584766f1940";

            var deptdd1OptVal = "Arts Resources Centre";
            var deptDispOpVal = _seleniumHelper.GetSelectDisplayValue(deptddId);
            Assert.AreEqual(deptdd1OptVal, deptDispOpVal, "Dept dropdown value is not correctly saved");

            //  --- rank  3cbfe358-8d4d-4f20-a84c-53ba4b450141   rankdd1OptVal = "Professor";
            var rankdd1OptVal = "Professor";
            var rankDispVal = _seleniumHelper.GetSelectDisplayValue(rankddId);
            Assert.AreEqual(rankdd1OptVal, rankDispVal, "Rank dropdown value is not correctly saved");


            //  --- not a chair   5ada22d2-2b7b-42aa-a449-79b04115af12    chairrbOptVal = "No";
            var chairrbOptVal = "No";
            var chairrbDispVal = _seleniumHelper.GetRadioDisplayValue(chairrbId);
            Assert.AreEqual(chairrbOptVal, chairrbDispVal, "RB1 value is not correctly saved");


            // chair name    8441d358-4296-45b8-8c29-8e31b94ff2c0
            var chairnameOptVal = "arcAdmin";
            var altChairOptID = "8441d358-4296-45b8-8c29-8e31b94ff2c0";
            var chairDispVal = _seleniumHelper.GetSelectDisplayValue(altChairOptID);
            Assert.AreEqual(chairnameOptVal, chairDispVal, "Chair name dropdown value is not correctly saved");

            // chair email  a8fdfd9d-9395-4485-b40f-d931016f5bf8
            var chairEmailvalue = "iwickram@ualberta.ca";
            var altChairEmailOptID = "a8fdfd9d-9395-4485-b40f-d931016f5bf8";
            var chairemailDispVal = _seleniumHelper.GetSelectDisplayValue(altChairEmailOptID);
            Assert.AreEqual(chairEmailvalue, chairemailDispVal, "Chair email  dropdown value is not correctly saved");

            //  --- project title 58b39ab2-fbf7-43df-ba76-59b7d04bc32f projNameVal = "Project Arc1";

            var projectNameDispVal = _seleniumHelper.GetTextFieldDisplayValue(projNameId);
            Assert.AreEqual(projNameVal, projectNameDispVal, "Project name value is not correctly saved");

            //  --- Project Description:  f89e70fa-14c2-495b-89cc-e98307f05bf5  projDescVal = "

            var projectDescDispVal = _seleniumHelper.GetTextFieldDisplayValue(projDescId);
            Assert.AreEqual(projDescVal, projectDescDispVal, "Project description value is not correctly saved");

            //subjects and ethics validation
            //  --- human animal subjects   bc5e74c6-3586-434e-aad4-e2bd92a2fabb  val = "No"; 1130+

            var involveSubjects = "Yes";
            var dispInvolveSubjects = _seleniumHelper.GetRadioDisplayValue(involveSubjectsrbId);
            Assert.AreEqual(involveSubjects, dispInvolveSubjects, "Radio BUtton for Involve Subjects  is not correctly saved");

            //  --- ethics approval     9a857bd0-369c-45e8-984e-ef1aefc9f368
            var approvalObtained = "Yes";
            var dispEthicsApprovalObtained = _seleniumHelper.GetRadioDisplayValue(approvalObtainedrbId);
            Assert.AreEqual(approvalObtained, dispEthicsApprovalObtained, "Radio BUtton for ethics approval is not correctly saved");

            //  --- ethics expriry date 3b762808-b1b5-4d0a-9ed8-3c54e5510be3 validaation
            var approvalExpiredateDispVal = _seleniumHelper.GetDateDisplayValue(approvalExpireDateId);
            Assert.AreEqual(approvalExpireDateVal.ToString("yyyy-MM-dd"), approvalExpiredateDispVal, "approval expiry date value is not correctly saved");


            //  --- name of conf 0eea8ef1-16d2-4ef3-be9c-65b947b329a9   confNameVal = "Catfish Annual";
            var confNameDispVal = _seleniumHelper.GetTextFieldDisplayValue(confNameId);
            Assert.AreEqual(confNameVal, confNameDispVal, "Conference Name is not correctly saved");


            //  --- conf start date 5d0d1ee2-16a7-4fef-84d6-f0bc8ba26ad0   confDate1Val = new DateTime(2021, 5, 5);

            var confDate1DispVal = _seleniumHelper.GetDateDisplayValue(confDate1Id);
            Assert.AreEqual(confDate1Val.ToString("yyyy-MM-dd"), confDate1DispVal, "conference start date is not correctly saved");

            // --- confr end date 2d5eab48-dd99-41ca-a8f1-cd29271d2779    confDate2Val = new DateTime(2021, 5, 25);

            var confDate2DispVal = _seleniumHelper.GetDateDisplayValue(confDate2Id);
            Assert.AreEqual(confDate2Val.ToString("yyyy-MM-dd"), confDate2DispVal, "conference start date is not correctly saved");

            // --- destination 22b3f496-c444-4ea2-8114-1d1feaff9813    destinationVal = "Winnipeg";

            var dispDestinationName = _seleniumHelper.GetSelectDisplayValue(destinationId);
            Assert.AreEqual(destinationVal, dispDestinationName, "Conference destination name is not correctly saved");


            //  --- deptarture date  5fc6c8b7-9c8f-48d0-ab22-d0baa3db20c1   travDate1Val = new DateTime(2021, 5, 1);

            var travDate1DispVal = _seleniumHelper.GetDateDisplayValue(travDate1Id);
            Assert.AreEqual(travDate1Val.ToString("yyyy-MM-dd"), travDate1DispVal, "traval start date is not correctly saved");


            //  --- return to date cb9ae842-12b2-416d-9042-d6ed7420ac4e  travDate2Val = new DateTime(2021, 5, 30);

            var travDate2DispVal = _seleniumHelper.GetDateDisplayValue(travDate2Id);
            Assert.AreEqual(travDate2Val.ToString("yyyy-MM-dd"), travDate2DispVal, "traval end date is not correctly saved");

            //  --- conf participation  64678688-16bd-41a9-8738-3ded02bc374f  participationVals = new string[] { "Invited Speaker", "Other" };

            var participationDispVals = _seleniumHelper.GetCheckboxDisplayValue(participationgroupId);
            Assert.AreEqual(participationVals, participationDispVals, "Participation by check box is not correctly saved");


            //  --- other participation 47c956cc-2c9a-4ace-a6bc-d374b8d83566

            var participationOtherDispVal = _seleniumHelper.GetSelectDisplayValue(participationOtherId);
            Assert.AreEqual(participationOtherVal, participationOtherDispVal, "participation other name is not correctly saved");

            //  --- airfare 8f938a04-2900-4ea6-a361-b1534c5199ec

            var dispAirfareVal = _seleniumHelper.GetDecimalDisplayValue(airfareId);
            Assert.AreEqual(airfareVal, dispAirfareVal, "Airfare value is not correctly saved");

            //  --- accomodation

            var accomadationDispVal = _seleniumHelper.GetDecimalDisplayValue(accomadationId);
            Assert.AreEqual(accomadationVal, accomadationDispVal, "Accomadation value is not correctly saved");

            //  --- per diem

            var perdiemDispVal = _seleniumHelper.GetDecimalDisplayValue(perdiemId);
            Assert.AreEqual(perdiemVal, perdiemDispVal, "Per diem value is not correctly saved");

            //  --- ground transport

            var groundDispVal = _seleniumHelper.GetDecimalDisplayValue(groundId);
            Assert.AreEqual(groundVal, groundDispVal, "Ground value is not correctly saved");


            //  --- conference registration

            var confRegDispVal = _seleniumHelper.GetDecimalDisplayValue(confRegId);
            Assert.AreEqual(confRegVal, confRegDispVal, "Conference Registration value is not correctly saved");


            //  --- addition expenses value

            var addExpDispVal = _seleniumHelper.GetDecimalDisplayValue(addExpId);
            Assert.AreEqual(addExpVal, addExpDispVal, "Additional expenses value is not correctly saved");


            //  --- addition expenses details   7b2c801b-25cb-48ed-9cb1-669d5067b360

            var addExpDetailsDispVal = _seleniumHelper.GetTextFieldDisplayValue(addExpDetailsId);
            Assert.AreEqual(addExpDetailsVal, addExpDetailsDispVal, "addition expenses details in text is not correctly saved");


            //  --- suporting documentation conferenced  7c0578e5-948f-4eed-a2d3-ba1baf11edfa
            //  _SAStestingdoc1.pdf

            var firstFileAttachment = "SAStestingdoc1.pdf";
            var firstFileAttachmentDisplayValue = _seleniumHelper.GetAttachmentFieldDisplayValue(chooseButtonId_1);
            Assert.AreEqual(firstFileAttachment, firstFileAttachmentDisplayValue, "Attachment is not correctly saved");


            //Justification test area
            //e929d9f2-4d0b-4771-8643-00138336a072
            var justDispVal = _seleniumHelper.GetTextFieldDisplayValue(justificationId);
            Assert.AreEqual(justDispVal, justificationVal, "Justification value is not correctly saved");



            //  --- Research and Creative Activity
            //  --- Another Travel Section

            //  --- RC destination Quebec   7afa5795-b4fa-4fc5-a442-e5e0676e3c3d
            var destination2DispVal = _seleniumHelper.GetTextFieldDisplayValue(destination2Id);
            Assert.AreEqual(destination2DispVal, destination2Val, "Second destination value is not correctly saved");

            //check travel second Date-departure 
            var SecondTravDate1DispVal = _seleniumHelper.GetDateDisplayValue(travDateQ1Id);
            Assert.AreEqual(travDateQ1Val.ToString("yyyy-MM-dd"), SecondTravDate1DispVal, "traval start date is not correctly saved");

            //check travel second Date-departure 
            var SecondTravDate2DispVal = _seleniumHelper.GetDateDisplayValue(travDateQ2Id);
            Assert.AreEqual(travDateQ2Val.ToString("yyyy-MM-dd"), SecondTravDate2DispVal, "traval start date is not correctly saved");


            //air fare  c949503a-4fda-4fa3-aefc-252aff92ab1d
            var qairfareDispVal = _seleniumHelper.GetDecimalDisplayValue(qairfareId);
            Assert.AreEqual(qairfareVal, qairfareDispVal, "second Air fare value is not correctly saved");

            //accomadations   bcea11d7-17a8-44dd-aeed-e221041f5714
            var qaccomadationDispal = _seleniumHelper.GetDecimalDisplayValue(qaccomadationId);
            Assert.AreEqual(qaccomadationVal, qaccomadationDispal, "second accomadation value is not correctly saved");

            //per diem   7bb1de5e-9cef-4318-a0fc-3d04afdb73b7
            var qperdiemDispVal = _seleniumHelper.GetDecimalDisplayValue(qperdiemId);
            Assert.AreEqual(qperdiemVal, qperdiemDispVal, "second per diem value is not correctly saved");


            //ground  7f30da14-70df-4762-a0ca-aa78e78e1df7
            var qgroundDispVal = _seleniumHelper.GetDecimalDisplayValue(qgroundId);
            Assert.AreEqual(qgroundVal, qgroundDispVal, "second ground travel  value is not correctly saved");



            //addition expenses  02908600-b5e1-49ff-9890-434d9a8bc587
            var qaddExpDispVal = _seleniumHelper.GetDecimalDisplayValue(qaddExpId);
            Assert.AreEqual(qaddExpVal, qaddExpDispVal, "second additional expenses value is not correctly saved");


            //addition expenses details    9d45c24e-e28b-487b-ae9c-857fb192b547
            var qaddExpDetailsDispVal = _seleniumHelper.GetTextFieldDisplayValue(qaddExpDetailsId);
            Assert.AreEqual(qaddExpDetailsVal, qaddExpDetailsDispVal, "Second addition expenses details in text is not correctly saved");


            //  --- suporting documentation second travel set  641e57e0-e878-414c-ab48-fc4de14739cd
            var secondFileAttachment = "SAStestingdoc2.pdf";
            var secondFileAttachmentDisplayValue = _seleniumHelper.GetAttachmentFieldDisplayValue(chooseButtonId_2);
            Assert.AreEqual(secondFileAttachment, secondFileAttachmentDisplayValue, "2nd Attachment is not correctly saved");

            //Second Justification test area    a181bebd-8212-410c-8672-985a98d70831
            var qjustificationDispVal = _seleniumHelper.GetTextFieldDisplayValue(qjustificationId);
            Assert.AreEqual(qjustificationDispVal, qjustificationVal, "Second justification value is not correctly saved");



            //  --- Personnel and Services
            //  --- Personnel and Services:  Estimate for Hiring Students

            //Hiring Students area    e503d887-7115-4405-80e5-2a0b6437c728
            var descriptionDispVal = _seleniumHelper.GetTextFieldDisplayValue(descriptionId);
            Assert.AreEqual(descriptionVal, descriptionDispVal, "Hiring Students Description  is not correctly saved");

            //Student Details area   b4e2939e-3504-421a-8e37-a37df7756c3f
            var studentDetailsDispVal = _seleniumHelper.GetTextFieldDisplayValue(studentDetailsId);
            Assert.AreEqual(studentDetailsVal, studentDetailsDispVal, "Hiring Student Details area is not correctly saved");

            // cost of student  d1e6d321-18c8-476d-bf41-9376bbbe331d
            var studentcostDispVal = _seleniumHelper.GetDecimalDisplayValue(studentcostId);
            Assert.AreEqual(studentcostVal, studentcostDispVal, "Cost of Student value is not correctly saved");


            //  --- Personnel and Services:  Estimates for Contracted Services
            // contract servisces description  ca9d88ba-9e18-4aa9-b21f-3c52b4b51111
            var contractDetailsDispVal = _seleniumHelper.GetTextFieldDisplayValue(contractDetailsId);
            Assert.AreEqual(contractDetailsVal, contractDetailsDispVal, "Contract servisces description Description  is not correctly saved");

            // contract prices   45de8e34-9ced-4ae2-b2b8-e7135559bd01
            var contractedcostDispVal = _seleniumHelper.GetDecimalDisplayValue(contractedcostId);
            Assert.AreEqual(contractedcostVal, contractedcostDispVal, "contract prices are not correctly saved");

            //file upload - supporting documents - SAStestingdoc3.pdf
            var thirdFileAttachment = "SAStestingdoc3.pdf";
            var thirdFileAttachmentDisplayValue = _seleniumHelper.GetAttachmentFieldDisplayValue(chooseButtonId_3);
            Assert.AreEqual(thirdFileAttachment, thirdFileAttachmentDisplayValue, "3rd Attachment is not correctly saved");

            // personal area justifiation  74edda56-f5d5-46c5-b9af-249e6b9a928a
            var pjustificationDispVal = _seleniumHelper.GetTextFieldDisplayValue(pjustificationId);
            Assert.AreEqual(pjustificationDispVal, pjustificationVal, "Justification in personal area - the details are not correctly saved");


            //  --- Personnel and Services:  Equipment and Materials  11705d20-5203-44ae-af0f-68459355e4dd
            var equipDispVal = _seleniumHelper.GetTextFieldDisplayValue(equipId);
            Assert.AreEqual(equipDispVal, equipVal, "Equipment and materials - the details are not correctly saved");


            //  --- Personnel and Services:  Estimates for Contracted Services
            // estimate description    fd24add9-d593-4120-aa0d-8341a964ddf7
            var estDetailsDispVal = _seleniumHelper.GetTextFieldDisplayValue(estDetailsId);
            Assert.AreEqual(estDetailsDispVal, estDetailsVal, "estimate details description- the details are not correctly saved");

            // estimate prices   7a5467df-7326-48ae-99ff-58bf1751f3eb
            var estCostDispVal = _seleniumHelper.GetDecimalDisplayValue(estCostId);
            Assert.AreEqual(estCostVal, estCostDispVal, "Estimate of contract services value is not correctly saved");

            //file upload - supporting documents - SAStestingdoc4.pdf
            var forthFileAttachment = "SAStestingdoc4.pdf";
            var forthFileAttachmentDisplayValue = _seleniumHelper.GetAttachmentFieldDisplayValue(chooseButtonId_4);
            Assert.AreEqual(forthFileAttachment, forthFileAttachmentDisplayValue, "4th Attachment is not correctly saved");


            //  --- Personnel and Services:  Teaching Release
            // 1st term release
            //cousre name    a8c263b1-f098-4fb9-a266-1b4213cc8548
            var course_1Disp_Val = _seleniumHelper.GetTextFieldDisplayValue(course_1_Id);
            Assert.AreEqual(course_1Disp_Val, course_1_Val, "First term course name is not correctly saved");

            //"Yes" Option  for "Release Required?" RB   46e8482f-f1d4-4d95-a8e8-907fac9e84f0  
            var rbReleaseVal = "yes";
            var rbReleaseDispVal = _seleniumHelper.GetRadioDisplayValue(relasese_1_Id);
            Assert.AreEqual(rbReleaseVal, rbReleaseDispVal, "Release yes or no radio button is not correctly saved");

            // release amount.  f512e48c-6073-49ef-95b0-ec88576d01ef
            var releaseAmount_1_DispVal = _seleniumHelper.GetDecimalDisplayValue(releaseAmount_1_Id);
            Assert.AreEqual(releaseAmount_1_Val, releaseAmount_1_DispVal, "Release time cost  is not correctly saved");


            // 2nd term release validation 1370
            //cousre name    "dcda46dd-ef40-4aac-8d45-210ae2a14636";
            var course_2Disp_Val = _seleniumHelper.GetTextFieldDisplayValue(course_2_Id);
            Assert.AreEqual(course_2Disp_Val, course_2_Val, "second term course name is not correctly saved");

            //"Yes" Option  for "Release Required?" RB    
            var rbRelease2Val = "No";
            var rbRelease2DispVal = _seleniumHelper.GetRadioDisplayValue(relasese_2_Id);
            Assert.AreEqual(rbRelease2Val, rbRelease2DispVal, "Release yes or no radio button is not correctly saved");

            // release amount.  
            var releaseAmount_2_DispVal = _seleniumHelper.GetDecimalDisplayValue(releaseAmount_2_Id);
            Assert.AreEqual(releaseAmount_2_Val, releaseAmount_2_DispVal, "Release time cost 2nd term is not correctly saved");
            // release   justification   24625214-3d47-45ae-a7fa-017d60f1e608

            var releasePleaDispVal = _seleniumHelper.GetTextFieldDisplayValue(releasePleaId);
            Assert.AreEqual(releasePleaDispVal, releasePleaVal, "Release time reasons - the details are not correctly saved");


            //totals
            //Conference Travel Amount Requested
            var ctar_dispVal = _seleniumHelper.GetDecimalDisplayValue(ctar_id);
            Assert.AreEqual(ctar_OnForm, ctar_dispVal, "Total Conference Travel Amount Requested value is not correctly saved");

            //Research / Creativity Activity Travel Amount Requested
            var rcatr_dispVal = _seleniumHelper.GetDecimalDisplayValue(rcatr_id);
            Assert.AreEqual(rcatr_onForm, rcatr_dispVal, "Total Research-Creativity Activity Travel Amount Requested value is not correctly saved");

            //Support for Research and Creative Activity Equipment and Materials 
            var rcaem_dispVal = _seleniumHelper.GetDecimalDisplayValue(rcaem_id);
            Assert.AreEqual(rcaem_onForm, rcaem_dispVal, "Support for Research and Creative Activity Equipment and Materials  Amount Requested value is not correctly saved");

            //Teaching release time 
            var ttr_dispVal = _seleniumHelper.GetDecimalDisplayValue(ttr_id);
            Assert.AreEqual(ttr_onForm, ttr_dispVal, "Teaching release time  Amount Requested value is not correctly saved");

            //Personnel and Services
            var ps_dispVal = _seleniumHelper.GetDecimalDisplayValue(ps_id);
            Assert.AreEqual(ps_onForm, ps_dispVal, "Personnel and Services amount, requested value is not correctly saved");

            //TOTAL ASK OF GRANT  
            var total_dispVal = _seleniumHelper.GetDecimalDisplayValue(total_id);
            Assert.AreEqual(total_onForm, total_dispVal, "Total amount requested value is not correctly saved");

            // after overview form part 

            //funding 1520

            //Selecting "yes" Option  for "previous SAS funding 5 years"  
            var rbprevSAS5Val = "Yes";
            var rbprevSAS5dispVal = _seleniumHelper.GetRadioDisplayValue(prevSAS5Id);
            Assert.AreEqual(rbprevSAS5Val, rbprevSAS5dispVal, "previous SAS funding, past 5 years, yes or no radio button is not correctly saved");

            //Validating - name of type of funding
            var pSASname_displayed = _seleniumHelper.GetTextFieldDisplayValue(pSASnameId);
            Assert.AreEqual(pSASnameVal, pSASname_displayed, "name of funding type   value is not correctly saved");

            //Validating _value of award
            var awardValueDispVal = _seleniumHelper.GetDecimalDisplayValue(awardValueId);
            Assert.AreEqual(awardValueVal, awardValueDispVal, "award value is not correctly saved");

            //Validating year of award
            var applYearDispVal = _seleniumHelper.GetIntegerDisplayValue(applYearId);
            Assert.AreEqual(applYearVal, applYearDispVal, "application year of award is not correctly saved");

            //Selecting "Spring" for cometition applied to:
            var compSeasonDispVal = _seleniumHelper.GetRadioDisplayValue(compSeasonId);
            Assert.AreEqual(compSeasonVal, compSeasonDispVal, "Competition season value is not correctly saved");

            //Validating Title of prevproject
            var PATitleDispVal = _seleniumHelper.GetTextFieldDisplayValue(PATitleId);
            Assert.AreEqual(PATitleVal, PATitleDispVal, "title of previous project award for: value is not correctly saved");

            //Validating description for prev project
            var PAdescDispVal = _seleniumHelper.GetTextFieldDisplayValue(PAdescId);
            Assert.AreEqual(PAdescVal, PAdescDispVal, "Description of previous project award for: value is not correctly saved");

            //validatin "yes" Option  for "previous SAS funding this project"  
            //var rbprevthisSASVal = "Yes";
            var prevthisSASdispVal = _seleniumHelper.GetRadioDisplayValue(prevthisSASId);
            Assert.AreEqual(prevthisSASVal, prevthisSASdispVal, "option previous SAS funding, this project, yes or no radio button is not correctly saved");

            //Validating how this relates
            var howRelatesDispVal = _seleniumHelper.GetTextFieldDisplayValue(howRelatesId);
            Assert.AreEqual(howRelatesVal, howRelatesDispVal, "How this relates value is not correctly saved");

            //validating "yes" Option  for "previous SAS funding this project"  
            var otherFundingOptDispVal = _seleniumHelper.GetRadioDisplayValue(otherFundingId);
            Assert.AreEqual(otherFundingOptVal, otherFundingOptDispVal, "option previous SAS funding, this project, yes or no radio button is not correctly saved");

            // if yes , other option funding
            //Validating funding other sources 9f1175ff-9761-4a8a-b604-98d6743f9e8d
            var otherFundingDispVal = _seleniumHelper.GetTextFieldDisplayValue(otherFundingdescId);
            Assert.AreEqual(otherFundingdescVal, otherFundingDispVal, "other funding sources value is not correctly saved");

            // Validating other funding fab1e156-776c-4616-aaad-789419065ee6
            var otherSupport2DispVal = _seleniumHelper.GetTextFieldDisplayValue(otherSupport2Id);
            Assert.AreEqual(otherSupport2DispVal, otherSupport2Val, "Other support details are not correctly saved");


            // end funding asserts

            // Validating Scholarly Publications b9cf535d-5445-4a94-8e35-c132afd26579
            var scholarlyPubDispVal = _seleniumHelper.GetTextFieldDisplayValue(scholarlyPubId);
            Assert.AreEqual(scholarlyPubVal, scholarlyPubDispVal, "Scholarly Publications area is not correctly saved");

            // Validating  Collaborator name
            var CollaboratorsNameDisp_Val = _seleniumHelper.GetTextFieldDisplayValue(CollaboratorsName_Id);
            Assert.AreEqual(CollaboratorsNameDisp_Val, CollaboratorsName_Val, "Collaborator name is not correctly saved");

            // Validating  Collaborator email
            var CollaboratorsEmailDisp_Val = _seleniumHelper.GetTextFieldDisplayValueAlt(CollaboratorsEmail_Id);
            Assert.AreEqual(CollaboratorsEmailDisp_Val, CollaboratorsEmail_Val, "Collaborator email is not correctly saved");

            //End of Validation tests

            _seleniumHelper.Driver.Close();
        }
        [Test]
        public void RequiredIfTest()
        {
            RefreshDatabase();
            _seleniumHelper.LoginLocal();

            //Navigating to the test page
            _seleniumHelper.GoToUrl("visible-if-/-required-if-fields");

            //==========================================================
            //====================Dropdown test=====================
            //==========================================================

            //Selecting Option1 for DD1
            var ddId = "942601c6-1cb7-4508-a5e6-577dcb475a4b";
            var ddOpt1Id = "21142581-074f-4638-96df-d550df60f8cc";
            var ddOpt1Val = "Option 1";
            _seleniumHelper.SelectDropdownOption(ddId, ddOpt1Id);
            //check opt1 has required fields
            bool ddOption1Required = _seleniumHelper.IsTextFieldRequired("ee794c7d-5ccd-4ff5-bf21-40cbd09519cd");
            Assert.AreEqual(false, ddOption1Required, "DD1 option 1, required if function not working properly");

            //Selecting Option2 for DD1
            var ddOpt2Id = "48aa27ca-b8aa-47fc-a8c4-22addebd3e0f";
            var ddOpt2Val = "Option 2";
            _seleniumHelper.SelectDropdownOption(ddId, ddOpt2Id);
            //check opt1 has required fields
            bool ddOption2Required = _seleniumHelper.IsTextFieldRequired("ee794c7d-5ccd-4ff5-bf21-40cbd09519cd");
            Assert.AreEqual(true, ddOption2Required, "DD1 option 2 required if function not working properly");

            //Selecting Option3 for DD1
            var ddOpt3Id = "05c5dab2-7e56-411d-9af0-03598d5ab5e4";
            var ddOpt3Val = "Option 3";
            _seleniumHelper.SelectDropdownOption(ddId, ddOpt3Id);
            //check opt1 has required fields
            bool ddOption3Required = _seleniumHelper.IsTextFieldRequired("ee794c7d-5ccd-4ff5-bf21-40cbd09519cd");
            Assert.AreEqual(false, ddOption3Required, "DD1 option 3 required if function not working properly");

            //Selecting Option4 for DD1
            var ddOpt4Id = "43b71beb-b69f-416a-9e68-f46027cf726e";
            var ddOpt4Val = "Option 4";
            _seleniumHelper.SelectDropdownOption(ddId, ddOpt4Id);
            //check opt1 has required fields
            bool ddOption4Required = _seleniumHelper.IsTextFieldRequired("ee794c7d-5ccd-4ff5-bf21-40cbd09519cd");
            Assert.AreEqual(false, ddOption4Required, "DD1 option 4 required if function not working properly");

            //==========================================================
            //====================Radio button test=====================
            //==========================================================

            //Selecting Option 1 for RB1
            var rbId = "19c12f23-6ec9-403e-8b28-d56e1bd70a7e";
            var rbOpt1Id = "6f773564-e198-4b6e-92cc-8f179ccfbf7d";
            var rbOpt1Val = "Option 1";
            _seleniumHelper.SelectRadioOption(rbId, rbOpt1Id);
            bool rbOption1Required = _seleniumHelper.IsTextFieldRequired("21c05a53-2a03-451f-a228-e67baa020fc5");
            Assert.AreEqual(true, rbOption1Required, "RB1 option 1, required if function not working properly");

            //Selecting Option 2 for RB1
            var rbOpt2Id = "f335e0cd-a2b0-40d8-9ad9-b93e8cae11b4";
            var rbOpt2Val = "Option 2";
            _seleniumHelper.SelectRadioOption(rbId, rbOpt2Id);
            bool rbOption2Required = _seleniumHelper.IsTextFieldRequired("21c05a53-2a03-451f-a228-e67baa020fc5");
            Assert.AreEqual(true, rbOption2Required, "RB1 option 2, required if function not working properly");

            //Selecting Option 3 for RB1
            var rbOpt3Id = "a1014373-2936-4b27-a263-1195cd159b2e";
            var rbOpt3Val = "Option 3";
            _seleniumHelper.SelectRadioOption(rbId, rbOpt3Id);
            bool rbOption3Required = _seleniumHelper.IsTextFieldRequired("21c05a53-2a03-451f-a228-e67baa020fc5");
            Assert.AreEqual(false, rbOption3Required, "RB1 option 3, required if function not working properly");

            //Selecting Option 4 for RB1
            var rbOpt4Id = "e9683b37-6a72-4d13-8381-9c8c9bab8fdf";
            var rbOpt4Val = "Option 4";
            _seleniumHelper.SelectRadioOption(rbId, rbOpt4Id);
            bool rbOption4Required = _seleniumHelper.IsTextFieldRequired("21c05a53-2a03-451f-a228-e67baa020fc5");
            Assert.AreEqual(false, rbOption4Required, "RB1 option 4, required if function not working properly");

            //==========================================================
            //========================Checkbox test=====================
            //==========================================================

            //Selecting Options for the CB1 and  testing for required on text box 3
            var chkId = "412211a2-ca2a-4332-a92a-9aa931523285";
            // text box 3 ID
            var chkTextFieldId = "59a6aa64-b51b-466d-a157-1a051f6ac6ed";

            //Selecting Option 1 for the CB1
            var chkOptCombination1Ids = new string[] { "2da93223-7ca2-403c-855f-4ffa0fdaf6e3" };
            var chkOptCombination1Vals = new string[] { "Option 1" };
            _seleniumHelper.SelectCheckOptions(chkId, chkOptCombination1Ids);
            bool cbCombination1Required = _seleniumHelper.IsTextFieldRequired(chkTextFieldId);
            Assert.AreEqual(false, cbCombination1Required, "CB1 option 1, required if function not working properly");
            _seleniumHelper.SelectCheckOptions(chkId, chkOptCombination1Ids);

            //Selecting Option 2 for the CB1
            var chkOptCombination2Ids = new string[] { "6f591878-4c95-49b2-b5c4-b5517c25d293" };
            var chkOptCombination2Vals = new string[] { "Option 2" };
            _seleniumHelper.SelectCheckOptions(chkId, chkOptCombination2Ids);
            bool cbCombination2Required = _seleniumHelper.IsTextFieldRequired(chkTextFieldId);
            Assert.AreEqual(false, cbCombination2Required, "CB1 option 2, required if function not working properly");
            _seleniumHelper.SelectCheckOptions(chkId, chkOptCombination2Ids);

            //Selecting Option 3 for the CB1
            var chkOptCombination3Ids = new string[] { "eff3d260-f988-43a9-9a64-a35bc8639613" };
            var chkOptCombination3Vals = new string[] { "Option 3" };
            _seleniumHelper.SelectCheckOptions(chkId, chkOptCombination3Ids);
            bool cbCombination3Required = _seleniumHelper.IsTextFieldRequired(chkTextFieldId);
            Assert.AreEqual(false, cbCombination3Required, "CB1 option 3, required if function not working properly");
            _seleniumHelper.SelectCheckOptions(chkId, chkOptCombination3Ids);

            //Selecting Option 4 for the CB1
            var chkOptCombination4Ids = new string[] { "bd0fdd18-ce6f-407d-8fe4-0ba71b334daf" };
            var chkOptCombination4Vals = new string[] { "Option 4" };
            _seleniumHelper.SelectCheckOptions(chkId, chkOptCombination4Ids);
            bool cbCombination4Required = _seleniumHelper.IsTextFieldRequired(chkTextFieldId);
            Assert.AreEqual(false, cbCombination4Required, "CB1 option 4, required if function not working properly");
            _seleniumHelper.SelectCheckOptions(chkId, chkOptCombination4Ids);

            //Selecting Option 1 & option 2 for the CB1
            //  --- Test required  for when selecting        Opt1 & Opt2         of CB1
            var chkOptCombination5Ids = new string[] { "2da93223-7ca2-403c-855f-4ffa0fdaf6e3", "6f591878-4c95-49b2-b5c4-b5517c25d293" };
            var chkOptCombination5Vals = new string[] { "Option 1", "Option 2" };
            _seleniumHelper.SelectCheckOptions(chkId, chkOptCombination5Ids);
            bool cbCombination5Required = _seleniumHelper.IsTextFieldRequired(chkTextFieldId);
            Assert.AreEqual(false, cbCombination5Required, "CB1 option 1 & option 2, required if function not working properly");
            _seleniumHelper.SelectCheckOptions(chkId, chkOptCombination5Ids);


            // assign data-model-id's to strings
            var opt_1_dmi = "2da93223-7ca2-403c-855f-4ffa0fdaf6e3";
            var opt_2_dmi = "6f591878-4c95-49b2-b5c4-b5517c25d293";
            var opt_3_dmi = "eff3d260-f988-43a9-9a64-a35bc8639613";
            var opt_4_dmi = "bd0fdd18-ce6f-407d-8fe4-0ba71b334daf";



            //  --- Test required  for when selecting  Opt1 & Opt3  of CB1 - should result in Text box3 requirement
            var cbComb_6_Ids = new string[] { opt_1_dmi, opt_3_dmi };
            var cbComb_6_Vals = new string[] { "Option 1", "Option 3" };
            _seleniumHelper.SelectCheckOptions(chkId, cbComb_6_Ids);
            bool cbComb_6_Required = _seleniumHelper.IsTextFieldRequired(chkTextFieldId);
            Assert.AreEqual(true, cbComb_6_Required, "CB1 option 1 & option 3, required if function not working properly");
            _seleniumHelper.SelectCheckOptions(chkId, cbComb_6_Ids);

            //  --- Test required  for when selecting  Opt1 & Opt4     of CB1
            var cbComb_7_Ids = new string[] { opt_1_dmi, opt_4_dmi };
            var cbComb_7_Vals = new string[] { "Option 1", "Option 4" };
            _seleniumHelper.SelectCheckOptions(chkId, cbComb_7_Ids);
            bool cbComb_7_Required = _seleniumHelper.IsTextFieldRequired(chkTextFieldId);
            Assert.AreEqual(false, cbComb_7_Required, "CB1 option 1 & option 4, required if function not working properly");
            _seleniumHelper.SelectCheckOptions(chkId, cbComb_7_Ids);

            //  --- Test required  for when selecting  Opt2 & Opt3     of CB1
            var cbComb_8_Ids = new string[] { opt_2_dmi, opt_3_dmi };
            var cbComb_8_Vals = new string[] { "Option 2", "Option 3" };
            _seleniumHelper.SelectCheckOptions(chkId, cbComb_8_Ids);
            bool cbComb_8_Required = _seleniumHelper.IsTextFieldRequired(chkTextFieldId);
            Assert.AreEqual(false, cbComb_8_Required, "CB1 option 2 & option 3, required if function not working properly");
            _seleniumHelper.SelectCheckOptions(chkId, cbComb_8_Ids);


            //  --- Test required  for when selecting  Opt2 & Opt4     of CB1
            var cbComb_9_Ids = new string[] { opt_2_dmi, opt_4_dmi };
            var cbComb_9_Vals = new string[] { "Option 2", "Option 4" };
            _seleniumHelper.SelectCheckOptions(chkId, cbComb_9_Ids);
            bool cbComb_9_Required = _seleniumHelper.IsTextFieldRequired(chkTextFieldId);
            Assert.AreEqual(false, cbComb_9_Required, "CB1 option 2 & option 4, required if function not working properly");
            _seleniumHelper.SelectCheckOptions(chkId, cbComb_9_Ids);

            //  --- Test required  for when selecting  Opt3 & Opt4      of CB1
            var cbComb_10_Ids = new string[] { opt_3_dmi, opt_4_dmi };
            var cbComb_10_Vals = new string[] { "Option 3", "Option 4" };
            _seleniumHelper.SelectCheckOptions(chkId, cbComb_10_Ids);
            bool cbComb_10_Required = _seleniumHelper.IsTextFieldRequired(chkTextFieldId);
            Assert.AreEqual(false, cbComb_10_Required, "CB1 option 3 & option 4, required if function not working properly");
            _seleniumHelper.SelectCheckOptions(chkId, cbComb_10_Ids);

            //  --- Test required  for when selecting  Opt1 & Opt2 & Opt3   of CB1 - should result in Text box3 requirement
            var cbComb_11_Ids = new string[] { opt_1_dmi, opt_2_dmi, opt_3_dmi };
            var cbComb_11_Vals = new string[] { "Option 1", "Option 2", "Option 3" };
            _seleniumHelper.SelectCheckOptions(chkId, cbComb_11_Ids);
            bool cbComb_11_Required = _seleniumHelper.IsTextFieldRequired(chkTextFieldId);
            Assert.AreEqual(true, cbComb_11_Required, "CB1 option 1, option 2 & option 3, required if function not working properly");
            _seleniumHelper.SelectCheckOptions(chkId, cbComb_11_Ids);

            //  --- Test required  for when selecting  Opt1 & Opt2 & Opt4   of CB1
            var cbComb_12_Ids = new string[] { opt_1_dmi, opt_2_dmi, opt_4_dmi };
            var cbComb_12_Vals = new string[] { "Option 1", "Option 2", "Option 4" };
            _seleniumHelper.SelectCheckOptions(chkId, cbComb_12_Ids);
            bool cbComb_12_Required = _seleniumHelper.IsTextFieldRequired(chkTextFieldId);
            Assert.AreEqual(false, cbComb_12_Required, "CB1 option 1, option 2 & option 4, required if function not working properly");
            _seleniumHelper.SelectCheckOptions(chkId, cbComb_12_Ids);

            //  --- Test required  for when selecting  Opt1 & Opt3 & Opt4   of CB1 - should result in Text box3 requirement
            var cbComb_13_Ids = new string[] { opt_1_dmi, opt_3_dmi, opt_4_dmi };
            var cbComb_13_Vals = new string[] { "Option 1", "Option 3", "Option 4" };
            _seleniumHelper.SelectCheckOptions(chkId, cbComb_13_Ids);
            bool cbComb_13_Required = _seleniumHelper.IsTextFieldRequired(chkTextFieldId);
            Assert.AreEqual(true, cbComb_13_Required, "CB1 option 1, option 3 & option 4, required if function not working properly");
            _seleniumHelper.SelectCheckOptions(chkId, cbComb_13_Ids);

            //  --- Test required  for when selecting  Opt2 & Opt3 & Opt4   of CB1
            var cbComb_14_Ids = new string[] { opt_2_dmi, opt_3_dmi, opt_4_dmi };
            var cbComb_14_Vals = new string[] { "Option 2", "Option 3", "Option 4" };
            _seleniumHelper.SelectCheckOptions(chkId, cbComb_14_Ids);
            bool cbComb_14_Required = _seleniumHelper.IsTextFieldRequired(chkTextFieldId);
            Assert.AreEqual(false, cbComb_14_Required, "CB1 option 2, option 3 & option 4, required if function not working properly");
            _seleniumHelper.SelectCheckOptions(chkId, cbComb_14_Ids);

            //  --- Test required  for when selecting  Opt1 & Opt2 & Opt3 & Opt4  of CB1 - should result in Text box3 requirement
            var cbComb_15_Ids = new string[] { opt_1_dmi, opt_2_dmi, opt_3_dmi, opt_4_dmi };
            var cbComb_15_Vals = new string[] { "Option 1", "Option 2", "Option 3", "Option 4" };
            _seleniumHelper.SelectCheckOptions(chkId, cbComb_15_Ids);
            bool cbComb_15_Required = _seleniumHelper.IsTextFieldRequired(chkTextFieldId);
            Assert.AreEqual(true, cbComb_15_Required, "CB1 option 1, option 2, option 3 & option 4, required if function not working properly");
            _seleniumHelper.SelectCheckOptions(chkId, cbComb_15_Ids);

            _seleniumHelper.Driver.Close();
        }

        [Test]
        public void CompositeFieldTest()
        {
            RefreshDatabase();
            _seleniumHelper.LoginLocal();

            //Navigating to the test page
            _seleniumHelper.GoToUrl("composite-field");


            var child1TextFieldValue = "Test User1";
            _seleniumHelper.SetCompositeTextFieldValue(child1TextFieldValue, 0, 0);

            var dateVal = new DateTime(2021, 3, 25);
            _seleniumHelper.SetCompositeDateValue(dateVal, 0, 1);

            var child1EmailFieldValue = "testuser1@example.com";
            _seleniumHelper.SetCompositeTextFieldValue(child1EmailFieldValue, 0, 2);

            var child1TextAreaValue = @"Identity configuration in ASP.NET can be quite confusing, especially if you want to customize setup properties.
                                      When you use a code - first approach using Entity Framework, you have full control over your user identity options.
                                      However when developers deal with bigger projects, they typically prefer to use a table - first approach in which they 
                                      create the database, then consume the information in the API, and lastly shape it in a way that it makes sense on the front end.
                                       
                                      So, configuring identity might work best with a mixed approach.";
            _seleniumHelper.SetCompositeTextFieldValue(child1TextAreaValue, 0, 3);

            var rbOptId = "558ca21b-3062-4773-9966-2cdace171c21";
            var rbOptVal = "Citizen";
            _seleniumHelper.SelectCompositeRadioOption(rbOptId, 0, 4);

            //Selecting English and Spanish for the CB1
            var chkOptIds = new string[] { "036afb53-f284-4671-9534-feeecdd52394", "915c10c2-4e54-49ec-9857-92a37e1e749e" };
            var chkOptVals = new string[] { "English", "Spanish" };
            _seleniumHelper.SelectCompositeCheckOptions(chkOptIds, 0, 5);

            //Clicking on the add button
            var dataItemTemplateId = "69abec30-69e0-4f60-891a-5df4d7ba1993";
            _seleniumHelper.ClickOnButtonType(dataItemTemplateId, "Add");


            //====================================================
            //================Fill User 2 Details=================
            //====================================================

            var child2TextFieldValue = "Test User2";
            _seleniumHelper.SetCompositeTextFieldValue(child2TextFieldValue, 1, 0);

            var date2Val = new DateTime(2021, 4, 21);
            _seleniumHelper.SetCompositeDateValue(date2Val, 1, 1);

            var child2EmailFieldValue = "testuser2@example.com";
            _seleniumHelper.SetCompositeTextFieldValue(child2EmailFieldValue, 1, 2);

            var child2TextAreaValue = @"However when developers deal with bigger projects, they typically prefer to use a table - first approach in which they 
                                      create the database, then consume the information in the API, and lastly shape it in a way that it makes sense on the front end.
                                      ";
            _seleniumHelper.SetCompositeTextFieldValue(child2TextAreaValue, 1, 3);

            var rbOpt2Id = "816a822b-71df-4fc9-ac5f-cd071cffc222";
            var rbOpt2Val = "Permenant Resident";
            _seleniumHelper.SelectCompositeRadioOption(rbOpt2Id, 1, 4);

            //Selecting English and Spanish for the CB1
            var chkOpt2Id = "036afb53-f284-4671-9534-feeecdd52394";
            var chkOpt2Val = "French";
            _seleniumHelper.SelectCompositeCheckOption(chkOpt2Id, 0, 5);

            //Clicking on the add button
            _seleniumHelper.ClickOnButtonType(dataItemTemplateId, "Add");

            _seleniumHelper.ClickOnRowDeleteButton(2, "fa fa-trash");

            //Clicking on sumbit button
            _seleniumHelper.ClickOnButtonType(dataItemTemplateId, "Submit");

            //Clicking on the modal confirmation button
            _seleniumHelper.CkickModalSubmit(dataItemTemplateId, "btn btn-success");

            //Clicking on the link to view detail view
            _seleniumHelper.ClickOnALink("click on here");
        }
        [Test]
        public void safetyinspectionTest()
        {
            RefreshDatabase();
            _seleniumHelper.LoginLocal();

            //Navigating to the test page
            _seleniumHelper.GoToUrl("safety-inspection");

            var dataItemTemplateId = "5e376611-96b7-4595-a7fd-6648d52ca01b";


            var inspectDate_ID = "426828e6-738e-49d7-93b1-acd6bae99129";


            var building_ID = "d5332411-df2d-49c6-9be5-42768acde063";
            var building_DD_HUBopt = "6e11cb16-f6da-423d-8c78-cb44285b58df";
            var building_Val = "HUB";

            var InspectorID = "3572b75a-0608-4f71-a5e4-89d4e9192c2e";
            var InspectorVal = "Archie Guy";
            var RoomID = "e1e73892-30e7-47b4-b5f9-5ffb40f4c5b8";
            var RoomVal = "Mall Area";

            var NumApprovedID = "c0b6b51f-fb50-4efe-8617-7c435b8a8a0e";
            var NumApprovedVal = "20";

            var NumActualID = "3956de37-7152-4c48-806c-708749d07519";
            var NumActualVal = "17";

            // Physical Distancing

            var distanceRB_ID = "73bff1f3-dd10-43ff-a585-34d0933b0f7a";
            var distanceRB_yes = "8705f7d8-595c-48bc-aa07-864159f2f708";
            var distanceRB_Val = "Yes";

            var masksRB_ID = "58ead174-151f-42f1-a272-83e9509a5e68";
            var masksRB_no = "5b0d260e-fe53-434c-8e96-b152a597a361";
            var masksRB_Val = "No";


            var distanceNote_ID = "05f3e9e8-348e-423d-901b-74f24acce752";
            var distanceNote_Val = "Check everything our, make sure its ok.";
            var distanceAssign_ID = "7691974a-49b4-43d5-9973-27e4887cc3ac";
            var distanceAssign_Val = "Jack Guy";

            // Personal Hygiene

            var sanitizeRB_ID = "7e7c3041-7272-430d-b837-e3d5da141497";
            var sanitizeRB_no = "8b716973-4acf-4ebb-b67f-5c67d4b42bae";
            var sanitizeRB_Val = "No";

            var cleanSinkRB_ID = "3acdfb1d-6218-452b-af92-98e789fe5acd";
            var cleanSinkRB_yes = "51ee6128-8459-4f9f-9cbc-0ba66be9a129";
            var cleanSinkRB_Val = "Yes";

            var soapRB_ID = "3ab368ac-ca68-4cf0-890b-acd3f92a01c5";
            var soapRB_no = "a2010105-e522-4e10-84aa-e18ebc0ddb05";
            var soapRB_Val = "No";

            var HygieneNote_ID = "a4043ac8-a03d-4d68-a0bb-682a3fa3f0c9";
            var HygieneNote_Val = "Need to buy more soap - strong stuff.";
            var HygieneAssign_ID = "5a635985-e1b5-4aec-a9c0-e7660babb38b";
            var HygieneAssign_Val = "Jane Gal";

            // Housekeeping

            var cleanRB_ID = "c9f48c7e-43b6-4e22-8546-8dba0470dcd2";
            var cleanRB_yes = "1eb083c3-251f-4d2f-a704-9d7f8b6a6f21";
            var cleanRB_Val = "Yes";

            var regularRB_ID = "079dde30-9586-4936-aafc-67f7d74c8e8f";
            var regularRB_yes = "f671c8b1-2604-458d-9692-6cdb78d1535d";
            var regularRB_Val = "Yes";

            var suppliesRB_ID = "b4bed4f9-fdac-4da4-a314-64e60815bb80";
            var suppliesRB_no = "fe4734b6-ae2c-4cb8-9ae1-d76cad3a0e39";
            var suppliesRB_Val = "No";

            var walkwaysRB_ID = "d0dacbed-224d-403c-a8c4-aa5b178c8aed";
            var walkwaysRB_yes = "a457c015-6035-45a5-bd81-f3a9f1e0f274";
            var walkwaysRB_Val = "Yes";

            var housekeepNote_ID = "3ce3dd6a-b44a-46af-8d46-1c34d06b473c";
            var housekeepNote_Val = "Need to stock up on more cleaning supplies.";
            var housekeepAssign_ID = "9ae193b3-0fb6-4122-a62e-2b0e67e95947";
            var housekeepAssign_Val = "Jan guyguy";

            // Training
            var covidTrainingRB_ID = "5f311b09-11be-4774-b0ef-7795fa5f644d";
            var covidTraining_yes = "0a50bd07-f451-4b30-8e8d-e5c16708f187";
            var covidTrainingRB_Val = "Yes";

            var emplTrainingRB_ID = "5e7ca9fb-c57e-4f02-b1c4-f47ec5dcc062";
            var emplTrainingRB_yes = "5a77ca10-cf34-49b6-94a7-cdf7cdb4f064";
            var emplTrainingRB_Val = "Yes";

            var trainingNote_ID = "75300a7e-707d-4ed6-8d3e-e89091da7432";
            var trainingNote_Val = "The cool guy needs more training.";
            var trainingAssign_ID = "69f3e648-173f-4875-b08d-92e6ddb3d255";
            var trainingAssign_Val = "Jacov guyguy";

            //Eyewash Stations

            var eyewashStationRB_ID = "baeb6cb3-1f53-40b6-a0aa-7905a5414b12";
            var eyewashStationRB_yes = "44c57c67-a7a4-4599-b5bc-9d3b887cbef2";
            var eyewashStationRB_Val = "Yes";

            var eyewashInfo_ID = "c9158253-3a61-49b1-8f38-27c176efaa26";
            var eyewashInfo_Val = "Hub 222, 2012, 2000.\r\nHub 444, 2014, 2008.";

            // Other
            var flush3minRB_ID = "0f86bfea-eb77-4115-ad17-69a3666cbb79";
            var flush3minRB_yes = "499bed9a-d72e-40d9-870d-68b05fdbc4b2";
            var flush3minRB_Val = "Yes";

            var ppeOnRB_ID = "0d58a5a5-6bf4-4adb-becc-185f912669a4";
            var ppeOnRB_yes = "ebceabf3-800d-4256-85a5-2535f16aba31";
            var ppeOnRB_Val = "Yes";

            var otherNote_ID = "50257822-17b0-4fa7-9ba5-c7ec19e72b67";
            var otherNote_Val = "Need to make other notes.";
            var otherAssign_ID = "41ee7227-c6a9-4afa-91d9-32029bc1d9ba";
            var otherAssign_Val = "Jupiter galgal";

            //---------------------------------------------------------------------------------

            //set inspection date
            var inspectDate_Val = new DateTime(2021, 3, 25);
            _seleniumHelper.SetDateValue(inspectDate_ID, inspectDate_Val);

            //set  building
            _seleniumHelper.SelectDropdownOption(building_ID, building_DD_HUBopt);
            //set inspector tf
            _seleniumHelper.SetTextFieldValue(InspectorID, InspectorVal);
            //set room tf
            _seleniumHelper.SetTextFieldValue(RoomID, RoomVal);
            // set number approved
            _seleniumHelper.SetNumberValue(NumApprovedID, NumApprovedVal);
            // actual number
            _seleniumHelper.SetNumberValue(NumActualID, NumActualVal);


            ////Physical Distancing
            // set distancing RB
            _seleniumHelper.SelectRadioOption(distanceRB_ID, distanceRB_yes);
            // set masks RB
            _seleniumHelper.SelectRadioOption(masksRB_ID, masksRB_no);
            // set Physical Distancing note
            _seleniumHelper.SetTextAreaValue(distanceNote_ID, distanceNote_Val);
            // set Physical Distancing assignee
            _seleniumHelper.SetTextFieldValue(distanceAssign_ID, distanceAssign_Val);

            ////Personal Hygiene
            // set sanatize RB
            _seleniumHelper.SelectRadioOption(sanitizeRB_ID, sanitizeRB_no);
            // set clesn sinks RB
            _seleniumHelper.SelectRadioOption(cleanSinkRB_ID, cleanSinkRB_yes);
            // set soap RB
            _seleniumHelper.SelectRadioOption(soapRB_ID, soapRB_no);


            // set Personal Hygiene note
            _seleniumHelper.SetTextAreaValue(HygieneNote_ID, HygieneNote_Val);

            // set Personal Hygiene assignee
            _seleniumHelper.SetTextFieldValue(HygieneAssign_ID, HygieneAssign_Val);

            ////House keeping
            // set clean RB
            _seleniumHelper.SelectRadioOption(cleanRB_ID, cleanRB_yes);
            // set regular cleaning RB
            _seleniumHelper.SelectRadioOption(regularRB_ID, regularRB_yes);
            // set supplies RB
            _seleniumHelper.SelectRadioOption(suppliesRB_ID, suppliesRB_no);
            //set walkways RB
            _seleniumHelper.SelectRadioOption(walkwaysRB_ID, walkwaysRB_yes);

            // set Housekeeping note
            _seleniumHelper.SetTextAreaValue(housekeepNote_ID, housekeepNote_Val);
            // set Housekeeping assignee
            _seleniumHelper.SetTextFieldValue(housekeepAssign_ID, housekeepAssign_Val);

            ////Training
            // set covid training RB
            _seleniumHelper.SelectRadioOption(covidTrainingRB_ID, covidTraining_yes);
            // set employee training RB
            _seleniumHelper.SelectRadioOption(emplTrainingRB_ID, emplTrainingRB_yes);

            // set Training note
            _seleniumHelper.SetTextAreaValue(trainingNote_ID, trainingNote_Val);
            // set Training assignee
            _seleniumHelper.SetTextFieldValue(trainingAssign_ID, trainingAssign_Val);


            ////Eye Stations

            // set eye station RB
            _seleniumHelper.SelectRadioOption(eyewashStationRB_ID, eyewashStationRB_yes);
            // set eye station info
            _seleniumHelper.SetTextAreaValue(eyewashInfo_ID, eyewashInfo_Val);
            ////other

            // set flush 3 min RB
            _seleniumHelper.SelectRadioOption(flush3minRB_ID, flush3minRB_yes);
            // set PPE RB
            _seleniumHelper.SelectRadioOption(ppeOnRB_ID, ppeOnRB_yes);

            // set other note
            _seleniumHelper.SetTextAreaValue(otherNote_ID, otherNote_Val);

            // set other assignee
            _seleniumHelper.SetTextFieldValue(otherAssign_ID, otherAssign_Val);


            //Clicking on sumbit button
            _seleniumHelper.ClickOnButtonType(dataItemTemplateId, "Submit");

            //Clicking on the modal confirmation button
            _seleniumHelper.CkickModalSubmit(dataItemTemplateId, "btn btn-success");

            //Clicking on the link to view detail view
            _seleniumHelper.ClickOnALink("click on here");

            //---------------------------------------------------------------------


            //Validating inspection date
            var inspectDate_disp = _seleniumHelper.GetDateDisplayValue(inspectDate_ID);
            Assert.AreEqual(inspectDate_Val.ToString("yyyy-MM-dd"), inspectDate_disp, "inspection date value is not correctly saved");

            //Validating building test field
            var building_disp = _seleniumHelper.GetSelectDisplayValue(building_ID);
            Assert.AreEqual(building_Val, building_disp, "building value is not correctly saved");

            //validate inspector tf
            var Inspector_disp = _seleniumHelper.GetTextFieldDisplayValue(InspectorID);
            Assert.AreEqual(InspectorVal, Inspector_disp, "inspector value is not correctly saved");

            //validate room tf
            var Roomdisp = _seleniumHelper.GetTextFieldDisplayValue(RoomID);
            Assert.AreEqual(RoomVal, Roomdisp, "room value is not correctly saved");

            // validate number approved
            var NumApproveddisp = _seleniumHelper.GetIntegerDisplayValue(NumApprovedID);
            Assert.AreEqual(NumApprovedVal, NumApproveddisp, "number approved value is not correctly saved");

            // actual number
            var NumActualDisp = _seleniumHelper.GetIntegerDisplayValue(NumActualID);
            Assert.AreEqual(NumActualVal, NumActualDisp, "actual number value is not correctly saved");


            ////Physical Distancing
            // validate distancing RB
            var distanceRB_disp = _seleniumHelper.GetRadioDisplayValue(distanceRB_ID);
            Assert.AreEqual(distanceRB_Val, distanceRB_disp, "distanced between occupants value is not correctly saved");

            // validate masks RB
            var masksRB_disp = _seleniumHelper.GetRadioDisplayValue(masksRB_ID);
            Assert.AreEqual(masksRB_Val, masksRB_disp, "wearing masks value is not correctly saved");

            // validate Physical Distancing note
            var distanceNote_disp = _seleniumHelper.GetTextFieldDisplayValue(distanceNote_ID);
            Assert.AreEqual(distanceNote_Val, distanceNote_disp, "Physical Distancing note  is not correctly saved");

            // validate Physical Distancing assignee
            var distanceAssign_disp = _seleniumHelper.GetTextFieldDisplayValue(distanceAssign_ID);
            Assert.AreEqual(distanceAssign_Val, distanceAssign_disp, "Physical Distancing assignee is not correctly saved");


            ////Personal Hygiene
            // validate sanatize RB
            var sanitizeRB_disp = _seleniumHelper.GetRadioDisplayValue(sanitizeRB_ID);
            Assert.AreEqual(sanitizeRB_Val, sanitizeRB_disp, "hand sanitize value is not correctly saved");

            // validate clean sinks RB
            var cleanSinkRB_disp = _seleniumHelper.GetRadioDisplayValue(cleanSinkRB_ID);
            Assert.AreEqual(cleanSinkRB_Val, cleanSinkRB_disp, "clean sink value is not correctly saved");

            // validate soap RB
            var soapRB_disp = _seleniumHelper.GetRadioDisplayValue(soapRB_ID);
            Assert.AreEqual(soapRB_Val, soapRB_disp, "soap supply value is not correctly saved");


            // validate Personal Hygiene note
            var HygieneNote_disp = _seleniumHelper.GetTextFieldDisplayValue(HygieneNote_ID);
            Assert.AreEqual(HygieneNote_Val, HygieneNote_disp, "Personal Hygiene note is not correctly saved");

            // validate Personal Hygiene assignee
            var HygieneAssign_disp = _seleniumHelper.GetTextFieldDisplayValue(HygieneAssign_ID);
            Assert.AreEqual(HygieneAssign_Val, HygieneAssign_disp, "Personal Hygiene assigneeis not correctly saved");

            ////House keeping
            // validate clean RB
            var cleanRB_disp = _seleniumHelper.GetRadioDisplayValue(cleanRB_ID);
            Assert.AreEqual(cleanRB_Val, cleanRB_disp, "clean RB value is not correctly saved");

            // validate regular cleaning RB
            var regularRB_disp = _seleniumHelper.GetRadioDisplayValue(regularRB_ID);
            Assert.AreEqual(regularRB_Val, regularRB_disp, "regular cleaning RB value is not correctly saved");

            // validate supplies RB
            var suppliesRB_disp = _seleniumHelper.GetRadioDisplayValue(suppliesRB_ID);
            Assert.AreEqual(suppliesRB_Val, suppliesRB_disp, "supplies RB value is not correctly saved");

            //validate walkways RB
            var walkwaysRB_disp = _seleniumHelper.GetRadioDisplayValue(walkwaysRB_ID);
            Assert.AreEqual(walkwaysRB_Val, walkwaysRB_disp, "walkways RB value is not correctly saved");

            // validate Housekeeping note
            var housekeepNote_disp = _seleniumHelper.GetTextFieldDisplayValue(housekeepNote_ID);
            Assert.AreEqual(housekeepNote_Val, housekeepNote_disp, "Housekeeping note is not correctly saved");

            // validate Housekeeping assignee
            var housekeepAssign_disp = _seleniumHelper.GetTextFieldDisplayValue(housekeepAssign_ID);
            Assert.AreEqual(housekeepAssign_Val, housekeepAssign_disp, "Housekeeping assignee is not correctly saved");

            ////Training
            // validate covid training RB
            var covidTrainingRB_disp = _seleniumHelper.GetRadioDisplayValue(covidTrainingRB_ID);
            Assert.AreEqual(covidTrainingRB_Val, covidTrainingRB_disp, "covid training RB is not correctly saved");

            // validate employee training RB
            var emplTrainingRB_disp = _seleniumHelper.GetRadioDisplayValue(emplTrainingRB_ID);
            Assert.AreEqual(emplTrainingRB_Val, emplTrainingRB_disp, "employee training RB is not correctly saved");

            // validate Training note
            var trainingNote_disp = _seleniumHelper.GetTextFieldDisplayValue(trainingNote_ID);
            Assert.AreEqual(trainingNote_Val, trainingNote_disp, "Training noteis not correctly saved");

            // validate Training assignee
            var trainingAssign_disp = _seleniumHelper.GetTextFieldDisplayValue(trainingAssign_ID);
            Assert.AreEqual(trainingAssign_Val, trainingAssign_disp, "Training assignee is not correctly saved");

            ////Eye Stations

            // validate eye station RB
            var eyewashStationRB_disp = _seleniumHelper.GetRadioDisplayValue(eyewashStationRB_ID);
            Assert.AreEqual(eyewashStationRB_Val, eyewashStationRB_disp, "eye station RB is not correctly saved");

            // validate eye station info
            var eyewashInfo_disp = _seleniumHelper.GetTextFieldDisplayValue(eyewashInfo_ID);
            Assert.AreEqual(eyewashInfo_Val, eyewashInfo_disp, "eye station info is not correctly saved");

            ////other

            // validate flush 3 min RB
            var flush3minRB_disp = _seleniumHelper.GetRadioDisplayValue(flush3minRB_ID);
            Assert.AreEqual(flush3minRB_Val, flush3minRB_disp, "flush 3 min RB value is not correctly saved");
            // validate PPE RB
            var ppeOnRB_val = _seleniumHelper.GetRadioDisplayValue(ppeOnRB_ID);
            Assert.AreEqual(ppeOnRB_Val, ppeOnRB_val, "PPE RB value is not correctly saved");

            // validate other note
            var otherNote_disp = _seleniumHelper.GetTextFieldDisplayValue(otherNote_ID);
            Assert.AreEqual(otherNote_Val, otherNote_disp, "other note is not correctly saved");

            // validate other assignee
            var otherAssign_disp = _seleniumHelper.GetTextFieldDisplayValue(otherAssign_ID);
            Assert.AreEqual(otherAssign_Val, otherAssign_disp, "other assigneeis not correctly saved");

            //close
            _seleniumHelper.Driver.Close();
        }

    }

}
