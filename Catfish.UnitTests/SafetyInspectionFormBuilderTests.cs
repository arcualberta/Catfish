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
    public class SafetyInspectionFormBuilderTests
    {
        private protected AppDbContext _db;
        private protected TestHelper _testHelper;
        
        string lang = "en";

        [SetUp]
        public void Setup()
        {
            _testHelper = new TestHelper();
            _db = _testHelper.Db;
        }

        [Test]
        public void CovidSafetyInspectionTest()
        {
           // string lang = "en";
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

            //ws.SetModel(template);

            //Get the Workflow object using the workflow service
            Workflow workflow = template.Workflow;

            //Defininig states
            State emptyState = workflow.AddState(ws.GetStatus(template.Id, "", true));
            State submittedState = workflow.AddState(ws.GetStatus(template.Id, "Submitted", true));
            State deleteState = workflow.AddState(ws.GetStatus(template.Id, "Deleted", true));


            //Defining email templates
            EmailTemplate adminNotification = template.GetEmailTemplate("Admin Notification", lang, true);
            adminNotification.SetDescription("This metadata set defines the email template to be sent to the admin when an inspector does not submit an inspection report timely.", lang);
            adminNotification.SetSubject("Safety Inspection Submission");
            adminNotification.SetBody("TBD");

            EmailTemplate inspectorSubmissionNotification = template.GetEmailTemplate("Inspector Notification", lang, true);
            inspectorSubmissionNotification.SetDescription("This metadata set defines the email template to be sent to an inspector when an inspection report is not submitted timely.", lang);
            inspectorSubmissionNotification.SetSubject("Safety Inspection Reminder");
            inspectorSubmissionNotification.SetBody("TBD");

            //Defininig the inspection form
            DataItem inspectionForm = template.GetDataItem("COVID-19 Inspection Form", true, lang);
            inspectionForm.IsRoot = true;
            inspectionForm.SetDescription("This template is designed for a weekly inspection of public health measures specific to COVID-19 and other return to campus requirements.", lang);

            inspectionForm.CreateField<DateField>("Inspection Date", lang, true)
                .IncludeTime = false;

            string[] optionBuilding = new string[] { "Arts and Convocation Hall", "Assiniboia Hall", "Fine Arts Building", "HM Tory Building", "HUB", "Humanities Centre", "Industrial Design Studio", "North Power Plant", "South Academic Building", "Timms Centre for the Arts", "Varsity Trailer" };
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
            inspectionForm.CreateField<RadioField>("Is there 2m (6.5 ft) of distance between all occupants?", lang, optionText, true)
                .FieldValueCssClass = "radio-inline"; ;
            inspectionForm.CreateField<RadioField>("Where physical distancing is not possible, are occupants wearing face masks?", lang, optionText, true)
                .FieldValueCssClass = "radio-inline"; ;
            var notes = inspectionForm.CreateField<TextArea>("Notes/Action", lang, false);
            notes.Cols = 30;
            notes.Rows = 5;

            inspectionForm.CreateField<TextField>("Assigned to", lang, false);

            inspectionForm.CreateField<InfoSection>(null, null)
                .AppendContent("h4", "Personal Hygiene", lang);
            inspectionForm.CreateField<RadioField>("Is a hand washing sink or hand sanitizer available?", lang, optionText, true)
                .FieldValueCssClass = "radio-inline"; ;
            inspectionForm.CreateField<RadioField>("Is the sink clean and free of contamination?", lang, optionText, true)
                .FieldValueCssClass = "radio-inline"; ;
            inspectionForm.CreateField<RadioField>("Is there an adequate supply of soap? ", lang, optionText, true)
                .FieldValueCssClass = "radio-inline"; ;
            notes = inspectionForm.CreateField<TextArea>("Notes/Action", lang, false);
            notes.Cols = 30;
            notes.Rows = 5;
            inspectionForm.CreateField<TextField>("Assigned to", lang, false);

            inspectionForm.CreateField<InfoSection>(null, null)
               .AppendContent("h4", "Housekeeping", lang);
            inspectionForm.CreateField<RadioField>("Is general housekeeping and cleanliness being maintained?", lang, optionText, true)
                .FieldValueCssClass = "radio-inline"; ;
            inspectionForm.CreateField<RadioField>("Are surfaces being disinfected on a regular basis?", lang, optionText, true)
                .FieldValueCssClass = "radio-inline"; ;
            inspectionForm.CreateField<RadioField>("Are there adequate cleaning supplies for the next 2 weeks?", lang, optionText, true)
                .FieldValueCssClass = "radio-inline"; ;
            inspectionForm.CreateField<RadioField>("Are walkways clear of trip hazards?", lang, optionText, true)
                .FieldValueCssClass = "radio-inline"; ;
            notes = inspectionForm.CreateField<TextArea>("Notes/Action", lang, false);
            notes.Cols = 30;
            notes.Rows = 5;

            inspectionForm.CreateField<TextField>("Assigned to", lang, false);

            inspectionForm.CreateField<InfoSection>(null, null)
               .AppendContent("h4", "Training", lang);
            inspectionForm.CreateField<RadioField>("Have all employees taken the COVID-19 Return to Campus training?", lang, optionText, true)
                .FieldValueCssClass = "radio-inline"; ;
            inspectionForm.CreateField<RadioField>("Have all employees been trained in your return to campus plan?", lang, optionText, true)
                .FieldValueCssClass = "radio-inline"; ;
            notes = inspectionForm.CreateField<TextArea>("Notes/Action", lang, false);
            notes.Cols = 30;
            notes.Rows = 5;

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
            eyeWashFlushed.FieldValueCssClass = "radio-inline"; ;
            var eyewashInfo = inspectionForm.CreateField<TextArea>("Eye Wash Station Info", lang, false);
            eyewashInfo.SetDescription("If you answer Yes to the above question, please provide the room number, date of the last annual test, and the year built for each eyewash station you flushed.", lang);
            eyewashInfo.Rows = 5;
            eyewashInfo.Cols = 30;

            inspectionForm.CreateField<InfoSection>(null, null)
               .AppendContent("h4", "Other", lang);
            inspectionForm.CreateField<RadioField>("Have all sinks been flushed for 3 minutes?", lang, optionText, true)
                .FieldValueCssClass = "radio-inline"; ;
            inspectionForm.CreateField<RadioField>("Is all appropriate PPE being worn?", lang, optionText, true)
                .FieldValueCssClass = "radio-inline"; ;

            notes = inspectionForm.CreateField<TextArea>("Notes/Action", lang, false);
            notes.Cols = 30;
            notes.Rows = 5;

            inspectionForm.CreateField<TextField>("Assigned to", lang, false);


            //Defininig roles
            WorkflowRole adminRole = workflow.AddRole(auth.GetRole("Safety_Admin", true));
            WorkflowRole inspectorRole = workflow.AddRole(auth.GetRole("Safety_Inspector", true));


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
                   .AddOwnerAuthorization()
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
            PopUp submitActionPopUp = submitPostAction.AddPopUp("WARNING: Submitting the Form", "Once submitted, you cannot update the form.", "");
            submitActionPopUp.AddButtons("Yes, submit", "true");
            submitActionPopUp.AddButtons("Cancel", "false");

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



            // Delete submission related workflow items
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


            db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.xml");

            //string json = JsonConvert.SerializeObject(template);
            //File.WriteAllText("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.json", json);
        }

        [Test]
        public void SafetyInspectionChecklistFormTest() { 
            string templateName = "Inspection Checklist Form Template";

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

            //Defininig the Submission revision Request form
            DataItem inspectionForm = template.GetDataItem("Inspection Checklist Form", true, lang);
            inspectionForm.IsRoot = true;
            inspectionForm.SetDescription("Faculty of Arts Checklist form.", lang);
            //inspectionForm.CreateField<InfoSection>(null, null)
            //    .AppendContent("H3", "UNIVERSITY OF ALBERTA, FACULTY OF ARTS INSPECTION CHECKLIST", lang);

            string[] departmentList = GetDepartmentList();
            inspectionForm.CreateField<SelectField>("Department:", lang, departmentList, true);
            inspectionForm.CreateField<TextField>("By", lang, true);
            inspectionForm.CreateField<DateField>("Date", lang, true);

            inspectionForm.CreateField<InfoSection>(null, null)
                .AppendContent("H5", "A. BUILDING AND WORK SPACE", lang, "alert alert-info");
           
            string[] options = GetYesNoList();
            string[] buildingWSQuestions = GetBuildingWPList();   
            TableField tf = inspectionForm.CreateField<TableField>("", lang, false, buildingWSQuestions.Length, buildingWSQuestions.Length);
            
            //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            tf.FieldLabelCssClass = "col-md-12 radio-inline";
            tf.FieldValueCssClass = "col-md-12";
            tf.TableHead.CreateField<InfoSection>("", lang, "", "Questions");
            var dam = tf.TableHead.CreateField<RadioField>("Options", lang, options);
            dam.SetFieldValueCssClass("col-md-2");
            dam.SetFieldLabelCssClass("col-md-2");
            dam.FieldValueCssClass = "radio-inline";
            //dam.SetFieldCssClass("radio-inline");

            var ta = tf.TableHead.CreateField<TextArea>("Comments/Corrective Actions Taken", lang);
            ta.Cols = 25;
            ta.Rows = 1;
            tf.SetColumnValues(0, buildingWSQuestions, lang);
            tf.AllowAddRows = false;

            inspectionForm.CreateField<InfoSection>(null, null)
               .AppendContent("H5", "B. EQUIPMENT AND MACHINERY", lang, "alert alert-info");


            string[] equipMaterials = GetEquipmentAndMaterials();
            var tf1 = inspectionForm.CreateField<TableField>("", lang, false, equipMaterials.Length, equipMaterials.Length);
            //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            tf1.FieldLabelCssClass = "col-md-12 radio-inline";
            tf1.FieldValueCssClass = "col-md-12";
            tf1.TableHead.CreateField<InfoSection>("", lang, "", "Questions");
            var dam1 = tf1.TableHead.CreateField<RadioField>("Options", lang, options);
            dam1.SetFieldValueCssClass("col-md-2");
            dam1.SetFieldLabelCssClass("col-md-2");
            dam1.FieldValueCssClass = "radio-inline";
            //dam.SetFieldCssClass("radio-inline");

            var ta1 = tf1.TableHead.CreateField<TextArea>("Comments/Corrective Actions Taken", lang);
            ta1.Cols = 25;
            ta1.Rows = 1;
            tf1.SetColumnValues(0, equipMaterials, lang);
            tf1.AllowAddRows = false;

            inspectionForm.CreateField<InfoSection>(null, null)
               .AppendContent("H5", "C. STORAGE (cabinets, shelving units, closets, bins, racks, space etc)", lang, "alert alert-info");


            string[] storages = GetStorage();
            var tf2 = inspectionForm.CreateField<TableField>("", lang, false, storages.Length, storages.Length);
            //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            tf2.FieldLabelCssClass = "col-md-12 radio-inline";
            tf2.FieldValueCssClass = "col-md-12";
            tf2.TableHead.CreateField<InfoSection>("", lang, "", "Questions");
            var dam2 = tf2.TableHead.CreateField<RadioField>("Options", lang, options);
            dam2.SetFieldValueCssClass("col-md-2");
            dam2.SetFieldLabelCssClass("col-md-2");
            dam2.FieldValueCssClass = "radio-inline";
            //dam.SetFieldCssClass("radio-inline");

            var ta2 = tf2.TableHead.CreateField<TextArea>("Comments/Corrective Actions Taken", lang);
            ta2.Cols = 25;
            ta2.Rows = 1;
            tf2.SetColumnValues(0, storages, lang);
            tf2.AllowAddRows = false;

            inspectionForm.CreateField<InfoSection>(null, null)
             .AppendContent("H3", "D. ELECTRICAL", lang);


            string[] electrical = GetElectricalList();
            var tf3 = inspectionForm.CreateField<TableField>("", lang, false, electrical.Length, electrical.Length);

            tf3.FieldLabelCssClass = "col-md-12 radio-inline";
            tf3.FieldValueCssClass = "col-md-12";
            tf3.TableHead.CreateField<InfoSection>("", lang, "", "Questions");
            var dam3 = tf3.TableHead.CreateField<RadioField>("Options", lang, options);
            dam3.SetFieldValueCssClass("col-md-2");
            dam3.SetFieldLabelCssClass("col-md-2");
            dam3.FieldValueCssClass = "radio-inline";


            var ta3 = tf3.TableHead.CreateField<TextArea>("Comments/Corrective Actions Taken", lang);
            ta3.Cols = 25;
            ta3.Rows = 1;
            tf3.SetColumnValues(0, electrical, lang);
            tf3.AllowAddRows = false;


            inspectionForm.CreateField<InfoSection>(null, null)
             .AppendContent("H5", "E. EMERGENCY PREPAREDNESS, MEDICAL & FIRST AID RESPONSE", lang, "alert alert-info");


            string[] emergencies = GetEmergencyList();
            var tf4 = inspectionForm.CreateField<TableField>("", lang, false, emergencies.Length, emergencies.Length);

            tf4.FieldLabelCssClass = "col-md-12 radio-inline";
            tf4.FieldValueCssClass = "col-md-12";
            tf4.TableHead.CreateField<InfoSection>("", lang, "", "Questions");
            var dam4 = tf4.TableHead.CreateField<RadioField>("Options", lang, options);
            dam4.SetFieldValueCssClass("col-md-2");
            dam4.SetFieldLabelCssClass("col-md-2");
            dam4.FieldValueCssClass = "radio-inline";


            var ta4 = tf4.TableHead.CreateField<TextArea>("Comments/Corrective Actions Taken", lang);
            ta4.Cols = 25;
            ta4.Rows = 1;
            tf4.SetColumnValues(0, emergencies, lang);
            tf4.AllowAddRows = false;

            //inspectionForm.CreateField<InfoSection>(null, null)
            // .AppendContent("H5", "F. HAZARDOUS MATERIALS-CHEMICALS", lang, "alert alert-info");


            //string[] haz = GetEmergencyList();
            //var tf5 = inspectionForm.CreateField<TableField>("", lang, false, haz.Length, haz.Length);

            //tf5.FieldLabelCssClass = "col-md-12 radio-inline";
            //tf5.FieldValueCssClass = "col-md-12";
            //tf5.TableHead.CreateField<InfoSection>("", lang, "", "Questions");
            //var dam5 = tf5.TableHead.CreateField<RadioField>("Options", lang, options);
            //dam5.SetFieldValueCssClass("col-md-2");
            //dam5.SetFieldLabelCssClass("col-md-2");
            //dam5.FieldValueCssClass = "radio-inline";


            //var ta5 = tf5.TableHead.CreateField<TextArea>("Comments/Corrective Actions Taken", lang);
            //ta5.Cols = 25;
            //ta5.Rows = 1;
            //tf5.SetColumnValues(0, haz, lang);
            //tf5.AllowAddRows = false;


            inspectionForm.CreateField<InfoSection>(null, null)
            .AppendContent("H5", "G. PERSONAL PROTECTIVE EQUIPMENT-PPE", lang, "alert alert-info");


            //string[] ppe = GetPPEList();
            //var tf6 = inspectionForm.CreateField<TableField>("", lang, false, ppe.Length, ppe.Length);

            //tf6.FieldLabelCssClass = "col-md-12 radio-inline";
            //tf6.FieldValueCssClass = "col-md-12";
            //tf6.TableHead.CreateField<InfoSection>("", lang, "", "Questions");
            //var dam6 = tf6.TableHead.CreateField<RadioField>("Options", lang, options);
            //dam6.SetFieldValueCssClass("col-md-2");
            //dam6.SetFieldLabelCssClass("col-md-2");
            //dam6.FieldValueCssClass = "radio-inline";
            

            //var ta6 = tf6.TableHead.CreateField<TextArea>("Comments/Corrective Actions Taken", lang);
            //ta6.Cols = 25;
            //ta6.Rows = 1;
            //tf6.SetColumnValues(0, ppe, lang);
            //tf6.AllowAddRows = false;


            inspectionForm.CreateField<InfoSection>(null, null)
            .AppendContent("H5", "H. GENERAL QUESTIONS - WORKERS/STUDENTS", lang, "alert alert-info");


            // string[] gen1 = GetGeneralQuestionsWorkers();
            // var tf7 = inspectionForm.CreateField<TableField>("", lang, false, gen1.Length, gen1.Length);
            // //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            // tf7.FieldLabelCssClass = "col-md-12 radio-inline";
            // tf7.FieldValueCssClass = "col-md-12";
            // tf7.TableHead.CreateField<InfoSection>("", lang, "", "Questions");
            // var dam7 = tf7.TableHead.CreateField<RadioField>("Options", lang, options);
            // dam7.SetFieldValueCssClass("col-md-2");
            // dam7.SetFieldLabelCssClass("col-md-2");
            // dam7.FieldValueCssClass = "radio-inline";
            // //dam.SetFieldCssClass("radio-inline");

            // var ta7 = tf7.TableHead.CreateField<TextArea>("Comments/Corrective Actions Taken", lang);
            // ta7.Cols = 25;
            // ta7.Rows = 1;
            // tf7.SetColumnValues(0, gen1, lang);
            // tf7.AllowAddRows = false;

            inspectionForm.CreateField<InfoSection>(null, null)
            .AppendContent("H5", "I. GENERAL QUESTIONS - SUPERVISORS AND MANAGEMENT", lang, "alert alert-info");


            // string[] gen2 = GetGeneralQuestionsManagement();
            // var tf8 = inspectionForm.CreateField<TableField>("", lang, false, gen2.Length, gen2.Length);
            // //tf.SetDescription("Where have the last 3 occurrences of this conference been held?", lang);
            // tf8.FieldLabelCssClass = "col-md-12 radio-inline";
            // tf8.FieldValueCssClass = "col-md-12";
            // tf8.TableHead.CreateField<InfoSection>("", lang, "", "Questions");
            // var dam8 = tf8.TableHead.CreateField<RadioField>("Options", lang, options);
            // dam8.SetFieldValueCssClass("col-md-2");
            // dam8.SetFieldLabelCssClass("col-md-2");
            // dam8.FieldValueCssClass = "radio-inline";
            // //dam.SetFieldCssClass("radio-inline");

            // var ta8 = tf8.TableHead.CreateField<TextArea>("Comments/Corrective Actions Taken", lang);
            // ta8.Cols = 25;
            // ta8.Rows = 1;
            // tf8.SetColumnValues(0, gen2, lang);
            // tf8.AllowAddRows = false;

            Define_Safety_RolesStatesWorkflow(workflow, ref template);
            db.SaveChanges();


            template.Data.Save("..\\..\\..\\..\\Examples\\safetyChecklist_generared.xml");

        }


        private void DefineEmailTemplate(ref ItemTemplate template)
        {
            string lang = "en";
            EmailTemplate adminNotification = template.GetEmailTemplate("Admin Notification", lang, true);
            adminNotification.SetDescription("This metadata set defines the email template to be sent to the admin when an inspector does not submit an inspection report timely.", lang);
            adminNotification.SetSubject("Safety Inspection Submission");
            adminNotification.SetBody("TBD");

            EmailTemplate inspectorSubmissionNotification = template.GetEmailTemplate("Inspector Notification", lang, true);
            inspectorSubmissionNotification.SetDescription("This metadata set defines the email template to be sent to an inspector when an inspection report is not submitted timely.", lang);
            inspectorSubmissionNotification.SetSubject("Safety Inspection Reminder");
            inspectorSubmissionNotification.SetBody("TBD");

        }
        private void Define_Safety_RolesStatesWorkflow(Workflow workflow, ref ItemTemplate template)
        {
            IWorkflowService ws = _testHelper.WorkflowService;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            State emptyState = workflow.AddState(ws.GetStatus(template.Id, "", true));
            State savedState = workflow.AddState(ws.GetStatus(template.Id, "Saved", true));
            State submittedState = workflow.AddState(ws.GetStatus(template.Id, "Submitted", true));
            State deleteState = workflow.AddState(ws.GetStatus(template.Id, "Deleted", true));

            WorkflowRole adminRole = workflow.AddRole(auth.GetRole("Safety_Admin", true));
            WorkflowRole inspectorRole = workflow.AddRole(auth.GetRole("Safety_Inspector", true));
            //********************************************************************************************//
            //                                     Submitting an safety form
            //Only safey inspectors can submit this form
            //*******************************************************************************************//
            GetAction startSubmissionAction = workflow.AddAction("Start Submission", nameof(TemplateOperations.Instantiate), "Home");
            startSubmissionAction.Access = GetAction.eAccess.Restricted;
            startSubmissionAction.AddStateReferances(emptyState.Id)
                .AddAuthorizedRole(inspectorRole.Id);

            //************************************************************************************************************//
            //                              Listing safety forms.
            //Admins and Inspectors can run the item-list report to list instances.
            //Note that the visibility of individual list entries is depend on the
            //Read permission on individual submissions.
            //**************************************************************************************************************//
            GetAction listSubmissionsAction = workflow.AddAction("List Submissions", nameof(TemplateOperations.ListInstances), "Home");
            listSubmissionsAction.Access = GetAction.eAccess.Restricted;
            listSubmissionsAction.AddStateReferances(submittedState.Id)
                   .AddOwnerAuthorization()
                   .AddAuthorizedRole(inspectorRole.Id)
                  .AddAuthorizedRole(adminRole.Id);

            listSubmissionsAction.AddStateReferances(savedState.Id)
                  .AddOwnerAuthorization()
                 .AddAuthorizedRole(adminRole.Id);

            //************************************************************************************************************//
            //Detailed submission safety forms.
            //Inspectors can view their own submissions.
            //Admins can view all submissions.
            //********************************************************************************************************** //

            GetAction viewSubmissionAction = workflow.AddAction("Details", nameof(TemplateOperations.Read), "List");
            viewSubmissionAction.Access = GetAction.eAccess.Restricted;
            viewSubmissionAction.AddStateReferances(submittedState.Id)
                .AddOwnerAuthorization()
                .AddAuthorizedRole(adminRole.Id);

            viewSubmissionAction.AddStateReferances(savedState.Id)
                .AddOwnerAuthorization()
                .AddAuthorizedRole(adminRole.Id);
            //Post action for submitting the form

            PostAction savePostAction = startSubmissionAction.AddPostAction(
                                                                               "Save",
                                                                               nameof(TemplateOperations.Update),
                                                                               @"<p>Your form saved successfully. 
                                                                                You can view/edit by <a href='@SiteUrl/items/@Item.Id'>click on here</a></p>"
                                                                               );
                savePostAction.ValidateInputs = false;
                savePostAction.AddStateMapping(emptyState.Id, savedState.Id, "Save");
         

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
                .AddAuthorizedRole(inspectorRole.Id)
                .AddAuthorizedRole(adminRole.Id);
           
           
            PostAction editSubmissionPostActionSave = editSubmissionAction.AddPostAction(
                                                                                       "Save",
                                                                                       nameof(TemplateOperations.Update),
                                                                                       @"<p>Your Conference Fund application saved successfully. 
                                                                                            You can view/edit by <a href='@SiteUrl/items/@Item.Id'>click on here</a></p>"
                                                                                       );
            editSubmissionPostActionSave.ValidateInputs = false;

            //Saved document can be saved without changing state
            editSubmissionPostActionSave.AddStateMapping(savedState.Id, savedState.Id, "Save");

            PostAction editSubmissionPostActionSubmit = editSubmissionAction.AddPostAction("Submit", nameof(TemplateOperations.Update));
            editSubmissionPostActionSubmit.AddStateMapping(savedState.Id, submittedState.Id, "Submit"); //current state, nextState, buttonLabel

            //Defining the pop-up for the above postActionSubmit action
            PopUp EditActionPopUpopUp = editSubmissionPostActionSubmit.AddPopUp("Confirmation", "Do you really want to submit this document?", "Once submitted, you cannot update the document.");
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

        private string[] GetDepartmentList()
        {
            return new string[] {
                "Anthropology",
                "Art and Design",
                "Dean's Office ",
                "Drama",
                "East Asian Studies",
                "Economics",
                "English and Film Studies",
                "History and Classics",
                "Linguistics",
                "Media and Technology Studies",
                "Modern Languages and Cultural Studies",
                "Music",
                "Philosophy",
                "Political Science",
                "Psychology",
                "Sociology",
                "Women's and Gender Studies"
            };
        }
        private string[] GetYesNoList()
        {
            return new string[] { "No", "Yes", "N/A" };
        }

        private string[] GetBuildingWPList()
        {
            return new string[] {
              
                "Are floors clean and free of loose materials and debris?",
                "Are floors free from protruding nails, splinters, holes and loose boards?",
                "Are floors free of oil and water spillage or leakage?",
                "Is absorbent available for immediate clean-up of spills and leaks?",
                "Are floors in good condition and free of uneven surfaces ?",
                "Are walkways, stairways and aisles kept clear of obstructions?",
                "Are stairs and handrails in good condition?",
                "Are doorways / exits clear of materials or equipment?",
                "Are entrances / exit doors in good working order?",
                "Are wall and ceiling fixtures fastened securely?",
                "Is there adequate lighting?",
                "Are ventilation and exhaust fans in good working order?"
            };
        }

        private string[] GetEquipmentAndMaterials()
        {

            return new string[] {
            "Is there a preventative maintenance program (inspection and service checklist) for equipment?",
            "Are operators properly trained?",
            "Are start / stop switches clearly marked and in easy reach?",
            "Protective Guards: are gear covers, pulley belt covers, pinch point guards, railings, blade guards in place?",
            "Are bolts / nuts on fixed guards properly secured?",
            "Are guards free of cracks?",
            "Are all guards in place and operating as designed?",
            "Is there enough work space to safely utilize the equipment?",
            "Are fumes, dust and exhaust controlled?",
            "Noise levels - Is hearing protection available and used properly?",
            "Are sharp objects including knives stored properly?",
            "Material Handling: Are hoists, forklifts, carts, trolleys in good working order, regularly inspected and properly maintained?",
            "Personnel Supporting Equipment: are scaffolds, scissor lifts, catwalks, platforms, life - lines, sling - chairs in good working order?",
            "Are ladders and step stools available where needed?",
            "Are ladders sufficient for their use and in good condition(no missing or damaged parts, bent parts, and with non - slip feet)?",
            "Are ladders set up properly before use?",
            "Are hand tools properly stored?",
            "Are kitchen equipment in good working order and properly maintained(FOH)?"
            };
        }

        private string[] GetStorage()
        {

            return new string[] {
            "Are storage units in good condition and stable?",
            "Are racks and platforms loaded only within the limits of their capacity?",
            "Is shelving of sufficient capacity for the items being stored on it?",
            "Is stored material stable and secure?",
            "Are heavy items stored between waist and below shoulder level on shelves?",
            "Are storage areas free from tipping hazards? ",
            "Are trolleys, carts or dollies available to move heavy items? ",
            "Is the storage space free of clutter and not congested?",
            "Are flammable liquids properly stored?",
            "Are gas cylinders properly secured?",
            "Does your storage layout minimize lifting problems?"
            };
        }
        private string[] GetElectricalList()
        {
            return new string[]
            {
                "Are electrical cords in good condition?",
                "Are extension cords out of the aisles/walkways where they can be abused?",
                "Is electrical wiring properly concealed?",
                "Are electrical outlets in good condition and not overloaded?",
                "Is there clear access (36\" clearance) to electrical panels and switch gear?",
                "Are electrical panels and fuse panels clearly labelled?",
                "Are proper plugs used?",
                "Are plugs, sockets, and switches in good condition?",
                "Are ground fault circuit interrupters available, if required ? "
            };
        }
        
        private string[] GetEmergencyList()
        {
            return new string[] {
                "Are exits and exit routes accessible (e.g., no items stored in the pathway or doorway)?",
                "Are (emergency) exits signs visible?",
                "Are fire extinguishers clearly marked?",
                "Are fire extinguishers properly installed on walls?",
                "Are extinguishers easily accessible? Not obstructed?",
                "Have extinguishers been inspected in the last 12 months?",
                "Are materials stacked away from sprinkler heads on ceiling?",
                "Are spill kits, first aid kits, telephones, pull stations available?",
                "Are first-aid supplies replenished as they are used?",
                "Is the AED unit ready with the indicator green?",
                "Are eyewash stations regularly inspected, clean and clear?",
                "Are eyes wash stations readily accessible?",
                "Are there proper signs for the locations of eyewash stations?",
                "Is the red emergency response signage posted and visible to all workers, students and visitors?"
            };
        }

        private string[] GetHazardousList() {
            return new string[] {
                "Has the safety data sheet (SDS) been reviewed before handling, moving, or storing the product?",
                "Is the storage area and products organized to keep incompatible products separated?",
                "Is appropriate PPE used for the products?",
                "Are chemical containers clearly labelled with manufacturer or workplace labels? ",
                "Are the containers in good condition free of loose seals or cracks? ",
                "Are hazardous products stored away from heat sources? ",
                "Are all products labelled, and are missing or damaged labels replaced immediately? ",
                "Is the ventilation for the storage area adequate for the products stored within",
                "Has WHMIS training been completed? ",
                "Are spill containment measures in place? ",
                "Are hazardous materials disposed of properly through CHEMATIX? "
            };
        }

        private string[] GetPPEList()
        {
            return new string[] {
                "PPE: Are safety goggles, glasses with side shields, face shields, appropriate foorwear, Respirators, gloves etc. available and in use?",
                "Are workers/students aware of PPE requirement for tasks?",
                "Do they know where to get the PPE ?",
                "Do they know how to use the PPE ?"
            };
        }
        private string[] GetGeneralQuestionsWorkers()
        {
            return new string[] {
               "Do they know what to do in case of an emergency?",
                "Do they know the locations of the nearest fire extinguisher, emergency pull station, and exit path?",
                "Do all workers/ students know how to get first aid assistance when needed?",
                 "Do they know their obligation to report incidents of all types including nearmisses?",
                "Do they understand their right to refuse unsafe work?",
                "Do they know the procedure for working alone?",
                "Do the know what to do in the event of violence/harassment?",
                "Do they know who their supervisor is or who to talk to regarding safety concerns?"
            };
        }

        private string[] GetGeneralQuestionsManagement()
        {
            return new string[] {
                "Are Maintenance issues raised, tracked, and followed up if not closed out timely?",
                "Are all corrective actions from the previous inspections closed out?",
                "Do supervisors know their obligation toward FoA Health and Safety Committee?",
                "Are pre - use inspections being performed on equipment ?"
            };
        }
    }
}
