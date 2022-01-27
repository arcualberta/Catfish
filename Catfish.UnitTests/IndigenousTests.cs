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
            string templateName = "Audio Recorder Form Template";

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
                
           var name = bcpForm.CreateField<TextField>("Title", lang,true);
            name.IsListEntryTitle = true;
            
            var country = bcpForm.CreateField<AudioRecorderField>("Record",lang, true, "mp3");
            

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
        public void AntiRacism_BlogSubmissionFormTest()
        {
            string lang = "en";
            string templateName = "Anti-racism de·col·o·nize blog submission form template";

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
            DataItem bcpForm = template.GetDataItem(templateName, true, lang);
            bcpForm.IsRoot = true;
            bcpForm.SetDescription("This template is designed for collecting the de·col·o·nize blog submission form", lang);

            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h1", "de·col·o·nize blog submission form", lang);
            bcpForm.CreateField<InfoSection>(null, null)
              .AppendContent("p", "To submit your entry for the Anti-racism Lab Blog, please fill out the form below", lang);

            var name = bcpForm.CreateField<TextField>("Name", lang, true);
            name.IsListEntryTitle = true;
            var email = bcpForm.CreateField<EmailField>("Email Address", lang, true);
            email.IsListEntryTitle = true;
            bcpForm.CreateField<TextField>("Faculty/Department", lang, true);

            string[] degrees = {"Undergraduate", "Masters","Doctoral","Professional Degree (i.e., Law, Medicine)" };
            bcpForm.CreateField<RadioField>("Degree Type", lang,degrees, true);
            bcpForm.CreateField<TextField>("Institution", lang, true);
            bcpForm.CreateField<TextField>("Country", lang, true);
            bcpForm.CreateField<TextField>("Please provide the title of your entry ", lang, true);

            var bio = bcpForm.CreateField<TextArea>("Please provide a short bio (max 150 words)", lang, true);
            bio.Cols = 50;
            bio.Rows = 5;
            var blog = bcpForm.CreateField<TextArea>("Please write a short description of your blog (150 words max)", lang, true);
            blog.Cols = 50;
            blog.Rows = 5;

           
            bcpForm.CreateField<AttachmentField>("Please attach a headshot of yourself", lang, true);

            bcpForm.CreateField<AttachmentField>("Please attach your blog submission below", lang, true);

            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("div", "Personal information provided is collected in accordance with Section 33(c) of the Alberta Freedom of Information and Protection of Privacy Act (the FOIP Act) and will be protected under Part 2 of that Act.", lang, "alert alert-info");

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //

            Define_AR_RolesStatesWorkflow1(workflow, ref template, bcpForm);
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\AntiRacism_blogSubmission.xml");

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
        private void Define_AR_RolesStatesWorkflow1(Workflow workflow, ref ItemTemplate template, DataItem wrForm)
        {
            IWorkflowService ws = _testHelper.WorkflowService;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            State emptyState = workflow.AddState(ws.GetStatus(template.Id, "", true));
            State submittedState = workflow.AddState(ws.GetStatus(template.Id, "Submitted", true));

            State deleteState = workflow.AddState(ws.GetStatus(template.Id, "Deleted", true));


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



            //Detailed submission bcp forms.
            //Inspectors can view their own submissions.
            //Admins can view all submissions.
            GetAction viewSubmissionAction = workflow.AddAction("Details", nameof(TemplateOperations.Read), "List");
            viewSubmissionAction.Access = GetAction.eAccess.Restricted;
            viewSubmissionAction.AddStateReferances(submittedState.Id)
             //  .AddOwnerAuthorization()
              .AddAuthorizedRole(adminRole.Id);
           
            // Edit submission related workflow items
            //Defining actions
            GetAction editSubmissionAction = workflow.AddAction("Edit Submission", nameof(TemplateOperations.Update), "Details");

            //Submissions can only be edited by admins
            editSubmissionAction.AddStateReferances(submittedState.Id)
                .AddAuthorizedRole(adminRole.Id);

            

            PostAction editPostActionSubmit = editSubmissionAction.AddPostAction("Submit", nameof(TemplateOperations.Update));//(Button Label, ActionName)
            editPostActionSubmit.AddStateMapping(submittedState.Id, submittedState.Id, "Submit");//current state, nectStae, buttonLabel
            //editPostActionSubmit.ValidateInputs = false;

            //Defining the pop-up for the above postActionSubmit action
            PopUp EditActionPopUpopUp = editPostActionSubmit.AddPopUp("Confirmation", "Do you really want to submit this document?", "Once submitted, you cannot update the document.");
            EditActionPopUpopUp.AddButtons("Yes, submit", "true");
            EditActionPopUpopUp.AddButtons("Cancel", "false");


        }

    }
}
