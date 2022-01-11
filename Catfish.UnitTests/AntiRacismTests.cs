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
    public class AntiRacismTests
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
        public void AntiRacism_UserExperienceFormTest()
        {
            string lang = "en";
            string templateName = "Anti-racism user experience Form Template";

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
            DataItem bcpForm = template.GetDataItem("Anti-racism user experience Form", true, lang);
            bcpForm.IsRoot = true;
            bcpForm.SetDescription("This template is designed for collecting user eperience about racism", lang);

            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h1", "Anti-racism Lab: Documenting experiences of racism", lang)
                 .AppendContent("p", "Please use this form to write about your everyday experiences of racism at the university. This is the online diary which we will ask you to keep doing for the next  6 months so that we can get an idea about the race climate of the university. Your everyday experiences can be racism experienced from other students, faculty and administrators. Try and write as fully as you can about the incident: what happened; who else was involved; what did/could you do; did you get any support from others who observed it; did you know where to go in the university for support; is there anywhere/anyone to go to for support and how effective was it in helping you?", lang)
                 .AppendContent("p", "Please use the same pseudonym each time you write about experiences and, if possible, please record incidents as they happen so that it will be a good reflection of events. Thanks a lot for particpating.", lang);

           var name = bcpForm.CreateField<TextField>("Please provide a pseudonym which you'd like to have assigned to your experience.", lang,true);
            name.IsListEntryTitle = true;
            
            var country = bcpForm.CreateField<TextField>("What country are you in?", lang, true);
            country.IsListEntryTitle = true;
            var other =bcpForm.CreateField<TextArea>("Please describe your experience?", lang, true);
            other.Cols = 50;
            other.Rows = 5;

            bcpForm.CreateField<DateField>("What date this incident occur?", lang, true);

            bcpForm.CreateField<InfoSection>(null, null)
                 //.AppendContent("div", @"Please take a moment to read and sign the consent form provided here (<a href='https://drive.google.com/file/d/15qYj61n1t93PPd6gGB5ru8QE8DE_bRsD/view?usp=sharing' target='_blank'>https://drive.google.com/file/d/15qYj61n1t93PPd6gGB5ru8QE8DE_bRsD/view?usp=sharing</a>). Please attach the signed copy below", lang, "alert alert-info");
                  .AppendContent("div", @"Please take a moment to read the consent form provided here (<a href='https://drive.google.com/file/d/15qYj61n1t93PPd6gGB5ru8QE8DE_bRsD/view?usp=sharing' target='_blank'>https://drive.google.com/file/d/15qYj61n1t93PPd6gGB5ru8QE8DE_bRsD/view?usp=sharing</a>)", lang, "alert alert-info");


            // bcpForm.CreateField<AttachmentField>("Upload Consent Form", lang, true);
            string[] permission = new string[] { "Yes" };
            //var confParticipation = sasForm.CreateField<CheckboxField>("Conference Participation", lang, participationRoles);
            bcpForm.CreateField<CheckboxField>("I have read and agree to the details of the consent form.", lang,permission, true);
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //

            Define_AR_RolesStatesWorkflow1(workflow, ref template, bcpForm);
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\AntiRacism_generared.xml");

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


            WorkflowRole adminRole = workflow.AddRole(auth.GetRole("Antiracism_Admin", true));
           

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
