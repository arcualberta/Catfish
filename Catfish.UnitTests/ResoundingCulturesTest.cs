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
    public class ResoundingCulturesTest
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
        public void ResoundingCultureFormTest()
        {
            string lang = "en";
            string templateName = "Resounding Cultures Form Template";

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

            //Defininig states
           
            //Defining email templates

            //Defininig the inspection form
            DataItem rcForm = template.GetDataItem(templateName, true, lang);
            rcForm.IsRoot = true;
            rcForm.SetDescription("This template is designed for Resounding Culture Form", lang);

           
            rcForm.CreateField<TextField>("Source Collection", lang, false);
            rcForm.CreateField<TextField>("Source Collection ID", lang, false);
            rcForm.CreateField<TextField>("URL", lang, false);
            rcForm.CreateField<TextField>("Title", lang, false);
            rcForm.CreateField<TextField>("Title (alt)", lang, false);
            rcForm.CreateField<TextField>("Creator(s)", lang, false, true);
            rcForm.CreateField<TextField>("Contributor(s)", lang, false, true);
            rcForm.CreateField<TextField>("Publisher", lang, false);
            rcForm.CreateField<TextField>("Type", lang, false);
            rcForm.CreateField<TextField>("Format (medium physical)", lang, false);


            rcForm.CreateField<TextField>("Format (medium digital)", lang, false);
            rcForm.CreateField<TextField>("Format (extent)", lang, false);
            rcForm.CreateField<TextField>("subject-English", lang, false, true);
            rcForm.CreateField<TextField>("suject - French", lang, false, true);
            rcForm.CreateField<TextField>("Medium of Performance", lang, false, true);
            rcForm.CreateField<TextField>("Thematic Areas", lang, false, true);
            rcForm.CreateField<TextField>("Keyword", lang, false, true);
            var desc = rcForm.CreateField<TextArea>("Description", lang, true);
            desc.Cols = 30;
            desc.Rows = 5;
            rcForm.CreateField<TextField>("Coverage (spatial)", lang, true);
            rcForm.CreateField<TextField>("Language", lang, true);
            rcForm.CreateField<TextField>("Date", lang, true);

            var access = rcForm.CreateField<TextArea>("Rights/Access Statements", lang, false);
            access.Cols = 30;
            access.Rows = 5;

            var notes = rcForm.CreateField<TextArea>("Notes/Problems", lang, false);
            notes.Cols = 30;
            notes.Rows = 5;


            rcForm.CreateField<TextField>("Related Records", lang, true);
           
            
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //

            Define_RC_RolesStatesWorkflow(workflow, ref template);
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\ResoundingCultureTemplate_generared.xml");

            //string json = JsonConvert.SerializeObject(template);
            //File.WriteAllText("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.json", json);
        }

        private void Define_RC_RolesStatesWorkflow(Workflow workflow, ref ItemTemplate template)
        {
            IWorkflowService ws = _testHelper.WorkflowService;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            State emptyState = workflow.AddState(ws.GetStatus(template.Id, "", true));
            State submittedState = workflow.AddState(ws.GetStatus(template.Id, "Submitted", true));
            State deleteState = workflow.AddState(ws.GetStatus(template.Id, "Deleted", true));

            WorkflowRole adminRole = workflow.AddRole(auth.GetRole("ResoundingCultureAdmin", true));
            WorkflowRole inspectorRole = workflow.AddRole(auth.GetRole("ResoundingCultureUser", true));


            // Submitting an bcp form
            //Only safey inspectors can submit this form
            GetAction startSubmissionAction = workflow.AddAction("Start Submission", nameof(TemplateOperations.Instantiate), "Home");
            startSubmissionAction.Access = GetAction.eAccess.Public;//.Restricted;
            startSubmissionAction.AddStateReferances(emptyState.Id);
               // .AddAuthorizedRole(inspectorRole.Id);

            //Listing bcp forms.
            //Admins and Inspectors can run the item-list report to list instances.
            //Note that the visibility of individual list entries is depend on the
            //Read permission on individual submissions.
            GetAction listSubmissionsAction = workflow.AddAction("List Submissions", nameof(TemplateOperations.ListInstances), "Home");
            listSubmissionsAction.Access = GetAction.eAccess.Restricted;
            listSubmissionsAction.AddStateReferances(submittedState.Id)
                   .AddOwnerAuthorization()
                  // .AddAuthorizedRole(inspectorRole.Id)
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

    }
}
