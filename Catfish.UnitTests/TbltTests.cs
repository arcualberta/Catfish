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
    public class TbltTests
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
        public void TBlt_ContactFormTest()
        {
            string lang = "en";
            string templateName = "Task-based Language Teaching Contact Form Template";

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
            DataItem bcpForm = template.GetDataItem("Task-based Language Teaching Contact Form", true, lang);
            bcpForm.IsRoot = true;
            bcpForm.SetDescription("This template is designed for Task-based Language Teaching Contact Form", lang);

            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h1", "Join Us", lang);
           
            bcpForm.CreateField<TextField>("Name", lang,true);
            var applicantEmail= bcpForm.CreateField<TextField>("Email", lang, true);
            bcpForm.CreateField<TextField>("Affiliation", lang, true);
            var other =bcpForm.CreateField<TextArea>(@"Please, tell us what language(s) you teach and what age groups (e.g., elementary, secondary, college/university, adults)", lang, true);
            other.Cols = 50;
            other.Rows = 5;
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //
           
            Define_TBLT_RolesStatesWorkflow(workflow, ref template, bcpForm, applicantEmail);
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\TBLT_ContactForm_generared.xml");

            //string json = JsonConvert.SerializeObject(template);
            //File.WriteAllText("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.json", json);
        }

        public void TBlt_SubmitResourcesFormTest()
        {
            string lang = "en";
            string templateName = "Task-based Language Teaching Submit Suggest Resources Form Template";

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
            DataItem bcpForm = template.GetDataItem("Task-based Language Teaching Submit Suggest Resources Form", true, lang);
            bcpForm.IsRoot = true;
            bcpForm.SetDescription("This template is designed for Task-based Language Teaching Submit Suggest Resources Form", lang);

            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h1", "Submit Resources", lang);

            bcpForm.CreateField<TextField>("Name", lang, true);
            var applicantEmail = bcpForm.CreateField<TextField>("Email", lang, true);
            bcpForm.CreateField<TextField>("Title", lang, true);
            var other = bcpForm.CreateField<TextArea>("Short Description", lang, true);
            other.Cols = 50;
            other.Rows = 3;
            bcpForm.CreateField<AttachmentField>("Resource Item", lang, false);
            bcpForm.CreateField<TextField>("Link to Resource(s)", lang, false).SetDescription("Link to a Google document (docs, slides, spreadsheets, etc.) or other resources", lang);

            string[] keywords = new string[] { "keyword1", "keyword1" };//TODO: NEED TO REPLACE

            bcpForm.CreateField<CheckboxField>("Keywords for this resource(s)", lang, keywords,false);

            bcpForm.CreateField<TextField>("Suggested Keyword(s)", lang, false);


            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //

            Define_TBLT_RolesStatesWorkflow(workflow, ref template, bcpForm, applicantEmail, "SubmitResource");
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\TBLT_SubmitResourceForm_generared.xml");

            //string json = JsonConvert.SerializeObject(template);
            //File.WriteAllText("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.json", json);
        }


        private EmailTemplate CreateApplicantEmailTemplate(ref ItemTemplate template, string formName=null)
        {
            string lang = "en";
            EmailTemplate applicantNotification = template.GetEmailTemplate("Applicant Notification", lang, true);
            applicantNotification.SetDescription("This metadata set defines the email template to be sent to the applicant.", lang);
            string body = @"<p>Thank you very much for your interest in the TBLT community of practice. We will review your request and we will get back to you in the next few days.</p>   
                             <br/><p> Kind regards,</p>
                             <p>The leadership team</p> ";
            string subject = "Join Task-based Language Teaching";
            if (!string.IsNullOrEmpty(formName) && formName.Equals("SubmitResource"))
            {
                body = @"<p>Thank you very much for your resource(s) suggestion. We will review it and add to our collection.</p>   
                             <br/><p> Kind regards,</p>
                             <p>The leadership team</p>";
                subject = "Submit Resource(s)";
            }

           
            applicantNotification.SetSubject(subject);
            applicantNotification.SetBody(body);

            return applicantNotification;

        }

        private EmailTemplate CreateAdminEmailTemplate(ref ItemTemplate template, string formName=null)
        {
            string lang = "en";
            EmailTemplate applicantNotification = template.GetEmailTemplate("Admin Notification", lang, true);
            applicantNotification.SetDescription("This metadata set defines the email template to be sent to the portal admin.", lang);
           
            string body = "<p>An application to join the TBLT CoP has been received and is awaiting your approval.</p>";
            string subject = "Join TBLT CoP Request";
            if (!string.IsNullOrEmpty(formName) && formName.Equals("SubmitResource"))
            {
                body = "<p>Resources have been suggested and are awaiting your approval.</p>";
                subject = "Submit Resource(s)";
            }
      
            applicantNotification.SetSubject(subject);
            applicantNotification.SetBody(body);

            return applicantNotification;

        }
        private void Define_TBLT_RolesStatesWorkflow(Workflow workflow, ref ItemTemplate template,DataItem tbltForm, TextField applicantEmail, string formName=null)
        {
            IWorkflowService ws = _testHelper.WorkflowService;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            State emptyState = workflow.AddState(ws.GetStatus(template.Id, "", true));
            State submittedState = workflow.AddState(ws.GetStatus(template.Id, "Submitted", true));
            State deleteState = workflow.AddState(ws.GetStatus(template.Id, "Deleted", true));

            WorkflowRole adminRole = workflow.AddRole(auth.GetRole("Portal Admin", true));
            WorkflowRole memberRole = workflow.AddRole(auth.GetRole("Member", true));


            // Submitting an bcp form
            //Only safey inspectors can submit this form
            GetAction startSubmissionAction = workflow.AddAction("Start Submission", nameof(TemplateOperations.Instantiate), "Home");
            startSubmissionAction.Access = GetAction.eAccess.Public;
            startSubmissionAction.AddStateReferances(emptyState.Id)
                                 .AddOwnerAuthorization();
           
            //Listing bcp forms.
            //Admins and Inspectors can run the item-list report to list instances.
            //Note that the visibility of individual list entries is depend on the
            //Read permission on individual submissions.
            GetAction listSubmissionsAction = workflow.AddAction("List Submissions", nameof(TemplateOperations.ListInstances), "Home");
            listSubmissionsAction.Access = GetAction.eAccess.Restricted;
            listSubmissionsAction.AddStateReferances(submittedState.Id)
                  // .AddOwnerAuthorization()
                  .AddAuthorizedRole(memberRole.Id)
                  .AddAuthorizedRole(adminRole.Id);


            //Detailed submission bcp forms.
            //Inspectors can view their own submissions.
            //Admins can view all submissions.
            GetAction viewSubmissionAction = workflow.AddAction("Details", nameof(TemplateOperations.Read), "List");
            viewSubmissionAction.Access = GetAction.eAccess.Restricted;
            viewSubmissionAction.AddStateReferances(submittedState.Id)
               // .AddOwnerAuthorization()
               .AddAuthorizedRole(memberRole.Id)
                .AddAuthorizedRole(adminRole.Id);


            //Post action for submitting the form
            PostAction submitPostAction = startSubmissionAction.AddPostAction("Submit", nameof(TemplateOperations.Update));
            submitPostAction.AddStateMapping(emptyState.Id, submittedState.Id, "Submit");

            //Defining the pop-up for the above submitPostAction action
            PopUp submitActionPopUp = submitPostAction.AddPopUp("WARNING: Submitting the Form", "Once submitted, you cannot update the form.", "");
            submitActionPopUp.AddButtons("Yes, submit", "true");
            submitActionPopUp.AddButtons("Cancel", "false");
            //============================================================================
            //                                 EMAIL 
            //==============================================================================

            EmailTemplate applicantEmailTemplate = CreateApplicantEmailTemplate(ref template, formName);
            EmailTrigger applicantNotificationEmailTrigger = workflow.AddTrigger("ToApplicant", "SendEmail");
            applicantNotificationEmailTrigger.AddRecipientByDataField(tbltForm.Id, applicantEmail.Id);
            applicantNotificationEmailTrigger.AddTemplate(applicantEmailTemplate.Id, "Join TBLT Request Notification");

            EmailTemplate adminEmailTemplate = CreateAdminEmailTemplate(ref template, formName);

            EmailTrigger notificationEmailTrigger = workflow.AddTrigger("ToChair", "SendEmail");
            notificationEmailTrigger.AddRecipientByEmail("tblt@ualberta.ca");
            notificationEmailTrigger.AddTemplate(adminEmailTemplate.Id, "Join Request Notification");


            //Defining trigger refs
            submitPostAction.AddTriggerRefs("0", applicantNotificationEmailTrigger.Id, "Applicant's Notification Email Trigger", submittedState.Id, true);
            submitPostAction.AddTriggerRefs("1", notificationEmailTrigger.Id, "Admin's Notification Email Trigger", submittedState.Id, true);


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

    }
}
