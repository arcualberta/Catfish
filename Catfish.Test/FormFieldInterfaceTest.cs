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

        //[Test]
        //public void RefreshData()
        //{
        //    RefreshDatabase();
        //}

        //public void LoginTest()
        //{
        //    IWebElement element = driver.FindElement(By.Name("pageGroup"));
        //    Assert.NotNull(element);
        //}

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
            var chkOptVals = new string[] { "Option 3", "Option 4"};
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
            var chairrbOptId = "fc6bf9a5-b0c7-44de-8cff-6f2eee8b3f91";
            //var chairrbOptVal = "No";
            _seleniumHelper.SelectRadioOption(chairrbId, chairrbOptId);

            //Setting project name
            var projNameId = "58b39ab2-fbf7-43df-ba76-59b7d04bc32f";
            var projNameVal = "Project Arc1";
            _seleniumHelper.SetTextFieldValue(projNameId, projNameVal);

            //Setting project description
            var projDescId = "f89e70fa-14c2-495b-89cc-e98307f05bf5";
            var projDescVal = "Officials with Alberta parks and environment sampled, or went fishing in the pond where the catfish was discovered and caught thirty catfish, from three generations — or catfish that have lived in the lake for about three years.";
            _seleniumHelper.SetTextAreaValue(projDescId, projDescVal);


           

            //Option  for involving subjects RB
            var involveSubjectsrbId = "bc5e74c6-3586-434e-aad4-e2bd92a2fabb";
            var involveSubjectsnoOptId = "b26717d3-c8e2-49d1-bb27-e98542ce748b";
            //var chairrbOptVal = "No";
            _seleniumHelper.SelectRadioOption(involveSubjectsrbId, involveSubjectsnoOptId);

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


            //Setting airfare value 700.50
            var airfareId = "8f938a04-2900-4ea6-a361-b1534c5199ec";
            var airfareVal = "700.50";
            _seleniumHelper.SetNumberValue(airfareId, airfareVal);


            // accomadation
            var accomadationId = "8eb97013-f4a9-4dad-98c4-7903bd07344e";
            var accomadationVal = "1050.50";
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
            var groundVal = "50.00";
            _seleniumHelper.SetNumberValue(groundId, groundVal);

            //Additional expenses
            var addExpId = "32538c9b-09b2-409b-8593-9f59ed04668a";
            var addExpVal = "100.00";
            _seleniumHelper.SetNumberValue(addExpId, addExpVal);

            //Details Additional expenses
            var addExpDetailsId = "7b2c801b-25cb-48ed-9cb1-669d5067b360";
            var addExpDetailsVal = "Catfish food: $90.00 \r\n" +
                                    "dental floss: $0.99 \r\n" +
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
                                    "to what catfish farmers do wrong in their day to day running of their businesses ";
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
            var qairfareVal = "707.50";
            _seleniumHelper.SetNumberValue(qairfareId, qairfareVal);

            //accomadations
            //bcea11d7-17a8-44dd-aeed-e221041f5714
            var qaccomadationId = "bcea11d7-17a8-44dd-aeed-e221041f5714";
            var qaccomadationVal = "1057.50";
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
            var qaddExpDetailsVal = "Catfish pate: $90.00 \r\n" +
                                    "Catfish eggs: $10.00 \r\n" +
                                    "Catfish whiskers: $7.00";
            _seleniumHelper.SetTextAreaValue(qaddExpDetailsId, qaddExpDetailsVal);


            //file upload - supporting documents - upload one pdf file
            

            var chooseButtonId_2 =  "641e57e0-e878-414c-ab48-fc4de14739cd";
            _seleniumHelper.UpLoadFile(chooseButtonId_2, "SAStestingdoc2.pdf");



            // q justifiation
            //a181bebd-8212-410c-8672-985a98d70831
            var qjustificationId = "a181bebd-8212-410c-8672-985a98d70831";
            var qjustificationVal = "Over the next few weeks, I will be revealing major reasons  " +
                                    "for poor returns on catfish farming investment. Below is an outline of these  " +
                                    "reasons, details to be posted in subsequent research activities. " +
                                    "I am not really surprised with the popularity of this research.  ";
            _seleniumHelper.SetTextAreaValue(qjustificationId, qjustificationVal);

            //---

            #endregion research_part

            // Personnel and Services ------------------------------------------------------------------------

            #region personal_part
            //description
            //e503d887-7115-4405-80e5-2a0b6437c728
            var descriptionId = "e503d887-7115-4405-80e5-2a0b6437c728";
            var descriptionVal = "One student will be helping us, teaching time release required,   " +
                                    "possible contracted work and necessary equipment  will be needed. \r\n" +
                                    "Finishing later this year. " +
                                    "Or maybe beginning of following year.  ";
            _seleniumHelper.SetTextAreaValue(descriptionId, descriptionVal);

            //----------------------Estimate for Hiring Students
            // click add button - assume first button
            //#Blocks_0__Item_Fields_54__addChildButton > input:nth-child(1)


            var addBtnSelecton = "#Blocks_0__Item_Fields_54__addChildButton > input:nth-child(1)";
            _seleniumHelper.ClickAddButton(addBtnSelecton);

            // enter infomation in new area
            // b4e2939e-3504-421a-8e37-a37df7756c3f
            var studentDetailsId = "b4e2939e-3504-421a-8e37-a37df7756c3f";
            var studentDetailsVal = "One Undergraduate student, \r\n  " +
                                 "10 hours * 55 days * 10.00 \r\n" +
                                 "July, august of this year.  ";
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
            var contractDetailsVal = "Drain ponds, Fill up ponds  " +
                                 "reroute river. \r\n" +
                                 "Restock catfish population.";
            _seleniumHelper.SetTextAreaValue(contractDetailsId, contractDetailsVal);


            // contract prices
            //45de8e34-9ced-4ae2-b2b8-e7135559bd01
            var contractedcostId = "45de8e34-9ced-4ae2-b2b8-e7135559bd01";
            var contractedcostVal = "4400.00";
            _seleniumHelper.SetNumberValue(contractedcostId, contractedcostVal);


            //file upload - supporting documents - upload one pdf file
            var chooseButtonId_3 ="b768fde3-3565-41fb-b0e3-59e63ced439e";
            // for now reuse file 2
            _seleniumHelper.UpLoadFile(chooseButtonId_3, "SAStestingdoc3.pdf");





            // p justifiation
            //74edda56-f5d5-46c5-b9af-249e6b9a928a
            var pjustificationId = "74edda56-f5d5-46c5-b9af-249e6b9a928a";
            var pjustificationVal = "Only ARC personnel can restock the ponds professionally  " +
                                    "They keep the water clean and the fish safe   " +
                                    "for any  subsequent research activities. " +
                                    "I am not going to let just anyone restock the fish.  ";
            _seleniumHelper.SetTextAreaValue(pjustificationId, pjustificationVal);

            // p equipment and materials
            //11705d20-5203-44ae-af0f-68459355e4dd
            var equipId = "11705d20-5203-44ae-af0f-68459355e4dd";
            var equipVal = "fishing rods - $500.00  " +
                           "gloves - $50.00   " +
                           "A good pair of gloves needed. " +
                           "to handle the  catfish.  ";
            _seleniumHelper.SetTextAreaValue(equipId, equipVal);


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

            // relaese amount.  f512e48c-6073-49ef-95b0-ec88576d01ef
            var releaseAmount_1_Id = "f512e48c-6073-49ef-95b0-ec88576d01ef";
            var releaseAmount_1_Val = "7700.00";
            _seleniumHelper.SetNumberValue(releaseAmount_1_Id, releaseAmount_1_Val);




            #endregion personal_part




            // ---------------------submit section
            // --------------------------

            //Clicking on the submit button
            var dataItemTemplateId = "button_wrapper_d5d50e2c-726e-4ba4-a512-635ad66ba8ea";
            _seleniumHelper.ClickSaveButton(dataItemTemplateId, "Save");

            //Clicking on the link to view detail view
            //_seleniumHelper.ClickOnALink("click on here");

            //Clicking on the link to view detail view
            //_seleniumHelper.ClickOnALink("Edit");



            //--------------------------validation section
            // --------------------------
        }
    }
}
