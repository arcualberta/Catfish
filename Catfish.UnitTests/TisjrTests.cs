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
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace Catfish.UnitTests
{
    public class TisjrTests
    {
        private protected AppDbContext _db;
        private protected TestHelper _testHelper;
        string[] YesNoOptionsText = new string[] { "Yes", "No" };
        string _templateName = "Tisjr Input Form Template";
        string _metadataSetName = "Tisjr Metadata Set";
        string lang = "en";
        string _apiKey = "";

        [SetUp]
        public void Setup()
        {
            _testHelper = new TestHelper();
            _db = _testHelper.Db;
            _apiKey = _testHelper.Configuration.GetSection("GoogleApiKey").Value;
        }

       
        [Test]
        public void TisjrFormTest()
        {
            string lang = "original";
           // string templateName = "Tisjr Input Form Template";

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

            //ws.SetModel(template);

            //Get the Workflow object using the workflow service
            Workflow workflow = template.Workflow;

            //Defining email templates
           
            //Defininig the inspection form
            DataItem bcpForm = template.GetDataItem(_templateName, true, lang);
            bcpForm.IsRoot = true;
            bcpForm.SetDescription("This template is designed for collecting metadata for Tisjr project", lang);

            bcpForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h1", "Data Collection Sheet", lang);

            string[] keywordList = GetWordKeywords();
             bcpForm.CreateField<SelectField>("Word", lang,keywordList, true);

            var passage =bcpForm.CreateField<TextArea>("Passage", lang, true, true);
            passage.Cols = 50;
            passage.Rows = 3;
            var translation =bcpForm.CreateField<TextArea>("Literal Translation", lang, true, true);
            translation.Cols = 50;
            translation.Rows = 3;
            var relation =bcpForm.CreateField<IntegerField>("Relation(year)", lang, true);
            relation.IsListEntryTitle = true;
            bcpForm.CreateField<IntegerField>("Volume", lang);
            var pageNum = bcpForm.CreateField<IntegerField>("Page Number", lang);
            pageNum.AllowMultipleValues = true;

            string[] classifications = GetClassifications();
            bcpForm.CreateField<SelectField>("Classification", lang, classifications);

            var notes = bcpForm.CreateField<TextArea>("Notes", lang);
            notes.AllowMultipleValues = true;
            notes.Cols = 50;
            notes.Rows = 3;
            var dbnotes = bcpForm.CreateField<TextArea>("Database Notes", lang);
            dbnotes.AllowMultipleValues = true;
            dbnotes.Cols = 50;
            dbnotes.Rows = 3;
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //

            Define_Tisjr_RolesStatesWorkflow1(workflow, ref template, bcpForm);
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\Tisjr_generared.xml");

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
        private void Define_Tisjr_RolesStatesWorkflow1(Workflow workflow, ref ItemTemplate template, DataItem wrForm)
        {
            IWorkflowService ws = _testHelper.WorkflowService;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            State emptyState = workflow.AddState(ws.GetStatus(template.Id, "", true));
            State submittedState = workflow.AddState(ws.GetStatus(template.Id, "Submitted", true));

            State deleteState = workflow.AddState(ws.GetStatus(template.Id, "Deleted", true));


            WorkflowRole adminRole = workflow.AddRole(auth.GetRole("Portal_Admin", true));
            WorkflowRole raRole = workflow.AddRole(auth.GetRole("Research_Assistant", true));


            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                     Submitting an form
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///
            GetAction startSubmissionAction = workflow.AddAction("Start Submission", nameof(TemplateOperations.Instantiate), "Home");
            startSubmissionAction.Access = GetAction.eAccess.Restricted;
            startSubmissionAction.AddStateReferances(emptyState.Id)
               .AddAuthorizedRole(raRole.Id)
               .AddAuthorizedRole(adminRole.Id);

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
                   .AddAuthorizedRole(raRole.Id)
                  .AddAuthorizedRole(adminRole.Id);



            //Detailed submission bcp forms.
            //Inspectors can view their own submissions.
            //Admins can view all submissions.
            GetAction viewSubmissionAction = workflow.AddAction("Details", nameof(TemplateOperations.Read), "List");
            viewSubmissionAction.Access = GetAction.eAccess.Restricted;
            viewSubmissionAction.AddStateReferances(submittedState.Id)
              //  .AddOwnerAuthorization()
              .AddAuthorizedRole(raRole.Id)
              .AddAuthorizedRole(adminRole.Id);
           
            // Edit submission related workflow items
            //Defining actions
            GetAction editSubmissionAction = workflow.AddAction("Edit Submission", nameof(TemplateOperations.Update), "Details");

            //Submissions can only be edited by admins
            editSubmissionAction.AddStateReferances(submittedState.Id)
                 .AddAuthorizedRole(raRole.Id)
                .AddAuthorizedRole(adminRole.Id);

            PostAction editPostActionSubmit = editSubmissionAction.AddPostAction("Submit", nameof(TemplateOperations.Update));//(Button Label, ActionName)
            editPostActionSubmit.AddStateMapping(submittedState.Id, submittedState.Id, "Submit");//current state, nectStae, buttonLabel
            //editPostActionSubmit.ValidateInputs = false;

            //Defining the pop-up for the above postActionSubmit action
            PopUp EditActionPopUpopUp = editPostActionSubmit.AddPopUp("Confirmation", "Do you really want to submit this document?", "Once submitted, you cannot update the document.");
            EditActionPopUpopUp.AddButtons("Yes, submit", "true");
            EditActionPopUpopUp.AddButtons("Cancel", "false");


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
            deleteSubmissionPostAction.AddStateMapping(submittedState.Id, deleteState.Id, "Delete");

            //Defining the pop-up for the above postAction action
            PopUp deleteSubmissionActionPopUpopUp = deleteSubmissionPostAction.AddPopUp("Confirmation", "Do you really want to delete this document?", "Once deleted, you cannot access this document.");
            deleteSubmissionActionPopUpopUp.AddButtons("Yes, delete", "true");
            deleteSubmissionActionPopUpopUp.AddButtons("Cancel", "false");

            //Defining state referances
            ////////deleteSubmissionAction.GetStateReference(savedState.Id, true)
            ////////    .AddOwnerAuthorization();
            deleteSubmissionAction.GetStateReference(submittedState.Id, true)
                .AddAuthorizedRole(adminRole.Id);


        }

        private string[] GetWordKeywords()
        {

            return new string[] { "Dieu",
                                  "Seigneur",
                                  "Divinité",
                                   "Divin(e)", "Prière", "Prier",
                                   "Péché", "Confession", "Démon", "Oki",
                                   "Manitou", "Diable", "Satan", "Jongleur","Sorcier", "Magicien", "Medecin", "Chirurgien",
                                   "Esprits", "Génie/Geniés"};
        }

        private string[] GetClassifications()
        {

            return new string[] { "Indigenous concept", "French concept" };
        }

        [Test]
        public void ImportData()
        {
            bool clearCurrentData = true;

            //Set maxEntries to a positive value to limit the maximum number of data entries to be imported.
            int maxEntries = -1;

            string multiValueSeparator = ";";


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

                    bool skipColumn = (colHeading == "suject - French") || string.IsNullOrEmpty(colValue);

                    if (!skipColumn)
                    {
                        bool multivalued = colHeadings[i].Equals("Creator(s)")
                            || colHeadings[i].Equals("Contributor(s)")
                            || colHeadings[i].Equals("subject - English")
                            || colHeadings[i].Equals("Genre/Form")
                            || colHeadings[i].Equals("Medium of Performance")
                            || colHeadings[i].Equals("Thematic Areas")
                            || colHeadings[i].Equals("Keyword")
                            || colHeadings[i].Equals("Coverage (spatial)")
                            || colHeadings[i].Equals("Language");

                        var vals = multivalued
                            ? colValue.Split(multiValueSeparator, StringSplitOptions.RemoveEmptyEntries)
                            : new string[] { colValue };

                        if (colHeadings[i].Equals("Description") || colHeadings[i].Equals("Rights/Access Statements") || colHeadings[i].Equals("Notes/Problems"))
                        {
                            for (int n = 0; n < vals.Length; ++n)
                                ms.SetFieldValue<TextArea>(colHeading, lang, vals[n], lang, false, n);
                        }
                        else
                        {
                            for (int n = 0; n < vals.Length; ++n)
                                ms.SetFieldValue<TextField>(colHeading, lang, vals[n], lang, false, n);
                        }
                    }
                    i++;
                }
                _db.Items.Add(item);

                if (maxEntries > 0 && rowCount == maxEntries)
                    break;

                rowCount++;
            }

            _db.SaveChanges();
        }

        [Test]
        public void ReIndex()
        {
            bool reindexAll = true;
            _testHelper.SolrBatchService.IndexItems(reindexAll);
        }


        public List<RowData> ReadGoogleSheet()
        {
            String spreadsheetId = "1sNfs19_hjGxMibp6Awgnqa16bxtmbmLTWqZgM-Yxfdk/edit#gid=1101487780";//==>google sheet Id
            String ranges = "A2:Z";// read from col A to Z, starting 2nd row

            SheetsService sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(),
                ApplicationName = "Google-Sheets",
                ApiKey = _apiKey
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
            //TODO: change the colomn name
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
                "Related Records",
                "Thumbnail"
            };

        }

    }
}
