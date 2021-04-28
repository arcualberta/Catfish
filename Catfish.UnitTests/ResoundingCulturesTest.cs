﻿using Catfish.Core.Authorization.Requirements;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Expressions;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Contents.Workflow;
using Catfish.Core.Services;
using Catfish.Test.Helpers;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Catfish.UnitTests
{
    public class ResoundingCulturesTest
    {
        private protected AppDbContext _db;
        private protected TestHelper _testHelper;

        string _templateName = "Resounding Culture Item Template";
        string _metadataSetName = "Resounding Culture Metadata Set";
        string lang = "en";


        [SetUp]
        public void Setup()
        {
            _testHelper = new TestHelper();
            _db = _testHelper.Db;
        }

        [Test]
        public void ResoundingCultureFormTest()
        {

            IWorkflowService ws = _testHelper.WorkflowService;
            AppDbContext db = _testHelper.Db;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            ItemTemplate template = db.ItemTemplates
                .Where(et => et.TemplateName == _templateName)
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
            template.TemplateName = _templateName;
            template.Name.SetContent(_templateName);



            //Get the Workflow object using the workflow service
            Workflow workflow = template.Workflow;

            //Defininig the inspection form

            MetadataSet rcForm = template.GetMetadataSet(_metadataSetName, true, lang);

            //create Item Template
           //  DataItem rcForm = template.GetDataItem(_templateName, true, lang);
            //rcForm.IsRoot = true;
            rcForm.SetDescription("This template is designed for Resounding Culture Form", lang);

            foreach(string label in GetColHeaders())
            {
                if (label.Equals("Description") || label.Equals("Rights/Access Statements") || label.Equals("Notes/Problems"))
                {
                    var desc = rcForm.CreateField<TextArea>(label, lang, true);
                    desc.Cols = 30;
                    desc.Rows = 5;
                }
                else {
                    rcForm.CreateField<TextField>(label, lang, false, true);
                }
            }
           

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

        [Test]
        public void ImportData()
        {
            bool clearCurrentData = true;

            if (clearCurrentData)
            {
                var entities = _db.Items.ToList();
                _db.Entities.RemoveRange(entities);
            }


            //Filling the form
            var template = _db.ItemTemplates.Where(it => it.TemplateName == _templateName).FirstOrDefault();
            Assert.IsNotNull(template);



            int rowCount = 1;
            foreach (RowData row in ReadGoogleSheet())
            {
                //Create a new item
                var item = template.Instantiate<Item>();

                //retrieve and populate metadat fields
                var ms = item.GetMetadataSet(_metadataSetName, false, lang);
               // Assert.IsNotNull(ms);

                string[] colHeadings = GetColHeaders();

                int i = 0;
                foreach (var col in row.Values)
                {
                    string colHeading = colHeadings[i];
                    string colValue = col.FormattedValue;

                    if (!string.IsNullOrEmpty(colValue))
                    {
                        if (colHeadings[i].Equals("Description") || colHeadings[i].Equals("Rights/Access Statements") || colHeadings[i].Equals("Notes/Problems"))
                        {
                            ms.SetFieldValue<TextArea>(colHeading, lang, colValue, lang);
                        }
                        else
                        {
                            ms.SetFieldValue<TextField>(colHeading, lang, colValue, lang);
                        }
                    }
                    i++;
                }
                _db.Items.Add(item);

                //for DEBUG only INSERT 2
                //if (rowCount == 2)
                //    break;

               // rowCount++;
            }

            _db.SaveChanges();
        }

        // [Test]
        public List<RowData> ReadGoogleSheet()
        {

            string apiKey = "AIzaSyA6LhLmkjcpqbq4Dpl9RwE_1QYi7BVUmX0"; //Google api key

            String spreadsheetId = "1YFS3QXGpNUtakBRXxsFmqqTYMYNv8bL-XbzZ3n6LRsI";//==>google sheet Id
            String ranges = "A2:Y";// read from col A to Y, starting 2nd row

            SheetsService sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(),
                ApplicationName = "Google-Sheets",
                ApiKey = apiKey
            });


            bool includeGridData = true;

            SpreadsheetsResource.GetRequest request = sheetsService.Spreadsheets.Get(spreadsheetId);
            request.Ranges = ranges;
            request.IncludeGridData = includeGridData;


            // To execute asynchronously in an async method, replace `request.Execute()` as shown:
            Google.Apis.Sheets.v4.Data.Spreadsheet response = request.Execute(); //await request.ExecuteAsync();
          

            // Read all the rows
            var values = response.Sheets[0].Data.Select(d => d).ToList();
          
            List<RowData> rows = values[0].RowData.ToList();

            return rows;
        }
        public static UserCredential GetCredential()
        {
            // TODO: Change placeholder below to generate authentication credentials. See:
            // https://developers.google.com/sheets/quickstart/dotnet#step_3_set_up_the_sample
            //
            // Authorize using one of the following scopes:
            //     "https://www.googleapis.com/auth/drive"
            //     "https://www.googleapis.com/auth/drive.file"
            //     "https://www.googleapis.com/auth/drive.readonly"
            //     "https://www.googleapis.com/auth/spreadsheets"
            //     "https://www.googleapis.com/auth/spreadsheets.readonly"
            return null;
        }


        private string[] GetColHeaders()
        {
            return new string[] {
                "Source Collection",
                "Source Collection ID",
                "URL",
                "Title",
                "Title (alt)",
                "Creator(s)",
                "Contributor(s)",
                "Publisher",
                "Type",
                "Format (medium physical)",
                "Format (medium digital)",
                "Format (extent)",
                "Genre/Form",
                "subject-English",
                "suject - French",
                "Medium of Performance",
                "Thematic Areas",
                "Keyword",
                "Description",
                "Coverage (spatial)",
                "Language",
                "Date",
                "Rights/Access Statements",
                "Notes/Problems",
                "Related Records"
            };

        }
    }
}

