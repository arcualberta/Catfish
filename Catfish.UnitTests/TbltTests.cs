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

        [Test]
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
            var goal = bcpForm.CreateField<TextArea>("Goal of the task", lang, true);
            goal.Cols = 50;
            goal.Rows = 3;


            bcpForm.CreateField<AttachmentField>("Resource Item", lang, false);
            bcpForm.CreateField<TextField>("Link to Resource(s)", lang, false).SetDescription("Link to a Google document (docs, slides, spreadsheets, etc.) or other resources", lang);

            string[] keywords = new string[] { "keyword1", "keyword2" };//TODO: NEED TO REPLACE

            bcpForm.CreateField<CheckboxField>("Keywords for this resource(s)", lang, keywords,false);

            bcpForm.CreateField<TextField>("Suggested Keyword(s)", lang, false);

            string[] consent = new string[] { "I confirm that I have obtained all necessary permissions to post the materials on tblt.ualberta.ca and I grant the TBLT CoP permission to host these materials." };
            bcpForm.CreateField<CheckboxField>("", lang, consent, true);

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

        private EmailTemplate CreateEditorEmailTemplate(ref ItemTemplate template, string formName=null)
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
            State savedState = workflow.AddState(ws.GetStatus(template.Id, "Saved", true));
            State submittedState = workflow.AddState(ws.GetStatus(template.Id, "Submitted", true));
            State deleteState = workflow.AddState(ws.GetStatus(template.Id, "Deleted", true));
            State approvedState = workflow.AddState(ws.GetStatus(template.Id, "Approved", true));
            State rejectedState = workflow.AddState(ws.GetStatus(template.Id, "Rejected", true));

            WorkflowRole editorRole = workflow.AddRole(auth.GetRole("Editor", true));
            WorkflowRole memberRole = workflow.AddRole(auth.GetRole("Member", true));

            //============================================================================
            //                                 EMAIL 
            //==============================================================================

            EmailTemplate applicantEmailTemplate = CreateApplicantEmailTemplate(ref template, formName);
            EmailTrigger applicantNotificationEmailTrigger = workflow.AddTrigger("ToApplicant", "SendEmail");
            applicantNotificationEmailTrigger.AddRecipientByDataField(tbltForm.Id, applicantEmail.Id);
            applicantNotificationEmailTrigger.AddTemplate(applicantEmailTemplate.Id, "Join TBLT Request Notification");

            EmailTemplate adminEmailTemplate = CreateEditorEmailTemplate(ref template, formName);

            EmailTrigger editorNotificationEmailTrigger = workflow.AddTrigger("ToEditor", "SendEmail");
            editorNotificationEmailTrigger.AddRecipientByEmail("tblt@ualberta.ca");
            editorNotificationEmailTrigger.AddTemplate(adminEmailTemplate.Id, "Join Request Notification");

            // =======================================
            // start submission related workflow items
            // =======================================

            //Defining actions
            GetAction startSubmissionAction = workflow.AddAction("Start Submission", nameof(TemplateOperations.Instantiate), "Home");

            startSubmissionAction.Access = GetAction.eAccess.Restricted;

            //Defining form template
            startSubmissionAction.AddTemplate(tbltForm.Id, "Task-based Language Teaching Submit Suggest Resources Form");

            //Defining post actions
            PostAction savePostAction = startSubmissionAction.AddPostAction("Save", nameof(TemplateOperations.Update),
                                                                            @"<p>Your TBLT application saved successfully. 
                                                                                You can view/edit by <a href='@SiteUrl/items/@Item.Id'>click on here</a></p>");
            savePostAction.ValidateInputs = false;
            savePostAction.AddStateMapping(emptyState.Id, savedState.Id, "Save");

            PostAction submitPostAction = startSubmissionAction.AddPostAction("Submit", nameof(TemplateOperations.Update),
                                                                                 @"<p>Thank you for submitting your TBLT application. 
                                                                                    Your editor has been automatically notified to provide an assessment about your application.
                                                                                 You can view your application and it's status by <a href='@SiteUrl/items/@Item.Id'> click on here. </a></p>");
            submitPostAction.AddStateMapping(emptyState.Id, submittedState.Id, "Submit");


            //Defining the pop-up for the above postActionSubmit action
            PopUp submitActionPopUp = submitPostAction.AddPopUp("Confirmation", "Do you really want to submit this document?", "Once submitted, you cannot update the document.");
            submitActionPopUp.AddButtons("Yes, submit", "true");
            submitActionPopUp.AddButtons("Cancel", "false");

            //Defining trigger refs
            submitPostAction.AddTriggerRefs("0", editorNotificationEmailTrigger.Id, "Editor's Notification Email Trigger");
            submitPostAction.AddTriggerRefs("1", applicantNotificationEmailTrigger.Id, "Owner Submission-notification Email Trigger");

            startSubmissionAction.AddStateReferances(emptyState.Id)
                .AddAuthorizedRole(memberRole.Id);


            // ================================================
            // List submission-instances related workflow items
            // ================================================
            GetAction listSubmissionsAction = workflow.AddAction("List Submissions", nameof(TemplateOperations.ListInstances), "Home");
            listSubmissionsAction.Access = GetAction.eAccess.Restricted;

            // Added state referances
            listSubmissionsAction.AddStateReferances(savedState.Id)
                .AddAuthorizedRole(editorRole.Id)
                .AddOwnerAuthorization();
            listSubmissionsAction.AddStateReferances(submittedState.Id)
                .AddAuthorizedRole(editorRole.Id)
                .AddOwnerAuthorization();
            listSubmissionsAction.AddStateReferances(approvedState.Id)
                .AddAuthorizedRole(editorRole.Id)
                .AddOwnerAuthorization();
            listSubmissionsAction.AddStateReferances(rejectedState.Id)
                .AddAuthorizedRole(editorRole.Id)
                .AddOwnerAuthorization();

            // ================================================
            // Read submission-instances related workflow items
            // ================================================

            //Defining actions
            GetAction viewDetailsSubmissionAction = workflow.AddAction("Details", nameof(TemplateOperations.Read), "List");

            viewDetailsSubmissionAction.Access = GetAction.eAccess.Restricted;

            // Added state referances
            viewDetailsSubmissionAction.AddStateReferances(savedState.Id)
                .AddAuthorizedRole(editorRole.Id)
                .AddOwnerAuthorization();
            listSubmissionsAction.AddStateReferances(submittedState.Id)
                .AddAuthorizedRole(editorRole.Id)
                .AddOwnerAuthorization();
            listSubmissionsAction.AddStateReferances(approvedState.Id)
                .AddAuthorizedRole(editorRole.Id)
                .AddOwnerAuthorization();
            listSubmissionsAction.AddStateReferances(rejectedState.Id)
                .AddAuthorizedRole(editorRole.Id)
                .AddOwnerAuthorization();


            // ================================================
            // Edit submission-instances related workflow items
            // ================================================
            GetAction editSubmissionAction = workflow.AddAction("Edit Submission", nameof(TemplateOperations.Update), "Details");
            editSubmissionAction.Access = GetAction.eAccess.Restricted;

            //Defining post actions
            PostAction editSubmissionPostActionSave = editSubmissionAction.AddPostAction("Save",
                                                                                        nameof(TemplateOperations.Update),
                                                                                        @"<p>Your TBLT application saved successfully. 
                                                                                            You can view/edit by <a href='@SiteUrl/items/@Item.Id'>click on here</a></p>");
            editSubmissionPostActionSave.ValidateInputs = false;
            PostAction editSubmissionPostActionSubmit = editSubmissionAction.AddPostAction("Submit",
                                                                                            nameof(TemplateOperations.Update),
                                                                                             @"<p>Thank you for submitting your TBLT application. 
                                                                                                Your editor has been automatically notified to provide an assessment about your application.
                                                                                             You can view your application and it's status by <a href='@SiteUrl/items/@Item.Id'> click on here. </a></p>");
            //Defining state mappings
            editSubmissionPostActionSave.AddStateMapping(savedState.Id, savedState.Id, "Save");
            editSubmissionPostActionSubmit.AddStateMapping(savedState.Id, submittedState.Id, "Submit");
            editSubmissionPostActionSave.AddStateMapping(submittedState.Id, savedState.Id, "Save");
            editSubmissionPostActionSubmit.AddStateMapping(submittedState.Id, submittedState.Id, "Submit");

            //Defining the pop-up for the above postActionSubmit action
            PopUp EditSubmissionActionPopUpopUp = editSubmissionPostActionSubmit.AddPopUp("Confirmation", "Do you really want to submit this document?", "Once submitted, you cannot update the document.");
            EditSubmissionActionPopUpopUp.AddButtons("Yes, submit", "true");
            EditSubmissionActionPopUpopUp.AddButtons("Cancel", "false");

            //Defining trigger refs
            //*******To Do*******
            // Implement a function to restrict the e-mail triggers when SAS Admin updated the document
            editSubmissionPostActionSubmit.AddTriggerRefs("0", editorNotificationEmailTrigger.Id, "Editor's Notification Email Trigger");
            editSubmissionPostActionSubmit.AddTriggerRefs("1", applicantNotificationEmailTrigger.Id, "Owner Submission-notification Email Trigger");

            //Defining state referances
            editSubmissionAction.GetStateReference(savedState.Id, true)
                .AddOwnerAuthorization();
            editSubmissionAction.GetStateReference(submittedState.Id, true)
                .AddAuthorizedRole(editorRole.Id);

            
            
            // ================================================
            // Delete submission-instances related workflow items
            // ================================================

            GetAction deleteSubmissionAction = workflow.AddAction("Delete Submission", nameof(CrudOperations.Delete), "Details");
            deleteSubmissionAction.Access = GetAction.eAccess.Restricted;

            //Defining post actions
            PostAction deleteSubmissionPostAction = deleteSubmissionAction.AddPostAction("Delete", "Save");
            deleteSubmissionPostAction.ValidateInputs = false;

            //Defining state mappings
            ////////deleteSubmissionPostAction.AddStateMapping(savedState.Id, deleteState.Id, "Delete");
            deleteSubmissionPostAction.AddStateMapping(rejectedState.Id, deleteState.Id, "Delete");

            //Defining the pop-up for the above postAction action
            PopUp deleteSubmissionActionPopUpopUp = deleteSubmissionPostAction.AddPopUp("Confirmation", "Do you really want to delete this document?", "Once deleted, you cannot access this document.");
            deleteSubmissionActionPopUpopUp.AddButtons("Yes, delete", "true");
            deleteSubmissionActionPopUpopUp.AddButtons("Cancel", "false");

            //Defining state referances
            ////////deleteSubmissionAction.GetStateReference(savedState.Id, true)
            ////////    .AddOwnerAuthorization();
            deleteSubmissionAction.GetStateReference(rejectedState.Id, true)
                .AddAuthorizedRole(editorRole.Id);

            // ================================================
            // Change State submission-instances related workflow items
            // ================================================

            //GetAction changeStateAction = workflow.AddAction("Update Document State", nameof(TemplateOperations.ChangeState), "Details");
            //changeStateAction.Access = GetAction.eAccess.Restricted;

            ////Define Revision Template
            //changeStateAction.AddTemplate(additionalNoteForm.Id, "Submission Change State");
            ////Defining post actions
            //PostAction changeStatePostAction = changeStateAction.AddPostAction("Change State", @"<p>Application status changed successfully. 
            //                                                                    You can view the document by <a href='@SiteUrl/items/@Item.Id'>click on here</a></p>");

            ////Defining state mappings
            //changeStatePostAction.AddStateMapping(reviewCompletedState.Id, inAdjudicationState.Id, "With Adjudication");

            ////Defining the pop-up for the above sendForRevisionSubmissionPostAction action
            //PopUp changeStateActionPopUpopUp = changeStatePostAction.AddPopUp("Confirmation", "Do you really want to change status ? ", "Once changed, you cannot revise this document.");
            //changeStateActionPopUpopUp.AddButtons("Yes", "true");
            //changeStateActionPopUpopUp.AddButtons("Cancel", "false");

            ////Defining states and their authorizatios
            //changeStateAction.GetStateReference(reviewCompletedState.Id, true)
            //    .AddAuthorizedRole(sasAdmin.Id);
            //changeStateAction.GetStateReference(inReviewState.Id, true)
            //    .AddAuthorizedRole(sasChair.Id);
            //changeStateAction.GetStateReference(inAdjudicationState.Id, true)
            //    .AddAuthorizedRole(sasAdmin.Id);

        }

    }
}
