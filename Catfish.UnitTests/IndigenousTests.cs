using Catfish.Core.Authorization.Requirements;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
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
    public class IndigenousTests
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
        public void AudioRecordingFormTest()
        {
            string lang = "en";
            string templateName = "Audio Recorder Form Template 2";

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
            DataItem bcpForm = template.GetDataItem("Audio Recorder Form", true, lang);
            bcpForm.IsRoot = true;
            bcpForm.SetDescription("This template is designed for collecting metadata for audio recording object", lang);

            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h1", "Audio Recorder", lang);
                
           var name = bcpForm.CreateField<TextField>("Your Name", lang,true);
            name.IsListEntryTitle = true;
            bcpForm.CreateField<TextField>("Your Email or Phone Number", lang, true);
            bcpForm.CreateField<TextField>("Your Story Title", lang, true);
            var description = bcpForm.CreateField<TextArea>("Please tell usa little bit about your story", lang, true);
            description.Cols = 50;
            description.Rows = 2;

            bcpForm.CreateField<AudioRecorderField>("Record",lang, false, "mp3");

            bcpForm.CreateField<InfoSection>("<div>The fields below are optional</div>", lang, "alert alert-info");

            string[] languages = new string[] { "Inuvialuktun", "English" };
            bcpForm.CreateField<CheckboxField>("Language", lang, languages,false);

            string[] dialect = new string[] { "Kangiryuarmiutun", "Uummarmiutun", "Sallirmiutun" };
            bcpForm.CreateField<CheckboxField>("Language", lang, dialect, false);

            bcpForm.CreateField<TextField>("Subject", lang, false);
            bcpForm.CreateField<TextField>("People", lang, false);
            bcpForm.CreateField<TextField>("Places", lang, false);

            string[] types = new string[] { "Audio", "Video", "Image", "Text" };
            bcpForm.CreateField<RadioField>("Type", lang, types, false);

            //  bcpForm.CreateField<AttachmentField>("Please attach a headshot of yourself", lang, false);

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //

            Define_AR_RolesStatesWorkflow1(workflow, ref template, bcpForm);
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\AudioRecording_generared.xml");

            //string json = JsonConvert.SerializeObject(template);
            //File.WriteAllText("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.json", json);
        }
        [Test]
        public void AttachmentFieldFormTest()
        {
            string lang = "en";
            string templateName = "Testing Attachment Field Form Template";

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
            DataItem bcpForm = template.GetDataItem("Testing AttachmentField Form", true, lang);
            bcpForm.IsRoot = true;
            bcpForm.SetDescription("This template is designed for collecting metadata for testing attachmentField ", lang);

            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h3", "Testing AttachmentField", lang);

            var name = bcpForm.CreateField<TextField>("Title", lang, false);
            name.IsListEntryTitle = true;

          
            bcpForm.CreateField<AttachmentField>("Please attach a headshot of yourself", lang, false);

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //

            Define_AR_RolesStatesWorkflow1(workflow, ref template, bcpForm);
             db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\testingAttachmentField_generared.xml");

           
        }


        //private EmailTemplate CreateApplicantEmailTemplate(ref ItemTemplate template, string formName=null)
        //{
        //    string lang = "en";
        //    EmailTemplate applicantNotification = template.GetEmailTemplate("Applicant Notification", lang, true);
        //    applicantNotification.SetDescription("This metadata set defines the email template to be sent to the applicant.", lang);
        //    string body = @"<p>Thank you very much for your interest in the TBLT community of practice. We will review your request and we will get back to you in the next few days.</p>   
        //                     <br/><p> Kind regards,</p>
        //                     <p>The leadership team</p> ";
        //    string subject = "Join Task-based Language Teaching";
        //    if (!string.IsNullOrEmpty(formName) && formName.Equals("SubmitResource"))
        //    {
        //        body = @"<p>Thank you very much for your resource(s) suggestion. We will review it and add to our collection.</p>   
        //                     <br/><p> Kind regards,</p>
        //                     <p>The leadership team</p>";
        //        subject = "Submit Resource(s)";
        //    }

           
        //    applicantNotification.SetSubject(subject);
        //    applicantNotification.SetBody(body);

        //    return applicantNotification;

        //}

        //private EmailTemplate CreateEditorEmailTemplate(ref ItemTemplate template, string formName=null)
        //{
        //    string lang = "en";
        //    EmailTemplate applicantNotification = template.GetEmailTemplate("Admin Notification", lang, true);
        //    applicantNotification.SetDescription("This metadata set defines the email template to be sent to the portal admin.", lang);
           
        //    string body = "<p>An application to join the TBLT CoP has been received and is awaiting your approval.</p>";
        //    string subject = "Join TBLT CoP Request";
        //    if (!string.IsNullOrEmpty(formName) && formName.Equals("SubmitResource"))
        //    {
        //        body = "<p>Resources have been suggested and are awaiting your approval.</p>";
        //        subject = "Submit Resource(s)";
        //    }
      
        //    applicantNotification.SetSubject(subject);
        //    applicantNotification.SetBody(body);

        //    return applicantNotification;

        //}
        private void Define_AR_RolesStatesWorkflow1(Workflow workflow, ref ItemTemplate template, DataItem wrForm)
        {
            IWorkflowService ws = _testHelper.WorkflowService;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            State emptyState = workflow.AddState(ws.GetStatus(template.Id, "", true));
            State submittedState = workflow.AddState(ws.GetStatus(template.Id, "Submitted", true));
            State deleteState = workflow.AddState(ws.GetStatus(template.Id, "Deleted", true));
            State approvedState = workflow.AddState(ws.GetStatus(template.Id, "Approved", true));
            State rejectedState = workflow.AddState(ws.GetStatus(template.Id, "Rejected", true));

            WorkflowRole adminRole = workflow.AddRole(auth.GetRole("Portal_Admin", true));


            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                     Submitting an form
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///
            GetAction startSubmissionAction = workflow.AddAction("Start Submission", nameof(TemplateOperations.Instantiate), "Home");
            startSubmissionAction.Access = GetAction.eAccess.Public;
            // Added state referances
            startSubmissionAction.AddStateReferances(emptyState.Id);


            //Defining form template
            startSubmissionAction.AddTemplate(wrForm.Id, "Start Submission Template");
            //Defining post actions

            PostAction submitPostAction = startSubmissionAction.AddPostAction("Submit", nameof(TemplateOperations.Update));
            submitPostAction.AddStateMapping(emptyState.Id, submittedState.Id, "Submit");

            //Defining the pop-up for the above postActionSubmit action
            PopUp submitActionPopUp = submitPostAction.AddPopUp("Confirmation", "Do you really want to submit this document?", "Once submitted, you cannot update the document.");
            submitActionPopUp.AddButtons("Yes, submit", "true");
            submitActionPopUp.AddButtons("Cancel", "false");

            //Defining trigger refs

            //EmailTemplate applicantEmailTemplate = CreateApplicantApplicationEmailTemplate(ref template);
            //EmailTrigger applicantNotificationEmailTrigger = workflow.AddTrigger("ToApplicant", "SendEmail");
            //applicantNotificationEmailTrigger.AddRecipientByDataField(wrForm.Id, applicantEmail.Id);
            //applicantNotificationEmailTrigger.AddTemplate(applicantEmailTemplate.Id, "Writer-in-Resident Application Notification");


            ////Defining trigger refs
            //submitPostAction.AddTriggerRefs("0", applicantNotificationEmailTrigger.Id, "Applicant's Notification Email Trigger", submittedState.Id, true);




            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                        Listing forms.
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///

            //Admins and Inspectors can run the item-list report to list instances.
            //Note that the visibility of individual list entries is depend on the
            //Read permission on individual submissions.
            GetAction listSubmissionsAction = workflow.AddAction("List Submissions", nameof(TemplateOperations.ListInstances), "Home");
            listSubmissionsAction.Access = GetAction.eAccess.Restricted;
            // Added state referances
            listSubmissionsAction.AddStateReferances(submittedState.Id)
                  //.AddOwnerAuthorization()
                  .AddAuthorizedRole(adminRole.Id);
            listSubmissionsAction.AddStateReferances(approvedState.Id)
                .AddAuthorizedRole(adminRole.Id);
            listSubmissionsAction.AddStateReferances(rejectedState.Id)
                .AddAuthorizedRole(adminRole.Id);

            //Detailed submission bcp forms.
            //Inspectors can view their own submissions.
            //Admins can view all submissions.
            GetAction viewSubmissionAction = workflow.AddAction("Details", nameof(TemplateOperations.Read), "List");
            viewSubmissionAction.Access = GetAction.eAccess.Restricted;
            viewSubmissionAction.AddStateReferances(submittedState.Id)
              .AddAuthorizedRole(adminRole.Id);
            viewSubmissionAction.AddStateReferances(approvedState.Id)
             .AddAuthorizedRole(adminRole.Id);
            viewSubmissionAction.AddStateReferances(rejectedState.Id)
                .AddAuthorizedRole(adminRole.Id);

            // Edit submission related workflow items
            //Defining actions
            GetAction editSubmissionAction = workflow.AddAction("Edit Submission", nameof(TemplateOperations.Update), "Details");

            //Submissions can only be edited by admins
            editSubmissionAction.AddStateReferances(submittedState.Id)
                .AddAuthorizedRole(adminRole.Id);
          

            PostAction editPostActionSubmit = editSubmissionAction.AddPostAction("Submit", nameof(TemplateOperations.Update));//(Button Label, ActionName)

            //ASK Isuru-- if the button label need to be "SUBMIT"
            editPostActionSubmit.AddStateMapping(submittedState.Id, submittedState.Id, "Submit");
            //editPostActionSubmit.ValidateInputs = false;

            //Defining the pop-up for the above postActionSubmit action
            PopUp EditActionPopUpopUp = editPostActionSubmit.AddPopUp("Confirmation", "Do you really want to submit this document?", "Once submitted, you cannot update the document.");
            EditActionPopUpopUp.AddButtons("Yes, submit", "true");
            EditActionPopUpopUp.AddButtons("Cancel", "false");

            editSubmissionAction.AddStateReferances(submittedState.Id)
              .AddAuthorizedRole(adminRole.Id);

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
                .AddAuthorizedRole(adminRole.Id);

            // ================================================
            // Change State submission-instances related workflow items
            // ================================================

            GetAction changeStateAction = workflow.AddAction("Update Document State", nameof(TemplateOperations.ChangeState), "Details");
            changeStateAction.Access = GetAction.eAccess.Restricted;

            
            //Defining post actions
            PostAction changeStatePostAction = changeStateAction.AddPostAction("Change State", @"<p>Application status changed successfully. </p>");

            //Defining state mappings
            changeStatePostAction.AddStateMapping(submittedState.Id, approvedState.Id, "Approve");
            changeStatePostAction.AddStateMapping(submittedState.Id, rejectedState.Id, "Reject");

            //Defining the pop-up for the above sendForRevisionSubmissionPostAction action
            PopUp changeStateActionPopUpopUp = changeStatePostAction.AddPopUp("Confirmation", "Do you really want to change status ? ", "Once changed, you cannot revise this document.");
            changeStateActionPopUpopUp.AddButtons("Yes", "true");
            changeStateActionPopUpopUp.AddButtons("Cancel", "false");

            //Defining states and their authorizatios
            changeStateAction.GetStateReference(submittedState.Id, true)
                .AddAuthorizedRole(adminRole.Id);

        }

    }
}
