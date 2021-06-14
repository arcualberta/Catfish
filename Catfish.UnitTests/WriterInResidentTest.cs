﻿using Catfish.Core.Authorization.Requirements;
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
    public class WriterInResidentTest
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
            string templateName = "Writer in Resident Form Template";

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
            DataItem wrForm = template.GetDataItem("Writer-in-Resident Application Form", true, lang);
            wrForm.IsRoot = true;
            wrForm.SetDescription("This template is designed for Writer-in-Resident Application Form", lang);

            wrForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h1", "Writer-in-Resident", lang);
           
            var applicantName = wrForm.CreateField<TextField>("Full Name", lang,true);
            var applicantEmail =wrForm.CreateField<TextField>("Email", lang, true);
            wrForm.CreateField<TextField>("Affiliation", lang, true);
            
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //
           
            Define_WR_RolesStatesWorkflow(workflow, ref template, wrForm, applicantName, applicantEmail);
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\WriterInResident_generared.xml");

            //string json = JsonConvert.SerializeObject(template);
            //File.WriteAllText("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.json", json);
        }

        
        private EmailTemplate CreateRejectionEmailTemplate(ref ItemTemplate template, TextField applicantName, TextField applicantEmail)
        {
            string lang = "en";
            string body = "Dear" + applicantName + ", "+
                "<p>Thank you for your application for the Writer-in-Residence position with the Department of English and Film Studies at the University of Alberta." +
                "We had a very strong field of applicants, and our selection committee read your application materials with interest and attention.I regret to say that following careful and difficult deliberations, you were not selected as next year’s Writer-in-Residence."+
                "All of us on the committee appreciate you taking the time to submit your work and we wish you all the best for your writing endeavours in the coming year and encourage you to reapply to the program in the future."+
                "</p>" +
                 "<p>Your sincerely,</p>" +

                 "<p> [Insert Names of the Committee & titles(i.e.Professor) if the system can do that ] </p>";

           EmailTemplate rejectionEmailNotification = template.GetEmailTemplate("Applicant Rejection Email Notification", lang, true);
            rejectionEmailNotification.SetDescription("This metadata set defines the email template to be sent to the portal admin.", lang);
            rejectionEmailNotification.SetSubject("Join Task-based Language Teaching");
            rejectionEmailNotification.SetBody(body);

            return rejectionEmailNotification;

        }

        private EmailTemplate CreateApplicantApplicationEmailTemplate(ref ItemTemplate template, TextField applicantName, TextField applicantEmail)
        {
            string lang = "en";
            string body = "Dear" + applicantName + ", " +
                "<p>Thank you for your application for the Writer-in-Residence position with the Department of English and Film Studies at the University of Alberta." +
                "We will review your application and will get back to you." +
                 "<p>Your sincerely,</p>" +

                 "<p> [Insert Names of the Committee & titles(i.e.Professor) if the system can do that ] </p>";

            EmailTemplate applicantEmailNotification = template.GetEmailTemplate("Applicant Rejection Email Notification", lang, true);
            applicantEmailNotification.SetDescription("This metadata set defines the email template to be sent to the portal admin.", lang);
            applicantEmailNotification.SetSubject("Join Task-based Language Teaching");
            applicantEmailNotification.SetBody(body);

            return applicantEmailNotification;

        }
        private void Define_WR_RolesStatesWorkflow(Workflow workflow, ref ItemTemplate template, DataItem wrForm, TextField applicantName, TextField applicantEmail)
        {
            IWorkflowService ws = _testHelper.WorkflowService;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            State emptyState = workflow.AddState(ws.GetStatus(template.Id, "", true));
            State submittedState = workflow.AddState(ws.GetStatus(template.Id, "Submitted", true));  
            State inReviewState = workflow.AddState(ws.GetStatus(template.Id, "InReview", true));
            State reviewCompletedState = workflow.AddState(ws.GetStatus(template.Id, "Review Completed", true));
            State inAdjudicationState = workflow.AddState(ws.GetStatus(template.Id, "In Adjudication", true));
            State acceptedState = workflow.AddState(ws.GetStatus(template.Id, "Accepted", true));
            State rejectedState = workflow.AddState(ws.GetStatus(template.Id, "Rejected", true));
            State deleteState = workflow.AddState(ws.GetStatus(template.Id, "Deleted", true));


            WorkflowRole adminRole = workflow.AddRole(auth.GetRole("WR_Admin", true)); 
            WorkflowRole reviewRole = workflow.AddRole(auth.GetRole("WR_Review", true));
            WorkflowRole adjudicatorRole = workflow.AddRole(auth.GetRole("WR_Adjudicator", true));

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
            submitPostAction.AddStateMapping(emptyState.Id, inReviewState.Id, "Submit");

            //Defining the pop-up for the above postActionSubmit action
            PopUp submitActionPopUp = submitPostAction.AddPopUp("Confirmation", "Do you really want to submit this document?", "Once submitted, you cannot update the document.");
            submitActionPopUp.AddButtons("Yes, submit", "true");
            submitActionPopUp.AddButtons("Cancel", "false");

            //Defining trigger refs
            EmailTemplate applicantNotificationEmail =CreateApplicantApplicationEmailTemplate(ref template, applicantName, applicantEmail);
             submitPostAction.AddTriggerRefs("0", applicantNotificationEmail.Id, "Applicant Notification Email Trigger");
            //  submitPostAction.AddTriggerRefs("1", applicantNotificationEmailTrigger.Id, "Owner Submission-notification Email Trigger");


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
                   .AddOwnerAuthorization()
                  .AddAuthorizedRole(reviewRole.Id)
                  .AddAuthorizedRole(adminRole.Id);
              
            listSubmissionsAction.AddStateReferances(inReviewState.Id)
                .AddAuthorizedRole(adminRole.Id)
                .AddAuthorizedRole(reviewRole.Id)
                .AddOwnerAuthorization();

            listSubmissionsAction.AddStateReferances(reviewCompletedState.Id)
                .AddAuthorizedRole(adminRole.Id)
               .AddAuthorizedRole(reviewRole.Id);

            listSubmissionsAction.AddStateReferances(inAdjudicationState.Id)
                .AddAuthorizedRole(adminRole.Id)
                .AddAuthorizedRole(adjudicatorRole.Id)
                .AddOwnerAuthorization();
            listSubmissionsAction.AddStateReferances(acceptedState.Id)
                .AddAuthorizedRole(adminRole.Id)
                .AddAuthorizedRole(adjudicatorRole.Id)
                .AddOwnerAuthorization();
            listSubmissionsAction.AddStateReferances(rejectedState.Id)
                .AddAuthorizedRole(adminRole.Id)
                .AddAuthorizedRole(adjudicatorRole.Id)
                .AddOwnerAuthorization();

            //Detailed submission bcp forms.
            //Inspectors can view their own submissions.
            //Admins can view all submissions.
            GetAction viewSubmissionAction = workflow.AddAction("Details", nameof(TemplateOperations.Read), "List");
            viewSubmissionAction.Access = GetAction.eAccess.Restricted;
            viewSubmissionAction.AddStateReferances(submittedState.Id)
               .AddOwnerAuthorization()
              .AddAuthorizedRole(reviewRole.Id)
              .AddAuthorizedRole(adminRole.Id)
              .AddAuthorizedRole(adjudicatorRole.Id);

            
           
            //============================================================================
            //                                 EMAIL 
            //==============================================================================
            EmailTemplate adminEmailTemplate = CreateEmailTemplate(ref template);

            //EmailTrigger notificationEmailTrigger = workflow.AddTrigger("ToChair", "SendEmail");
            //notificationEmailTrigger.AddRecipientByEmail("mruaini@ualberta.ca");
            //notificationEmailTrigger.AddTemplate(adminEmailTemplate.Id, "Join Request Notification");
            //Defining trigger refs
          //  submitPostAction.AddTriggerRefs("0", notificationEmailTrigger.Id, "Chair's Notification Email Trigger", submittedState.Id, true);
            

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

            GetAction viewChildFormDetailsAction = workflow.AddAction("ChildForm", nameof(TemplateOperations.ChildFormView), "List");

            viewChildFormDetailsAction.Access = GetAction.eAccess.Restricted;

            // Added state referances

            viewChildFormDetailsAction.AddStateReferances(inReviewState.Id)
                .AddAuthorizedRole(adminRole.Id)
                .AddAuthorizedRole(reviewRole.Id);

            viewChildFormDetailsAction.AddStateReferances(reviewCompletedState.Id)
                  .AddAuthorizedRole(adminRole.Id)
                  .AddAuthorizedRole(adjudicatorRole.Id)
                .AddAuthorizedRole(reviewRole.Id);

            viewChildFormDetailsAction.AddStateReferances(inAdjudicationState.Id)
                 .AddAuthorizedRole(adminRole.Id)
                  .AddAuthorizedRole(adjudicatorRole.Id);

            viewChildFormDetailsAction.AddStateReferances(acceptedState.Id)
                 .AddAuthorizedRole(adminRole.Id)
                  .AddAuthorizedRole(adjudicatorRole.Id);

            viewChildFormDetailsAction.AddStateReferances(rejectedState.Id)
                 .AddAuthorizedRole(adminRole.Id)
                  .AddAuthorizedRole(adjudicatorRole.Id);


            //===================================================================================================================================
            //                                           Create Other Forms
            //===================================================================================================================================
            TextField assessFormApplicantName;
            EmailField assessFormApplicantEmail;
            //SelectField assessFormDept;
            //SelectField assessFormRank;
            DataItem reviewForm = CreateReviewForm(template,
                out assessFormApplicantName,
                out assessFormApplicantEmail);
            // var reviewForm = CreateReviewForm(template);
            DataItem addNoteForm = CreateAddNotesForm(template);
            DataItem adjudicationForm = CreateAdjudicationForm(template);
            DataItem addrankingForm = CreateAddRankingForm(template);


            // ================================================
            // Review submission-instances related workflow items
            // ================================================

            GetAction reviewSubmissionAction = workflow.AddAction("Review Submission", nameof(TemplateOperations.Review), "Details");
            reviewSubmissionAction.Access = GetAction.eAccess.Restricted;

            //Defining form template
            reviewSubmissionAction.AddTemplate(reviewForm.Id, "Start Review Submission");

            //Defining post actions
            PostAction ReviewSubmissionPostAction = reviewSubmissionAction.AddPostAction("Start Review",
                                                                                nameof(TemplateOperations.Update),
                                                                                @"<p>Review assessment submitted successfully. 
                                                                                You can view the document and your review by <a href='@SiteUrl/items/@Item.Id'>click on here</a></p>");

            //Defining state mappings
            ReviewSubmissionPostAction.AddStateMapping(inReviewState.Id, reviewCompletedState.Id, "Submit Assessment");

            //Defining the pop-up for the above postAction action
            PopUp reviewSubmissionActionPopUpopUp = ReviewSubmissionPostAction.AddPopUp("Confirmation", "Confirm the Assessment?", "Once submitted, you cannot resubmit an assessment.");
            reviewSubmissionActionPopUpopUp.AddButtons("Yes, complete", "true");
            reviewSubmissionActionPopUpopUp.AddButtons("Cancel", "false");


            reviewSubmissionAction.GetStateReference(inReviewState.Id, true)
                .AddAuthorizedRole(adminRole.Id);


            // ================================================
            // Change State submission-instances related workflow items
            // ================================================

            GetAction changeStateAction = workflow.AddAction("Update Document State", nameof(TemplateOperations.ChangeState), "Details");
            changeStateAction.Access = GetAction.eAccess.Restricted;

            //Define Revision Template
              changeStateAction.AddTemplate(addNoteForm.Id, "Submission Change State");
            //Defining post actions
            PostAction changeStatePostAction = changeStateAction.AddPostAction("Change State", @"<p>Application status changed successfully. 
                                                                                You can view the document by <a href='@SiteUrl/items/@Item.Id'>click on here</a></p>");

            //Defining state mappings
            changeStatePostAction.AddStateMapping(reviewCompletedState.Id, inAdjudicationState.Id, "With Adjudication");

            //Defining the pop-up for the above sendForRevisionSubmissionPostAction action
            PopUp changeStateActionPopUpopUp = changeStatePostAction.AddPopUp("Confirmation", "Do you really want to change status ? ", "Once changed, you cannot revise this document.");
            changeStateActionPopUpopUp.AddButtons("Yes", "true");
            changeStateActionPopUpopUp.AddButtons("Cancel", "false");

            //Defining states and their authorizatios
            changeStateAction.GetStateReference(reviewCompletedState.Id, true)
                .AddAuthorizedRole(adminRole.Id)
                .AddAuthorizedRole(adjudicatorRole.Id)
                .AddAuthorizedRole(reviewRole.Id);   

            changeStateAction.GetStateReference(inReviewState.Id, true)
                 .AddAuthorizedRole(adminRole.Id)
                .AddAuthorizedRole(reviewRole.Id);
            changeStateAction.GetStateReference(inAdjudicationState.Id, true)
                 .AddAuthorizedRole(adminRole.Id)
                .AddAuthorizedRole(adjudicatorRole.Id);
              
            // ========================================================
            // Adjudication Decision related workflow items
            // ========================================================
            GetAction adjudicationDecisionAction = workflow.AddAction("Adjudication Decision", nameof(TemplateOperations.ChangeState), "Details");

            //Define Revision Template
            adjudicationDecisionAction.AddTemplate(adjudicationForm.Id, "Adjudication Decision");

            PostAction adjudicationDecisionPostAction = adjudicationDecisionAction.AddPostAction("Adjudication Decision", "Save", @"<p>Application final decision made successfully. 
                                                                                You can view the document by <a href='@SiteUrl/items/@Item.Id'>click on here</a></p>"
                                                                                );
            adjudicationDecisionPostAction.AddStateMapping(inAdjudicationState.Id, acceptedState.Id, "Accept");
            adjudicationDecisionPostAction.AddStateMapping(inAdjudicationState.Id, rejectedState.Id, "Reject");

            //Defining the pop-up for the above sendForRevisionSubmissionPostAction action
            PopUp adjudicationDecisionPopUpopUp = adjudicationDecisionPostAction.AddPopUp("Confirmation", "Do you really want to make a decision ? ", "Once changed, you cannot revise this document.");
            adjudicationDecisionPopUpopUp.AddButtons("Yes", "true");
            adjudicationDecisionPopUpopUp.AddButtons("Cancel", "false");

            //////adjudicationDecisionAction.GetStateReference(inAdjudicationState.Id, true)
            //////    .AddAuthorizedRole(sasAdjudication.Id);
            adjudicationDecisionAction.GetStateReference(inAdjudicationState.Id, true)
                .AddAuthorizedRole(adminRole.Id);

        }

        private DataItem CreateReviewForm(ItemTemplate template, 
            out TextField applicantName,
            out EmailField applicantEmail)
        {
            string lang = "en";
            DataItem form = template.GetDataItem("Chair's Assessment Form", true, lang);
            form.IsRoot = false;
            form.SetDescription("This template is designed for Chair's Assessment", lang);

            applicantName = form.CreateField<TextField>("Applicant Name", lang, true); //ideally this field will be automatically filled from the "Applicant Name" field on the main form
            applicantEmail = form.CreateField<EmailField>("Applicant Email", lang, true);//ideally this field will be automatically filled from the "Applicant Email" field on the main form
            

         
           // rank = form.CreateField<SelectField>("Rank:", lang, rankOptions, true);

            form.CreateField<TextArea>("Comment", lang, true).SetAttribute("cols", 50);

            //string[] priorities = new string[]{ "Top Priority",
            //                    "High Priority",
            //                    "Moderate Priority",
            //                    "Low Priority",
            //                    "Should Not Be Funded"};
            //form.CreateField<RadioField>("Please rank the importance of the research or conference travel to the applicant’s career.", lang, priorities, true);
            //form.CreateField<RadioField>("Please rank the importance of the venue (for conference travel only).", lang, priorities, true);
          
           
            return form;
        }

        private DataItem CreateAddNotesForm(ItemTemplate template)
        {
            string lang = "en";
            DataItem form = template.GetDataItem("Writer-in-Resident Notes Form", true, lang);
            form.IsRoot = false;
            form.SetDescription("This template is designed for Writer-in-Resident Notes Form", lang);


            form.CreateField<TextArea>("Notes", lang, false).SetAttribute("cols", 50);


            return form;
        }
        private DataItem CreateAdjudicationForm(ItemTemplate template)
        {
            string lang = "en";
            DataItem form = template.GetDataItem("Adjudication Result", true, lang);
            form.IsRoot = false;
            form.SetDescription("This template is designed for Adjudication Result Form", lang);

            string[] decisions = new string[]{ "Approved",
                                "Declined"};

            form.CreateField<RadioField>("Decision", lang, decisions, true);
            form.CreateField<DecimalField>("Total Awarded", lang, false);
            form.CreateField<TextArea>("Comments", lang, true).SetAttribute("cols", 50);


            return form;
        }
        private DataItem CreateAddRankingForm(ItemTemplate template)
        {
            string lang = "en";
            DataItem form = template.GetDataItem("Ranking", true, lang);
            form.IsRoot = false;
            form.SetDescription("This template is designed for Writer-in-Resident Ranking Form", lang);


            form.CreateField<EmailField>("Reviewer Email", lang, true);
            form.CreateField<IntegerField>("Ranking", lang, true).SetDescription("Please score this submission with a value between 0 and 100", lang);
            form.CreateField<TextArea>("Comments", lang, true).SetAttribute("cols", 50);


            return form;
        }


    }
}
