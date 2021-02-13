﻿using Catfish.Core.Authorization.Requirements;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Expressions;
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
using System.Linq.Expressions;
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
            WorkflowRole centralAdminRole = workflow.AddRole(auth.GetRole("CentralAdmin", true));
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

            listSubmissionAction.GetStateReference(deanOfficeRevisionState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(deanOfficeRevisionCompletedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(deanOfficeRevisionSaveState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(aacWithState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(aacRevisionState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(aacRevisionSaveState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(aacRevisionCompletedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(aacApprovedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(aecWithState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(aecRevisionState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(aecRevisionSaveState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(aecRevisionCompletedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(aecApprovedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(afcWithState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(afcRevisionState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(afcRevisionSaveState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(afcRevisionCompletedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(afcApprovedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(gfcWithState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(gfcRevisionState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(gfcRevisionSaveState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(gfcRevisionCompletedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(gfcApprovedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(moveToDraftCorrectState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            listSubmissionAction.GetStateReference(moveToDraftErrorState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
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

            viewDetailsSubmissionAction.GetStateReference(deanOfficeRevisionState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(deanOfficeRevisionCompletedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(deanOfficeRevisionSaveState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(aacWithState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(aacRevisionState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(aacRevisionSaveState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(aacRevisionCompletedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(aacApprovedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(aecWithState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(aecRevisionState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(aecRevisionSaveState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(aecRevisionCompletedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(aecApprovedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(afcWithState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(afcRevisionState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(afcRevisionSaveState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(afcRevisionCompletedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(afcApprovedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(gfcWithState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(gfcRevisionState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(gfcRevisionSaveState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(gfcRevisionCompletedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(gfcApprovedState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(moveToDraftCorrectState.Id, true)
                .AddAuthorizedRole(departmentAdmin.Id);

            viewDetailsSubmissionAction.GetStateReference(moveToDraftErrorState.Id, true)
                .AddAuthorizedRole(centralAdminRole.Id)
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
            moveToDraftErrorPostAction.AddStateMapping(gfcApprovedState.Id, moveToDraftErrorState.Id, "Move To Draft Error");

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
            
            string[] optionBuilding = new string[] {"Arts and Convocation Hall", "Assiniboia Hall", "Fine Arts Building", "HM Tory Building", "HUB", "Humanities Centre", "Industrial Design Studio", "North Power Plant", "South Academic Building", "Timms Centre for the Arts", "Varsity Trailer" };
            inspectionForm.CreateField<SelectField>("Building", lang, optionBuilding, true);
            inspectionForm.CreateField<TextField>("Inspected By", lang, true, true);
            inspectionForm.CreateField<TextField>("Room/Area", lang, true, true);


            //inspectionForm.CreateField<CheckboxField>("Room/Area Check:", lang, optionBuilding);

            inspectionForm.CreateField<IntegerField>("Number of Approved People", lang, true)
                .SetDescription("Refer to the number indicated in your Return to Campus Plan", lang);

            inspectionForm.CreateField<IntegerField>("Number of People in the Work Area", lang, true)
                .SetDescription("Provide the number of people actually working in the space during the inspection", lang);

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
            //Admins and Inspectors can run the item-list report to list instances.
            //Note that the visibility of individual list entries is depend on the
            //Read permission on individual submissions.
            GetAction listSubmissionsAction = workflow.AddAction("List Submissions", nameof(TemplateOperations.ListInstances), "Home");
            listSubmissionsAction.Access = GetAction.eAccess.Restricted;
            listSubmissionsAction.AddStateReferances(submittedState.Id)
                .AddAuthorizedRole(inspectorRole.Id)
                .AddAuthorizedRole(adminRole.Id);

            //Detailed submission inspection forms.
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
        public void SmallFormTest()
        {
            string lang = "en";
            string templateName = "Small Test Form Template";

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

            //Defininig the form
            DataItem inspectionForm = template.GetDataItem("Small Test Form", true, lang);
            inspectionForm.IsRoot = true;
            inspectionForm.SetDescription("This template is designed testing visible-if, required-if, and computed fields", lang);

            string[] options = new string[] { "Option 1", "Option 2", "Option 3", "Option 4" };
            var dd1 = inspectionForm.CreateField<SelectField>("DD 1", lang, options, true);
            var radio1 = inspectionForm.CreateField<RadioField>("RB 1", lang, options, true);
            var checkbox1 = inspectionForm.CreateField<CheckboxField>("CB 1", lang, options, false);

            var textbox1 = inspectionForm.CreateField<TextField>("Text 1", lang, false, false);
            textbox1.RequiredCondition
                .AppendLogicalExpression(dd1, ComputationExpression.eRelational.EQUAL, dd1.GetOption("Option 2", lang));
            textbox1.SetDescription("Required if DD 1 = Option 2", lang);

            var textbox2 = inspectionForm.CreateField<TextField>("Text 2", lang, false, false);
            textbox2.RequiredCondition
                .AppendLogicalExpression(radio1, new Option[2] { radio1.GetOption("Option 1", lang), radio1.GetOption("Option 2", lang) }, ComputationExpression.eLogical.OR);
            textbox2.SetDescription("Required if RB 1 = Option 1 OR Option 2", lang);

            var textbox3 = inspectionForm.CreateField<TextField>("Text 3", lang, false, false);
            textbox3.RequiredCondition
                .AppendLogicalExpression(checkbox1, new Option[2] { checkbox1.GetOption("Option 1", lang), checkbox1.GetOption("Option 3", lang) }, Core.Models.Contents.Expressions.ComputationExpression.eLogical.AND);
            textbox3.SetDescription("Required if CB 1 = Option 1 AND Option 3", lang);

            var textarea1 = inspectionForm.CreateField<TextArea>("Text 4", lang, false, false);
            textarea1.VisibilityCondition
              .AppendLogicalExpression(dd1, ComputationExpression.eRelational.EQUAL, dd1.GetOption("Option 3", lang));
            textarea1.SetDescription("Visible if DD 1 = Option 3", lang);

            //Drop-down menu with conditional options
            var dd2OOptions = new string[] { "A1", "A2", "B1", "B2", "B3", "B4" };
            var dd2 = inspectionForm.CreateField<SelectField>("DD 2", lang, dd2OOptions, true);
            dd2.SetDescription("The option group \"A\" should appear when Option 1 is selected for DD 1 and the option group \"B\" should appear other times.", lang);
            foreach (var option in dd2.Options.Where(op => op.OptionText.GetContent(lang).StartsWith("A")))
                option.VisibilityCondition.AppendLogicalExpression(dd1,
                    ComputationExpression.eRelational.EQUAL,
                    dd1.Options.Where(op => op.OptionText.GetContent(lang) == options[0]).First());

            foreach (var option in dd2.Options.Where(op => op.OptionText.GetContent(lang).StartsWith("B")))
                option.VisibilityCondition.AppendLogicalExpression(dd1,
                    ComputationExpression.eRelational.NOT_EQ,
                    dd1.Options.Where(op => op.OptionText.GetContent(lang) == options[0]).First());


            //BEGIN: SAS Chair Functionality
            //==============================
            //Note: for every index in the departmentNames array, the corresponding index in the 
            //      departmentChairs array should specify the chair of that department. However,
            //      the departmentChairs array should have one option at the end, which represents the Dean
            var departmentNames = new string[]  { "Dept 1",  "Dept 2",  "Dept 3",  "Dept 4",  "Dept 5",  "Dept 6" };
            var departmentChairs = new string[] { "Chair 1", "Chair 2", "Chair 3", "Chair 4", "Chair 5", "Chair 6", "Dean" };
    
            var departmentsDropDown = inspectionForm.CreateField<SelectField>("Departments", lang, departmentNames, true);

            var areYouChairOptions = new string[] { "Yes", "No" };
            var areYouChair = inspectionForm.CreateField<RadioField>("Are you the chair", lang, areYouChairOptions, true);
            
            var chairsDropDown = inspectionForm.CreateField<SelectField>("Departments", lang, departmentChairs, true);

            //Iterating through all chair options except for the last one, which is the Dean
            for(var i=0; i< chairsDropDown.Options.Count-1; ++i) 
            {
                var chair = chairsDropDown.Options[i];

                //This shair should be visible only if the corresponding departmet
                //has been selected for the departmentsDropDown AND the second 
                //option (i.e. index = 1, or in other wards, the value of "No") 
                //has been selected for the areYouChair radio button
                chair.VisibilityCondition
                    .AppendLogicalExpression(departmentsDropDown, ComputationExpression.eRelational.EQUAL, departmentsDropDown.Options[i])
                    .AppendOperator(ComputationExpression.eLogical.AND)
                    .AppendLogicalExpression(areYouChair, ComputationExpression.eRelational.EQUAL, areYouChair.Options[1]);
            }


            //Setting the visibility of the last chairs option ("Dean"). This option should be 
            //visible if and only if "Yes" is selected for the areYouChair radio field irrespective of
            //what department is selected in the departmentsDropDown
            chairsDropDown.Options[chairsDropDown.Options.Count - 1].VisibilityCondition
                .AppendLogicalExpression(areYouChair, ComputationExpression.eRelational.EQUAL, areYouChair.Options[0]);

            //END: SAS Chair Functionality
            //==============================


            //Fields for testing Computed Fields
            //==================================
            var x = inspectionForm.CreateField<DecimalField>("x", lang, false, false);
            var y = inspectionForm.CreateField<DecimalField>("y", lang, false, false);
            var z = inspectionForm.CreateField<DecimalField>("z", lang, false, false);

            var a = inspectionForm.CreateField<DecimalField>("a = x", lang, false, false);
            a.ValueExpression.AppendValue(x);

            var b = inspectionForm.CreateField<DecimalField>("b = x + y", lang, false, false);
            b.ValueExpression
                .AppendValue(x)
                .AppendOperator(ComputationExpression.eMath.PLUS)
                .AppendValue(y);

            var c = inspectionForm.CreateField<DecimalField>("c = x * (y + z)", lang, false, false);
            c.ValueExpression
                .AppendValue(x)
                .AppendOperator(ComputationExpression.eMath.MULT)
                .AppendOpenBrace()
                .AppendValue(y)
                .AppendOperator(ComputationExpression.eMath.PLUS)
                .AppendValue(z)
                .AppendClosedBrace();

            //END: Fields for testing Computed Fields
            //=======================================

            //Full width text area
            inspectionForm.CreateField<TextArea>("This is field with a long field name displayed at full width", lang, false, false)
                .SetSize(10, 60)
                .SetFieldLabelCssClass("col-md-12")
                .SetFieldValueCssClass("col-md-12");


            //Setting the default value of a text field
            inspectionForm.CreateField<TextField>("Institution", lang, false, false, "University of Alberta");

            var dd3 = inspectionForm.CreateField<SelectField>("DD 3", lang, new string[] { 
                "user 1 : email_1@example.org $ Postal Address 1", 
                "user 2 : email_2@example.org $ Postal Address 2", 
                "user 3 : email_3@example.org $ Postal Address 3" });

            //Setting the default value of a text field dynamically to a value selected by a dropdown menu
            inspectionForm.CreateField<TextField>("DD3 Value", lang, false, false, "University of Alberta")
                .ValueExpression.AppendReadableValue(dd3);

            inspectionForm.CreateField<TextField>("DD3 User", lang, false, false, "University of Alberta")
                .ValueExpression.AppendReadableValue(dd3, null, ":", true);

            inspectionForm.CreateField<TextField>("DD3 Email", lang, false, false, "University of Alberta")
                .ValueExpression.AppendReadableValue(dd3, ":", "$", true);

            inspectionForm.CreateField<TextField>("DD3 Postal Address", lang, false, false, "University of Alberta")
                .ValueExpression.AppendReadableValue(dd3, "$", null, true);

            //Defininig roles
            WorkflowRole adminRole = workflow.AddRole(auth.GetRole("Admin", true));
            WorkflowRole inspectorRole = workflow.AddRole(auth.GetRole("Inspector", true));


            // Submitting an inspection form
            //Only safey inspectors can submit this form
            GetAction startSubmissionAction = workflow.AddAction("Start Submission", nameof(TemplateOperations.Instantiate), "Home");
            startSubmissionAction.Access = GetAction.eAccess.Restricted;
            startSubmissionAction.AddStateReferances(emptyState.Id)
                .AddAuthorizedRole(inspectorRole.Id);

            //Listing inspection form submissions.
            //Inspectors can list their own submissions.
            //Admins can list all submissions.
            GetAction listSubmissionsAction = workflow.AddAction("List Submissions", nameof(TemplateOperations.ListInstances), "Home");
            listSubmissionsAction.Access = GetAction.eAccess.Restricted;
            listSubmissionsAction.AddStateReferances(submittedState.Id)
                .AddOwnerAuthorization()
                .AddAuthorizedRole(adminRole.Id);

            //Detailed submission inspection forms.
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
            PopUp deleteSubmissionActionPopUpopUp = deleteSubmissionPostAction.AddPopUp("WARNING: Delete", "Deleting the submission. Please confirm.", "");
            deleteSubmissionActionPopUpopUp.AddButtons("Yes, delete", "true");
            deleteSubmissionActionPopUpopUp.AddButtons("Cancel", "false");


            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\smallTestFormWorkflow_generared.xml");

            string json = JsonConvert.SerializeObject(template);
            File.WriteAllText("..\\..\\..\\..\\Examples\\smallTestFormWorkflow_generared.json", json);
        }


        [Test]
        public void GapSasTest()
        {
            string lang = "en";
            string templateName = "SAS Application Winter 2021";

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


            //=============================================================================== Defininig SAS form
            DataItem sasForm = template.GetDataItem("SAS Application Form", true, lang);
            sasForm.IsRoot = true;
            sasForm.SetDescription("This template is designed for SAS Application Grant", lang);

            // ====================================================== APLICANT INFORMATION
            sasForm.CreateField<InfoSection>(null, null)
                .AppendContent("h1", "Applicant Information", lang);
            sasForm.CreateField<InfoSection>(null, null)
               .AppendContent("div", "The Adjudication committee is a multi-disciplinary committee. Please write for someone who does not understand your work and/or field.<br/>Be clear and concise in your explanations, and make sure your justifications are detailed.", lang);

            sasForm.CreateField<TextField>("Applicant Name:", lang, true, true);
            var applicantEmail = sasForm.CreateField<TextField>("Email Address:", lang, true, true)
                .SetDescription("Please use your UAlberta CCID email address.", lang);

            string[] departmentList = GetDepartmentList(); 
           
            var dept = sasForm.CreateField<SelectField>("Department:", lang, departmentList, true);

            string[] rank = new string[]{ "Assistant Professor",
                                "Associate Professor",
                                "Professor",
                                "FSO",
                                "Other"};
            sasForm.CreateField<SelectField>("Rank:", lang, rank, true);

            //==============================================================================CHAIR's Contact Information
            sasForm.CreateField<InfoSection>(null, null)
                .AppendContent("h1", "Chair’s Contact Information", lang)
                .AppendContent("div", "<i>When the applicant is a Department Chair, the Dean's information must be provided.</i>", lang);

            string[] optionText = new string[] { "Yes", "No" };
            var isChair = sasForm.CreateField<RadioField>("Are you the Department Chair?", lang, optionText, true);
            //if department Chair  -- the chair will be the dean
            //the order of the department chair list have to be in the same order of the list Department above
            // the first one is the Dean ==> no association with the department -- exception
            string[] chairDept = GetDepartmentChair();
           

          
            var chair = sasForm.CreateField<SelectField>("Chair:", lang, chairDept, true);
           

            //Iterating through all chair options except for the last one, which is the Dean
            for (var i = 0; i < chair.Options.Count-1; ++i)
            {
                var selectedChair = chair.Options[i];

                selectedChair.VisibilityCondition
                    .AppendLogicalExpression(dept, ComputationExpression.eRelational.EQUAL, dept.Options[i])
                    .AppendOperator(ComputationExpression.eLogical.AND)
                    .AppendLogicalExpression(isChair, ComputationExpression.eRelational.EQUAL, isChair.Options[1]);
               
            }

            //Setting the visibility of the last chairs option ("Dean"). This option should be 
            //visible if and only if "Yes" is selected for the areYouChair radio field.
            chair.Options[chair.Options.Count - 1].VisibilityCondition
                .AppendLogicalExpression(isChair, ComputationExpression.eRelational.EQUAL, isChair.Options[0]);

            string delimiter = ":";

            var chairName = sasForm.CreateField<TextField>("Chair's Name:", lang, true).SetDefaultReferenceValue(chair, delimiter, 0);
            var chairEmail = sasForm.CreateField<TextField>("Chair's Email:", lang, true).SetDefaultReferenceValue(chair, delimiter, 1);

            //========================================================================PROJECT DETAILS

            sasForm.CreateField<InfoSection>(null, null)
                .AppendContent("h1", "Project Details", lang);

            sasForm.CreateField<TextField>("Project Title:", lang, true, true);
            sasForm.CreateField<TextArea>("Project Description:", lang, true, true).SetDescription("Describe your research project and explain why this is a necessary and urgent application for funding. Maximum 250 words.", lang);
           var isInvolveAnimal = sasForm.CreateField<RadioField>("Does this project involve human or animal subjects?", lang, optionText, true);
           
            sasForm.CreateField<InfoSection>(null, null)
                 .AppendContent("div", @"<i>Please note that proof of ethics approval may be required before any grant awarded will be released. For more information concerning ethics clearance, 
                    please refer to the Research Ethics Office website.</i>", lang).VisibilityCondition.AppendLogicalExpression(isInvolveAnimal, ComputationExpression.eRelational.EQUAL, isInvolveAnimal.GetOption("Yes", lang));
            var ethicApproval = sasForm.CreateField<RadioField>("Has Ethics approval already been obtained for this project?", lang, optionText, false);      
            ethicApproval.VisibilityCondition.AppendLogicalExpression(isInvolveAnimal, ComputationExpression.eRelational.EQUAL, isInvolveAnimal.GetOption("Yes", lang));

            //this kind of chaining.append not working
          
            sasForm.CreateField<DateField>("Ethics Expiry Date:", lang, false)
                        .VisibilityCondition
                        .AppendLogicalExpression(isInvolveAnimal, ComputationExpression.eRelational.EQUAL, isInvolveAnimal.Options[0])
                        .AppendOperator(ComputationExpression.eLogical.AND)
                       .AppendLogicalExpression(ethicApproval, ComputationExpression.eRelational.EQUAL, ethicApproval.Options[0]); ;


            //========================================================================BUDGET DETAILS

            sasForm.CreateField<InfoSection>(null, null)
                .AppendContent("h1", "Budget Details", lang)
                .AppendContent("h2", "Conference Travel", lang)
                .AppendContent("p", "<i>All travel expenses are reimbursed in compliance with the UAPPOL Travel Expense Policies</i>", lang)
                .AppendContent("div",
                    @"<p>
                    <ul>
                         <li> For conference travel, EFF SAS allows one night before and one night after a conference to a maximum of 7 days.</li>
                                    <li> Applicants must include detailed estimates of their transportation and hotel costs.These should be from travel agents, hotels, and / or travel booking websites.Combined travel and hotel estimates are not accepted.</li>
                                    <li> Limits:
                                    <ul>
                                    <li> Conference travel in Canada / US maximum - $2, 250.00 </li>
                                    <li> Conference travel to all other destinations maximum - $3, 350.00 </li>
                                    </ul>
                                    </li>
                                    </ul></p>",
                    lang,
                    "alert alert-info")

              .AppendContent("div", "Please specify the travel cost breakdown and attach supporting documentation below.Please enter numerical values only.Do not use $ or, when entering your dollar values.", lang, "alert alert-warning");
            sasForm.CreateField<TextField>("Name of Conference", lang);
            sasForm.CreateField<DateField>("Date of Conference", lang);
            sasForm.CreateField<TextField>("Destination", lang);
            sasForm.CreateField<DateField>("Departure Date", lang);
            sasForm.CreateField<DateField>("Return Date", lang);

            string[] participationRoles = new string[] { "Invited Speaker", "Panel Member", "Refereed Paper Presentation", "Poster Presentation", "Other" };
            var confParticipation = sasForm.CreateField<CheckboxField>("Conference Participation", lang, participationRoles);

            var otherParticipationRole = sasForm.CreateField<TextField>("Other - please specify", lang);
            otherParticipationRole.VisibilityCondition
              .AppendLogicalExpression(confParticipation, confParticipation.GetOption("Other", lang), true); 


            sasForm.CreateField<DecimalField>("Airfare", lang).SetDescription("Includes: Airfare, trip cancellation insurance, seat selection and baggage fees", lang);
            sasForm.CreateField<DecimalField>("Accomodation", lang);
            sasForm.CreateField<DecimalField>("Per Diem / Meals", lang).SetDescription(@"<p>See UAPPOL <a href='https://policiesonline.ualberta.ca/PoliciesProcedures/Procedures/Travel-Expense-Procedure-Appendix-A-Schedule-of-Allowable-Expenses.pdf' target='_blank'>Travel Expense Procedure, Appendix A: Schedule of Allowable Travel Expenses</a> for allowable amounts and breakdown.</p>", lang);
            sasForm.CreateField<DecimalField>("Ground Transportation", lang).SetDescription("Transportation to/from airport/home only", lang);
            sasForm.CreateField<DecimalField>("Conference Registration", lang);
            sasForm.CreateField<DecimalField>("Additional Expenses", lang).SetDescription("Includes bus, train, car rental, travel insurance, visas, incidentals NOT already included in Airfare, Accommodation or Ground Transportation", lang);
            sasForm.CreateField<TextArea>("Details Additional Expenses", lang);
            //attachment field
            //Supported Documentation -
            sasForm.CreateField<AttachmentField>("Supporting Documentation", lang).SetDescription(@"Please attach the required travel and conference supporting documentation here as <span style='color: Red;'>a <b>single PDF document</b>. [Be sure to review the section of the Policies and Procedures on required supporting documentation]</span>", lang);

            sasForm.CreateField<TextArea>("Justification", lang).SetDescription("Explain the significance of this conference to your research and scholarly career. Maximum 250 words.", lang);


            // =================================================================================  RESEARCH AND CREATIVE ACTIVITY
            sasForm.CreateField<InfoSection>(null, null)

                .AppendContent("h2", "Research and Creative Activity", lang)
                .AppendContent("h3", "Travel", lang)
                .AppendContent("div",
                    @"<p>
                    <ul>
                         <li>Research travel has no time limit, but will be funded to a maximum of $5,000.00</li>
                         <li>Applicants must include detailed estimates of their transportation and hotel costs. These should be from travel agents, hotels, and/or travel booking websites. Combined travel and hotel estimates are not accepted.</li>    
                           </ul></p>",
                    lang,
                    "alert alert-info")
                .AppendContent("div", "Please specify the travel cost breakdown and attach supporting documentation below. Please enter numerical values only. Do not use $ or , when entering your dollar values.", lang, "alert alert-warning");



            sasForm.CreateField<TextField>("Destination", lang);
            sasForm.CreateField<DateField>("Departure Date", lang);
            sasForm.CreateField<DateField>("Return Date", lang);
            sasForm.CreateField<DecimalField>("Airfare", lang).SetDescription("Includes: Airfare, trip cancellation insurance, seat selection and baggage fees", lang);
            sasForm.CreateField<DecimalField>("Accomodation", lang);
            sasForm.CreateField<DecimalField>("Per Diem / Meals", lang).SetDescription(@"<p>See UAPPOL <a href='https://policiesonline.ualberta.ca/PoliciesProcedures/Procedures/Travel-Expense-Procedure-Appendix-A-Schedule-of-Allowable-Expenses.pdf' target='_blank'>Travel Expense Procedure, Appendix A: Schedule of Allowable Travel Expenses</a> for allowable amounts and breakdown.</p>", lang);
            sasForm.CreateField<DecimalField>("Ground Transportation", lang).SetDescription("Transportation to/from airport/home only", lang);
            sasForm.CreateField<DecimalField>("Conference Registration", lang);
            sasForm.CreateField<DecimalField>("Additional Expenses", lang).SetDescription("Includes bus, train, car rental, travel insurance, visas, incidentals NOT already included in Airfare, Accommodation or Ground Transportation", lang);
            sasForm.CreateField<TextArea>("Details of Additional Expenses", lang);

            sasForm.CreateField<AttachmentField>("Supported Documentation", lang).SetDescription(@"Please attach the required travel and conference supporting documentation here as <span style='color: Red;'>a <b>single PDF document</b>. [Be sure to review the section of the Policies and Procedures on required supporting documentation]</span>", lang);

            sasForm.CreateField<TextArea>("Justification", lang).SetDescription("<i>Explain how this travel is essential to advancing your research. Maximum 250 words.</i>", lang);


            //=============================================================================== Personnel and Services
            sasForm.CreateField<InfoSection>(null, null)
                .AppendContent("h1", "Personnel and Services", lang)
                .AppendContent("div", "Failure to provide a detailed estimate will result in automatic disqualification of this part of the application.", lang, "alert alert-warning");

            sasForm.CreateField<TextArea>("Description of personnel to be hired or services to be purchased", lang).SetDescription(@"<i>Provide and outline of the type of work to be undertaken, the expected time frame for completing that work, and a comment on why it is essential to your research. Maximum 250 words.</ i>", lang);
            sasForm.CreateField<InfoSection>(null, null)
               .AppendContent("h3", "Estimate for Hiring Students", lang)
               .AppendContent("div", @"<i>For each student to be hired, click 'Add' and indicate whether the student will be an undergraduate, MA, or PhD student, and specify the period for which they will be hired, the number of hours to be worked, and related calculations.
                     Applicants are expected to adhere to the Collective Agreement when calculating salaries for graduate students. Research budgets for casual student labour must reflect the minimum rate (award + salary + benefits)
                    for Doctoral and Masters students.</i>", lang);
           
            // DataItem personnelBudgetForm = CreatePersonnelBudgetForm(template)
            CompositeField personnelBudget = sasForm.CreateField<CompositeField>("", lang, false);
            personnelBudget = CreatePersonnelBudgetItemForm(personnelBudget,lang, 0);


            sasForm.CreateField<InfoSection>(null, null)
               .AppendContent("h3", "Estimates for Contracted Services", lang)
               .AppendContent("div", @"For each contracted service, click 'Add' and then provide the requested information. Please describe the services to be provided. Note that you must also attach written estimates for all contracted services. Estimates should be combined into one single PDF document.", lang);

            //Allow adding multiple PersonnelBudgetItem Form TODO
            // DataItem personnelBudgetForm = CreatePersonnelBudgetForm(template)
            CompositeField contractServ = sasForm.CreateField<CompositeField>("", lang, false);
            contractServ = CreatePersonnelBudgetItemForm(contractServ, lang, 0);

            sasForm.CreateField<AttachmentField>("Contractor Cost Estimates", lang).SetDescription(@"<i>Attach written estimate for contracted services as <span style='color: Red;'> <b>single PDF document</b></i>.<br/>
[Required if funding requested for professional services]</span>", lang);
            sasForm.CreateField<TextArea>("Justification", lang).SetDescription("Why are these services required for this project at this time? [Maximum 250 words]", lang);

            //====================================================================== EQUIPMENT AND MATERIAL
            sasForm.CreateField<InfoSection>(null, null)
              .AppendContent("h3", "Equipment and Materials", lang)
              .AppendContent("div", @"<i>Failure to provide a written estimate will result in automatic disqualification of this part of the application.</i>", lang);

            sasForm.CreateField<TextArea>("Equipment and Material Justification", lang).SetDescription("Provide a description of the materials and equipment you plan to purchase and outline why they are essential to your research project at this time. Maximum 250 words.", lang);

            sasForm.CreateField<InfoSection>(null, null)
             .AppendContent("h3", "Estimates for Equipment and Material", lang);

            // allow to add 1 or more Personnel budget Item Form here TODO
            // DataItem personnelBudgetForm = CreatePersonnelBudgetForm(template)
            CompositeField estimateEquip = sasForm.CreateField<CompositeField>("", lang, false);
            estimateEquip = CreatePersonnelBudgetItemForm(estimateEquip, lang, 0);


            sasForm.CreateField<AttachmentField>("Vendor Cost Estimates", lang).SetDescription(@"<i>Please submit documentation as a <span style='color: Red;'> <b>single PDF document</b></i>.<br/>
[Required if funding requested for equipment and materials]</span>", lang);

            // =============================================  TEACHING RELEASE
            sasForm.CreateField<InfoSection>(null, null)
             .AppendContent("h3", "Teaching Release", lang)
             .AppendContent("div", @"<i>List the courses you are scheduled to teach in the academic year for which release time is requested, indicating in which of these courses you wish to be released from teaching. Use the + button to add courses.</i>", lang);
            sasForm.CreateField<InfoSection>(null, null)
            .AppendContent("h5", "1st Term", lang);
            //TODO: add sub Form here
            CompositeField firstTerm = sasForm.CreateField<CompositeField>("", lang, false);
            firstTerm = CreateTeachingReleaseForm(firstTerm, lang, 1);

            sasForm.CreateField<InfoSection>(null, null)
           .AppendContent("h5", "2nd Term", lang);
            //TODO: add sub Form here
            CompositeField secTerm = sasForm.CreateField<CompositeField>("", lang, false);
            secTerm = CreateTeachingReleaseForm(secTerm, lang, 0);

            sasForm.CreateField<TextArea>("Justification", lang)
                .SetDescription("<i>Explain why release time is urgent and necessary at this moment. Maximum 250 words.</i>", lang);

            //==================================================== OVERVIEW OF FUND REQUESTED =======================================================================
            sasForm.CreateField<InfoSection>(null, null)
            .AppendContent("h1", "Overview of Funds Requested", lang)
            .AppendContent("div", "This section auto - populates once you have entered your budget details under the appropriate section(s).", lang, "alert alert-info");

            sasForm.CreateField<DecimalField>("Conference Travel Amount Requested", lang);
            sasForm.CreateField<DecimalField>("Research / Creativity Activity Travel Amount Requested", lang);
            sasForm.CreateField<DecimalField>("Support for Research and Creative Activity Equipment and Materials", lang);
            sasForm.CreateField<IntegerField>("Teaching release time", lang);
            sasForm.CreateField<DecimalField>("Personnel and Services", lang);
            sasForm.CreateField<DecimalField>("TOTAL ASK OF GRANT", lang);


            // ====================================================== Other and Previous Funding ========================================================================
            sasForm.CreateField<InfoSection>(null, null)
           .AppendContent("h1", "Other and Previous Funding", lang);

            var otherFunding = sasForm.CreateField<RadioField>("Have you received any SAS funding in the past 5 years?", lang, optionText, true);

            CompositeField summaryFund = sasForm.CreateField<CompositeField>("", lang, false);
            summaryFund.VisibilityCondition
              .AppendLogicalExpression(otherFunding, ComputationExpression.eRelational.EQUAL, otherFunding.GetOption("Yes", lang));
            summaryFund = CreateFundingSummaryForm(summaryFund, lang, 1);


            var previousFundRB = sasForm.CreateField<RadioField>("Previous SAS Funding for <i>this</i> Project", lang, optionText, true);
            previousFundRB.SetDescription("Have you previously received SAS funding in regard to this project?", lang);

            var previousTA = sasForm.CreateField<TextArea>("How Past SAS Funding Relates to?", lang)
              .SetDescription("How is the current application related to or different from the previous application already funded? Maximum 250 words.", lang);
              previousTA.VisibilityCondition.AppendLogicalExpression(previousFundRB, ComputationExpression.eRelational.EQUAL, previousFundRB.GetOption("Yes", lang));


            var otherFundingBefore = sasForm.CreateField<RadioField>("Other Funding", lang, optionText, true);
            otherFundingBefore.SetDescription("Have you sought support for this project from SSHRC, the Canada Council for the Arts, the Killam Fund, or any other agency, internal, or external sources of funding?", lang);
            sasForm.CreateField<TextArea>("Other Sources of Funding", lang)
             .SetDescription(@"Please identify other sources of funding for this project, and explain how and why these multiple sources of funding are essential to your research. Why, for example, are you applying for SAS funding if you have a related SSHRC or Killam research grant? Maximum 250 words.", lang)
             .VisibilityCondition.AppendLogicalExpression(otherFundingBefore, ComputationExpression.eRelational.EQUAL, otherFundingBefore.GetOption("Yes", lang));

            sasForm.CreateField<TextArea>("Why not, or do you intend to do so?", lang)
            .SetDescription(@"Maximum 250 words.", lang)
            .VisibilityCondition.AppendLogicalExpression(otherFundingBefore, ComputationExpression.eRelational.EQUAL, otherFundingBefore.GetOption("No", lang));


            sasForm.CreateField<TextArea>("Other Support Sources", lang)
               .SetDescription("Please identify additional sources of support for this project, if applicable. (Such as honoraria, sales revenues, commissions, etc.). Maximum 250 words.", lang);

            // ====================================================== Scholarly Publications ========================================================================
            sasForm.CreateField<InfoSection>(null, null)
           .AppendContent("h1", "Scholarly Publications", lang);

            sasForm.CreateField<TextArea>("Publications over the past 5 years.", lang)
             .SetDescription("Please list your scholarly and/or creative publications over the past 5 years. Include conference presentations, displays, and exhibits or performances. Complete CVs will not be accepted.", lang);

            sasForm.CreateField<InfoSection>(null, null)
          .AppendContent("h3", "Collaborators", lang)
          .AppendContent("div", "Required only if collaborator is a Faculty of Arts faculty member.", lang,"alert alert-info");
            // TO DO -- Add Collaborator(s) below
            CompositeField collaborator = sasForm.CreateField<CompositeField>("", lang, false);

            collaborator = CreateCollaboratorForm(collaborator, lang, 0);

            //================================================ Submit form ===============================
            sasForm.CreateField<InfoSection>(null, null)
                .AppendContent("div", @"<h1>Save or Submit Your Application</h1>
                                       <div>To complete the application later, please click on the Save button below. You will be given a randomly generated reference number for which you need to submit a password. You can retrieve 
                                      the application using this reference number and the password and complete it later. Unfortunately if you misplace the reference number or forget the password, 
                                       you will have to start over a new application.</div>

                                      <div>If you have completed your application, please use the Submit button below. Submitted applications cannot be modified. 
                                            <span style='color: Red'>If your submission is successful, you should get a confirmation email</span>. Please check for this email <span style='color:Red'>before</span> you close your browser. 
                                If you don't see the email, <span style='color: Red'>your application may not have been submitted</span>, so please contact Nancie Hodgson, Research Coordinator (resarts@ualberta.ca).</div>", lang, "alert alert-info");

            //=============================================================================             Defininig roles
            WorkflowRole adminRole = workflow.AddRole(auth.GetRole("Admin", true));
            WorkflowRole inspectorRole = workflow.AddRole(auth.GetRole("Inspector", true));
            WorkflowRole chairRole = workflow.AddRole(auth.GetRole("Chair", true));



            //Defining email templates
           // string emailBody = "";
            //emailBody = "<p>Dear" +((TextField)chairName).GetValue(lang) + ",</p><br/>" +
            //                        "<p>A faculty member from your department has applied for a SAS grant.Please click on this link: @Link[Sas Application|@Model] to provide your assessment about this application."+
            //                        "You will be required to log in with your CCID email.</p> <br/>" +
            //                        "<p>Thank you.</p>";

            EmailTemplate chairEmailTemplate = ws.GetEmailTemplate("Chair Email Template", true);
            chairEmailTemplate.SetDescription("This metadata set defines the email template to be sent to chair of the department or Dean when user apply for the grant.", lang);
            chairEmailTemplate.SetSubject("SAS Application");
            chairEmailTemplate.SetBody("emailBody");


            EmailTemplate applicantSubmissionNotification = ws.GetEmailTemplate("Applicant Notification", true);
            applicantSubmissionNotification.SetDescription("This metadata set defines the email template to be sent to the applicant when application's submitted.", lang);
            applicantSubmissionNotification.SetSubject("SAS Application Submission");
            //emailBody = @"<p>Dear Colleague,</p>
            //                    <p>
            //                    Thank you for submitting your SAS grant application. 
            //                    Your chair has been automatically notified to provide an assessment about your application. 
            //                    We will inform you of the decision when the application review process is completed. 
            //                    </p>
            //                    <p>
            //                    Thank you.
            //                    </p>
            //                    <p>
            //                    Steve Patten <br />
            //                    Associate Dean (Research)
            //                    </p>";

            applicantSubmissionNotification.SetBody("emailBody");


            //Defining triggers
             //Feb 12 2021
            //EmailTrigger applicantNotificationEmailTrigger = workflow.AddTrigger("ToApplicant", "SendEmail");
            //applicantNotificationEmailTrigger.AddRecipientByEmail(((TextField)applicantEmail).GetValue("en"));
            //applicantNotificationEmailTrigger.AddTemplate(applicantSubmissionNotification.Id, "Applicant Email Notification");

            //EmailTrigger chairNotificationEmailTrigger = workflow.AddTrigger("ToChair", "SendEmail");
            //chairNotificationEmailTrigger.AddRecipientByEmail(((TextField)chairEmail).GetValue("en"));
            //chairNotificationEmailTrigger.AddTemplate(chairEmailTemplate.Id, "Chair Email Notification");



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
            PopUp submitActionPopUp = submitPostAction.AddPopUp("WARNING: Submitting the Form", "Once submitted, you cannot update the form.", "");
            submitActionPopUp.AddButtons("Yes, submit", "true");
            submitActionPopUp.AddButtons("Cancel", "false");

            //Defining trigger refs -- added Feb 12 2021
          //  submitPostAction.AddTriggerRefs("0", applicantNotificationEmailTrigger.Id, "Applicant Submission Notification Email Trigger");
          //  submitPostAction.AddTriggerRefs("1", chairNotificationEmailTrigger.Id, "Chair Submission-notification Email Trigger");

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
            PopUp deleteSubmissionActionPopUpopUp = deleteSubmissionPostAction.AddPopUp("WARNING: Delete", "Deleting the submission. Please confirm.", "");
            deleteSubmissionActionPopUpopUp.AddButtons("Yes, delete", "true");
            deleteSubmissionActionPopUpopUp.AddButtons("Cancel", "false");


            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\SASform_generared.xml");

            // string json = JsonConvert.SerializeObject(template);
            // File.WriteAllText("..\\..\\..\\..\\Examples\\SASform_generared.json", json);
        }
        private string[] GetDepartmentList()
        {
            string[] dept =new string[] { "Anthropology",
                                                "Art & Design",
                                                "Drama",
                                                "East Asian Studies",
                                                "Economics",
                                                "English and Film Studies",
                                                "History and Classics",
                                                "Linguistics",
                                                "Modern Languages and Cultural Studies (MLCS)",
                                                "Music",
                                                "Philosophy",
                                                "Political Science",
                                                "Psychology",
                                                "Sociology",
                                                "Women's and Gender Studies",
                                                "Media and Technology Studies",
                                                "Arts Resources Centre" };

            return dept;
        }

        private string[] GetDepartmentChair()
        {
            string[] chairDept = new string[] {
                                                "Pamela Willoughby: pwilloug @ualberta.ca",
                                                "Aidan Rowe: rowe1@ualberta.ca",
                                                "Melanie Dreyer-Lude : dreyerlu@ualberta.ca",
                                                "Christopher Lupke: lupke@ualberta.ca",
                                                 "Rick Szostak: rszostak@ualberta.ca",
                                                 "Cecily Devereux: devereux@ualberta.ca",
                                                 "Ryan Dunch: rdunch@ualberta.ca",
                                                 "Herb Colston: colston@ualberta.ca",
                                                 "Alla Nedashkiviska: allan@ualberta.ca",
                                                 "Patricia Tao: ptao@ualberta.ca",
                                                 "Marie-Eve Morin: mmorin1@ualberta.ca",
                                                 "Catherine Kellogg: ckellogg@ualberta.ca",
                                                 "Anthony Singhal: asinghal@ualberta.ca",
                                                 "Sara Dorow: sdorow@ualberta.ca",
                                                 "Michelle Meagher: mmmeaghe@ualberta.ca",
                                                 "Astrid Ensslin: ensslin@ualberta.ca",
                                                 "arcAdmin : mruaini@ualberta.ca",
                                                "Dean: Steve Patten : spatten@ualberta.ca"};
            return chairDept;
        }

        private CompositeField CreatePersonnelBudgetItemForm(CompositeField comField, string lang="en", int min=0, int max=0)
        {

            comField.CreateChildTemplate("PersonnelBugdet", "Personnel Budget Item", lang);
            comField.ChildTemplate.CreateField<TextArea>("Provide details and calculations as specified above", lang, false);
            comField.ChildTemplate.CreateField<DecimalField>("Estimated Cost", lang, false);
            comField.Min = min;
            comField.Max = max;//0 means unlimited
            comField.AllowMultipleValues = true;
            comField.InsertChildren();
            return comField;
        }

        private CompositeField CreateTeachingReleaseForm(CompositeField comField, string lang= "en", int min = 0, int max = 0)
        {
          
            comField.CreateChildTemplate("Teaching Release", "Teaching Release", lang);
            comField.ChildTemplate.CreateField<TextField>("Couse Name", lang, false);
            comField.ChildTemplate.CreateField<TextField>("Release Required?", lang, false);
            comField.ChildTemplate.CreateField<DecimalField>("Amount Requested ($)", lang, false);
            comField.Min = min;
            comField.Max = max;//0 means unlimited
            comField.AllowMultipleValues = true;
            comField.InsertChildren();
            return comField;
        }
        private CompositeField CreateCollaboratorForm(CompositeField comField, string lang = "en", int min = 0, int max = 0)
        {
            comField.CreateChildTemplate("Collaborator", "Collaborator Information", lang);

            comField.ChildTemplate.CreateField<TextField>("Full Name", lang, false);
            comField.ChildTemplate.CreateField<TextField>("Email Address", lang, false);
            comField.Min = min;
            comField.Max = max;//0 means unlimited
            comField.AllowMultipleValues = true;
            comField.InsertChildren();
            return comField;
        }

        private CompositeField CreateFundingSummaryForm(CompositeField comField, string lang = "en", int min = 0, int max = 0)
        {
            
            string[] optionText = new string[] {"Spring", "Fall" };
            comField.CreateChildTemplate("Funding Summary", "SAS Funding Summary", lang);

            comField.ChildTemplate.CreateField<TextField>("Type of Funding", lang, false);
            comField.ChildTemplate.CreateField<DecimalField>("Value of Award", lang, false);
            comField.ChildTemplate.CreateField<IntegerField>("Year of Application", lang, false);
            comField.ChildTemplate.CreateField<SelectField>("Competition Applied To", lang,optionText, false);
            comField.ChildTemplate.CreateField<TextField>("Title of Project", lang, false);
            comField.ChildTemplate.CreateField<TextArea>("Brief Description of project", lang, false).SetDescription("Maximum 100 words.", lang);
            comField.Min = min;
            comField.Max = max;//0 means unlimited
            comField.AllowMultipleValues = true;
            comField.InsertChildren();
            return comField;
        }

        [Test]
        public void TestFileUpload()
        {
            string lang = "en";
            string templateName = "Attachment Test";

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

            //=============================================================================== Defininig SAS form
            DataItem sasForm = template.GetDataItem("Submission Form", true, lang);
            sasForm.IsRoot = true;
            
            sasForm.SetDescription("This template is designed for SAS Application Grant", lang);


            sasForm.CreateField<TextField>("Applicant Name:", lang, true);
            sasForm.CreateField<AttachmentField>("Supported Documentation", lang).SetDescription(@"Please attach the required travel and conference supporting documentation here as <span style='color: Red;'>a <b>single PDF document</b>. [Be sure to review the section of the Policies and Procedures on required supporting documentation]</span>", lang);
           
            WorkflowRole adminRole = workflow.AddRole(auth.GetRole("Admin", true));
            WorkflowRole chairRole = workflow.AddRole(auth.GetRole("Chair", true));

            // Submitting an inspection form
            //Only safey inspectors can submit this form
            GetAction startSubmissionAction = workflow.AddAction("Start Submission", nameof(TemplateOperations.Instantiate), "Home");
            startSubmissionAction.Access = GetAction.eAccess.Public;

            //Listing forms.
            //Applicants can list their own submissions.
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
            PopUp submitActionPopUp = submitPostAction.AddPopUp("WARNING: Submitting the Form", "Once submitted, you cannot update the form.", "");
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
            PopUp deleteSubmissionActionPopUpopUp = deleteSubmissionPostAction.AddPopUp("WARNING: Delete", "Deleting the submission. Please confirm.", "");
            deleteSubmissionActionPopUpopUp.AddButtons("Yes, delete", "true");
            deleteSubmissionActionPopUpopUp.AddButtons("Cancel", "false");


            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\AttachmentTest_generared.xml");

        }

        [Test]
        public void CompositeFormFieldTest()
        {
            string lang = "en";
            string templateName = "Composite Field Test Form Template";

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


            //Defininig the inspection form
            DataItem inspectionForm = template.GetDataItem("Composite Field Test Form", true, lang);
            inspectionForm.IsRoot = true;

            inspectionForm.CreateField<DateField>("Date Date", lang, true)
                .IncludeTime = false;

            string[] optionText = new string[] { "Yes", "No", "N/A" };
            inspectionForm.CreateField<RadioField>("Is there 2m (6.5 ft) of distance between all occupants?", lang, optionText, true);


            CompositeField authors = inspectionForm.CreateField<CompositeField>("Authors", lang, true);
            authors.CreateChildTemplate("Author", "Author information", lang);
            authors.ChildTemplate.CreateField<TextField>("Name", lang, false);
            authors.ChildTemplate.CreateField<TextField>("Email", lang, false);
           // authors.ChildTemplate.CreateField<TextField>("Phone", lang, false);

            authors.Min = 1;
            authors.Max = 3;//0 means unlimited
            authors.AllowMultipleValues = true;
            authors.InsertChildren();

            //string[] optionBuilding = new string[] { "Arts and Convocation Hall", "Assiniboia Hall", "Fine Arts Building", "HM Tory Building", "HUB", "Humanities Centre", "Industrial Design Studio", "North Power Plant", "South Academic Building", "Timms Centre for the Arts", "Varsity Trailer" };
            //inspectionForm.CreateField<SelectField>("Building", lang, optionBuilding, true);
            //inspectionForm.CreateField<TextField>("Inspected By", lang, true, true);
            //inspectionForm.CreateField<IntegerField>("Number of People in the work area", lang, true);


            //Defininig roles
            WorkflowRole adminRole = workflow.AddRole(auth.GetRole("Admin", true));
            WorkflowRole inspectorRole = workflow.AddRole(auth.GetRole("Inspector", true));


            // Submitting an inspection form
            //Only safey inspectors can submit this form
            GetAction startSubmissionAction = workflow.AddAction("Start Submission", nameof(TemplateOperations.Instantiate), "Home");
            startSubmissionAction.Access = GetAction.eAccess.Restricted;
            startSubmissionAction.AddStateReferances(emptyState.Id)
                .AddAuthorizedRole(inspectorRole.Id);

            //Listing inspection form submissions.
            //Inspectors can list their own submissions.
            //Admins can list all submissions.
            GetAction listSubmissionsAction = workflow.AddAction("List Submissions", nameof(TemplateOperations.ListInstances), "Home");
            listSubmissionsAction.Access = GetAction.eAccess.Restricted;
            listSubmissionsAction.AddStateReferances(submittedState.Id)
                .AddOwnerAuthorization()
                .AddAuthorizedRole(adminRole.Id);

            //Detailed submission inspection forms.
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
            PopUp deleteSubmissionActionPopUpopUp = deleteSubmissionPostAction.AddPopUp("WARNING: Delete", "Deleting the submission. Please confirm.", "");
            deleteSubmissionActionPopUpopUp.AddButtons("Yes, delete", "true");
            deleteSubmissionActionPopUpopUp.AddButtons("Cancel", "false");


            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\compositeFormFieldTest_generared.xml");

          //  string json = JsonConvert.SerializeObject(template);
          //  File.WriteAllText("..\\..\\..\\..\\Examples\\compositeFormFieldTest_generared.json", json);
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
