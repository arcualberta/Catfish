using Catfish.Core.Authorization.Requirements;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Expressions;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Contents.Workflow;
using Catfish.Core.Services;
using Catfish.Test.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catfish.UnitTests
{
    public class BusinessContinuityPlanFormBuilderTests
    {
        private protected AppDbContext _db;
        private protected TestHelper _testHelper;
        string[] YesNoOptionsText = new string[] { "Yes", "No" };

        [SetUp]
        public void Setup()
        {
            _testHelper = new TestHelper();
            _db = _testHelper.Db;
        }

       
        [Test]
        public void BCP_BusinessProfileWSTest()
        {
            string lang = "en";
            string templateName = "BCP Department/Business Unit Profile Worksheet Template";

            IWorkflowService ws = _testHelper.WorkflowService;
            AppDbContext db = _testHelper.Db;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            ItemTemplate template = db.ItemTemplates
                .Where(et => et.TemplateName == templateName)
                .FirstOrDefault();

            if (template == null)
            {
                template = new ItemTemplate();
                db.ItemTemplates.Add(template);
            }
            else
            {
                ItemTemplate t = new ItemTemplate();
                t.Id = template.Id;
                template.Data = t.Data;
                template.Initialize(false);
            }
            template.TemplateName = templateName;
            template.Name.SetContent(templateName);

            //ws.SetModel(template);

            //Get the Workflow object using the workflow service
            Workflow workflow = template.Workflow;

            //Defining email templates
           
            //Defininig the inspection form
            DataItem bcpForm = template.GetDataItem("BCP Department/Business Unit Profile Worksheet Form", true, lang);
            bcpForm.IsRoot = true;
            bcpForm.SetDescription("This template is designed for BCP Department/Business Unit Profile Worksheet Form", lang);

            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h1", "Department / Business Unit Profile", lang);
            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("div", "<b>Instructions:</b> Fill in relevantinformation about the department or unit", lang, "alert alert-info");
           
            bcpForm.CreateField<TextField>("Department/Unit Name", lang,true);
            bcpForm.CreateField<TextField>("Department/Unit Director", lang, true);

            var address = bcpForm.CreateField<TextArea>("Street Address", lang, true);
            address.Cols = 30;
            address.Rows = 5;

            var maddress = bcpForm.CreateField<TextArea>("Mailing Address <b><i>(if different)</i></b>", lang, false);
            maddress.Cols = 30;
            maddress.Rows = 5;

            bcpForm.CreateField<TextField>("Person to contact to discuss emergency planning", lang, true);

            bcpForm.CreateField<IntegerField>("Number of staff", lang, true);
            var staff = bcpForm.CreateField<TextArea>("Staff who are part of department Emergency Team", lang, true);
            staff.Cols = 30;
            staff.Rows = 5;

            string[] optionTexts = GetYesNoList();
            var ePlan = bcpForm.CreateField<RadioField>("Do you have an emergency plan?", lang, optionTexts, true);
            ePlan.FieldValueCssClass = "radio-inline";

            var latVisit = bcpForm.CreateField<DateField>("Last time it was revised: ", lang);
                 latVisit.VisibilityCondition.AppendLogicalExpression(ePlan, ComputationExpression.eRelational.EQUAL, ePlan.Options[1]);
                 latVisit.RequiredCondition.AppendLogicalExpression(ePlan, ComputationExpression.eRelational.EQUAL, ePlan.Options[1]);
           

            var cPlan = bcpForm.CreateField<RadioField>("Do you have a business continuity plan?", lang, optionTexts, true);
                cPlan.FieldValueCssClass = "radio-inline";
            var lVisit = bcpForm.CreateField<DateField>("Last time it was revised", lang);
                lVisit.VisibilityCondition.AppendLogicalExpression(cPlan, ComputationExpression.eRelational.EQUAL, cPlan.Options[1]);
                lVisit.RequiredCondition.AppendLogicalExpression(cPlan, ComputationExpression.eRelational.EQUAL, cPlan.Options[1]);
              


            var bGen = bcpForm.CreateField<RadioField>("Does your facility have a backup generator?", lang, optionTexts, true);
                bGen.FieldValueCssClass = "radio-inline";
            var power = bcpForm.CreateField<TextField>("What does it power", lang);
                power.VisibilityCondition.AppendLogicalExpression(bGen, ComputationExpression.eRelational.EQUAL, bGen.Options[1]);
                power.RequiredCondition.AppendLogicalExpression(bGen, ComputationExpression.eRelational.EQUAL, bGen.Options[1]);

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //

            Define_BCP_RolesStatesWorkflow(workflow, ref template);
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\BPC_DEptWorksheet_generared.xml");

            //string json = JsonConvert.SerializeObject(template);
            //File.WriteAllText("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.json", json);
        }

        [Test]
        public void BCP_MainEssentialFunctionandBATest()
        {
            string lang = "en";
            string templateName = "BCP Essential Function and Business Impact Analysis Worksheet: Main Form";

            IWorkflowService ws = _testHelper.WorkflowService;
            AppDbContext db = _testHelper.Db;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            ItemTemplate template = db.ItemTemplates
                .Where(et => et.TemplateName == templateName)
                .FirstOrDefault();

            if (template == null)
            {
                template = new ItemTemplate();
                db.ItemTemplates.Add(template);
            }
            else
            {
                ItemTemplate t = new ItemTemplate();
                t.Id = template.Id;
                template.Data = t.Data;
                template.Initialize(false);
            }
            template.TemplateName = templateName;
            template.Name.SetContent(templateName);

            //ws.SetModel(template);

            //Get the Workflow object using the workflow service
            Workflow workflow = template.Workflow;

            //Defininig states


            //Defining email templates
           // Define_EmailTemplate();

            //Defininig the inspection form
            DataItem bcpForm = template.GetDataItem(templateName, true, lang);
            bcpForm.IsRoot = true;
            bcpForm.SetDescription("This template is designed for BCP Essential Function and Business Impact Analysis Worksheet Form", lang);

            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h1", "Essential Function and Business Impact Analysis Worksheet", lang);
            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("div", "<b>Instructions:</b> Complete one worksheetfor each essential function for your department or unit.", lang, "alert alert-info");

           
            bcpForm.CreateField<TextField>("Organization or Department", lang, true);
            bcpForm.CreateField<TextField>("Essential Function", lang, true);

            var desc = bcpForm.CreateField<TextArea>("Brief Description", lang, true);
            desc.Cols = 30;
            desc.Rows = 5;
            desc.SetDescription("<i>What is this function responsible for? What does it accomplish?</i>", lang);

            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h5", "Priority Rating + RTO", lang)
                 .AppendContent("div", "<i>RTO = Recovery Time Objective (Maximum time this function can be down before significant problems would occur)</i>", lang);

            bcpForm.CreateField<RadioField>("Priority Rating and RTO", lang, GetRTOList(), true).SetFieldLabelCssClass("col-md-12")
                            .SetFieldValueCssClass("col-md-12");
            bcpForm.CreateField<InfoSection>(null, null)
                    .AppendContent("h4", "Key Personnel for this Function", lang);
            bcpForm.CreateField<TextField>("Primary", lang, true);
            var alt =bcpForm.CreateField<TextArea>("Alternate", lang, false);
            alt.Cols = 30;
            alt.Rows = 5;

            bcpForm.CreateField<TextField>("Key Roles Required to Perform the Function", lang, true).SetDescription(@"<i>(Admin Asst., RN, manager, financial analysis, etc.)</i>", lang);
            bcpForm.CreateField<TextField>("Vendors Vital to this Function", lang, true);


            bcpForm.CreateField<InfoSection>(null, null).AppendContent("h3", "RESOURCE REQUIREMENTS", lang);
            string[] reqTechOptions = GetReqTechs();
            var reqTech = bcpForm.CreateField<CheckboxField>("Required [U]Tech Products and Services", lang, reqTechOptions, false);
            var othT = bcpForm.CreateField<TextField>("Other [U]Tech Products and Service", lang);
            int lastInd = GetReqTechs().Length - 1;
            
            othT.VisibilityCondition.AppendLogicalExpression(reqTech, reqTech.Options[lastInd], true);
        
            var rt = bcpForm.CreateField<TextArea>("Required [U]Tech Applications", lang, false);
            rt.Cols = 30;
            rt.Rows = 5;

            var ee = bcpForm.CreateField<TextArea>("Essential External Websites", lang, false);
            ee.Cols = 30;
            ee.Rows = 5;

            var rf = bcpForm.CreateField<TextArea>("Required Facilities", lang, false);
            rf.Cols = 30;
            rf.Rows = 5;

            var vr = bcpForm.CreateField<TextArea>("Vital Records and Private Information", lang, false);
            vr.Cols = 30;
            vr.Rows = 5;

            bcpForm.CreateField<InfoSection>(null, null).AppendContent("h3", "DEPENDENCIES and PEAK PERIODS", lang);

            var ud = bcpForm.CreateField<TextArea>("Upstream Dependencies", lang, false);
            ud.Cols = 30;
            ud.Rows = 5;
            ud.SetDescription("Other departments vital to this function that you rely on", lang);

            var dd = bcpForm.CreateField<TextArea>("Downstream Dependencies", lang, false);
            dd.Cols = 30;
            dd.Rows = 5;
            dd.SetDescription("Other departments that rely on this function", lang);
          

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //

            Define_BCP_RolesStatesWorkflow(workflow, ref template);
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\BPC_MainEssential_generared.xml");

            //string json = JsonConvert.SerializeObject(template);
            //File.WriteAllText("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.json", json);
        }

        [Test]
        public void BCP_ConsequencesEssentialFunctionandBATest()
        {
            string lang = "en";
            string templateName = "BCP Essential Function and Business Impact Analysis Worksheet: Consequences Form";

            IWorkflowService ws = _testHelper.WorkflowService;
            AppDbContext db = _testHelper.Db;
           // IAuthorizationService auth = _testHelper.AuthorizationService;

            ItemTemplate template = db.ItemTemplates
                .Where(et => et.TemplateName == templateName)
                .FirstOrDefault();

            if (template == null)
            {
                template = new ItemTemplate();
                db.ItemTemplates.Add(template);
            }
            else
            {
                ItemTemplate t = new ItemTemplate();
                t.Id = template.Id;
                template.Data = t.Data;
                template.Initialize(false);
            }
            template.TemplateName = templateName;
            template.Name.SetContent(templateName);

            //ws.SetModel(template);

            //Get the Workflow object using the workflow service
            Workflow workflow = template.Workflow;

            //Defininig states

            //Defining email templates
          
            //Defininig the inspection form
            DataItem bcpForm = template.GetDataItem("BCP Essential Function and Business Impact Analysis Worksheet Consequences Form", true, lang);
            bcpForm.IsRoot = true;
            bcpForm.SetDescription("This template is designed for BCP Essential Function and Business Impact Analysis Worksheet Form", lang);

            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h1", "Essential Function and Business Impact Analysis Worksheet", lang);
            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("div", "<b>Instructions:</b> Complete one worksheetfor each essential function for your department or unit.", lang, "alert alert-info");


            bcpForm.CreateField<TextField>("Peak Periods", lang, true).SetDescription("<i>Significant or demanding months for this function</i>",lang);
           
          
            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h3", "HARMFUL CONSEQUENCES", lang)
                 .AppendContent("div", @"Suppose the essential function is not resumed quickly following a major disruption or disaster.Which of the listed harmful consequences might occur, 
                    and how long after the disaster might the harm begin to occur ? Check(X) the box to indicate when harm might occur.Select N / A 
                    if the consequence does not apply to the essential function you are evaluating.", lang, "alert alert-info");


            ////TABLE FIELD
            string[] consequences = GetPossibleHarmfulConsequences();
            TableField tf = bcpForm.CreateField<TableField>("", lang,false,consequences.Length,consequences.Length);
            ////tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            tf.FieldLabelCssClass = "col-md-12";
            tf.FieldValueCssClass = "col-md-12";
         
           
            tf.TableHead.CreateField<InfoSection>("", lang);
            
            tf.TableHead.CreateField<SelectField>("How long after a disaster might the harm occur?", lang, GetDisruptionOptions(), false);

           
            var ta = tf.TableHead.CreateField<TextArea>("Comments", lang);
            ta.Cols = 25;
            ta.Rows = 1;
            tf.SetColumnValues(0, consequences, lang);
            tf.AllowAddRows = false;
            
            
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles  and Workflow                                          //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //
            Define_BCP_RolesStatesWorkflow(workflow, ref template);
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\BPC_ConsequencesEssential_generared.xml");

            //string json = JsonConvert.SerializeObject(template);
            //File.WriteAllText("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.json", json);
        }

        [Test]
        public void BCP_SpecializedSuppliesTest()
        {
            string lang = "en";
            string templateName = "BCP Specialized Supplies Worksheet Template";

            IWorkflowService ws = _testHelper.WorkflowService;
            AppDbContext db = _testHelper.Db;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            ItemTemplate template = db.ItemTemplates
                .Where(et => et.TemplateName == templateName)
                .FirstOrDefault();

            if (template == null)
            {
                template = new ItemTemplate();
                db.ItemTemplates.Add(template);
            }
            else
            {
                ItemTemplate t = new ItemTemplate();
                t.Id = template.Id;
                template.Data = t.Data;
                template.Initialize(false);
            }
            template.TemplateName = templateName;
            template.Name.SetContent(templateName);

            //ws.SetModel(template);

            //Get the Workflow object using the workflow service
            Workflow workflow = template.Workflow;

            //Defining email templates

            //Defininig the inspection form
            DataItem bcpForm = template.GetDataItem("BCP Specialized Supplies Worksheet Form", true, lang);
            bcpForm.IsRoot = true;
            bcpForm.SetDescription("This template is designed for BCP Specialized Supplies Worksheet Form", lang);

            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h1", "Specialized Supplies Worksheet", lang);
            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("div", "<b>Instructions:</b> List all of the specialized supplies used by your department or unit. Create an Excel spreadsheet if your list is extensive.", lang, "alert alert-info");

            TableField tf = bcpForm.CreateField<TableField>("", lang, false, 1, 100);
            //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            tf.FieldLabelCssClass = "col-md-12";
            tf.FieldValueCssClass = "col-md-12";

            tf.TableHead.CreateField<TextField>("Item", lang);
            tf.TableHead.CreateField<TextField>("Vendor/Supplier", lang);
            tf.TableHead.CreateField<TextField>("Ordered Through", lang);
            tf.TableHead.CreateField<TextField>("Special Instructions", lang);
            tf.AppendRows(1);
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //

            Define_BCP_RolesStatesWorkflow(workflow, ref template);
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\BPC_SpecializedSuppliesWorksheet_generared.xml");

            //string json = JsonConvert.SerializeObject(template);
            //File.WriteAllText("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.json", json);
        }

        [Test]
        public void BCP_EssentialVendorsWorksheetTest()
        {
            string lang = "en";
            string templateName = "BCP Essential Vendors Worksheet Template";

            IWorkflowService ws = _testHelper.WorkflowService;
            AppDbContext db = _testHelper.Db;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            ItemTemplate template = db.ItemTemplates
                .Where(et => et.TemplateName == templateName)
                .FirstOrDefault();

            if (template == null)
            {
                template = new ItemTemplate();
                db.ItemTemplates.Add(template);
            }
            else
            {
                ItemTemplate t = new ItemTemplate();
                t.Id = template.Id;
                template.Data = t.Data;
                template.Initialize(false);
            }
            template.TemplateName = templateName;
            template.Name.SetContent(templateName);

            //ws.SetModel(template);

            //Get the Workflow object using the workflow service
            Workflow workflow = template.Workflow;

            //Defining email templates

            //Defininig the inspection form
            DataItem bcpForm = template.GetDataItem("BCP Essential Vendors Worksheet Form", true, lang);
            bcpForm.IsRoot = true;
            bcpForm.SetDescription("This template is designed for BCP Essential Vendors Worksheet Form", lang);

            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h1", "Essential Vendors Worksheet", lang);
            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("div", "<b>Instructions:</b> List all of the essential vendors used by your department or unit. Create an Excel spreadsheet if your list is extensive.", lang, "alert alert-info");

            TableField tf = bcpForm.CreateField<TableField>("", lang, false, 1, 100);
            //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            tf.FieldLabelCssClass = "col-md-12";
            tf.FieldValueCssClass = "col-md-12";

            tf.TableHead.CreateField<TextField>("Company Name", lang);
            tf.TableHead.CreateField<TextField>("Description", lang);
            tf.TableHead.CreateField<TextField>("Contact Name", lang);
            tf.TableHead.CreateField<TextField>("Contact Business Phone", lang);
            tf.TableHead.CreateField<TextField>("Contact Cell Phone", lang);
            tf.TableHead.CreateField<TextField>("Contact Email", lang);
            tf.TableHead.CreateField<TextField>("Contact After Hours #", lang);
            tf.AppendRows(1);
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //

            Define_BCP_RolesStatesWorkflow(workflow, ref template);
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\BPC_EssentialVendorsWorksheet_generared.xml");

            //string json = JsonConvert.SerializeObject(template);
            //File.WriteAllText("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.json", json);
        }

        [Test]
        public void BCP_SpecializedEquipmentWorksheetTest()
        {
            string lang = "en";
            string templateName = "BCP Specialized Equipment Worksheet Template";

            IWorkflowService ws = _testHelper.WorkflowService;
            AppDbContext db = _testHelper.Db;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            ItemTemplate template = db.ItemTemplates
                .Where(et => et.TemplateName == templateName)
                .FirstOrDefault();

            if (template == null)
            {
                template = new ItemTemplate();
                db.ItemTemplates.Add(template);
            }
            else
            {
                ItemTemplate t = new ItemTemplate();
                t.Id = template.Id;
                template.Data = t.Data;
                template.Initialize(false);
            }
            template.TemplateName = templateName;
            template.Name.SetContent(templateName);

            //ws.SetModel(template);

            //Get the Workflow object using the workflow service
            Workflow workflow = template.Workflow;

            //Defining email templates

            //Defininig the inspection form
            DataItem bcpForm = template.GetDataItem("BCP Specialized Equipment Worksheet Form", true, lang);
            bcpForm.IsRoot = true;
            bcpForm.SetDescription("This template is designed for BCP Specialized Equipment Worksheet Form", lang);

            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h1", "Specialized Equipment Worksheet", lang);
            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("div", "<b>Instructions:</b> List all specialized, hard-to-replace equipment used by your department or unit.", lang, "alert alert-info");

            TableField tf = bcpForm.CreateField<TableField>("", lang, false, 1, 50);
            //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            tf.FieldLabelCssClass = "col-md-12";
            tf.FieldValueCssClass = "col-md-12";

            tf.TableHead.CreateField<TextField>("Equipment", lang);
            tf.TableHead.CreateField<TextField>("Model #", lang);
            tf.TableHead.CreateField<TextField>("Serial #", lang);
            tf.TableHead.CreateField<TextField>("Supplier", lang);
            tf.TableHead.CreateField<TextField>("Purchased through CWRU Procurement", lang);
            tf.TableHead.CreateField<TextField>("CWRU Inventory ID #", lang);
            tf.TableHead.CreateField<TextField>("EHS ID #", lang);
            tf.TableHead.CreateField<TextField>("Special Requirements", lang);
            tf.AppendRows(1);
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //

            Define_BCP_RolesStatesWorkflow(workflow, ref template);
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\BPC_SpecializedEquipmentWorksheet_generared.xml");

            //string json = JsonConvert.SerializeObject(template);
            //File.WriteAllText("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.json", json);
        }


        [Test]
        public void BCP_MitigationWorksheetTest()
        {
            string lang = "en";
            string templateName = "BCP Mitigation/Follow-Up Actions Worksheet Template";

            IWorkflowService ws = _testHelper.WorkflowService;
            AppDbContext db = _testHelper.Db;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            ItemTemplate template = db.ItemTemplates
                .Where(et => et.TemplateName == templateName)
                .FirstOrDefault();

            if (template == null)
            {
                template = new ItemTemplate();
                db.ItemTemplates.Add(template);
            }
            else
            {
                ItemTemplate t = new ItemTemplate();
                t.Id = template.Id;
                template.Data = t.Data;
                template.Initialize(false);
            }
            template.TemplateName = templateName;
            template.Name.SetContent(templateName);

            //ws.SetModel(template);

            //Get the Workflow object using the workflow service
            Workflow workflow = template.Workflow;

            //Defining email templates

            //Defininig the inspection form
            DataItem bcpForm = template.GetDataItem("BCP Mitigation/Follow-Up Actions Worksheet Form", true, lang);
            bcpForm.IsRoot = true;
            bcpForm.SetDescription("This template is designed for BCP Mitigation/Follow-Up Actions Worksheet Form", lang);

            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h1", "Mitigation/Follow-Up Actions Worksheet", lang);
            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("div", "<b>Instructions:</b> List significant issues that have been identified during the planning process and need to be addressed.   Include any possible solutions, due date, responsible parties, and date issue was resolved.", lang, "alert alert-info");

            TableField tf = bcpForm.CreateField<TableField>("", lang, false, 1, 100);
            //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            tf.FieldLabelCssClass = "col-md-12";
            tf.FieldValueCssClass = "col-md-12";

            tf.TableHead.CreateField<TextField>("Business Continuity Issue / Problem", lang);
            tf.TableHead.CreateField<TextField>("Possible Corrective Action(s)", lang);
            tf.TableHead.CreateField<TextField>("Assigned To", lang);
            tf.TableHead.CreateField<DateField>("Due Date", lang);
            tf.TableHead.CreateField<DateField>("Completion Date", lang);
       
            tf.AppendRows(1);
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //

            Define_BCP_RolesStatesWorkflow(workflow, ref template);
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\BPC_MitigationWorksheet_generared.xml");

            //string json = JsonConvert.SerializeObject(template);
            //File.WriteAllText("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.json", json);
        }

        [Test]
        public void BCP_MinSiteRequirementWorksheetTest()
        {
            string lang = "en";
            string templateName = "BCP Minimum Site Requirements Worksheet Template";

            IWorkflowService ws = _testHelper.WorkflowService;
            AppDbContext db = _testHelper.Db;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            ItemTemplate template = db.ItemTemplates
                .Where(et => et.TemplateName == templateName)
                .FirstOrDefault();

            if (template == null)
            {
                template = new ItemTemplate();
                db.ItemTemplates.Add(template);
            }
            else
            {
                ItemTemplate t = new ItemTemplate();
                t.Id = template.Id;
                template.Data = t.Data;
                template.Initialize(false);
            }
            template.TemplateName = templateName;
            template.Name.SetContent(templateName);

            //ws.SetModel(template);

            //Get the Workflow object using the workflow service
            Workflow workflow = template.Workflow;

            //Defining email templates

            //Defininig the inspection form
            DataItem bcpForm = template.GetDataItem("BCP Minimum Site Requirements Worksheet Form", true, lang);
            bcpForm.IsRoot = true;
            bcpForm.SetDescription("This template is designed for BCP Minimum Site Requirements Worksheet Form", lang);

            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h1", "Minimum Site Requirements Worksheet", lang);
            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("div", "<b>Instructions:</b> List all requirements for relocation. Create an Excel spreadsheet if your list is extensive.", lang, "alert alert-info");
            string[] spaceCat = new string[] { "Total square footage", "Reception area", "Private offices", "Shared offices or cubicles",
                                                 "Conference rooms", "Storage rooms","Copy/Mail room","Support staff work space","Specialized rooms",
                                                  "Other space", "Specialized equipment","Specialized supplies","Hard-line telephones",
                                                   "Other helpful information:"};
            TableField tf = bcpForm.CreateField<TableField>("", lang, true, spaceCat.Length, spaceCat.Length);
            //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            tf.FieldLabelCssClass = "col-md-12";
            tf.FieldValueCssClass = "col-md-12";
            tf.TableHead.CreateField<InfoSection>("", lang,"", "Space");
            tf.TableHead.CreateField<TextField>("Minimum Required", lang);
            var ta = tf.TableHead.CreateField<TextArea>("Comments/Notes", lang);
            ta.Cols = 25;
            ta.Rows = 1;
            tf.SetColumnValues(0, spaceCat, lang);
            tf.AllowAddRows = false;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //

            Define_BCP_RolesStatesWorkflow(workflow, ref template);
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\BPC_MinSiteRequirementWorksheet_generared.xml");

            //string json = JsonConvert.SerializeObject(template);
            //File.WriteAllText("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.json", json);
        }

        [Test]
        public void BCP_StaffRelocationWorksheetTest()
        {
            string lang = "en";
            string templateName = "BCP Staff Relocation Worksheet Template";

            IWorkflowService ws = _testHelper.WorkflowService;
            AppDbContext db = _testHelper.Db;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            ItemTemplate template = db.ItemTemplates
                .Where(et => et.TemplateName == templateName)
                .FirstOrDefault();

            if (template == null)
            {
                template = new ItemTemplate();
                db.ItemTemplates.Add(template);
            }
            else
            {
                ItemTemplate t = new ItemTemplate();
                t.Id = template.Id;
                template.Data = t.Data;
                template.Initialize(false);
            }
            template.TemplateName = templateName;
            template.Name.SetContent(templateName);

            //ws.SetModel(template);

            //Get the Workflow object using the workflow service
            Workflow workflow = template.Workflow;

            //Defining email templates

            //Defininig the inspection form
            DataItem bcpForm = template.GetDataItem("BCP Staff Relocation Worksheet Form", true, lang);
            bcpForm.IsRoot = true;
            bcpForm.SetDescription("This template is designed for BCP Staff Relocation Worksheet Form", lang);

            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h1", "Staff Relocation Worksheet", lang);
            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("div", "<b>Instructions:</b> List staff and indicate their space and equipment needs. Only include the items they need but don’t have as a result of the event.", lang, "alert alert-info");
            string[] options = new string[] { "Yes", "No"};
            TableField tf = bcpForm.CreateField<TableField>("", lang, true, 1, 50);
            //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            tf.FieldLabelCssClass = "col-md-12";
            tf.FieldValueCssClass = "col-md-12";
         
            tf.TableHead.CreateField<TextField>("Staff Member", lang);
            tf.TableHead.CreateField<RadioField>("NoNeeds / SameSpace", lang,options).FieldValueCssClass = "radio-inline";
            tf.TableHead.CreateField<RadioField>("WorkFromHome", lang, options).FieldValueCssClass = "radio-inline";
            tf.TableHead.CreateField<RadioField>("NewLocation", lang, options).FieldValueCssClass = "radio-inline";
            tf.TableHead.CreateField<RadioField>("PrivateOffice", lang, options).FieldValueCssClass = "radio-inline";
            tf.TableHead.CreateField<RadioField>("Cubicle   ", lang, options).FieldValueCssClass = "radio-inline";
            tf.TableHead.CreateField<RadioField>("Computer   ", lang, options).FieldValueCssClass = "radio-inline";
            tf.TableHead.CreateField<RadioField>("Monitor   ", lang, options).FieldValueCssClass = "radio-inline";
            tf.TableHead.CreateField<RadioField>("Printer   ", lang, options).FieldValueCssClass = "radio-inline";
            tf.TableHead.CreateField<RadioField>("PhoneDesk/Cell", lang, options).FieldValueCssClass = "radio-inline";
            tf.TableHead.CreateField<RadioField>("OfficeFiles", lang, options).FieldValueCssClass = "radio-inline";
            tf.TableHead.CreateField<RadioField>("AccessToServer", lang, options).FieldValueCssClass = "radio-inline";
            tf.TableHead.CreateField<RadioField>("SpecialEquipment", lang, options).FieldValueCssClass = "radio-inline";
            tf.TableHead.CreateField<RadioField>("SpecialSpace", lang, options).FieldValueCssClass = "radio-inline";
            tf.TableHead.CreateField<TextField>("Other", lang);
            tf.AppendRows(1);

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //

            Define_BCP_RolesStatesWorkflow(workflow, ref template);
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\BPC_StaffRelocationWorksheet_generared.xml");

            //string json = JsonConvert.SerializeObject(template);
            //File.WriteAllText("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.json", json);
        }

        [Test]
        public void BCP_RecoveryPlanningWorksheetTest()
        {
            string lang = "en";
            string templateName = "BCP Recovery Planning Worksheet Template";

            IWorkflowService ws = _testHelper.WorkflowService;
            AppDbContext db = _testHelper.Db;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            ItemTemplate template = db.ItemTemplates
                .Where(et => et.TemplateName == templateName)
                .FirstOrDefault();

            if (template == null)
            {
                template = new ItemTemplate();
                db.ItemTemplates.Add(template);
            }
            else
            {
                ItemTemplate t = new ItemTemplate();
                t.Id = template.Id;
                template.Data = t.Data;
                template.Initialize(false);
            }
            template.TemplateName = templateName;
            template.Name.SetContent(templateName);

            //ws.SetModel(template);

            //Get the Workflow object using the workflow service
            Workflow workflow = template.Workflow;

            //Defining email templates

            //Defininig the inspection form
            DataItem bcpForm = template.GetDataItem("BCP Recovery Planning Worksheet Form", true, lang);
            bcpForm.IsRoot = true;
            bcpForm.SetDescription("This template is designed for BCP Recovery Planning Worksheet Form", lang);

            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h1", "Recovery Planning Worksheet", lang);
            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("div", "<b>Instructions:</b>  Complete one worksheet for each essential function for your department or unit.", lang, "alert alert-info");
           
            var tf1=bcpForm.CreateField<TextField>("Essential Function Recovery Strategy: ", lang, true).SetDescription("Ensure the continuation of (enter name of function):", lang);
            //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            tf1.FieldLabelCssClass = "col-md-12";
            tf1.FieldValueCssClass = "col-md-12";

            tf1 = bcpForm.CreateField<TextField>("Requirements:", lang, true).SetDescription("(List of required “must have” items or systems)", lang);
            //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            tf1.FieldLabelCssClass = "col-md-12";
            tf1.FieldValueCssClass = "col-md-12";

            tf1 = bcpForm.CreateField<TextField>("Key Roles:", lang, true).SetDescription("(List of roles or qualifications needed for this function. Facilities supervisors, financial analysis, RN, etc.)", lang);
            //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            tf1.FieldLabelCssClass = "col-md-12";
            tf1.FieldValueCssClass = "col-md-12";

            bcpForm.CreateField<InfoSection>("<h3>Individualized Recovery Tasks</h3>", lang).AppendContent("div", @"
<b>Instructions</b>: Describe your backup plan for each of the items below. If none exists write None. Skip any Task that does not apply to this function (Example: the function does not require any specialized equipment or supplies)", lang, "alert alert-info");
            var ta1 = bcpForm.CreateField<TextArea>("Recovery Task #1: Operate with reduced staff", lang).SetDescription("How would you continue this function if your usual workforce was reduced by 50% for an extended period of time?", lang);
            //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            ta1.FieldLabelCssClass = "col-md-12";
            ta1.FieldValueCssClass = "col-md-12";
            ta1.SetAttribute("cols", "50");
            ta1.SetAttribute("rows", "5");

            ta1 = bcpForm.CreateField<TextArea>("Recovery Task #2: Loss of essential facilities", lang).SetDescription("What would you do if you did not have access to the primary facilities needed for this function?  List each facility and describe your back-up plan.", lang);
            //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            ta1.FieldLabelCssClass = "col-md-12";
            ta1.FieldValueCssClass = "col-md-12";
            ta1.SetAttribute("cols", "50");
            ta1.SetAttribute("rows", "5");

            ta1 = bcpForm.CreateField<TextArea>("Recovery Task #3: Loss of essential IT services and applications", lang).SetDescription("What would you do if you lost access to your essential [U]Tech services (e.g., email, internet) or applications (e.g., HCM, SIS)?  List each service and application and describe your back-up plan.", lang);
            //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            ta1.FieldLabelCssClass = "col-md-12";
            ta1.FieldValueCssClass = "col-md-12";
            ta1.SetAttribute("cols", "50");
            ta1.SetAttribute("rows", "5");

            ta1 = bcpForm.CreateField<TextArea>("Recovery Task #4: Loss of essential or specialized equipment", lang).SetDescription("What would you do if your essential equipment failed? List the equipment and describe your back-up plan.", lang);
            //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            ta1.FieldLabelCssClass = "col-md-12";
            ta1.FieldValueCssClass = "col-md-12";
            ta1.SetAttribute("cols", "50");
            ta1.SetAttribute("rows", "5");

            ta1 = bcpForm.CreateField<TextArea>("Recovery Task #5: Loss of essential or specialized supplies", lang)
                .SetDescription("What would you do if you ran out of specialized supplies? How long could you function before you would need to restock? What is your back-up plan?", lang);
            //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            ta1.FieldLabelCssClass = "col-md-12";
            ta1.FieldValueCssClass = "col-md-12";
            ta1.SetAttribute("cols", "50");
            ta1.SetAttribute("rows", "5");

            ta1 = bcpForm.CreateField<TextArea>("Recovery Task #6: Loss of essential upstream dependent departments or services", lang)
                .SetDescription("What would you do if you lost access to an upstream dependent department or service needed for this function?  List each dependency and describe your back-up plan.", lang);
            //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            ta1.FieldLabelCssClass = "col-md-12";
            ta1.FieldValueCssClass = "col-md-12";
            ta1.SetAttribute("cols", "50");
            ta1.SetAttribute("rows", "5");

            ta1 = bcpForm.CreateField<TextArea>("Recovery Task #7: Loss of utilities", lang)
                .SetDescription("What would happen if you lost basic utilities like electricity, water, HVAV? List each utility and describe your back-up plan.", lang);
            //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            ta1.FieldLabelCssClass = "col-md-12";
            ta1.FieldValueCssClass = "col-md-12";
            ta1.SetAttribute("cols", "50");
            ta1.SetAttribute("rows", "5");

            ta1 = bcpForm.CreateField<TextArea>("Recovery Task #8: Other", lang)
                .SetDescription("List any other essential item, service, vendor, or person, that this function replies on that is not captured above. Indicate how long could you operate without the item or person. Describe your plan for continuing operations without it / them.", lang);
            //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            ta1.FieldLabelCssClass = "col-md-12";
            ta1.FieldValueCssClass = "col-md-12";
            ta1.SetAttribute("cols", "50");
            ta1.SetAttribute("rows", "5");
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //

            Define_BCP_RolesStatesWorkflow(workflow, ref template);
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\BPC_RecoveryPlanningWorksheet_generared.xml");

            //string json = JsonConvert.SerializeObject(template);
            //File.WriteAllText("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.json", json);
        }

        [Test]
        public void BCP_IndividualDamageAssessmentWorksheetTest()
        {
            string lang = "en";
            string templateName = "BCP Individual Damage-Assessment Worksheet Template";

            IWorkflowService ws = _testHelper.WorkflowService;
            AppDbContext db = _testHelper.Db;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            ItemTemplate template = db.ItemTemplates
                .Where(et => et.TemplateName == templateName)
                .FirstOrDefault();

            if (template == null)
            {
                template = new ItemTemplate();
                db.ItemTemplates.Add(template);
            }
            else
            {
                ItemTemplate t = new ItemTemplate();
                t.Id = template.Id;
                template.Data = t.Data;
                template.Initialize(false);
            }
            template.TemplateName = templateName;
            template.Name.SetContent(templateName);

            //ws.SetModel(template);

            //Get the Workflow object using the workflow service
            Workflow workflow = template.Workflow;

            //Defining email templates

            //Defininig the inspection form
            DataItem bcpForm = template.GetDataItem("BCP Individual Damage-Assessment Worksheet Form", true, lang);
            bcpForm.IsRoot = true;
            bcpForm.SetDescription("This template is designed for BCP Individual Damage-Assessment Worksheet Form", lang);

            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h1", "Individual Damage-Assessment Worksheet", lang);
            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("div", "<b>Instructions:</b>  Complete one form for each office or work space that was affected.", lang, "alert alert-info");

            bcpForm.CreateField<TextField>("Employee Name:", lang, true);
            bcpForm.CreateField<TextField>("Title:", lang, true);
            bcpForm.CreateField<TextField>("Address of Damage:", lang, true);
            bcpForm.CreateField<TextField>("Room Number:", lang, true);
            bcpForm.CreateField<DateField>("Date of Incident:", lang, true);
            bcpForm.CreateField<DateField>("Date Completing Form:", lang, true);


            bcpForm.CreateField<InfoSection>("<h4>Incident Summary</h4>", lang, "alert alert-info");
            string[] incidents = { "Burst Water Pipe", "Fire","Flooding","Other" };
            string[] bwpDetails = {"Fire System","Heat/AC System","Waste","Other" };

            var chkIncidents = bcpForm.CreateField<CheckboxField>("", lang, incidents);
            chkIncidents.FieldValueCssClass = "radio-inline col-md-12";
            chkIncidents.FieldLabelCssClass = "col-md-12";
            var otherIncident = bcpForm.CreateField<TextField>("Other Incident - Please specify:", lang);
            otherIncident.VisibilityCondition.AppendLogicalExpression(chkIncidents, chkIncidents.GetOption("Other", lang), true);
            
            var chkIncidentDet = bcpForm.CreateField<CheckboxField>("More specific of Burst Water Pipe", lang, bwpDetails);
            chkIncidentDet.FieldLabelCssClass = "col-md-12";
            chkIncidentDet.FieldValueCssClass = "radio-inline col-md-12";
            chkIncidentDet.VisibilityCondition.AppendLogicalExpression(chkIncidents, chkIncidents.GetOption("Burst Water Pipe", lang), true);
            foreach (var option in chkIncidentDet.Options)
                option.VisibilityCondition.AppendLogicalExpression(chkIncidents, chkIncidents.GetOption("Burst Water Pipe", lang), true);


            var otherIncidentDet = bcpForm.CreateField<TextField>("Other Burst Water Pipe - Please Specify:", lang);
            otherIncidentDet.VisibilityCondition.AppendLogicalExpression(chkIncidentDet, chkIncidentDet.GetOption("Other", lang), true);

            //======================================== Space Affected =======================================================//
            bcpForm.CreateField<InfoSection>("<h4>Space Affected</h4>", lang, "alert alert-info");
            string[] spaces = { "Individual office", "Cubicle", "Storage", "Shared office", "Lab / Research", "Library / Museum", "Other"};
           

            var chkSpaces = bcpForm.CreateField<CheckboxField>("", lang, spaces);
            chkSpaces.FieldValueCssClass = "radio-inline col-md-12";
            var sharedSpace = bcpForm.CreateField<TextField>("Shared With:", lang);
            sharedSpace.VisibilityCondition.AppendLogicalExpression(chkSpaces, chkSpaces.GetOption("Shared office", lang), true);
            var otherSpace = bcpForm.CreateField<TextField>("Other - Please specify:", lang);
            otherSpace.VisibilityCondition.AppendLogicalExpression(chkSpaces, chkSpaces.GetOption("Other", lang), true);


            //============================OVER ALL Damage Assessment ==========================================================//
            bcpForm.CreateField<InfoSection>("<h4>Overall Damage Assessment</h4>", lang, "alert alert-info");
            var desc= bcpForm.CreateField<TextArea>("Brief description of damage:", lang);
            desc.FieldLabelCssClass="col-md-12";
            desc.Rows = 5;
            desc.Cols = 50;

            // ===================================== Itemize damage ===========================================================
            string[] items = new string[] { "Computer", "Monitor", "Printer", "Phone",
                                                 "Files,Documents", "Desk","Chair","File Cabinet (not files)","Bookshelf",
                                                  "Rugs", "Other Furniture","Specialized Equipment"
                                                   };
            string[] options = new string[] { "Destryed", "Major", "Minor"};
            TableField tf = bcpForm.CreateField<TableField>("", lang, true, items.Length, items.Length);
            //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            tf.FieldLabelCssClass = "col-md-12 radio-inline";
            tf.FieldValueCssClass = "col-md-12";
            tf.TableHead.CreateField<InfoSection>("", lang, "", "Item");
            var dam = tf.TableHead.CreateField<RadioField>("Damage", lang, options);
            dam.SetFieldValueCssClass("col-md-2");
            dam.SetFieldLabelCssClass("col-md-2");
            dam.FieldValueCssClass= "radio-inline";
            //dam.SetFieldCssClass("radio-inline");
           
            var ta = tf.TableHead.CreateField<TextArea>("Comments / Damage Caused By", lang);
            ta.Cols = 25;
            ta.Rows = 1;
            tf.SetColumnValues(0, items, lang);
            tf.AllowAddRows = false;


            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //

            Define_BCP_RolesStatesWorkflow(workflow, ref template);
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\BPC_IndividualDamageAssessmentWorksheet_generared.xml");

            //string json = JsonConvert.SerializeObject(template);
            //File.WriteAllText("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.json", json);
        }


        private void DefineEmailTemplate(ref ItemTemplate template)
        {
            string lang = "en";
            EmailTemplate adminNotification = template.GetEmailTemplate("Admin Notification", lang, true);
            adminNotification.SetDescription("This metadata set defines the email template to be sent to the admin when an inspector does not submit an inspection report timely.", lang);
            adminNotification.SetSubject("Safety Inspection Submission");
            adminNotification.SetBody("TBD");

            EmailTemplate inspectorSubmissionNotification = template.GetEmailTemplate("Inspector Notification", lang, true);
            inspectorSubmissionNotification.SetDescription("This metadata set defines the email template to be sent to an inspector when an inspection report is not submitted timely.", lang);
            inspectorSubmissionNotification.SetSubject("Safety Inspection Reminder");
            inspectorSubmissionNotification.SetBody("TBD");

        }
        private void Define_BCP_RolesStatesWorkflow(Workflow workflow, ref ItemTemplate template)
        {
            IWorkflowService ws = _testHelper.WorkflowService;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            State emptyState = workflow.AddState(ws.GetStatus(template.Id, "", true));
            State submittedState = workflow.AddState(ws.GetStatus(template.Id, "Submitted", true));
            State deleteState = workflow.AddState(ws.GetStatus(template.Id, "Deleted", true));

            WorkflowRole adminRole = workflow.AddRole(auth.GetRole("BusinessContPlanAdmin", true));
            WorkflowRole inspectorRole = workflow.AddRole(auth.GetRole("BusinessContPlanUser", true));


            // Submitting an bcp form
            //Only safey inspectors can submit this form
            GetAction startSubmissionAction = workflow.AddAction("Start Submission", nameof(TemplateOperations.Instantiate), "Home");
            startSubmissionAction.Access = GetAction.eAccess.Restricted;
            startSubmissionAction.AddStateReferances(emptyState.Id)
                .AddAuthorizedRole(inspectorRole.Id);

            //Listing bcp forms.
            //Admins and Inspectors can run the item-list report to list instances.
            //Note that the visibility of individual list entries is depend on the
            //Read permission on individual submissions.
            GetAction listSubmissionsAction = workflow.AddAction("List Submissions", nameof(TemplateOperations.ListInstances), "Home");
            listSubmissionsAction.Access = GetAction.eAccess.Restricted;
            listSubmissionsAction.AddStateReferances(submittedState.Id)
                   .AddOwnerAuthorization()
                   .AddAuthorizedRole(inspectorRole.Id)
                  .AddAuthorizedRole(adminRole.Id);


            //Detailed submission bcp forms.
            //Inspectors can view their own submissions.
            //Admins can view all submissions.
            GetAction viewSubmissionAction = workflow.AddAction("Details", nameof(TemplateOperations.Read), "List");
            viewSubmissionAction.Access = GetAction.eAccess.Restricted;
            viewSubmissionAction.AddStateReferances(submittedState.Id)
                .AddOwnerAuthorization()
                .AddAuthorizedRole(adminRole.Id);


            //Post action for submitting the form
            PostAction submitPostAction = startSubmissionAction.AddPostAction("Submit", nameof(TemplateOperations.Update));
            submitPostAction.AddStateMapping(emptyState.Id, submittedState.Id, "Submit");

            //Defining the pop-up for the above submitPostAction action
            PopUp submitActionPopUp = submitPostAction.AddPopUp("WARNING: Submitting the Form", "Once submitted, you cannot update the form.", "");
            submitActionPopUp.AddButtons("Yes, submit", "true");
            submitActionPopUp.AddButtons("Cancel", "false");

            // Edit submission related workflow items
            //Defining actions
            GetAction editSubmissionAction = workflow.AddAction("Edit Submission", nameof(TemplateOperations.Update), "Details");

            //Submissions can only be edited by admins
            editSubmissionAction.AddStateReferances(submittedState.Id)
                .AddAuthorizedRole(adminRole.Id);

            //Defining post actions
            //  PostAction editPostActionSave = editSubmissionAction.AddPostAction("Save", "Save");
            //   editPostActionSave.AddStateMapping(submittedState.Id, submittedState.Id, "Save");

            PostAction editPostActionSubmit = editSubmissionAction.AddPostAction("Submit", nameof(TemplateOperations.Update));//(Button Label, ActionName)
            editPostActionSubmit.AddStateMapping(submittedState.Id, submittedState.Id, "Submit");//current state, nectStae, buttonLabel
            //editPostActionSubmit.ValidateInputs = false;

            //Defining the pop-up for the above postActionSubmit action
            PopUp EditActionPopUpopUp = editPostActionSubmit.AddPopUp("Confirmation", "Do you really want to submit this document?", "Once submitted, you cannot update the document.");
            EditActionPopUpopUp.AddButtons("Yes, submit", "true");
            EditActionPopUpopUp.AddButtons("Cancel", "false");



            // Delete submission related workflow items
            /*
            //Defining actions. Only admin can delete a submission
            GetAction deleteSubmissionAction = workflow.AddAction("Delete Submission", nameof(CrudOperations.Delete), "Details");
            deleteSubmissionAction.Access = GetAction.eAccess.Restricted;
            deleteSubmissionAction.AddStateReferances(submittedState.Id)
                .AddAuthorizedRole(adminRole.Id);



            //Defining post actions
            PostAction deleteSubmissionPostAction = deleteSubmissionAction.AddPostAction("Delete", "Save");
            deleteSubmissionPostAction.AddStateMapping(submittedState.Id, deleteState.Id, "Delete");
            deleteSubmissionPostAction.ValidateInputs = false;

            //Defining the pop-up for the above postActionSubmit action
            PopUp deleteSubmissionActionPopUpopUp = deleteSubmissionPostAction.AddPopUp("WARNING: Delete", "Do you really want to delete this submission?", "");
            deleteSubmissionActionPopUpopUp.AddButtons("Yes, delete", "true");
            deleteSubmissionActionPopUpopUp.AddButtons("Cancel", "false");
            */

        }

        private string[] GetDisruptionOptions()
        {

            return new string[] { "N/A", "0-2 days", "1 week", "2 weeks", "3 weeks", "4 weeks", ">4 weeks" };
        }
        private string[] GetYesNoList()
        {
            return new string[] { "No", "Yes" };
        }

        private string[] GetRTOList()
        {
            return new string[] { "<b>Critical:</b> Directly impacts life, health, safety, or security. Cannot stop. RTO <b>less than</b> 4 hours",
             "<b>High:</b> Must continue at normal or increased level. Pausing for more than 24 hours may cause significant consequences or serious harm. RTO <b>less than</b> 24 hours",
            "<b>Medium:</b> Must continue if at all possible, perhaps in reduced mode. Stopping for more than one week may cause major disruption. RTO <b>less than</b> 1 week",
             "<b>Low:</b> May be suspended for up to one month without causing significant disruption. RTO <b>less than</b> 1 month",
            "<b>Deferable:</b> May pause and resume when conditions permit. RTO <b>over</b> 1 month" };
        }

        private string[] GetReqTechs()
        {

            return new string[] { "Network Services",  "Email",  "Telephone", "SIS", "HCM", "VPN",  "Others" };
        }

        private string[] GetPossibleHarmfulConsequences()
        {
            List<string> conseq = new List<string>();
            conseq.Add("01. Disruption of teaching?");
            conseq.Add("02. Disruption of research?");
            conseq.Add("03. Departure of faculty?");
            conseq.Add("04. Departure of staff?");
            conseq.Add("05. Departure of students? ");
            conseq.Add("06. Well-being of staff/faculty?");
            conseq.Add("07. Well-being of students?");
            conseq.Add("08. Payment deadlines unmet by campus?");
            conseq.Add("09. Loss of revenue to campus?");
            conseq.Add("10. Legal obligations unmet by campus?");
            conseq.Add("11. Legal harm to the University?");
            conseq.Add("12. Impact on other campus unit(s)?");
            conseq.Add("13. Impact on important business partner(s)?");
            conseq.Add("14. Impact on the Faculty of Arts’ brand image? ");
            conseq.Add("15. Function without power?");
            conseq.Add("16. Other harmful consequence?");
          

            return conseq.ToArray();
        }

    }
}
