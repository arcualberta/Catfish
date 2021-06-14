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
                 .AppendContent("h1", "Join Us", lang);
           
            bcpForm.CreateField<TextField>("Name", lang,true);
            bcpForm.CreateField<TextField>("Email", lang, true);
            bcpForm.CreateField<TextField>("Affiliation", lang, true);
            var other =bcpForm.CreateField<TextArea>(@"Please, tell us what language(s) you teach and what age groups (e.g., elementary, secondary, college/university, adults)", lang, false);
            other.Cols = 50;
            other.Rows = 5;
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //
           
            Define_TBLT_RolesStatesWorkflow(workflow, ref template);
            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\TBLT_ContactForm_generared.xml");

            //string json = JsonConvert.SerializeObject(template);
            //File.WriteAllText("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.json", json);
        }

        
        private EmailTemplate CreateEmailTemplate(ref ItemTemplate template)
        {
            string lang = "en";
            string body = "<p>Dear Admin</p><p>Some one has requested to join TBLT. </p>";
            EmailTemplate adminNotification = template.GetEmailTemplate("Admin Notification", lang, true);
            adminNotification.SetDescription("This metadata set defines the email template to be sent to the portal admin.", lang);
            adminNotification.SetSubject("Join Task-based Language Teaching");
            adminNotification.SetBody(body);

            return adminNotification;

        }
        private void Define_TBLT_RolesStatesWorkflow(Workflow workflow, ref ItemTemplate template)
        {
            IWorkflowService ws = _testHelper.WorkflowService;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            State emptyState = workflow.AddState(ws.GetStatus(template.Id, "", true));
            State submittedState = workflow.AddState(ws.GetStatus(template.Id, "Submitted", true));
            State deleteState = workflow.AddState(ws.GetStatus(template.Id, "Deleted", true));

            WorkflowRole adminRole = workflow.AddRole(auth.GetRole("Portal Admin", true));
            WorkflowRole memberRole = workflow.AddRole(auth.GetRole("Member", true));


            // Submitting an bcp form
            //Only safey inspectors can submit this form
            GetAction startSubmissionAction = workflow.AddAction("Start Submission", nameof(TemplateOperations.Instantiate), "Home");
            startSubmissionAction.Access = GetAction.eAccess.Public;
            startSubmissionAction.AddStateReferances(emptyState.Id)
                                 .AddOwnerAuthorization();
           
            //Listing bcp forms.
            //Admins and Inspectors can run the item-list report to list instances.
            //Note that the visibility of individual list entries is depend on the
            //Read permission on individual submissions.
            GetAction listSubmissionsAction = workflow.AddAction("List Submissions", nameof(TemplateOperations.ListInstances), "Home");
            listSubmissionsAction.Access = GetAction.eAccess.Restricted;
            listSubmissionsAction.AddStateReferances(submittedState.Id)
                  // .AddOwnerAuthorization()
                  .AddAuthorizedRole(memberRole.Id)
                  .AddAuthorizedRole(adminRole.Id);


            //Detailed submission bcp forms.
            //Inspectors can view their own submissions.
            //Admins can view all submissions.
            GetAction viewSubmissionAction = workflow.AddAction("Details", nameof(TemplateOperations.Read), "List");
            viewSubmissionAction.Access = GetAction.eAccess.Restricted;
            viewSubmissionAction.AddStateReferances(submittedState.Id)
               // .AddOwnerAuthorization()
               .AddAuthorizedRole(memberRole.Id)
                .AddAuthorizedRole(adminRole.Id);


            //Post action for submitting the form
            PostAction submitPostAction = startSubmissionAction.AddPostAction("Submit", nameof(TemplateOperations.Update));
            submitPostAction.AddStateMapping(emptyState.Id, submittedState.Id, "Submit");

            //Defining the pop-up for the above submitPostAction action
            PopUp submitActionPopUp = submitPostAction.AddPopUp("WARNING: Submitting the Form", "Once submitted, you cannot update the form.", "");
            submitActionPopUp.AddButtons("Yes, submit", "true");
            submitActionPopUp.AddButtons("Cancel", "false");
            //============================================================================
            //                                 EMAIL 
            //==============================================================================
            //EmailTemplate adminEmailTemplate = CreateEmailTemplate(ref template);

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

        }

        private string[] GetDisruptionOptions()
        {

            return new string[] { "N/A", "0-2 days", "1 week", "2 weeks", "3 weeks", "4 weeks", ">4 weeks" };
        }
        private string[] GetYesNoList()
        {
            return new string[] { "No", "Yes" };
        }

        private string[] GetRTOList()
        {
            return new string[] { "<b>Critical:</b> Directly impacts life, health, safety, or security. Cannot stop. RTO <b>less than</b> 4 hours",
             "<b>High:</b> Must continue at normal or increased level. Pausing for more than 24 hours may cause significant consequences or serious harm. RTO <b>less than</b> 24 hours",
            "<b>Medium:</b> Must continue if at all possible, perhaps in reduced mode. Stopping for more than one week may cause major disruption. RTO <b>less than</b> 1 week",
             "<b>Low:</b> May be suspended for up to one month without causing significant disruption. RTO <b>less than</b> 1 month",
            "<b>Deferable:</b> May pause and resume when conditions permit. RTO <b>over</b> 1 month" };
        }

        private string[] GetReqTechs()
        {

            return new string[] { "Network Services",  "Email",  "Telephone", "SIS", "HCM", "VPN",  "Others" };
        }

        private string[] GetPossibleHarmfulConsequences()
        {
            List<string> conseq = new List<string>();
            conseq.Add("01. Disruption of teaching?");
            conseq.Add("02. Disruption of research?");
            conseq.Add("03. Departure of faculty?");
            conseq.Add("04. Departure of staff?");
            conseq.Add("05. Departure of students? ");
            conseq.Add("06. Well-being of staff/faculty?");
            conseq.Add("07. Well-being of students?");
            conseq.Add("08. Payment deadlines unmet by campus?");
            conseq.Add("09. Loss of revenue to campus?");
            conseq.Add("10. Legal obligations unmet by campus?");
            conseq.Add("11. Legal harm to the University?");
            conseq.Add("12. Impact on other campus unit(s)?");
            conseq.Add("13. Impact on important business partner(s)?");
            conseq.Add("14. Impact on the Faculty of Arts’ brand image? ");
            conseq.Add("15. Function without power?");
            conseq.Add("16. Other harmful consequence?");
          

            return conseq.ToArray();
        }

    }
}
