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
                 .AppendContent("h1", "Join Us", lang)
                 .AppendContent("p", "<i>If you are interested in becoming a member of the TBLT CoP, please fill out this form.</ i>", lang);
           
            bcpForm.CreateField<TextField>("Name", lang,true);
            var applicantEmail= bcpForm.CreateField<EmailField>("Email", lang, true);
            bcpForm.CreateField<TextField>("Affiliation", lang, true);
            var other =bcpForm.CreateField<TextArea>(@"Please, tell us what language(s) you teach and what age groups (e.g., elementary, secondary, college/university, adults)", lang, true);
            other.Cols = 50;
            other.Rows = 5;

            bcpForm.CreateField<CheckboxField>("", lang, new string[] { "I confirm that I have read the <a href='https://tblt.ualberta.ca/foip-notification-statement'>FOIP Notification Statement</a> and the <a href='https://tblt.ualberta.ca/terms-of-use'>Terms of Use</a> of this website, as well as the applicable University of Alberta’s Policies linked in them, and that I agree to their terms." }, true);

            //Defininig the Comments form
            DataItem commentsForm = template.GetDataItem("TBLT Comment Form", true, lang);
            commentsForm.IsRoot = false;
            commentsForm.SetDescription("This is the form to be filled by theeditor when make a decision.", lang);
            commentsForm.CreateField<TextArea>("Comments", lang, true);
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //

            Define_TBLT_ContactWorkflow(workflow, ref template, bcpForm, commentsForm, applicantEmail);
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
            string _metadatsetName = "TBLT Category Metadata";

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

            //Get the Workflow object using the workflow service
            Workflow workflow = template.Workflow;

            MetadataSet keywordMeta = template.GetMetadataSet(_metadatsetName, true, lang);
            keywordMeta.IsTemplate = false;
            string[] proficientcy = GetProficientcyLevel();
            keywordMeta.CreateField<CheckboxField>("Proficiency level", lang,proficientcy, true);
            string[] langSkills = GetLanguageSkills();
            keywordMeta.CreateField<CheckboxField>("Language skills", lang, langSkills, true);
            string[] modes = GetDeliveryModes();
            keywordMeta.CreateField<CheckboxField>("Mode", lang, modes, true);


            //Defininig the submission form
            DataItem bcpForm = template.GetDataItem("Task-based Language Teaching Submit Suggest Resources Form", true, lang);
            bcpForm.IsRoot = true;
            bcpForm.SetDescription("This template is designed for Task-based Language Teaching Submit Suggest Resources Form", lang);

            var name = bcpForm.CreateField<TextField>("Your name", lang, true);
            name.IsListEntryTitle = true;
            var applicantEmail = bcpForm.CreateField<EmailField>("Email address", lang, true);
            var title =bcpForm.CreateField<TextField>("Title of task", lang, true);
            title.IsListEntryTitle = true;

            var other = bcpForm.CreateField<TextArea>("Short description", lang, true);
            other.Cols = 50;
            other.Rows = 3;
            var goal = bcpForm.CreateField<TextArea>("Goal of the task", lang, true);
            goal.Cols = 50;
            goal.Rows = 3;

            string[] ageOptions = new string[] { "3-5", "6-9", "10-12", "13-15", "16-18", "adults" };
            bcpForm.CreateField<CheckboxField>("Age range of learners", lang, ageOptions, false);

            bcpForm.CreateField<TextField>("Language", lang, true);
            bcpForm.CreateField<TextField>("Topic/theme/content", lang, true);
            bcpForm.CreateField<TextField>("Grammar feature", lang, false);

            bcpForm.CreateField<AttachmentField>("Resource Item", lang, false);
            bcpForm.CreateField<TextField>("Link to Resource(s)", lang, false).SetDescription("Link to a Google document (docs, slides, spreadsheets, etc.) or other resources", lang);

            bcpForm.CreateField<FieldContainerReference>("Please, select the appropriate keywords that best describe your resource", lang,
                FieldContainerReference.eRefType.metadata, keywordMeta.Id);

            var additionalKeywords = bcpForm.CreateField<TextArea>("Additional keywords", lang, false);
            additionalKeywords.SetDescription("Any new keywords you would like to suggest for existing or new categories", lang);
            additionalKeywords.Cols = 50;
            additionalKeywords.Rows = 5;

            bcpForm.CreateField<CheckboxField>("Permission to publish", lang, new string[] { "Yes, I confirm" },true)
                .SetDescription("Please, confirm that you own this material and/or that you have obtained all necessary permissions to post the material on the TBLT CoP Website", lang);
            bcpForm.CreateField<CheckboxField>("Permission to use", lang, new string[] { "Yes, I confirm" }, true)
                .SetDescription("Please confirm that you grant the TBLT CoP a <a href='https://creativecommons.org/licenses/by-nc-sa/4.0/'>Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International (CC BY-NC-SA 4.0) license</a> to use this material (except in those cases where an alternative license has been indicated by you on the applicable material).", lang);


            //Defininig the Comments form
            DataItem commentsForm = template.GetDataItem("TBLT Comment Form", true, lang);
            commentsForm.IsRoot = false;
            commentsForm.SetDescription("This is the form to be filled by theeditor when make a decision.", lang);
            commentsForm.CreateField<TextArea>("Comments", lang, true);

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //

            Define_TBLT_RolesStatesWorkflow(workflow, ref template, bcpForm, commentsForm, applicantEmail, "SubmitResource");
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
        private void Define_TBLT_RolesStatesWorkflow(Workflow workflow, ref ItemTemplate template,DataItem tbltForm, DataItem commentsForm, EmailField applicantEmail=null, string formName=null)
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
            if (applicantEmail != null)
            {
               
                applicantNotificationEmailTrigger.AddRecipientByDataField(tbltForm.Id, applicantEmail.Id);
                applicantNotificationEmailTrigger.AddTemplate(applicantEmailTemplate.Id, "Join TBLT Request Notification");
            }
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

            ////Defining post actions
            ////PostAction savePostAction = startSubmissionAction.AddPostAction("Save", nameof(TemplateOperations.Update),
            ////                                                                @"<p>Thank you for saving your resource to the Task-based Language Teaching resource collection. 
            ////                                                                        Your submission should be view/edit at the <a href='@SiteUrl/resources'>resources page.</a></p>");
            ////savePostAction.ValidateInputs = false;
            ////savePostAction.AddStateMapping(emptyState.Id, savedState.Id, "Save");

            PostAction submitPostAction = startSubmissionAction.AddPostAction("Submit", nameof(TemplateOperations.Update),
                                                                                 @"<p>Thank you for submitting your resource to the Task-based Language Teaching resource collection. 
                                                                                    Your submission should be visible at the <a href='@SiteUrl/resources/@Item.Id'> resources page. </a></p>");
            submitPostAction.AddStateMapping(emptyState.Id, submittedState.Id, "Submit");


            //Defining the pop-up for the above postActionSubmit action
            PopUp submitActionPopUp = submitPostAction.AddPopUp("Confirm Submission", "Do you want to submit this resource to the resource collection? Once submitted, you cannot edit it.", "");
            submitActionPopUp.AddButtons("Yes, submit", "true");
            submitActionPopUp.AddButtons("Cancel", "false");

            //Defining trigger refs
            submitPostAction.AddTriggerRefs("0", editorNotificationEmailTrigger.Id, "Editor's Notification Email Trigger");
            if (applicantEmail != null)
            {
                submitPostAction.AddTriggerRefs("1", applicantNotificationEmailTrigger.Id, "Owner Submission-notification Email Trigger");
            }

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
            viewDetailsSubmissionAction.AddStateReferances(submittedState.Id)
                .AddAuthorizedRole(editorRole.Id)
                .AddOwnerAuthorization();
            viewDetailsSubmissionAction.AddStateReferances(approvedState.Id)
                .AddAuthorizedRole(editorRole.Id)
                .AddOwnerAuthorization();
            viewDetailsSubmissionAction.AddStateReferances(rejectedState.Id)
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
            if (applicantEmail != null)
            {
                 editSubmissionPostActionSubmit.AddTriggerRefs("1", applicantNotificationEmailTrigger.Id, "Owner Submission-notification Email Trigger");
            }

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

            GetAction changeStateAction = workflow.AddAction("Update Document State", nameof(TemplateOperations.ChangeState), "Details");
            changeStateAction.Access = GetAction.eAccess.Restricted;

            //Define Revision Template
            changeStateAction.AddTemplate(commentsForm.Id, "Comments");
            //Defining post actions
            PostAction changeStatePostAction = changeStateAction.AddPostAction("Change State", @"<p>Application status changed successfully. 
                                                                                You can view the document by <a href='@SiteUrl/items/@Item.Id'>click on here</a></p>");

            //Defining state mappings
            changeStatePostAction.AddStateMapping(submittedState.Id, approvedState.Id, "Approve");
            changeStatePostAction.AddStateMapping(submittedState.Id, rejectedState.Id, "Reject");

            //Defining the pop-up for the above sendForRevisionSubmissionPostAction action
            PopUp adjudicationDecisionPopUpopUp = changeStatePostAction.AddPopUp("Confirmation", "Do you really want to make a decision ? ", "Once changed, you cannot revise this document.");
            adjudicationDecisionPopUpopUp.AddButtons("Yes", "true");
            adjudicationDecisionPopUpopUp.AddButtons("Cancel", "false");

            //Defining states and their authorizatios
            changeStateAction.GetStateReference(submittedState.Id, true)
                .AddAuthorizedRole(editorRole.Id); 

        }
        private void Define_TBLT_ContactWorkflow(Workflow workflow, ref ItemTemplate template, DataItem tbltForm, DataItem commentsForm, EmailField applicantEmail = null, string formName = null)
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
            if (applicantEmail != null)
            {

                applicantNotificationEmailTrigger.AddRecipientByDataField(tbltForm.Id, applicantEmail.Id);
                applicantNotificationEmailTrigger.AddTemplate(applicantEmailTemplate.Id, "Join TBLT Request Notification");
            }
            EmailTemplate adminEmailTemplate = CreateEditorEmailTemplate(ref template, formName);

            EmailTrigger editorNotificationEmailTrigger = workflow.AddTrigger("ToEditor", "SendEmail");
            editorNotificationEmailTrigger.AddRecipientByEmail("tblt@ualberta.ca");
            editorNotificationEmailTrigger.AddTemplate(adminEmailTemplate.Id, "Join Request Notification");

            // =======================================
            // start submission related workflow items
            // =======================================

            //Defining actions
            GetAction startSubmissionAction = workflow.AddAction("Start Submission", nameof(TemplateOperations.Instantiate), "Home");

            startSubmissionAction.Access = GetAction.eAccess.Public;

            //Defining form template
            startSubmissionAction.AddTemplate(tbltForm.Id, "Task-based Language Teaching Submit Contact Us Form");

            ////Defining post actions
            ////PostAction savePostAction = startSubmissionAction.AddPostAction("Save", nameof(TemplateOperations.Update),
            ////                                                                @"<p>Thank you for saving your resource to the Task-based Language Teaching resource collection. 
            ////                                                                        Your submission should be view/edit at the <a href='@SiteUrl/resources'>resources page.</a></p>");
            ////savePostAction.ValidateInputs = false;
            ////savePostAction.AddStateMapping(emptyState.Id, savedState.Id, "Save");

            PostAction submitPostAction = startSubmissionAction.AddPostAction("Submit", nameof(TemplateOperations.Update),
                                                                                 @"<p>Thank you for your interest in joining the TBLT CoP. You will hear from us soon.</p>");
            submitPostAction.AddStateMapping(emptyState.Id, submittedState.Id, "Submit");


            //Defining the pop-up for the above postActionSubmit action
            PopUp submitActionPopUp = submitPostAction.AddPopUp("Confirm Submission", "Do you want to submit this TBLT join request? Once submitted, you cannot edit it.", "");
            submitActionPopUp.AddButtons("Yes, submit", "true");
            submitActionPopUp.AddButtons("Cancel", "false");

            //Defining trigger refs
            submitPostAction.AddTriggerRefs("0", editorNotificationEmailTrigger.Id, "Editor's Join Notification Email Trigger");
            if (applicantEmail != null)
            {
                submitPostAction.AddTriggerRefs("1", applicantNotificationEmailTrigger.Id, "Owner join-notification Email Trigger");
            }

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
            viewDetailsSubmissionAction.AddStateReferances(submittedState.Id)
                .AddAuthorizedRole(editorRole.Id)
                .AddOwnerAuthorization();
            viewDetailsSubmissionAction.AddStateReferances(approvedState.Id)
                .AddAuthorizedRole(editorRole.Id)
                .AddOwnerAuthorization();
            viewDetailsSubmissionAction.AddStateReferances(rejectedState.Id)
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
            if (applicantEmail != null)
            {
                editSubmissionPostActionSubmit.AddTriggerRefs("1", applicantNotificationEmailTrigger.Id, "Owner Submission-notification Email Trigger");
            }

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

            GetAction changeStateAction = workflow.AddAction("Update Document State", nameof(TemplateOperations.ChangeState), "Details");
            changeStateAction.Access = GetAction.eAccess.Restricted;

            //Define Revision Template
            changeStateAction.AddTemplate(commentsForm.Id, "Comments");
            //Defining post actions
            PostAction changeStatePostAction = changeStateAction.AddPostAction("Change State", @"<p>Application status changed successfully. 
                                                                                You can view the document by <a href='@SiteUrl/items/@Item.Id'>click on here</a></p>");

            //Defining state mappings
            changeStatePostAction.AddStateMapping(submittedState.Id, approvedState.Id, "Approve");
            changeStatePostAction.AddStateMapping(submittedState.Id, rejectedState.Id, "Reject");

            //Defining the pop-up for the above sendForRevisionSubmissionPostAction action
            PopUp adjudicationDecisionPopUpopUp = changeStatePostAction.AddPopUp("Confirmation", "Do you really want to make a decision ? ", "Once changed, you cannot revise this document.");
            adjudicationDecisionPopUpopUp.AddButtons("Yes", "true");
            adjudicationDecisionPopUpopUp.AddButtons("Cancel", "false");

            //Defining states and their authorizatios
            changeStateAction.GetStateReference(submittedState.Id, true)
                .AddAuthorizedRole(editorRole.Id);

        }

        [Test]
        public void TBlt_SubmitDiscussionFormTest()
        {
            string lang = "en";
            string templateName = "Task-based Language Teaching Discussion Form Template";

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
            DataItem bcpForm = template.GetDataItem("Task-based Language Teaching Discussion Form", true, lang);
            bcpForm.IsRoot = true;
            bcpForm.SetDescription("This template is designed for Task-based Language Teaching Discussion Form", lang);

            var title = bcpForm.CreateField<TextField>("Title ", lang, true);
            title.IsListEntryTitle = true; // this will identified as the item label in Collection content page

            bcpForm.CreateField<TextField>("Author", lang, true);
            var content = bcpForm.CreateField<TextArea>("Post content ", lang, true);
            content.Cols = 50;
            content.Rows = 3;
            content.RichText = true;

            bcpForm.CreateField<CheckboxField>("Keywords", lang, GetDiscussionKeywords(), false);

            var otherKeywords = bcpForm.CreateField<TextArea>("Other keywords (separate by commas)", lang, false);
            otherKeywords.Cols = 50;
            otherKeywords.Rows = 3;
            otherKeywords.RichText = false;


            //Defininig the Comments form
            DataItem commentsForm = template.GetDataItem("TBLT Comment Form", true, lang);
            commentsForm.IsRoot = false;
            commentsForm.SetDescription("This is the form to be filled by the editor when make a decision.", lang);
            commentsForm.CreateField<TextArea>("Comments", lang, true);

            Define_TBLT_DiscussionWorkflow(workflow, ref template, bcpForm, commentsForm, "SubmitDiscussion");
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\TBLT_DiscussionForm_generared.xml");
        }

        private void Define_TBLT_DiscussionWorkflow(Workflow workflow, ref ItemTemplate template, DataItem tbltForm, DataItem commentsForm, string formName = null)
        {
            IWorkflowService ws = _testHelper.WorkflowService;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            State emptyState = workflow.AddState(ws.GetStatus(template.Id, "", true));
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
            applicantNotificationEmailTrigger.AddOwnerAsRecipient();
            applicantNotificationEmailTrigger.AddTemplate(applicantEmailTemplate.Id, "Join TBLT Comment Notification");

            EmailTemplate adminEmailTemplate = CreateEditorEmailTemplate(ref template, formName);

            EmailTrigger editorNotificationEmailTrigger = workflow.AddTrigger("ToEditor", "SendEmail");
            editorNotificationEmailTrigger.AddRecipientByEmail("tblt@ualberta.ca");
            editorNotificationEmailTrigger.AddTemplate(adminEmailTemplate.Id, "Join Comment Notification");

            // =======================================
            // start submission related workflow items
            // =======================================

            //Defining actions
            GetAction startSubmissionAction = workflow.AddAction("Start Submission", nameof(TemplateOperations.Instantiate), "Home");

            startSubmissionAction.Access = GetAction.eAccess.Restricted;

            //Defining form template
            startSubmissionAction.AddTemplate(tbltForm.Id, "Task-based Language Teaching Comment Form");

            //Defining post actions
            PostAction submitPostAction = startSubmissionAction.AddPostAction("Submit", nameof(TemplateOperations.Update),
                                                                                 @"<p>Thank you for submitting your post to the Task-based Language Teaching discussion forum. 
                                                                                    Your post should be visible at the <a href='@SiteUrl/discussion/@Item.Id'> forum page. </a></p>");
            submitPostAction.AddStateMapping(emptyState.Id, submittedState.Id, "Submit");


            //Defining the pop-up for the above postActionSubmit action
            PopUp submitActionPopUp = submitPostAction.AddPopUp("Confirm Submission", "Do you want to submit this post to the discussion forum? Once submitted, you cannot edit it.", "");
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
            viewDetailsSubmissionAction.AddStateReferances(submittedState.Id)
                .AddAuthorizedRole(editorRole.Id)
                .AddOwnerAuthorization();
            viewDetailsSubmissionAction.AddStateReferances(approvedState.Id)
                .AddAuthorizedRole(editorRole.Id)
                .AddOwnerAuthorization();
            viewDetailsSubmissionAction.AddStateReferances(rejectedState.Id)
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
                                                                                             @"<p>Thank you for submitting your post to the Task-based Language Teaching discussion forum. 
                                                                                    Your post should be visible at the <a href='@SiteUrl/discussion-forum'> forum page. </a></p>");
            //Defining state mappings
            editSubmissionPostActionSubmit.AddStateMapping(submittedState.Id, submittedState.Id, "Submit");

            //Defining the pop-up for the above postActionSubmit action
            PopUp EditSubmissionActionPopUpopUp = editSubmissionPostActionSubmit.AddPopUp("Confirm Submission", "Do you want to submit this post to the discussion forum? Once submitted, you cannot edit it.", "");
            EditSubmissionActionPopUpopUp.AddButtons("Yes, submit", "true");
            EditSubmissionActionPopUpopUp.AddButtons("Cancel", "false");

            //Defining trigger refs
            //*******To Do*******
            // Implement a function to restrict the e-mail triggers when SAS Admin updated the document
            editSubmissionPostActionSubmit.AddTriggerRefs("0", editorNotificationEmailTrigger.Id, "Editor's Notification Email Trigger");
            editSubmissionPostActionSubmit.AddTriggerRefs("1", applicantNotificationEmailTrigger.Id, "Owner Submission-notification Email Trigger");

            //Defining state referances
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

            GetAction changeStateAction = workflow.AddAction("Update Document State", nameof(TemplateOperations.ChangeState), "Details");
            changeStateAction.Access = GetAction.eAccess.Restricted;

            //Define Revision Template
            changeStateAction.AddTemplate(commentsForm.Id, "Comments");
            //Defining post actions
            PostAction changeStatePostAction = changeStateAction.AddPostAction("Change State", @"<p>Application status changed successfully. 
                                                                                You can view the document by <a href='@SiteUrl/items/@Item.Id'>click on here</a></p>");

            //Defining state mappings
            changeStatePostAction.AddStateMapping(submittedState.Id, approvedState.Id, "Approve");
            changeStatePostAction.AddStateMapping(submittedState.Id, rejectedState.Id, "Reject");

            //Defining the pop-up for the above sendForRevisionSubmissionPostAction action
            PopUp adjudicationDecisionPopUpopUp = changeStatePostAction.AddPopUp("Confirmation", "Do you really want to make a decision ? ", "Once changed, you cannot revise this document.");
            adjudicationDecisionPopUpopUp.AddButtons("Yes", "true");
            adjudicationDecisionPopUpopUp.AddButtons("Cancel", "false");

            //Defining states and their authorizatios
            changeStateAction.GetStateReference(submittedState.Id, true)
                .AddAuthorizedRole(editorRole.Id);

        }

        private string[] GetDiscussionKeywords()
        {
            return new string[]
            {
                "Real-life tasks",
                "Assessment",
                "Grammar",
                "Listening",
                "Reading",
                "Writing",
                "Speaking",
                "Vocabulary",
                "Interaction",
                "Materials",
                "Teacher training",
                "Technology",
                "Age-appropriate tasks",
                "Games",
                "Culture"
            };
        }

        private string[] GetProficientcyLevel()
        {
            return new string[] { "Lower beginners/ A1",
                                "Upper beginners/ A2",
                                "Lower intermediate/ B1",
                                "Upper intermediate/ B2",
                                "Lower advanced/ C1",
                                "Upper advanced/ C2"
            };
        }

        private string[] GetLanguageSkills()
        {
            return new string[] { "Reading",
                                "Listening",
                                "Writing",
                                "Oral production",
                                "Oral interaction",
                                "Pronunciation"
            };
        }

        private string[] GetDeliveryModes()
        {
            return new string[] { "Face-to-face (in person)",
                                "Digital (online)",
                                "Hybrid (in person and online)",
                                "Asynchronous",
                                "Synchronous"
            };
        }
    }
}
