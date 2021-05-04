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

            //Defininig states
            //State emptyState = workflow.AddState(ws.GetStatus(template.Id, "", true));
            //State submittedState = workflow.AddState(ws.GetStatus(template.Id, "Submitted", true));
            //State deleteState = workflow.AddState(ws.GetStatus(template.Id, "Deleted", true));


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

             var latVisit = bcpForm.CreateField<DateField>("Last time it was revised: ", lang);
                 latVisit.VisibilityCondition.AppendLogicalExpression(ePlan, ComputationExpression.eRelational.EQUAL, ePlan.Options[1]);
                 latVisit.RequiredCondition.AppendLogicalExpression(ePlan, ComputationExpression.eRelational.EQUAL, ePlan.Options[1]);

            var cPlan = bcpForm.CreateField<RadioField>("Do you have a business continuity plan?", lang, optionTexts, true);
            var lVisit = bcpForm.CreateField<DateField>("Last time it was revised", lang);
                lVisit.VisibilityCondition.AppendLogicalExpression(cPlan, ComputationExpression.eRelational.EQUAL, cPlan.Options[1]);
                lVisit.RequiredCondition.AppendLogicalExpression(cPlan, ComputationExpression.eRelational.EQUAL, cPlan.Options[1]);


            var bGen = bcpForm.CreateField<RadioField>("Does your facility have a backup generator?", lang, optionTexts, true);
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
            
            tf.TableHead.CreateField<RadioField>("How long after a disaster might the harm occur?", lang, GetDisruptionOptions(), false);
           
            tf.TableHead.CreateField<TextField>("Comments", lang,false);
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
