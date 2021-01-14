﻿using Catfish.Core.Authorization.Requirements;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Contents.Workflow;
using Catfish.Core.Services;
using Catfish.Services;
using Catfish.Tests.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.UnitTests
{
    public class WorkdlowTests
    {
        private protected AppDbContext _db;
       private protected TestHelper _testHelper;

        [SetUp]
        public void Setup()
        {
            _testHelper = new TestHelper();
            _db = _testHelper.Db;
        }

        [Test]
        public void SelectFieldInitTest()
        {
            string file = "..\\..\\..\\..\\Examples\\selectfield.xml";
            XDocument doc = XDocument.Load(file);
            XmlModel model = XmlModel.InstantiateContentModel(doc.Root);
            SelectField field = model as SelectField;
            var options = field.Options;
        }

/*
        [Test]
        public void ContractLetterWorkflowBuildTest()
        {
            string lang = "en";
            string templateName = "Trust Funded GRA/GRAF Contract";
            AppDbContext db = _testHelper.Db;

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

            IWorkflowService ws = _testHelper.WorkflowService;
            ws.SetModel(template);

            IAuthorizationService auth = _testHelper.AuthorizationService;
            ws.SetModel(template);
            
            //Email template for trust account hoder
            EmailTemplate trustAccountHolderNotification = ws.GetEmailTemplate("Trust Account Holder Notification", true);
            trustAccountHolderNotification.SetDescription("This metadata set defines the email template to be sent for the trust-account holder when a new contract is created.", lang);
            trustAccountHolderNotification.SetSubject("Trust-funded Contract Review");
            trustAccountHolderNotification.SetBody("Please review @Link[this contract letter|@Model] to be funded by one of your trust accounts and provide your decision within 2 weeks.\n\nThank you");

            //Email template for the associate chair
            EmailTemplate deptChairNotification = ws.GetEmailTemplate("Associate Chair Notification", true);
            deptChairNotification.SetDescription("This metadata set defines the email template to be sent for the associate chair when a new contract is created.", lang);
            deptChairNotification.SetSubject("Graduate Contract Review (Trust Funded)");
            deptChairNotification.SetBody("Please review @Link[this contract letter|@Model] to be funded by a trust accounts and provide your decision within 2 weeks.\n\nThank you");

            //Email template for the department admin
            EmailTemplate deptAdminNotification = ws.GetEmailTemplate("Department Admin Notification", true);
            deptAdminNotification.SetDescription("This metadata set defines the email template to be sent for the program admin when another party makes a change to a contract.", lang);
            deptAdminNotification.SetSubject("Cnotract Status Update (Trust Funded)");
            deptAdminNotification.SetBody("The status of @Link[this contract letter|@Model] has been updated.\n\nThank you");

            //Contract letter
            DataItem contract = template.GetDataItem("Contract Letter", true, lang);
            contract.IsRoot = true;
            contract.SetDescription("This is the template for the contract letter.", lang);
            contract.CreateField<TextField>("First Name", lang, true);
            contract.CreateField<TextField>("Last Name", lang, true);
            contract.CreateField<TextField>("Student ID", lang, true);
            contract.CreateField<TextField>("Student Email", lang, true, true);
            contract.CreateField<TextField>("Department", lang, true, false, "East Asian Studies");
            contract.CreateField<SelectField>("Type of Appointment", lang, new string[] { "Graduate Research Assistant", "Graduate Research Assistantship Fellowship" }, true, 0);
            contract.CreateField<TextField>("Assignment", lang, true);

            contract.CreateField<InfoSection>("Period of Appointment", lang, "alert alert-info");
            contract.CreateField<DateField>("Appointment Start", lang, true);
            contract.CreateField<DateField>("Appointment End", lang, true);

            contract.CreateField<InfoSection>("Stipend", lang, "alert alert-info");
            contract.CreateField<IntegerField>("Rate", lang, true);
            contract.CreateField<IntegerField>("Award", lang, true);
            contract.CreateField<IntegerField>("Salary", lang, true);

            //Get the Workflow object using the workflow service
            Workflow workflow = ws.GetWorkflow(true);


            //Defininig roles
            WorkflowRole centralAdminRole = workflow.AddRole(auth.GetRole("Admin", true));
            WorkflowRole departmentAdmin = workflow.AddRole(auth.GetRole("DepartmentAdmin", true));

            // start submission related workflow items
            //Defining actions
            GetAction startSubmissionAction = workflow.AddAction("Start Contract", nameof(TemplateOperations.Instantiate), "Home");
            startSubmissionAction.AddAuthorizedRole(departmentAdmin.Id);

            //Defining form template
            startSubmissionAction.AddTemplate(contract.Id, "Contract Letter Template");


            //EntityTemplate oldTemplate = db.EntityTemplates.Where(et => et.TemplateName == template.TemplateName).FirstOrDefault();
            //if (oldTemplate == null)
            //    db.EntityTemplates.Add(template);
            //else
            //    oldTemplate.Content = template.Content;
            db.SaveChanges();

            //Save the template to a file
            template.Data.Save("..\\..\\..\\..\\Examples\\ContractLetterWorkflow.xml");
        }
*/

        [Test]
        public void CalendarManagementSystemWorkflowBuildTest()
        {
            string lang = "en";
            string templateName = "Calendar Management System Workflow";
            string centralAdminEmail = "artsrnd@ualberta.ca";


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

            ws.SetModel(template);

            //Get the Workflow object using the workflow service
            Workflow workflow = ws.GetWorkflow(true);

            //Defininig states
            State emptyState = workflow.AddState(ws.GetStatus(template.Id,"",true));
            State savedState = workflow.AddState(ws.GetStatus(template.Id, "Saved", true));
            State submittedState = workflow.AddState(ws.GetStatus(template.Id, "Submitted", true));
            State deleteState = workflow.AddState(ws.GetStatus(template.Id, "Deleted", true));

            State deanOfficeRevisionState = workflow.AddState(ws.GetStatus(template.Id, "DeanOfficeRevision", true));
            State deanOfficeRevisionCompletedState = workflow.AddState(ws.GetStatus(template.Id, "DeanOfficeRevisionCompleted", true));
            State deanOfficeRevisionSaveState = workflow.AddState(ws.GetStatus(template.Id, "SavedDeanOfficeRevision", true));

            State aacWithState = workflow.AddState(ws.GetStatus(template.Id, "WithAAC", true));
            State aacRevisionState = workflow.AddState(ws.GetStatus(template.Id, "AACRevision", true));
            State aacRevisionSaveState = workflow.AddState(ws.GetStatus(template.Id, "SavedAACRevision", true));
            State aacRevisionCompletedState = workflow.AddState(ws.GetStatus(template.Id, "AACRevisionCompleted", true));
            State aacApprovedState = workflow.AddState(ws.GetStatus(template.Id, "AACApproved", true));

            State aecWithState = workflow.AddState(ws.GetStatus(template.Id, "WithAEC", true));
            State aecRevisionState = workflow.AddState(ws.GetStatus(template.Id, "AECRevision", true));
            State aecRevisionSaveState = workflow.AddState(ws.GetStatus(template.Id, "SavedAECRevision", true));
            State aecRevisionCompletedState = workflow.AddState(ws.GetStatus(template.Id, "AECRevisionCompleted", true));
            State aecApprovedState = workflow.AddState(ws.GetStatus(template.Id, "AECApproved", true));


            State afcWithState = workflow.AddState(ws.GetStatus(template.Id, "WithAFC", true));
            State afcRevisionState = workflow.AddState(ws.GetStatus(template.Id, "AFCRevision", true));
            State afcRevisionSaveState = workflow.AddState(ws.GetStatus(template.Id, "SavedAFCRevision", true));
            State afcRevisionCompletedState = workflow.AddState(ws.GetStatus(template.Id, "AFCRevisionCompleted", true));
            State afcApprovedState = workflow.AddState(ws.GetStatus(template.Id, "AFCApproved", true));


            State gfcWithState = workflow.AddState(ws.GetStatus(template.Id, "WithGFC", true));
            State gfcRevisionState = workflow.AddState(ws.GetStatus(template.Id, "GFCRevision", true));
            State gfcRevisionSaveState = workflow.AddState(ws.GetStatus(template.Id, "SavedGFCRevision", true));
            State gfcRevisionCompletedState = workflow.AddState(ws.GetStatus(template.Id, "GFCRevisionCompleted", true));
            State gfcApprovedState = workflow.AddState(ws.GetStatus(template.Id, "GFCApproved", true));

            State moveToDraftErrorState = workflow.AddState(ws.GetStatus(template.Id, "MoveToDraftError", true));
            State moveToDraftCorrectState = workflow.AddState(ws.GetStatus(template.Id, "MoveToDraftCorrect", true));





            //Defining email templates
            EmailTemplate centralAdminNotification = ws.GetEmailTemplate("Central Admin Notification", true);
            centralAdminNotification.SetDescription("This metadata set defines the email template to be sent to the central admin when a dept admin makes a submission.", lang);
            centralAdminNotification.SetSubject("Calendar Change Submission");
            centralAdminNotification.SetBody("A @Link[calendar chane|@Model] was submitted.\n\nThank you");

            EmailTemplate deptAdminSubmissionNotification = ws.GetEmailTemplate("Dept. Admin Submission Admin Notification", true);
            deptAdminSubmissionNotification.SetDescription("This metadata set defines the email template to be sent to the dept admin when he/she submits a new or revised calendar change.", lang);
            deptAdminSubmissionNotification.SetSubject("Calendar Change Submission");
            deptAdminSubmissionNotification.SetBody("A @Link[calendar change|@Model] was submitted.\n\nThank you");

            EmailTemplate revisionNotification = ws.GetEmailTemplate("Central Admin Revision Notification", true);
            revisionNotification.SetDescription("This metadata set defines the email template to be sent to the dept admin when central admin make a revision request.", lang);
            revisionNotification.SetSubject("Calendar Change Submission Need to Revise");
            revisionNotification.SetBody("A @Link[calendar change|@Model] need to revise.\n\nThank you");

            EmailTemplate moveToDraftCalendarNotification = ws.GetEmailTemplate("Move to Draft Calendar Notification", true);
            moveToDraftCalendarNotification.SetDescription("This metadata set defines the email template to be sent to the dept admin when the submission request move to the draft calendar.", lang);
            moveToDraftCalendarNotification.SetSubject("Calendar Change Submission moved to the draft calendar");
            moveToDraftCalendarNotification.SetBody("A @Link[calendar change|@Model] moved to the draft calendar.\n\nThank you");

            //Defininig the Calendar Change Request form
            DataItem calendarChangeForm = template.GetDataItem("Calendar Change Request", true, lang);
            calendarChangeForm.IsRoot = true;
            calendarChangeForm.SetDescription("This is the form to be filled by the department admin when a calndar change is requested.", lang);
            calendarChangeForm.CreateField<TextField>("Course Name", lang, true);
            calendarChangeForm.CreateField<TextField>("Course Number", lang, true);
            calendarChangeForm.CreateField<TextArea>("Change Description", lang, true);

            //Defininig the Submission revision Request form
            DataItem commentsForm = template.GetDataItem("Submission Revision Request", true, lang);
            commentsForm.IsRoot = false;
            commentsForm.SetDescription("This is the form to be filled by the central admin when a submission revision is requested.", lang);
            commentsForm.CreateField<TextArea>("Request Details", lang, true);

            //Defining name mappings
            //TODO: Add functionality for EntityTemplate to allow us define a sequence of metadata set fields
            //      to be used as table headings in the list view. Use this functionality to specify the
            //      Course Name and Course Number as the list-view table headings for this schema.
            //      In the actual listing page, we should show the values of these set of fields and the "owner"
            //      of the root data object, the created time-stamp of the root data object, and the satust of the
            //      entity.

            ////Defininig groups
            //WorkflowGroup musicGroup = workflow.AddGroup("Music");
            //WorkflowGroup dramaGroup = workflow.AddGroup("Drama");
            //WorkflowGroup arcGroup = workflow.AddGroup("ARC");


            //Defininig roles
            WorkflowRole centralAdminRole = workflow.AddRole(auth.GetRole("Admin", true));
            WorkflowRole departmentAdmin = workflow.AddRole(auth.GetRole("DepartmentAdmin", true));


            ////Defining users
            //WorkflowUser centralAdminUser = workflow.AddUser("centraladmin@ualberta.ca");
            //centralAdminUser.AddRoleReference(centralAdminRole.Id, "Central Admin User");
            //WorkflowUser deptUser = workflow.AddUser("departmentadmin1@ualberta.ca");
            //deptUser.AddRoleReference(departmentAdmin.Id, "Dept. Admin User");
            //deptUser = workflow.AddUser("departmentadmin2@ualberta.ca");
            //deptUser.AddRoleReference(departmentAdmin.Id, "Dept. Admin User");

            //Defining triggers
            EmailTrigger centralAdminNotificationEmailTrigger = workflow.AddTrigger("ToCentralAdmin", "SendEmail");
            centralAdminNotificationEmailTrigger.AddRecipientByEmail(centralAdminEmail);
            centralAdminNotificationEmailTrigger.AddTemplate(centralAdminNotification.Id, "Central Admin Notification");

            EmailTrigger ownerSubmissionNotificationEmailTrigger = workflow.AddTrigger("ToOwnerOnDocumentSubmission", "SendEmail");
            ownerSubmissionNotificationEmailTrigger.AddOwnerAsRecipient();
            ownerSubmissionNotificationEmailTrigger.AddTemplate(deptAdminSubmissionNotification.Id, "Owner's submission-notification");

            EmailTrigger RevisionNotificationEmailTrigger = workflow.AddTrigger("ToOwnerOnDocumentRevision", "SendEmail");
            RevisionNotificationEmailTrigger.AddOwnerAsRecipient();
            RevisionNotificationEmailTrigger.AddTemplate(revisionNotification.Id, "Owner's revision-notification");

            EmailTrigger MovedToDraftCalendarEmailTrigger = workflow.AddTrigger("ToOwnerOnMovedToDraftCalendar", "SendEmail");
            MovedToDraftCalendarEmailTrigger.AddOwnerAsRecipient();
            MovedToDraftCalendarEmailTrigger.AddTemplate(moveToDraftCalendarNotification.Id, "Owner's moved to calendar notification");

            // =======================================
            // start submission related workflow items
            // =======================================
           
            //Defining actions
            GetAction startSubmissionAction = workflow.AddAction("Start Submission", nameof(TemplateOperations.Instantiate), "Home");

            //Defining form template
            startSubmissionAction.AddTemplate(calendarChangeForm.Id, "Start Submission Template");

            //Defining post actions
            PostAction postActionSave = startSubmissionAction.AddPostAction("Save", nameof(TemplateOperations.Update));
            postActionSave.AddStateMapping(emptyState.Id, savedState.Id, "Save");
            PostAction postActionSubmit = startSubmissionAction.AddPostAction("Submit", nameof(TemplateOperations.Update));
            postActionSubmit.AddStateMapping(emptyState.Id, submittedState.Id, "Submit");

            //Defining the pop-up for the above postActionSubmit action
            PopUp startSubmissionActionPopUp = postActionSubmit.AddPopUp("Confirmation","Do you really want to submit this document?", "Once submitted, you cannot update the document.");
            startSubmissionActionPopUp.AddButtons("Yes, submit", "true");
            startSubmissionActionPopUp.AddButtons("Cancel", "false");

            //Defining trigger refs
            postActionSubmit.AddTriggerRefs("0", centralAdminNotificationEmailTrigger.Id, "Central Admin Notification Email Trigger");
            postActionSubmit.AddTriggerRefs("1", ownerSubmissionNotificationEmailTrigger.Id, "Owner Submission-notification Email Trigger");

            //Defining authorizatios
            startSubmissionAction.AddAuthorizedRole(emptyState.Id, departmentAdmin.Id);
            startSubmissionAction.AddAuthorizedDomain(emptyState.Id, "@ualberta.ca");
            startSubmissionAction.AddAuthorizedDomain(emptyState.Id, "@ucalgary.ca");


            // ================================================
            // List submission-instances related workflow items
            // ================================================

            //Defining actions
            GetAction listSubmissionAction = workflow.AddAction("List Submissions", nameof(TemplateOperations.ListInstances), "Home");

            //Defining states and their authorizatios
            listSubmissionAction.GetStateReference(submittedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(savedState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            // ================================================
            // Read submission-instances related workflow items
            // ================================================

            //Defining actions
            GetAction viewDetailsSubmissionAction = workflow.AddAction("Details", nameof(TemplateOperations.Read), "List");

            //Defining states and their authorizatios
            viewDetailsSubmissionAction.GetStateReference(submittedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(savedState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);



            // Edit submission related workflow items
            // =======================================
            //Defining actions
            GetAction editSubmissionAction = workflow.AddAction("Edit Submission", nameof(TemplateOperations.Update), "Details");

            //Defining post actions
            PostAction editSubmissionPostActionSave = editSubmissionAction.AddPostAction("Save", "Save");
            PostAction editSubmissionPostActionSubmit = editSubmissionAction.AddPostAction("Submit", "Save");

            //Defining state mappings
            editSubmissionPostActionSave.AddStateMapping(savedState.Id, savedState.Id, "Save");
            editSubmissionPostActionSave.AddStateMapping(deanOfficeRevisionState.Id, deanOfficeRevisionSaveState.Id, "Save");
            editSubmissionPostActionSave.AddStateMapping(aacRevisionState.Id, aacRevisionSaveState.Id, "Save");
            editSubmissionPostActionSave.AddStateMapping(aecRevisionState.Id, aecRevisionSaveState.Id, "Save");
            editSubmissionPostActionSave.AddStateMapping(afcRevisionState.Id, afcRevisionSaveState.Id, "Save");
            editSubmissionPostActionSave.AddStateMapping(gfcRevisionState.Id, gfcRevisionSaveState.Id, "Save");


            editSubmissionPostActionSubmit.AddStateMapping(savedState.Id, submittedState.Id, "Submit");
            editSubmissionPostActionSubmit.AddStateMapping(submittedState.Id, submittedState.Id, "Submit");
            editSubmissionPostActionSubmit.AddStateMapping(deanOfficeRevisionState.Id, deanOfficeRevisionCompletedState.Id, "Submit");
            editSubmissionPostActionSubmit.AddStateMapping(deanOfficeRevisionSaveState.Id, deanOfficeRevisionCompletedState.Id, "Submit");
            editSubmissionPostActionSubmit.AddStateMapping(aacRevisionState.Id, aacRevisionCompletedState.Id, "Submit");
            editSubmissionPostActionSubmit.AddStateMapping(aacRevisionSaveState.Id, aacRevisionCompletedState.Id, "Submit");
            editSubmissionPostActionSubmit.AddStateMapping(aecRevisionState.Id, aecRevisionCompletedState.Id, "Submit");
            editSubmissionPostActionSubmit.AddStateMapping(aecRevisionSaveState.Id, aecRevisionCompletedState.Id, "Submit");
            editSubmissionPostActionSubmit.AddStateMapping(afcRevisionState.Id, afcRevisionCompletedState.Id, "Submit");
            editSubmissionPostActionSubmit.AddStateMapping(afcRevisionSaveState.Id, afcRevisionCompletedState.Id, "Submit");
            editSubmissionPostActionSubmit.AddStateMapping(gfcRevisionState.Id, gfcRevisionCompletedState.Id, "Submit");
            editSubmissionPostActionSubmit.AddStateMapping(gfcRevisionSaveState.Id, gfcRevisionCompletedState.Id, "Submit");


            //Defining the pop-up for the above postActionSubmit action
            PopUp EditSubmissionActionPopUpopUp = editSubmissionPostActionSubmit.AddPopUp("Confirmation", "Do you really want to submit this document?", "Once submitted, you cannot update the document.");
            EditSubmissionActionPopUpopUp.AddButtons("Yes, submit", "true");
            EditSubmissionActionPopUpopUp.AddButtons("Cancel", "false");

            //Defining trigger refs
            editSubmissionPostActionSubmit.AddTriggerRefs("0", centralAdminNotificationEmailTrigger.Id, "Central Admin Notification Email Trigger");
            editSubmissionPostActionSubmit.AddTriggerRefs("1", ownerSubmissionNotificationEmailTrigger.Id, "Owner Submission-notification Email Trigger");

            //Defining state referances
            editSubmissionAction.GetStateReference(savedState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);
            editSubmissionAction.GetStateReference(submittedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            editSubmissionAction.GetStateReference(deanOfficeRevisionState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);
            editSubmissionAction.GetStateReference(deanOfficeRevisionSaveState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);
            editSubmissionAction.GetStateReference(aacRevisionState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);
            editSubmissionAction.GetStateReference(aacRevisionSaveState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);
            editSubmissionAction.GetStateReference(aecRevisionState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);
            editSubmissionAction.GetStateReference(aecRevisionSaveState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);
            editSubmissionAction.GetStateReference(afcRevisionState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);
            editSubmissionAction.GetStateReference(afcRevisionSaveState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);
            editSubmissionAction.GetStateReference(gfcRevisionState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);
            editSubmissionAction.GetStateReference(gfcRevisionSaveState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            //editSubmissionAction.AddStateReferances(savedState.Id);
            //editSubmissionAction.AddStateReferances(deanOfficeRevisionState.Id);
            //editSubmissionAction.AddStateReferances(deanOfficeRevisionSaveState.Id);
            //editSubmissionAction.AddStateReferances(aacRevisionState.Id);
            //editSubmissionAction.AddStateReferances(aacRevisionSaveState.Id);
            //editSubmissionAction.AddStateReferances(aecRevisionState.Id);
            //editSubmissionAction.AddStateReferances(aecRevisionSaveState.Id);
            //editSubmissionAction.AddStateReferances(afcRevisionState.Id);
            //editSubmissionAction.AddStateReferances(afcRevisionSaveState.Id);
            //editSubmissionAction.AddStateReferances(gfcRevisionState.Id);
            //editSubmissionAction.AddStateReferances(gfcRevisionSaveState.Id);



            //Defining authorizatios
            //RoleReference roleRef = editSubmissionAction.AddAuthorizedRole(savedState.Id, departmentAdmin.Id);


            // Delete submission related workflow items
            //Defining actions
            GetAction deleteSubmissionAction = workflow.AddAction("Delete Submission", "Delete", "Details");

            //Defining post actions
            PostAction deleteSubmissionPostAction = deleteSubmissionAction.AddPostAction("Delete", "Save");
            deleteSubmissionPostAction.AddStateMapping(savedState.Id, deleteState.Id, "Delete");

            //Defining the pop-up for the above postActionSubmit action
            PopUp deleteSubmissionActionPopUpopUp = deleteSubmissionPostAction.AddPopUp("Confirmation", "Do you really want to delete this document?", "Once deleted, you cannot access this document.");
            deleteSubmissionActionPopUpopUp.AddButtons("Yes, delete", "true");
            deleteSubmissionActionPopUpopUp.AddButtons("Cancel", "false");

            //Defining state referances
            deleteSubmissionAction.AddStateReferances(savedState.Id);

            //Defining authorizatios
            //deleteSubmissionAction.AddAuthorization(ownerRole.Id);


            // Purge submission related workflow items
            //Defining actions
            GetAction purgeSubmissionAction = workflow.AddAction("Purge Submission", "Purge", "Details");

            //Defining post actions
            PostAction purgeSubmissionPostAction = purgeSubmissionAction.AddPostAction("Purge", "Purge");
            deleteSubmissionPostAction.AddStateMapping(deleteState.Id, emptyState.Id, "Purge");
            //Defining the pop-up for the above postActionSubmit action
            PopUp purgeSubmissionActionPopUpopUp = purgeSubmissionPostAction.AddPopUp("Confirmation", "Do you really want to purge this document ? ", "Once purged, you cannot recover this document.");
            purgeSubmissionActionPopUpopUp.AddButtons("Yes, purge", "true");
            purgeSubmissionActionPopUpopUp.AddButtons("Cancel", "false");

            //Defining state referances
            purgeSubmissionAction.AddStateReferances(deleteState.Id);

            //Defining authorizatios
            //purgeSubmissionAction.AddAuthorization(ownerRole.Id);


            // Revision request related workflow items
            //Defining actions
            GetAction sendForRevisionSubmissionAction = workflow.AddAction("Send for Revision", nameof(TemplateOperations.ChangeState), "Details");

            //Define Revision Template
            sendForRevisionSubmissionAction.AddTemplate(commentsForm.Id, "Submission Revision Template");

            //Defining post actions
            PostAction sendForRevisionSubmissionPostAction = sendForRevisionSubmissionAction.AddPostAction("Send for Revision", "ChangeState");

            //Defining state mappings
            sendForRevisionSubmissionPostAction.AddStateMapping(submittedState.Id, deanOfficeRevisionState.Id, "Send for Revision");
            sendForRevisionSubmissionPostAction.AddStateMapping(aacWithState.Id, aacRevisionState.Id, "Send for Revision");
            sendForRevisionSubmissionPostAction.AddStateMapping(aecWithState.Id, aecRevisionState.Id, "Send for Revision");
            sendForRevisionSubmissionPostAction.AddStateMapping(afcWithState.Id, afcRevisionState.Id, "Send for Revision");
            sendForRevisionSubmissionPostAction.AddStateMapping(gfcWithState.Id, gfcRevisionState.Id, "Send for Revision");
            sendForRevisionSubmissionPostAction.AddStateMapping(deanOfficeRevisionCompletedState.Id, deanOfficeRevisionState.Id, "Send for Revision");
            sendForRevisionSubmissionPostAction.AddStateMapping(aacRevisionCompletedState.Id, aacRevisionState.Id, "Send for Revision");
            sendForRevisionSubmissionPostAction.AddStateMapping(aecRevisionCompletedState.Id, aecRevisionState.Id, "Send for Revision");
            sendForRevisionSubmissionPostAction.AddStateMapping(afcRevisionCompletedState.Id, afcRevisionState.Id, "Send for Revision");
            sendForRevisionSubmissionPostAction.AddStateMapping(gfcRevisionCompletedState.Id, gfcRevisionState.Id, "Send for Revision");

            //Defining the pop-up for the above sendForRevisionSubmissionPostAction action
            PopUp sendForRevisionSubmissionActionPopUpopUp = sendForRevisionSubmissionPostAction.AddPopUp("Confirmation", "Do you really want to revise this document ? ", "");
            sendForRevisionSubmissionActionPopUpopUp.AddButtons("Yes", "true");
            sendForRevisionSubmissionActionPopUpopUp.AddButtons("Cancel", "false");

            //Defining trigger refs
            sendForRevisionSubmissionPostAction.AddTriggerRefs("0", RevisionNotificationEmailTrigger.Id, "Send for Revision Notification Email Trigger");

            //Defining states and their authorizatios
            sendForRevisionSubmissionAction.GetStateReference(submittedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            sendForRevisionSubmissionAction.GetStateReference(deanOfficeRevisionCompletedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            sendForRevisionSubmissionAction.GetStateReference(aacWithState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            sendForRevisionSubmissionAction.GetStateReference(aacRevisionCompletedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            sendForRevisionSubmissionAction.GetStateReference(aecWithState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            sendForRevisionSubmissionAction.GetStateReference(aecRevisionCompletedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            sendForRevisionSubmissionAction.GetStateReference(afcWithState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            sendForRevisionSubmissionAction.GetStateReference(afcRevisionCompletedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            sendForRevisionSubmissionAction.GetStateReference(gfcWithState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            sendForRevisionSubmissionAction.GetStateReference(gfcRevisionCompletedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            ////Defining state referances
            //sendForRevisionSubmissionAction.AddStateReferances(submittedState.Id);
            //sendForRevisionSubmissionAction.AddStateReferances(deanOfficeRevisionCompletedState.Id);
            //sendForRevisionSubmissionAction.AddStateReferances(aacWithState.Id);
            //sendForRevisionSubmissionAction.AddStateReferances(aacRevisionCompletedState.Id);
            //sendForRevisionSubmissionAction.AddStateReferances(aecWithState.Id);
            //sendForRevisionSubmissionAction.AddStateReferances(aecRevisionCompletedState.Id);
            //sendForRevisionSubmissionAction.AddStateReferances(afcWithState.Id);
            //sendForRevisionSubmissionAction.AddStateReferances(afcRevisionCompletedState.Id);
            //sendForRevisionSubmissionAction.AddStateReferances(gfcWithState.Id);
            //sendForRevisionSubmissionAction.AddStateReferances(gfcRevisionCompletedState.Id);

            ////Defining authorizatios
            //sendForRevisionSubmissionAction.AddAuthorizedRole(submittedState.Id, centralAdminRole.Id);

            // Revision request related workflow items
            //Defining actions
            GetAction changeStateAction = workflow.AddAction("Update Document State", nameof(TemplateOperations.ChangeState), "Details");

            //Define Revision Template
            changeStateAction.AddTemplate(commentsForm.Id, "Submission Change State");

            //Defining post actions
            PostAction changeStatePostAction = changeStateAction.AddPostAction("Change State", "Comment");

            //Defining state mappings
            changeStatePostAction.AddStateMapping(submittedState.Id, aacWithState.Id, "With AAC");
            changeStatePostAction.AddStateMapping(deanOfficeRevisionCompletedState.Id, aacWithState.Id, "With AAC");
            changeStatePostAction.AddStateMapping(aacWithState.Id, aacApprovedState.Id, "AAC Approved");

            changeStatePostAction.AddStateMapping(aacApprovedState.Id, aecWithState.Id, "With AEC");
            changeStatePostAction.AddStateMapping(aecWithState.Id, aecApprovedState.Id, "AEC Approved");

            changeStatePostAction.AddStateMapping(aacRevisionCompletedState.Id, aacWithState.Id, "With AAC");
            changeStatePostAction.AddStateMapping(aacRevisionCompletedState.Id, aecWithState.Id, "With AEC");

            changeStatePostAction.AddStateMapping(aecRevisionCompletedState.Id, aacWithState.Id, "With AAC");
            changeStatePostAction.AddStateMapping(aecRevisionCompletedState.Id, aecWithState.Id, "With AEC");

            changeStatePostAction.AddStateMapping(aecApprovedState.Id, afcWithState.Id, "With AFC");
            changeStatePostAction.AddStateMapping(afcWithState.Id, afcApprovedState.Id, "AFC Approved");
            changeStatePostAction.AddStateMapping(aecRevisionCompletedState.Id, afcWithState.Id, "With AFC");
            changeStatePostAction.AddStateMapping(afcRevisionCompletedState.Id, afcWithState.Id, "With AFC");
            changeStatePostAction.AddStateMapping(afcRevisionCompletedState.Id, aecWithState.Id, "With AEC");
            changeStatePostAction.AddStateMapping(afcRevisionCompletedState.Id, aacWithState.Id, "With AAC");
            changeStatePostAction.AddStateMapping(afcRevisionCompletedState.Id, afcApprovedState.Id, "AFC Approved");

            changeStatePostAction.AddStateMapping(afcApprovedState.Id, gfcWithState.Id, "With GFC");
            changeStatePostAction.AddStateMapping(gfcWithState.Id, gfcApprovedState.Id, "GFC Approved");
            changeStatePostAction.AddStateMapping(gfcRevisionCompletedState.Id, aacWithState.Id, "With AAC");
            changeStatePostAction.AddStateMapping(gfcRevisionCompletedState.Id, aecWithState.Id, "With AEC");
            changeStatePostAction.AddStateMapping(gfcRevisionCompletedState.Id, afcWithState.Id, "With AFC");
            changeStatePostAction.AddStateMapping(gfcRevisionCompletedState.Id, gfcWithState.Id, "With GFC");

            //Defining the pop-up for the above sendForRevisionSubmissionPostAction action
            PopUp changeStateActionPopUpopUp = changeStatePostAction.AddPopUp("Confirmation", "Do you really want to change status ? ", "Once changed, you cannot revise this document.");
            changeStateActionPopUpopUp.AddButtons("Yes", "true");
            changeStateActionPopUpopUp.AddButtons("Cancel", "false");


            //Defining states and their authorizatios
            changeStateAction.GetStateReference(submittedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            changeStateAction.GetStateReference(deanOfficeRevisionCompletedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            changeStateAction.GetStateReference(aacWithState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            changeStateAction.GetStateReference(aacRevisionCompletedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            changeStateAction.GetStateReference(aecWithState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            changeStateAction.GetStateReference(aecRevisionCompletedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            changeStateAction.GetStateReference(afcWithState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            changeStateAction.GetStateReference(afcRevisionCompletedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            changeStateAction.GetStateReference(gfcWithState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            changeStateAction.GetStateReference(gfcRevisionCompletedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            changeStateAction.GetStateReference(aacApprovedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            changeStateAction.GetStateReference(aecApprovedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            changeStateAction.GetStateReference(afcApprovedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            changeStateAction.GetStateReference(gfcApprovedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);

            ////Defining state referances
            //changeStateAction.AddStateReferances(submittedState.Id);
            //changeStateAction.AddStateReferances(deanOfficeRevisionCompletedState.Id);
            //changeStateAction.AddStateReferances(aacWithState.Id);
            //changeStateAction.AddStateReferances(aacRevisionCompletedState.Id);
            //changeStateAction.AddStateReferances(aecWithState.Id);
            //changeStateAction.AddStateReferances(aecRevisionCompletedState.Id);
            //changeStateAction.AddStateReferances(afcWithState.Id);
            //changeStateAction.AddStateReferances(afcRevisionCompletedState.Id);
            //changeStateAction.AddStateReferances(gfcWithState.Id);
            //changeStateAction.AddStateReferances(gfcRevisionCompletedState.Id);
            //changeStateAction.AddStateReferances(aacApprovedState.Id);
            //changeStateAction.AddStateReferances(aecApprovedState.Id);
            //changeStateAction.AddStateReferances(afcApprovedState.Id);

            //changeStateAction.AddStateReferances(gfcApprovedState.Id);

            ////Defining authorizatios
            //changeStateAction.AddAuthorizedRole(submittedState.Id, centralAdminRole.Id);

            // Calender request move to draft related workflow items
            //Defining actions
            GetAction moveToDraftAction = workflow.AddAction("Move to Draft Calendar", "ChangeState", "Details");

            //Define Revision Template
            moveToDraftAction.AddTemplate(commentsForm.Id, "Move to draft State");

            //Defining post actions
            PostAction moveToDraftCorrectPostAction = moveToDraftAction.AddPostAction("Correct", "Comment");

            //Defining state mappings
            moveToDraftCorrectPostAction.AddStateMapping(gfcApprovedState.Id, moveToDraftCorrectState.Id, "Move To Draft Correct");
            moveToDraftCorrectPostAction.AddStateMapping(moveToDraftErrorState.Id, moveToDraftCorrectState.Id, "Move To Draft Correct");

            //Defining the pop-up for the above sendForRevisionSubmissionPostAction action
            PopUp moveToDraftCorrctActionPopUpopUp = moveToDraftCorrectPostAction.AddPopUp("Confirmation", "Do you really want to move to dreaft this document?", "Once moved, you cannot revise this document.");
            moveToDraftCorrctActionPopUpopUp.AddButtons("Yes", "true");
            moveToDraftCorrctActionPopUpopUp.AddButtons("Cancel", "false");

            //Defining trigger refs
            moveToDraftCorrectPostAction.AddTriggerRefs("0", MovedToDraftCalendarEmailTrigger.Id, "Moved to Draft Notification Email Trigger");
            
            //Defining post actions
            PostAction moveToDraftErrorPostAction = moveToDraftAction.AddPostAction("With Error", "Comment");

            //Defining state mappings
            moveToDraftErrorPostAction.AddStateMapping(moveToDraftErrorState.Id, moveToDraftCorrectState.Id, "Move To Draft Correct");

            //Defining the pop-up for the above sendForRevisionSubmissionPostAction action
            PopUp moveToDraftErrorActionPopUpopUp = moveToDraftErrorPostAction.AddPopUp("Confirmation", "Do you really want to move to draft in error state this document?", "Once moved, you cannot access this document.");
            moveToDraftErrorActionPopUpopUp.AddButtons("Yes", "true");
            moveToDraftErrorActionPopUpopUp.AddButtons("Cancel", "false");
            
            //Defining trigger refs
            moveToDraftErrorPostAction.AddTriggerRefs("0", MovedToDraftCalendarEmailTrigger.Id, "Moved to Draft Notification Email Trigger");

            moveToDraftAction.GetStateReference(gfcApprovedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            changeStateAction.GetStateReference(moveToDraftErrorState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id);
            ////Defining state referances
            //moveToDraftAction.AddStateReferances(gfcApprovedState.Id);
            //moveToDraftAction.AddStateReferances(moveToDraftErrorState.Id);

            ////Defining authorizatios
            //moveToDraftAction.AddAuthorizedRole(gfcApprovedState.Id, centralAdminRole.Id);

            auth.EnsureUserRoles(workflow.GetWorkflowRoles());
            auth.EnsureGroups(workflow.GetWorkflowGroups(), template.Id);

            //Save the template to the database
            ////AppDbContext db = _testHelper.Db;
            ////EntityTemplate oldTemplate = db.EntityTemplates.Where(et => et.TemplateName == template.TemplateName).FirstOrDefault();
            ////if (oldTemplate == null)
            ////    db.EntityTemplates.Add(template);
            ////else
            ////{
            ////    template.Id = oldTemplate.Id;
            ////    oldTemplate.Content = template.Content;
            ////}

            db.SaveChanges();


            template.Data.Save("..\\..\\..\\..\\Examples\\CalendarManagementWorkflow_generared.xml");

        }

        /// <summary>
        /// creating safety inspection form
        /// </summary>
        [Test]
        public void CentralAmericaContactFormTest()
        {
            string lang = "en";
            string templateName = "Central America Contact Form";
          //  string centralAdminEmail = "artsrnd@ualberta.ca";


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

            ws.SetModel(template);

            //Get the Workflow object using the workflow service
            Workflow workflow = ws.GetWorkflow(true);

            //Defininig states
            State emptyState = workflow.AddState(ws.GetStatus(template.Id, "", true));
            State savedState = workflow.AddState(ws.GetStatus(template.Id, "Saved", true));
            State submittedState = workflow.AddState(ws.GetStatus(template.Id, "Submitted", true));
            State deleteState = workflow.AddState(ws.GetStatus(template.Id, "Deleted", true));


            //Defining email templates
            EmailTemplate centralAdminNotification = ws.GetEmailTemplate("Event Admin Notification", true);
            centralAdminNotification.SetDescription("This metadata set defines the email template to be sent to the event admin when a user register.", lang);
            centralAdminNotification.SetSubject("Central America Conference");
            centralAdminNotification.SetBody("A @Link[registration form|@Model] was submitted.\n\nThank you"); //???


            //Defininig the Submission revision Request form
            DataItem registrationForm = template.GetDataItem("Central America Registration Form", true, lang);
            registrationForm.IsRoot = true;
            registrationForm.SetDescription("This is the form to be filled by users when a registration is submitted.", lang);
            
            registrationForm.CreateField<TextField>("First Name", lang, true);
            registrationForm.CreateField<TextField>("Last Name", lang, true);  
            registrationForm.CreateField<TextField>("Email", lang, true, true);
            registrationForm.CreateField<TextField>("Country", lang, true);

           

            //Defininig roles
            WorkflowRole centralAdminRole = workflow.AddRole(auth.GetRole("Admin", true));
          
           
            // start submission related workflow items
            //Defining actions
            GetAction startSubmissionAction = workflow.AddAction("Start Registration Submission", nameof(TemplateOperations.Instantiate), "Home");
            startSubmissionAction.Access = GetAction.eAccess.Public;

            //Defining form template
            startSubmissionAction.AddTemplate(registrationForm.Id, "Start Registration Template");

            //Defining post actions
            PostAction postActionSave = startSubmissionAction.AddPostAction("Save", nameof(TemplateOperations.Update));
            postActionSave.AddStateMapping(emptyState.Id, savedState.Id, "Save");
            PostAction postActionSubmit = startSubmissionAction.AddPostAction("Submit", nameof(TemplateOperations.Update));
            postActionSubmit.AddStateMapping(emptyState.Id, submittedState.Id, "Submit");

          

            // Edit submission related workflow items
            //Defining actions
            GetAction editSubmissionAction = workflow.AddAction("Edit Submission", "Edit", "Details");

            //Defining post actions
            PostAction editSubmissionPostActionSave = editSubmissionAction.AddPostAction("Save", "Save");
            PostAction editSubmissionPostActionSubmit = editSubmissionAction.AddPostAction("Submit", "Save");

            //Defining state mappings
            editSubmissionPostActionSave.AddStateMapping(savedState.Id, savedState.Id, "Save");
            


            editSubmissionPostActionSubmit.AddStateMapping(savedState.Id, submittedState.Id, "Submit");
           


            //Defining the pop-up for the above postActionSubmit action
            PopUp EditSubmissionActionPopUpopUp = editSubmissionPostActionSubmit.AddPopUp("WARNING: Submitting Document", "Once submitted, you cannot make any changes. Are you sure you want to continue?","");
            EditSubmissionActionPopUpopUp.AddButtons("Yes, submit", "true");
            EditSubmissionActionPopUpopUp.AddButtons("Cancel", "false");

            //Defining trigger refs
            //editSubmissionPostActionSubmit.AddTriggerRefs("0", centralAdminNotificationEmailTrigger.Id, "Central Admin Notification Email Trigger");
            //editSubmissionPostActionSubmit.AddTriggerRefs("1", ownerSubmissionNotificationEmailTrigger.Id, "Owner Submission-notification Email Trigger");

            //Defining state referances
            editSubmissionAction.AddStateReferances(savedState.Id);



            //Defining authorizatios
            editSubmissionAction.AddAuthorizedRole(submittedState.Id, centralAdminRole.Id);
           // editSubmissionAction.AddAuthorizedRole(supervisorRole.Id);

            // Delete submission related workflow items
            //Defining actions
            GetAction deleteSubmissionAction = workflow.AddAction("Delete Registration Submission", "Delete", "Details");

            //Defining post actions
            PostAction deleteSubmissionPostAction = deleteSubmissionAction.AddPostAction("Delete", "Save");
            deleteSubmissionPostAction.AddStateMapping(savedState.Id, deleteState.Id, "Delete");

            //Defining the pop-up for the above postActionSubmit action
            PopUp deleteSubmissionActionPopUpopUp = deleteSubmissionPostAction.AddPopUp("WARNING: Delete", "Are you sure you want to delete the registration submission?","");
            deleteSubmissionActionPopUpopUp.AddButtons("Yes, delete", "true");
            deleteSubmissionActionPopUpopUp.AddButtons("Cancel", "false");

            //Defining state referances
            deleteSubmissionAction.AddStateReferances(savedState.Id);

            //MR Nov 23 2020
            GetAction readSubmissionAction = workflow.AddAction("List", "Read", "List");
           // GetAction readSubmissionAction = workflow.AddAction("List", nameof(TemplateOperations.Read), "List");
            readSubmissionAction.Access = GetAction.eAccess.Restricted;
            readSubmissionAction.AddAuthorizedRole(submittedState.Id, centralAdminRole.Id);

            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\centralAmericaRegistration_generared.xml");
        }

        [Test]
        public void SafetyInspectionWorkflowBuildTest()
        {
            string lang = "en";
            string templateName = "Safety Inspection System Workflow";

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

            ws.SetModel(template);

            //Get the Workflow object using the workflow service
            Workflow workflow = ws.GetWorkflow(true);

            //Defininig states
            State emptyState = workflow.AddState(ws.GetStatus(template.Id, "", true));
            State savedState = workflow.AddState(ws.GetStatus(template.Id, "Saved", true));
            State submittedState = workflow.AddState(ws.GetStatus(template.Id, "Submitted", true));
            State deleteState = workflow.AddState(ws.GetStatus(template.Id, "Deleted", true));




            //Defining email templates
            EmailTemplate centralAdminNotification = ws.GetEmailTemplate("Central Admin Notification", true);
            centralAdminNotification.SetDescription("This metadata set defines the email template to be sent to the central admin when a dept admin makes a submission.", lang);
            centralAdminNotification.SetSubject("Safety Inspection Submission");
            centralAdminNotification.SetBody("A @Link[safety form|@Model] was submitted.\n\nThank you"); 

            EmailTemplate deptAdminSubmissionNotification = ws.GetEmailTemplate("User Notification", true);
            deptAdminSubmissionNotification.SetDescription("This metadata set defines the email template to be sent to the dept admin when he/she submits a safety inspection form.", lang);
            deptAdminSubmissionNotification.SetSubject("Safety Inspection Submission");
            deptAdminSubmissionNotification.SetBody("A @Link[[safety form|@Model] was submitted.\n\nThank you");



            //Defininig the Submission revision Request form
            DataItem inspectionForm = template.GetDataItem("Safety Inspection Form", true, lang);
            inspectionForm.IsRoot = true;
            inspectionForm.SetDescription("This is the form to be filled by the central admin when a submission revision is requested.", lang);
            inspectionForm.CreateField<InfoSection>("BUILDING AND WORK SPACE", lang);
            string[] optionText = new string[] { "Yes", "No", "N/A" };
            inspectionForm.CreateField<OptionsField>("Are floors clean and free of loose materials and debris?", lang, optionText);
            inspectionForm.CreateField<OptionsField>("Are floors free from protruding nails, splinters, holes and loose boards?", lang, optionText);
            inspectionForm.CreateField<OptionsField>("Are floors free of oil and water spillage or leakage?", lang, optionText);
            inspectionForm.CreateField<OptionsField>("Is absorbent available for immediate clean-up of spills and leaks?", lang, optionText);
            inspectionForm.CreateField<OptionsField>("Are walkways, stairways and aisles kept clear of obstructions? ", lang, optionText);
            inspectionForm.CreateField<OptionsField>("Are stairs and handrails in good condition?", lang, optionText);
            inspectionForm.CreateField<OptionsField>("Are doorways/exits clear of materials or equipment?", lang, optionText);
            inspectionForm.CreateField<OptionsField>("Are entrances/exit doors in good working order?", lang, optionText);
            inspectionForm.CreateField<OptionsField>("Are wall and ceiling fixtures fastened securely?", lang, optionText);
            inspectionForm.CreateField<OptionsField>("Are ventilation and exhaust fans in good working order?", lang, optionText);
            inspectionForm.CreateField<TextArea>("Comments", lang, true);

            //Defining name mappings
            //TODO: Add functionality for EntityTemplate to allow us define a sequence of metadata set fields
            //      to be used as table headings in the list view. Use this functionality to specify the
            //      Course Name and Course Number as the list-view table headings for this schema.
            //      In the actual listing page, we should show the values of these set of fields and the "owner"
            //      of the root data object, the created time-stamp of the root data object, and the satust of the
            //      entity.



            //Defininig roles
            WorkflowRole centralAdminRole = workflow.AddRole(auth.GetRole("Admin", true));
            WorkflowRole supervisorRole = workflow.AddRole(auth.GetRole("Supervisor", true));

            //Defining triggers
            //EmailTrigger centralAdminNotificationEmailTrigger = workflow.AddTrigger("ToCentralAdmin", "SendEmail");
            //centralAdminNotificationEmailTrigger.AddRecipientByEmail("centraladmin@ualberta.ca");
            //centralAdminNotificationEmailTrigger.AddTemplate(centralAdminNotification.Id, "Central Admin Notification");

            //EmailTrigger ownerSubmissionNotificationEmailTrigger = workflow.AddTrigger("ToOwnerOnDocumentSubmission", "SendEmail");
            //ownerSubmissionNotificationEmailTrigger.AddOwnerAsRecipient();
            //ownerSubmissionNotificationEmailTrigger.AddTemplate(deptAdminSubmissionNotification.Id, "Owner's submission-notification");


            // start submission related workflow items
            //Defining actions
            GetAction startSubmissionAction = workflow.AddAction("Start Submission", nameof(TemplateOperations.Instantiate), "Home");

            //Defining form template
            startSubmissionAction.AddTemplate(inspectionForm.Id, "Start Submission Template");

            //Defining post actions
            PostAction postActionSave = startSubmissionAction.AddPostAction("Save", nameof(TemplateOperations.Update));
            postActionSave.AddStateMapping(emptyState.Id, savedState.Id, "Save");
            PostAction postActionSubmit = startSubmissionAction.AddPostAction("Submit", nameof(TemplateOperations.Update));
            postActionSubmit.AddStateMapping(emptyState.Id, submittedState.Id, "Submit");

            //Defining the pop-up for the above postActionSubmit action
            PopUp startSubmissionActionPopUp = postActionSubmit.AddPopUp("WARNING: Submitting Document", "Once submitted, you cannot update the document.","");
            startSubmissionActionPopUp.AddButtons("Yes, submit", "true");
            startSubmissionActionPopUp.AddButtons("Cancel", "false");

            //Defining trigger refs
            //postActionSubmit.AddTriggerRefs("0", centralAdminNotificationEmailTrigger.Id, "Central Admin Notification Email Trigger");
            //postActionSubmit.AddTriggerRefs("1", ownerSubmissionNotificationEmailTrigger.Id, "Owner Submission-notification Email Trigger");

            //Defining authorizatios
            // startSubmissionAction.AddAuthorizedRole(supervisorRole.Id);
            startSubmissionAction.AddAuthorizedDomain(emptyState.Id, "@ualberta.ca");


            // Edit submission related workflow items
            //Defining actions
            GetAction editSubmissionAction = workflow.AddAction("Edit Submission", "Edit", "Details");

            //Defining post actions
            PostAction editSubmissionPostActionSave = editSubmissionAction.AddPostAction("Save", "Save");
            PostAction editSubmissionPostActionSubmit = editSubmissionAction.AddPostAction("Submit", "Save");

            //Defining state mappings
            editSubmissionPostActionSave.AddStateMapping(savedState.Id, savedState.Id, "Save");



            editSubmissionPostActionSubmit.AddStateMapping(savedState.Id, submittedState.Id, "Submit");



            //Defining the pop-up for the above postActionSubmit action
            PopUp EditSubmissionActionPopUpopUp = editSubmissionPostActionSubmit.AddPopUp("WARNING: Submitting Document", "Once submitted, you cannot update the document.","");
            EditSubmissionActionPopUpopUp.AddButtons("Yes, submit", "true");
            EditSubmissionActionPopUpopUp.AddButtons("Cancel", "false");

            //Defining trigger refs
            //editSubmissionPostActionSubmit.AddTriggerRefs("0", centralAdminNotificationEmailTrigger.Id, "Central Admin Notification Email Trigger");
            //editSubmissionPostActionSubmit.AddTriggerRefs("1", ownerSubmissionNotificationEmailTrigger.Id, "Owner Submission-notification Email Trigger");

            //Defining state referances
            editSubmissionAction.AddStateReferances(savedState.Id);



            //Defining authorizatios
            editSubmissionAction.AddAuthorizedRole(submittedState.Id, centralAdminRole.Id);
            editSubmissionAction.AddAuthorizedRole(savedState.Id, supervisorRole.Id);

            // Delete submission related workflow items
            //Defining actions
            GetAction deleteSubmissionAction = workflow.AddAction("Delete Submission", "Delete", "Details");

            //Defining post actions
            PostAction deleteSubmissionPostAction = deleteSubmissionAction.AddPostAction("Delete", "Save");
            deleteSubmissionPostAction.AddStateMapping(savedState.Id, deleteState.Id, "Delete");

            //Defining the pop-up for the above postActionSubmit action
            PopUp deleteSubmissionActionPopUpopUp = deleteSubmissionPostAction.AddPopUp("WARNING: Delete", "Deleting the submission. Please confirm.","");
            deleteSubmissionActionPopUpopUp.AddButtons("Yes, delete", "true");
            deleteSubmissionActionPopUpopUp.AddButtons("Cancel", "false");

            //Defining state referances
            deleteSubmissionAction.AddStateReferances(savedState.Id);

            //Defining authorizatios
            //deleteSubmissionAction.AddAuthorization(ownerRole.Id);


            //// Purge submission related workflow items
            ////Defining actions
            //GetAction purgeSubmissionAction = workflow.AddAction("Purge Submission", "Purge", "Details");

            ////Defining post actions
            //PostAction purgeSubmissionPostAction = purgeSubmissionAction.AddPostAction("Purge", "Purge");
            //deleteSubmissionPostAction.AddStateMapping(deleteState.Id, emptyState.Id, "Purge");
            ////Defining the pop-up for the above postActionSubmit action
            //PopUp purgeSubmissionActionPopUpopUp = purgeSubmissionPostAction.AddPopUp("WARNING: Deleting Permanently", "When purged, the document cannot be recovered. Please confirm.");
            //purgeSubmissionActionPopUpopUp.AddButtons("Yes, purge", "true");
            //purgeSubmissionActionPopUpopUp.AddButtons("Cancel", "false");

            ////Defining state referances
            //purgeSubmissionAction.AddStateReferances(deleteState.Id);

            ////Defining authorizatios
            ////purgeSubmissionAction.AddAuthorization(ownerRole.Id);


            //// Revision request related workflow items
            ////Defining actions
            //GetAction sendForRevisionSubmissionAction = workflow.AddAction("Send for Revision", "ChangeState", "Details");

            ////Define Revision Template
            //sendForRevisionSubmissionAction.AddTemplate(commentsForm.Id, "Submission Revision Template");

            ////Defining post actions
            //PostAction sendForRevisionSubmissionPostAction = sendForRevisionSubmissionAction.AddPostAction("Send for Revision", "ChangeState");

            ////Defining state mappings
            //sendForRevisionSubmissionPostAction.AddStateMapping(submittedState.Id, deanOfficeRevisionState.Id, "Send for Revision");
            //sendForRevisionSubmissionPostAction.AddStateMapping(aacWithState.Id, aacRevisionState.Id, "Send for Revision");
            //sendForRevisionSubmissionPostAction.AddStateMapping(aecWithState.Id, aecRevisionState.Id, "Send for Revision");
            //sendForRevisionSubmissionPostAction.AddStateMapping(afcWithState.Id, afcRevisionState.Id, "Send for Revision");
            //sendForRevisionSubmissionPostAction.AddStateMapping(gfcWithState.Id, gfcRevisionState.Id, "Send for Revision");
            //sendForRevisionSubmissionPostAction.AddStateMapping(deanOfficeRevisionCompletedState.Id, deanOfficeRevisionState.Id, "Send for Revision");
            //sendForRevisionSubmissionPostAction.AddStateMapping(aacRevisionCompletedState.Id, aacRevisionState.Id, "Send for Revision");
            //sendForRevisionSubmissionPostAction.AddStateMapping(aecRevisionCompletedState.Id, aecRevisionState.Id, "Send for Revision");
            //sendForRevisionSubmissionPostAction.AddStateMapping(afcRevisionCompletedState.Id, afcRevisionState.Id, "Send for Revision");
            //sendForRevisionSubmissionPostAction.AddStateMapping(gfcRevisionCompletedState.Id, gfcRevisionState.Id, "Send for Revision");

            ////Defining the pop-up for the above sendForRevisionSubmissionPostAction action
            //PopUp sendForRevisionSubmissionActionPopUpopUp = sendForRevisionSubmissionPostAction.AddPopUp("WARNING: Revision Document", "Do you really want to revise this document?.");
            //sendForRevisionSubmissionActionPopUpopUp.AddButtons("Yes", "true");
            //sendForRevisionSubmissionActionPopUpopUp.AddButtons("Cancel", "false");

            ////Defining trigger refs
            //sendForRevisionSubmissionPostAction.AddTriggerRefs("0", RevisionNotificationEmailTrigger.Id, "Send for Revision Notification Email Trigger");

            ////Defining state referances
            //sendForRevisionSubmissionAction.AddStateReferances(submittedState.Id);
            //sendForRevisionSubmissionAction.AddStateReferances(deanOfficeRevisionCompletedState.Id);
            //sendForRevisionSubmissionAction.AddStateReferances(aacWithState.Id);
            //sendForRevisionSubmissionAction.AddStateReferances(aacRevisionCompletedState.Id);
            //sendForRevisionSubmissionAction.AddStateReferances(aecWithState.Id);
            //sendForRevisionSubmissionAction.AddStateReferances(aecRevisionCompletedState.Id);
            //sendForRevisionSubmissionAction.AddStateReferances(afcWithState.Id);
            //sendForRevisionSubmissionAction.AddStateReferances(afcRevisionCompletedState.Id);
            //sendForRevisionSubmissionAction.AddStateReferances(gfcWithState.Id);
            //sendForRevisionSubmissionAction.AddStateReferances(gfcRevisionCompletedState.Id);

            ////Defining authorizatios
            //sendForRevisionSubmissionAction.AddAuthorizedRole(centralAdminRole.Id);

            //// Revision request related workflow items
            ////Defining actions
            //GetAction changeStateAction = workflow.AddAction("Update Document State", "ChangeState", "Details");

            ////Define Revision Template
            //changeStateAction.AddTemplate(commentsForm.Id, "Submission Change State");

            ////Defining post actions
            //PostAction changeStatePostAction = changeStateAction.AddPostAction("Change State", "Comment");

            ////Defining state mappings
            //changeStatePostAction.AddStateMapping(submittedState.Id, aacWithState.Id, "With AAC");
            //changeStatePostAction.AddStateMapping(deanOfficeRevisionCompletedState.Id, aacWithState.Id, "With AAC");
            //changeStatePostAction.AddStateMapping(aacWithState.Id, aacApprovedState.Id, "AAC Approved");

            //changeStatePostAction.AddStateMapping(aacApprovedState.Id, aecWithState.Id, "With AEC");
            //changeStatePostAction.AddStateMapping(aecWithState.Id, aecApprovedState.Id, "AEC Approved");

            //changeStatePostAction.AddStateMapping(aacRevisionCompletedState.Id, aacWithState.Id, "With AAC");
            //changeStatePostAction.AddStateMapping(aacRevisionCompletedState.Id, aecWithState.Id, "With AEC");

            //changeStatePostAction.AddStateMapping(aecRevisionCompletedState.Id, aacWithState.Id, "With AAC");
            //changeStatePostAction.AddStateMapping(aecRevisionCompletedState.Id, aecWithState.Id, "With AEC");

            //changeStatePostAction.AddStateMapping(aecApprovedState.Id, afcWithState.Id, "With AFC");
            //changeStatePostAction.AddStateMapping(afcWithState.Id, afcApprovedState.Id, "AFC Approved");
            //changeStatePostAction.AddStateMapping(aecRevisionCompletedState.Id, afcWithState.Id, "With AFC");
            //changeStatePostAction.AddStateMapping(afcRevisionCompletedState.Id, afcWithState.Id, "With AFC");
            //changeStatePostAction.AddStateMapping(afcRevisionCompletedState.Id, aecWithState.Id, "With AEC");
            //changeStatePostAction.AddStateMapping(afcRevisionCompletedState.Id, aacWithState.Id, "With AAC");
            //changeStatePostAction.AddStateMapping(afcRevisionCompletedState.Id, afcApprovedState.Id, "AFC Approved");

            //changeStatePostAction.AddStateMapping(afcApprovedState.Id, gfcWithState.Id, "With GFC");
            //changeStatePostAction.AddStateMapping(gfcWithState.Id, gfcApprovedState.Id, "GFC Approved");
            //changeStatePostAction.AddStateMapping(gfcRevisionCompletedState.Id, aacWithState.Id, "With AAC");
            //changeStatePostAction.AddStateMapping(gfcRevisionCompletedState.Id, aecWithState.Id, "With AEC");
            //changeStatePostAction.AddStateMapping(gfcRevisionCompletedState.Id, afcWithState.Id, "With AFC");
            //changeStatePostAction.AddStateMapping(gfcRevisionCompletedState.Id, gfcWithState.Id, "With GFC");

            ////Defining the pop-up for the above sendForRevisionSubmissionPostAction action
            //PopUp changeStateActionPopUpopUp = changeStatePostAction.AddPopUp("WARNING: Change State", "Do you really want to change the document state?.");
            //changeStateActionPopUpopUp.AddButtons("Yes", "true");
            //changeStateActionPopUpopUp.AddButtons("Cancel", "false");

            ////Defining state referances
            //changeStateAction.AddStateReferances(submittedState.Id);
            //changeStateAction.AddStateReferances(deanOfficeRevisionCompletedState.Id);
            //changeStateAction.AddStateReferances(aacWithState.Id);
            //changeStateAction.AddStateReferances(aacRevisionCompletedState.Id);
            //changeStateAction.AddStateReferances(aecWithState.Id);
            //changeStateAction.AddStateReferances(aecRevisionCompletedState.Id);
            //changeStateAction.AddStateReferances(afcWithState.Id);
            //changeStateAction.AddStateReferances(afcRevisionCompletedState.Id);
            //changeStateAction.AddStateReferances(gfcWithState.Id);
            //changeStateAction.AddStateReferances(gfcRevisionCompletedState.Id);
            //changeStateAction.AddStateReferances(aacApprovedState.Id);
            //changeStateAction.AddStateReferances(aecApprovedState.Id);
            //changeStateAction.AddStateReferances(afcApprovedState.Id);
            //changeStateAction.AddStateReferances(gfcApprovedState.Id);

            ////Defining authorizatios
            //changeStateAction.AddAuthorizedRole(centralAdminRole.Id);

            //// Calender request move to draft related workflow items
            ////Defining actions
            //GetAction moveToDraftAction = workflow.AddAction("Move to Draft Calendar", "ChangeState", "Details");

            ////Define Revision Template
            //moveToDraftAction.AddTemplate(commentsForm.Id, "Move to draft State");

            ////Defining post actions
            //PostAction moveToDraftCorrectPostAction = moveToDraftAction.AddPostAction("Correct", "Comment");

            ////Defining state mappings
            //moveToDraftCorrectPostAction.AddStateMapping(gfcApprovedState.Id, moveToDraftCorrectState.Id, "Move To Draft Correct");
            //moveToDraftCorrectPostAction.AddStateMapping(moveToDraftErrorState.Id, moveToDraftCorrectState.Id, "Move To Draft Correct");

            ////Defining the pop-up for the above sendForRevisionSubmissionPostAction action
            //PopUp moveToDraftCorrctActionPopUpopUp = moveToDraftCorrectPostAction.AddPopUp("WARNING: In the Draft Calendar -  Correct", "Do you really want to change the document state?.");
            //moveToDraftCorrctActionPopUpopUp.AddButtons("Yes", "true");
            //moveToDraftCorrctActionPopUpopUp.AddButtons("Cancel", "false");

            ////Defining trigger refs
            //moveToDraftCorrectPostAction.AddTriggerRefs("0", MovedToDraftCalendarEmailTrigger.Id, "Moved to Draft Notification Email Trigger");

            ////Defining post actions
            //PostAction moveToDraftErrorPostAction = moveToDraftAction.AddPostAction("With Error", "Comment");

            ////Defining state mappings
            //moveToDraftErrorPostAction.AddStateMapping(moveToDraftErrorState.Id, moveToDraftCorrectState.Id, "Move To Draft Correct");

            ////Defining the pop-up for the above sendForRevisionSubmissionPostAction action
            //PopUp moveToDraftErrorActionPopUpopUp = moveToDraftErrorPostAction.AddPopUp("WARNING: In the Draft Calendar -  Error", "Do you really want to change the document state?.");
            //moveToDraftErrorActionPopUpopUp.AddButtons("Yes", "true");
            //moveToDraftErrorActionPopUpopUp.AddButtons("Cancel", "false");

            ////Defining trigger refs
            //moveToDraftErrorPostAction.AddTriggerRefs("0", MovedToDraftCalendarEmailTrigger.Id, "Moved to Draft Notification Email Trigger");

            ////Defining state referances
            //moveToDraftAction.AddStateReferances(gfcApprovedState.Id);
            //moveToDraftAction.AddStateReferances(moveToDraftErrorState.Id);

            ////Defining authorizatios
            //moveToDraftAction.AddAuthorizedRole(centralAdminRole.Id);

            //auth.EnsureUserRoles(workflow.GetWorkflowRoles());
            //auth.EnsureGroups(workflow.GetWorkflowGroups(), template.Id);



            db.SaveChanges();


            template.Data.Save("..\\..\\..\\..\\Examples\\safetyInfoWorkflow_generared.xml");

        }

        [Test]
        public void CovidSafetyInspectionTest()
        {
            string lang = "en";
            string templateName = "COVID-19 Inspection Template";

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

            ws.SetModel(template);

            //Get the Workflow object using the workflow service
            Workflow workflow = ws.GetWorkflow(true);

            //Defininig states
            State emptyState = workflow.AddState(ws.GetStatus(template.Id, "", true));
            State submittedState = workflow.AddState(ws.GetStatus(template.Id, "Submitted", true));
            State deleteState = workflow.AddState(ws.GetStatus(template.Id, "Deleted", true));


            //Defining email templates
            EmailTemplate adminNotification = ws.GetEmailTemplate("Admin Notification", true);
            adminNotification.SetDescription("This metadata set defines the email template to be sent to the admin when an inspector does not submit an inspection report timely.", lang);
            adminNotification.SetSubject("Safety Inspection Submission");
            adminNotification.SetBody("TBD");

            EmailTemplate inspectorSubmissionNotification = ws.GetEmailTemplate("Inspector Notification", true);
            inspectorSubmissionNotification.SetDescription("This metadata set defines the email template to be sent to an inspector when an inspection report is not submitted timely.", lang);
            inspectorSubmissionNotification.SetSubject("Safety Inspection Reminder");
            inspectorSubmissionNotification.SetBody("TBD");

            //Defininig the inspection form
            DataItem inspectionForm = template.GetDataItem("COVID-19 Inspection Form", true, lang);
            inspectionForm.IsRoot = true;
            inspectionForm.SetDescription("This template is designed for a weekly inspection of public health measures specific to COVID-19 and other return to campus requirements.", lang);

            inspectionForm.CreateField<DateField>("Inspection Date", lang, true)
                .IncludeTime =  false;
            
            string[] optionBuilding = new string[] { "", "Arts and Convocation Hall", "Assiniboia Hall", "Fine Arts Building", "HM Tory Building", "HUB", "Humanities Centre", "Industrial Design Studio", "North Power Plant", "South Academic Building", "Timms Centre for the Arts", "Varsity Trailer" };
            inspectionForm.CreateField<SelectField>("Building", lang, optionBuilding, true);
            inspectionForm.CreateField<TextField>("Inspected By", lang, true, true);
            inspectionForm.CreateField<TextField>("Room/Area", lang, true, true);


            //inspectionForm.CreateField<CheckboxField>("Room/Area Check:", lang, optionBuilding);

            inspectionForm.CreateField<IntegerField>("Number of People in the work area", lang, true);

            inspectionForm.CreateField<InfoSection>(null, null)
                .AppendContent("h4", "Physical Distancing", lang);

            string[] optionText = new string[] { "Yes", "No", "N/A" };
            inspectionForm.CreateField<RadioField>("Is there 2m (6.5 ft) of distance between all occupants?", lang, optionText, true );
            inspectionForm.CreateField<RadioField>("Where physical distancing is not possible, are occupants wearing face masks?", lang, optionText, true);
            inspectionForm.CreateField<TextArea>("Notes/Action", lang, false);
            inspectionForm.CreateField<TextField>("Assigned to", lang, false);

            inspectionForm.CreateField<InfoSection>(null, null)
                .AppendContent("h4", "Personal Hygiene", lang);
            inspectionForm.CreateField<RadioField>("Is a hand washing sink or hand sanitizer available?", lang, optionText, true);
            inspectionForm.CreateField<RadioField>("Is the sink clean and free of contamination?", lang, optionText, true);
            inspectionForm.CreateField<RadioField>("Is there an adequate supply of soap? ", lang, optionText, true);
            inspectionForm.CreateField<TextArea>("Notes/Action", lang, false);
            inspectionForm.CreateField<TextField>("Assigned to", lang, false);

            inspectionForm.CreateField<InfoSection>(null, null)
               .AppendContent("h4", "Housekeeping", lang);
            inspectionForm.CreateField<RadioField>("Is general housekeeping and cleanliness being maintained?", lang, optionText, true);
            inspectionForm.CreateField<RadioField>("Are surfaces being disinfected on a regular basis?", lang, optionText, true);
            inspectionForm.CreateField<RadioField>("Are there adequate cleaning supplies for the next 2 weeks?", lang, optionText, true);
            inspectionForm.CreateField<RadioField>("Are walkways clear of trip hazards?", lang, optionText, true);
            inspectionForm.CreateField<TextArea>("Notes/Action", lang, false);
            inspectionForm.CreateField<TextField>("Assigned to", lang, false);

            inspectionForm.CreateField<InfoSection>(null, null)
               .AppendContent("h4", "Training", lang);
            inspectionForm.CreateField<RadioField>("Have all employees taken the COVID-19 Return to Campus training?", lang, optionText, true);
            inspectionForm.CreateField<RadioField>("Have all employees been trained in your return to campus plan?", lang, optionText, true);
            inspectionForm.CreateField<TextArea>("Notes/Action", lang, false);
            inspectionForm.CreateField<TextField>("Assigned to", lang, false);

            inspectionForm.CreateField<InfoSection>(null, null)
                .AppendContent("h4", "Eyewash Stations", lang)
                .AppendContent("div",
                    @"
<b>Instructions</b>
<ol>
    <li><em>Ensure the eyewash station is readily accessible. Move all obstructions away from the station.</em></li>
    <li><em>Most stations will be installed above a sink/drain. If there is no sink/drain, obtain a bucket/pan to capture the water from the test, or funnel it into the sink.</em></li>
    <li><em>Activate the station for a minimum of 3-minutes.</em></li>
    <li><em>The protective cap(s) should come off automatically and the water temperature should stabilize. A 3-minute flush allows for the removal of any build-up in the system.</em></li>
    <li><em>After 3-minutes, deactivate the unit, clean-up any spilled water, and initial the test below.</em></li>
</ol>
<p>If there are any deficiencies found in the weekly test, contact the maintenance desk at 780-492-4833.</p>",
                    lang,
                    "alert alert-info");

            var eyeWashFlushed = inspectionForm.CreateField<RadioField>("Have eyewash stations been flushed in the last week?", lang, optionText, true);
            inspectionForm.CreateField<TextArea>("Eyewash station info", lang, false)
                .SetDescription("If you answer Yes to the above question, please provide the room number, date of the last annual test, and the year built for each eyewash station you flushed.", lang);
            //                .SetVisibleIf(eyeWashFlushed, optionText[0]);
            
            inspectionForm.CreateField<InfoSection>(null, null)
               .AppendContent("h4", "Other", lang);
            inspectionForm.CreateField<RadioField>("Have all sinks been flushed for 3 minutes?", lang, optionText, true);
            inspectionForm.CreateField<RadioField>("Is all appropriate PPE being worn?", lang, optionText, true);
          
            inspectionForm.CreateField<TextArea>("Notes/Action", lang, false);
            inspectionForm.CreateField<TextField>("Assigned to", lang, false);


            //Defininig roles
            WorkflowRole adminRole = workflow.AddRole(auth.GetRole("Admin", true));
            WorkflowRole inspectorRole = workflow.AddRole(auth.GetRole("Inspector", true));


            // Submitting an inspection form
            //Only safey inspectors can submit this form
            GetAction startSubmissionAction = workflow.AddAction("Start Submission", nameof(TemplateOperations.Instantiate), "Home");
            startSubmissionAction.Access = GetAction.eAccess.Restricted;
            startSubmissionAction.AddStateReferances(emptyState.Id)
                .AddAuthorizedRole(inspectorRole.Id);

            //Listing inspection forms.
            //Inspectors can list their own submissions.
            //Admins can list all submissions.
            GetAction listSubmissionsAction = workflow.AddAction("List Submissions", nameof(TemplateOperations.ListInstances), "Home");
            listSubmissionsAction.Access = GetAction.eAccess.Restricted;
            listSubmissionsAction.AddStateReferances(submittedState.Id)
                .AddOwnerAuthorization()
                .AddAuthorizedRole(adminRole.Id);


            //Post action for submitting the form
            PostAction submitPostAction = startSubmissionAction.AddPostAction("Submit", nameof(TemplateOperations.Update));
            submitPostAction.AddStateMapping(emptyState.Id, submittedState.Id, "Submit");

            //Defining the pop-up for the above submitPostAction action
            PopUp submitActionPopUp = submitPostAction.AddPopUp("WARNING: Submitting the Form", "Once submitted, you cannot update the form.","");
            submitActionPopUp.AddButtons("Yes, submit", "true");
            submitActionPopUp.AddButtons("Cancel", "false");

            // Edit submission related workflow items
            //Defining actions
            GetAction editSubmissionAction = workflow.AddAction("Edit Submission", "Edit", "Details");

            //Submissions can only be edited by admins
            editSubmissionAction.AddStateReferances(submittedState.Id)
                .AddAuthorizedRole(adminRole.Id);

            //Defining post actions
            PostAction editPostActionSave = editSubmissionAction.AddPostAction("Save", "Save");
            editPostActionSave.AddStateMapping(submittedState.Id, submittedState.Id, "Save");


            // Delete submission related workflow items
            //Defining actions. Only admin can delete a submission
            GetAction deleteSubmissionAction = workflow.AddAction("Delete Submission", "Delete", "Details");
            deleteSubmissionAction.AddStateReferances(submittedState.Id)
                .AddAuthorizedRole(adminRole.Id);

            //Defining post actions
            PostAction deleteSubmissionPostAction = deleteSubmissionAction.AddPostAction("Delete", "Save");
            deleteSubmissionPostAction.AddStateMapping(submittedState.Id, deleteState.Id, "Delete");

            //Defining the pop-up for the above postActionSubmit action
            PopUp deleteSubmissionActionPopUpopUp = deleteSubmissionPostAction.AddPopUp("WARNING: Delete", "Deleting the submission. Please confirm.","");
            deleteSubmissionActionPopUpopUp.AddButtons("Yes, delete", "true");
            deleteSubmissionActionPopUpopUp.AddButtons("Cancel", "false");


            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.xml");

            string json = JsonConvert.SerializeObject(template);
            File.WriteAllText("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.json", json);
        }





        [Test]
        public void TestEntityTemplateLoad()
        {
            string filename = "..\\..\\..\\..\\Examples\\CalendarManagementWorkflow_generared.xml";
            string content = File.ReadAllText(filename);
            EntityTemplate template = new EntityTemplate();
            template.Content = content;

            _db.EntityTemplates.Add(template);
            _db.SaveChanges();
        }
    }
}
