using Catfish.Core.Authorization.Requirements;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
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
    class AttachmentFieldTest
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
        public void AttachmentFieldFormTest()
        {
            string lang = "en";
            string templateName = "Attachmemnt Field Test Template";

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


            //Defininig the  form
            DataItem rootForm = template.GetDataItem("Attachment Field Test Form", true, lang);
            rootForm.IsRoot = true;

            var name = rootForm.CreateField<AttachmentField>("Attachments", lang, true);
            name.IsListEntryTitle = true;


            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //

            Define_RolesStatesWorkflow1(workflow, ref template, rootForm);
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\AttachmentFieldTestForm_generared.xml");

        }

        private void Define_RolesStatesWorkflow1(Workflow workflow, ref ItemTemplate template, DataItem wrForm)
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
            PopUp submitActionPopUp = submitPostAction.AddPopUp("Confirmation", "Do you want to submit this document?", "Once submitted, you cannot update the document.");
            submitActionPopUp.AddButtons("Yes, submit", "true");
            submitActionPopUp.AddButtons("Cancel", "false");




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
