using Catfish.Core.Authorization.Requirements;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Contents.Workflow;
using Catfish.Core.Services;
using Catfish.Test.Helpers;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Google.Apis.Drive.v3.Data;
using System.Threading.Tasks;
using System.Threading;
using Google.Apis.Util.Store;

namespace Catfish.UnitTests
{
    public class IGResearchDirectoryTest
    {
        private protected AppDbContext _db;
        private protected TestHelper _testHelper;
        string[] YesNoOptionsText = new string[] { "Yes", "No" };
        string _templateName = "IG Research Directory Submission Form Template";
        string _metadataSetName = "IG Research Directory Submission Metadata";
        string lang = "en";
        string _apiKey = "";

        public readonly Guid FORM_ID = Guid.Parse("49a7a1d3-0194-4703-b3d8-747acbf3bbfa");
        public readonly Guid NAME_ID = Guid.Parse("04e54156-f480-41e6-a76a-210456b66499");
        public readonly Guid EMAIL_ID = Guid.Parse("4fa650bc-a20b-4a39-b7d6-40728f2461dd");
        public readonly Guid PRONOUNES_ID = Guid.Parse("69d866ae-94bc-4701-ab72-998a4d8bc070");
        public readonly Guid SHOW_PRONOUNES_ID = Guid.Parse("65409507-a783-4823-a442-9cbceb98ef14");
        public readonly Guid POSITION_ID = Guid.Parse("016caaba-a0c7-4acc-b0c2-813376b2f32c");
        public readonly Guid SHOW_POSITION_ID = Guid.Parse("039767df-0849-4557-a917-3b97e4b095dc");
        public readonly Guid ORGANIZATION_ID = Guid.Parse("98b5b06d-b4e8-4a48-ad2c-7a36b9857064");
        public readonly Guid DISABILITY_ID = Guid.Parse("b7f7b8f0-f2c9-422e-a91d-217fa4c5da8d");
        public readonly Guid SHOW_DISABILITY_ID = Guid.Parse("76a5b848-4512-4ca4-9d2e-3affdf6efc6b");
        public readonly Guid RACE_ID = Guid.Parse("6d4efd13-6b63-4659-ba6f-0e796d50fac0");
        public readonly Guid SHOW_RACE_ID = Guid.Parse("4861c888-3d75-48c2-9de1-2388fd7dfb0e");
        public readonly Guid ETHNICITY_ID = Guid.Parse("b2b63066-a17e-45dd-a3ae-a6c1f815de7b");
        public readonly Guid SHOW_ETHNICITY_ID = Guid.Parse("b3034d56-319b-426e-9850-4536db9f503f");
        public readonly Guid GENDER_IDENTITY_ID = Guid.Parse("deffa707-cd34-4f2b-9fcd-2e8c5161f5c9");
        public readonly Guid SHOW_GENDER_IDENTITY_ID = Guid.Parse("69202d56-96ce-4db8-a8b3-a9b7f0b034fd");
        public readonly Guid KEYWORDS_ID = Guid.Parse("b3efe807-0c6c-4b81-ae89-bdc6ef6f5d6e");
        public readonly Guid ADDITIONAL_KEYWORDS_ID = Guid.Parse("fc0ca69f-21a6-406a-94ca-2b122d6d8d34");
        public readonly Guid RESEARCH_QUESTION_ID = Guid.Parse("0774a8bf-bcf0-4733-8480-85a3b9e1451e");
        public readonly Guid COMMUNITY_BASED_PROJECTS_ID = Guid.Parse("f3964ff7-9c8b-4286-a8ab-8020d9fc421a");
        public readonly Guid EXTERNAL_LINKS_ID = Guid.Parse("44f39fe7-f3af-47da-a2b6-f610c2f0eda3");
        public readonly Guid SHOW_EXTERNAL_LINKS_ID = Guid.Parse("7f1d3c61-0285-4aab-94e5-4a65aa5366bb");
        public readonly Guid COLLABORATORS_ID = Guid.Parse("afb2a6b5-e6ba-406c-90b3-7c10bf4faa73");
        public readonly Guid IMAGE_ID = Guid.Parse("962b6d6d-e397-4f24-8096-ed1655880af2");
        public readonly Guid CONSENT_ID = Guid.Parse("84f22416-34ac-4451-bc59-e403bcd684fa");

        [SetUp]
        public void Setup()
        {
            _testHelper = new TestHelper();
            _db = _testHelper.Db;
            _apiKey = _testHelper.Configuration.GetSection("GoogleApiKey").Value;
        }


        [Test]
        public void IGRD_SubmissionFormTest()
        {
            bool saveChangesToDatabase = true;

            //string lang = "en";
            string templateName = "IG Research Directory Submission Form Template";
            // string _metadatsetName = "IG Research Directory Submission Metadata";

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

            //Defininig the submission form
            DataItem rdForm = template.GetDataItem(templateName, true, lang);
            rdForm.IsRoot = true;
            rdForm.Id = FORM_ID;
            rdForm.SetDescription("This template is designed for IG Research Directory Submission Form", lang);


            rdForm.CreateField<InfoSection>(null, null)
                 .AppendContent("div", @"Protection of Privacy - Personal information provided is collected in accordance with Section 33(c) of the 
               Alberta Freedom of Information and Protection of Privacy Act (the FOIP Act) and will be protected under Part 2 of that Act. 
It will be used for the purpose of the Intersections of Gender Researcher Directory. <i>Information collected will be used to administer and manage the Gender Researcher Directory. 
Information will be used to highlight and mobilize intersectional research, for statistical reporting, and to identify and support equity seeking groups.  
Any public disclosures of information from the directory will be in aggregate form only.</i><br/><br/>Should you require further information about collection, use and disclosure of personal information, please contact  intersectionsofgender@ualberta.ca.
", lang, "alert alert-info");
            //Fields identified by * are mandatory
            rdForm.CreateField<InfoSection>(null, null)
                .AppendContent("div", @"Fields identified by <span style='color: Red;'>*</span> are mandatory", lang, "alert alert-warning");
            rdForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h3", @"Section 1: Contact Information", lang, "alert alert-info");
            var applicantEmail = rdForm.CreateField<EmailField>("Email address", lang, true);
            applicantEmail.IsListEntryTitle = true;
            applicantEmail.Id = EMAIL_ID;

            var name = rdForm.CreateField<TextField>("Name (First and Last)", lang, true);
            name.Id = NAME_ID;
            name.IsListEntryTitle = true;

            RadioField publicShow;

            string[] pronounsList = new string[] { "they/them", "she/her", "he/him", "Would rather not say", "Another" };
            var pronouns = rdForm.CreateField<CheckboxField>("Pronouns", lang, pronounsList, true);
            pronouns.Id = PRONOUNES_ID;
            pronouns.CssClass = "pronounsMultiCheck";
            pronouns.Options.Last().ExtendedOption = true;
            pronouns.SolrFieldType = eSolrFieldType._ss;
            (publicShow = rdForm.CreateField<RadioField>("Show pronounce on my public profile", lang, new String[] {"Yes", "No"}, true)).Required = true;
            publicShow.Id = SHOW_PRONOUNES_ID;

            string[] positionList = new string[] { "Assistant Professor", "Assistant Clinical Professor", "Associate Professor", "Professor", "Academic Teaching Staff", "Professor Emerit/a/us", "Retired",
                "Faculty Member", "Postdoctoral Fellow", "Graduate Student", "Research Assistant", "Another" };
            //rdForm.CreateField<TextField>("Position", lang, true);
            var position = rdForm.CreateField<CheckboxField>("Position", lang, positionList, true);
            position.Id = POSITION_ID;
            position.CssClass = "positionMultiCheck";
            position.Options.Last().ExtendedOption = true;
            position.SolrFieldType = eSolrFieldType._ss;
            (publicShow = rdForm.CreateField<RadioField>("Show position on my public profile", lang, new String[] { "Yes", "No" }, true)).Required = true;
            publicShow.Id = SHOW_POSITION_ID;

            rdForm.CreateField<TextField>("Faculty/Department/Organization", lang, true).Id = ORGANIZATION_ID;


            //////////////////////////////////////                         SECTION 2    ////////////////////////////////////////////////////////////////////////////////
            ///
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            rdForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h3", "Section 2: Self-identification", lang, "alert alert-info");

            rdForm.CreateField<InfoSection>(null, null)
                 .AppendContent("div", @"This information will be used to identify equity-seeking groups in order for IG to highlight, support, and mobilize their intersectional research. This information will remain private unless “display this on my public profile” is checked for each identity category. Your completed profile is important as it helps all researchers see themselves in the fabric of the University of Alberta community. We realize self-identification is a complex matter and that multiple categories may be selected and/or that a more thorough self-identification may be provided in the “Another'' field - we welcome feedback on this process. 
", lang, "alert alert-info");

            string[] disabilitiesList = new string[] { "Deaf", "Neurodivergent", "Experiencing disability", "Not living with a disability", "Another" };
            var disabilities = rdForm.CreateField<CheckboxField>("Living with disability", lang, disabilitiesList, true);
            disabilities.Id = DISABILITY_ID;
            disabilities.CssClass = "disabilitiesMultiCheck";
            disabilities.Options.Last().ExtendedOption = true;
            disabilities.SolrFieldType = eSolrFieldType._ss;
            (publicShow = rdForm.CreateField<RadioField>("Show disability conditions on my public profile", lang, new String[] { "Yes", "No" }, true)).Required = true;
            publicShow.Id = SHOW_DISABILITY_ID;

            string[] raceList = new string[] { "Indigenous", "Black", "Person of colour", "White", "Another" };
            var race = rdForm.CreateField<CheckboxField>("Race", lang, raceList, true);
            race.Id = RACE_ID;
            race.CssClass = "raceMultiCheck";
            race.Options.Last().ExtendedOption = true;
            race.SolrFieldType = eSolrFieldType._ss;
            (publicShow = rdForm.CreateField<RadioField>("Show race on my public profile", lang, new String[] { "Yes", "No" }, true)).Required = true;
            publicShow.Id = SHOW_RACE_ID;

            rdForm.CreateField<TextField>("Ethnicity", lang).Id = ETHNICITY_ID;
            (publicShow = rdForm.CreateField<RadioField>("Show ethnicity on my public profile", lang, new String[] { "Yes", "No" }, true)).Required = true;
            publicShow.Id = SHOW_ETHNICITY_ID;

            string[] genderList = new string[] { "Two-Spirit", "Gender non-binary", "Genderfluid", "Transgender", "Woman", "Man", "Another" };
            var gender = rdForm.CreateField<CheckboxField>("Gender identity", lang, genderList, true);
            gender.Id = GENDER_IDENTITY_ID;
            gender.CssClass = "genderMultiCheck";
            gender.Options.Last().ExtendedOption = true;
            gender.SolrFieldType = eSolrFieldType._ss;
            (publicShow = rdForm.CreateField<RadioField>("Show gender identity on my public profile", lang, new String[] { "Yes", "No" }, true)).Required = true;
            publicShow.Id = SHOW_GENDER_IDENTITY_ID;



            //////////////////////////////////////                         SECTION 3    ////////////////////////////////////////////////////////////////////////////////
            ///
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            rdForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h3", "Section 3: Keywords", lang, "alert alert-info");
         
            string[] keywords = GetKeywords();
            var definedkeys = rdForm.CreateField<CheckboxField>("Identify keywords that are related to your research area", lang,
                 keywords, true);
            definedkeys.Id = KEYWORDS_ID;
            definedkeys.CssClass = "multiSelectKeywords";
            definedkeys.SolrFieldType = eSolrFieldType._ss;

            var undefinedKeys = rdForm.CreateField<TextField>("Please add keywords that are specific to your research area not already identified above.", lang, false);
            undefinedKeys.CssClass = "undefinedKeys";
            undefinedKeys.Id = ADDITIONAL_KEYWORDS_ID;


            //////////////////////////////////////                         SECTION 4    ////////////////////////////////////////////////////////////////////////////////
            ///
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            rdForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h3", "Section 4: Research Area and Community Involvement ", lang, "alert alert-info");

            var researchDes = rdForm.CreateField<TextArea>("Provide your research question or description in under 50 words. Please indicate how your work relates to IG.", lang, true);
            researchDes.Id = RESEARCH_QUESTION_ID;
            researchDes.Cols = 50;
            researchDes.Rows = 2;

            var commProj = rdForm.CreateField<TextArea>("What, if any,  community-based projects are you involved in? This could include activist work, community-based research and engagement, and/or volunteerism related to your research.", lang);
            commProj.Id = COMMUNITY_BASED_PROJECTS_ID;
            commProj.SetDescription("Max text length 100 words.", lang);
            commProj.Cols = 50;
            commProj.Rows = 2;

            rdForm.CreateField<TextField>("Please provide links to your work. We hope directory users can learn more about your research and community work.", lang, true)
                .SetDescription(@"Examples: Websites/blogs, social media pages/accounts (FB, IG, Twitter, tumblr, etc.) <br/>
                             Publications/reports or other digital content relevant to your work (google scholar, Academia.edu). 
                             Digital media (radio, podcast)", lang)
                .Id = EXTERNAL_LINKS_ID;
            (publicShow = rdForm.CreateField<RadioField>("Show links to my work on my public profile", lang, new String[] {"Yes", "No"}, true)).Required = true;
            publicShow.Id = SHOW_EXTERNAL_LINKS_ID;

            //////////////////////////////////////                         SECTION 5    ////////////////////////////////////////////////////////////////////////////////
            ///
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            rdForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h3", "Section 5: Collaboration", lang, "alert alert-info")
                 .AppendContent("p", "Are you currently collaborating with researchers at the U of A? If so, please use the search box on the homepage to see if they’re already in our database. If you cannot find their names, please fill in their name(s) in the form field", lang);

            var colaborator = rdForm.CreateField<TextField>("Collaborators", lang);
            colaborator.Id = COLLABORATORS_ID;


            //////////////////////////////////////                         SECTION 6    ////////////////////////////////////////////////////////////////////////////////
            ///
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            rdForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h3", "Section 6: Image upload", lang, "alert alert-info");
            var imgField = rdForm.CreateField<AttachmentField>("Please upload an image - it can be a headshot, avatar, or an image that is representative of your work.", lang)
              .SetDescription(@"(JPEG or PNG only, 300X300 dpi). If no image is uploaded, we will use an IG Silhouette", lang) as AttachmentField;
            imgField.Id = IMAGE_ID;


            //////////////////////////////////////                         SECTION 7    ////////////////////////////////////////////////////////////////////////////////
            ///
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            rdForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h3", "Section 7: Electronic waiver", lang, "alert alert-info");
            var consent = rdForm.CreateField<RadioField>("Do we have your consent for your researcher profile to be shared on this public directory? (This excludes self-identification information provided above.)", lang, new String[] { "Yes", "No" }, true);
            consent.CssClass = "radio-inline";
            consent.Required = true;
            consent.Id = CONSENT_ID;
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //

            Define_IGRD_ResourcesForumWorkflow(workflow, ref template, rdForm, applicantEmail, null);

            InitFieldAggregations(template);

            if (saveChangesToDatabase)
                db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\IGRD_Form_generared_new.xml");

            //string json = JsonConvert.SerializeObject(template);
            //File.WriteAllText("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.json", json);
        }


        private EmailTemplate CreateApplicantEmailTemplate(ref ItemTemplate template, string formName = null)
        {
            string lang = "en";
            EmailTemplate applicantNotification = template.GetEmailTemplate("Applicant Notification", lang, true);
            applicantNotification.SetDescription("This metadata set defines the email template to be sent to the applicant.", lang);
            string body = @"<p>Thank you very much for your submission to IG Research Directory</p>   
                             <br/><p> Kind regards,</p>
                             <p>T</p> ";
            string subject = "IG Research Directory Submission";
            //if (!string.IsNullOrEmpty(formName) && formName.Equals("SubmitResource"))
            //{
            //    body = @"<p>Thank you very much for your resource(s) suggestion. We will review it and add to our collection.</p>   
            //                 <br/><p> Kind regards,</p>
            //                 <p>The leadership team</p>";
            //    subject = "Submit Resource(s)";
            //}


            applicantNotification.SetSubject(subject);
            applicantNotification.SetBody(body);

            return applicantNotification;

        }

        private EmailTemplate CreateEditorEmailTemplate(ref ItemTemplate template, string formName = null)
        {
            string lang = "en";
            EmailTemplate applicantNotification = template.GetEmailTemplate("Admin Notification", lang, true);
            applicantNotification.SetDescription("This metadata set defines the email template to be sent to the portal admin.", lang);

            string body = "<p>A user has submit to th eIG Research Directoty.</p>";
            string subject = "IGRD submission";
            //if (!string.IsNullOrEmpty(formName) && formName.Equals("SubmitResource"))
            //{
            //    body = "<p>Resources have been suggested and are awaiting your approval.</p>";
            //    subject = "Submit Resource(s)";
            //}

            applicantNotification.SetSubject(subject);
            applicantNotification.SetBody(body);

            return applicantNotification;

        }
        private void Define_IGRD_ResourcesForumWorkflow(Workflow workflow, ref ItemTemplate template, DataItem tbltForm, EmailField applicantEmail = null, string formName = null)
        {
            IWorkflowService ws = _testHelper.WorkflowService;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            State emptyState = workflow.AddState(ws.GetStatus(template.Id, "", true));
            State submittedState = workflow.AddState(ws.GetStatus(template.Id, "Submitted", true));
            State approvedState = workflow.AddState(ws.GetStatus(template.Id, "Approved", true));
            State deleteState = workflow.AddState(ws.GetStatus(template.Id, "Deleted", true));

            WorkflowRole adminRole = workflow.AddRole(auth.GetRole("Admin", true));
            WorkflowRole memberRole = workflow.AddRole(auth.GetRole("Member", true));

            //============================================================================
            //                                 EMAIL 
            //==============================================================================

            EmailTemplate applicantEmailTemplate = CreateApplicantEmailTemplate(ref template, formName);
            EmailTrigger applicantNotificationEmailTrigger = workflow.AddTrigger("ToApplicant", "SendEmail");
            if (applicantEmail != null)
            {

                applicantNotificationEmailTrigger.AddRecipientByDataField(tbltForm.Id, applicantEmail.Id);
                applicantNotificationEmailTrigger.AddTemplate(applicantEmailTemplate.Id, "Submission to  IGRD Notification");
            }
            EmailTemplate adminEmailTemplate = CreateEditorEmailTemplate(ref template, formName);

            EmailTrigger adminNotificationEmailTrigger = workflow.AddTrigger("ToAdmin", "SendEmail");
            adminNotificationEmailTrigger.AddRecipientByEmail("arcrcg@ualberta.ca"); //////////////////////////////NEED TO REPLACE!!!!
            adminNotificationEmailTrigger.AddTemplate(adminEmailTemplate.Id, "Submission to  IGRD Notification");

            // =======================================
            // start submission related workflow items
            // =======================================

            //Defining actions
            GetAction startSubmissionAction = workflow.AddAction("Start Submission", nameof(TemplateOperations.Instantiate), "Home");

            startSubmissionAction.Access = GetAction.eAccess.Public;

            //Defining form template
            startSubmissionAction.AddTemplate(tbltForm.Id, "IG Research Directory Submission Form");

            ////Defining post actions

            PostAction submitPostAction = startSubmissionAction.AddPostAction("Submit", nameof(TemplateOperations.Update),
                                                                                 @"<p>Thank you for submitting your resource to the Task-based Language Teaching resource collection. 
                                                                                    We will review and add it to the  <a href='@SiteUrl/resources'> resources collection </a>.</p>");
            submitPostAction.AddStateMapping(emptyState.Id, submittedState.Id, "Submit");

            //Defining the pop-up for the above postActionSubmit action
            PopUp submitActionPopUp = submitPostAction.AddPopUp("Confirm Submission", "Do you want to submit this resource to the resource collection? Once submitted, you cannot edit it.", "");
            submitActionPopUp.AddButtons("Yes, submit", "true");
            submitActionPopUp.AddButtons("Cancel", "false");

            //Defining trigger refs
            submitPostAction.AddTriggerRefs("0", adminNotificationEmailTrigger.Id, "Editor's Notification Email Trigger");
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

            // Added state referances. The public should be able to list
            // the submissions in the submitted state.
            // Added state referances
            listSubmissionsAction.AddStateReferances(submittedState.Id)
                .AddAuthorizedRole(adminRole.Id)
                .AddOwnerAuthorization();
            listSubmissionsAction.AddStateReferances(approvedState.Id)
                .AddAuthorizedRole(adminRole.Id)
                .AddOwnerAuthorization();
            listSubmissionsAction.AddStateReferances(deleteState.Id)
                .AddAuthorizedRole(adminRole.Id);

            // ================================================
            // Read submission-instances related workflow items
            // ================================================

            //Defining actions
            GetAction viewDetailsSubmissionAction = workflow.AddAction("Details", nameof(TemplateOperations.Read), "List");

            viewDetailsSubmissionAction.Access = GetAction.eAccess.Restricted;

            listSubmissionsAction.AddStateReferances(submittedState.Id)
                .AddAuthorizedRole(adminRole.Id)
                .AddOwnerAuthorization();
            listSubmissionsAction.AddStateReferances(approvedState.Id)
                .AddAuthorizedRole(adminRole.Id)
                .AddOwnerAuthorization();
            listSubmissionsAction.AddStateReferances(deleteState.Id)
                .AddAuthorizedRole(adminRole.Id);


            // ================================================
            // Edit submission-instances related workflow items
            // ================================================
            GetAction editSubmissionAction = workflow.AddAction("Edit Submission", nameof(TemplateOperations.Update), "Details");
            editSubmissionAction.Access = GetAction.eAccess.Restricted;

            PostAction editSubmissionPostActionSubmit = editSubmissionAction.AddPostAction("Submit",
                                                                                            nameof(TemplateOperations.Update),
                                                                                             @"<p>Thank you for submitting to IG Research Directory. 
                                                                                                
                                                                                             You can view your application and it's status by <a href='@SiteUrl/items/@Item.Id'> click on here. </a></p>");
            //Defining state mappings
            editSubmissionPostActionSubmit.AddStateMapping(submittedState.Id, submittedState.Id, "Submit");

            //Defining the pop-up for the above postActionSubmit action
            PopUp EditSubmissionActionPopUpopUp = editSubmissionPostActionSubmit.AddPopUp("Confirmation", "Do you really want to submit this document?", "Once submitted, you cannot update the document.");
            EditSubmissionActionPopUpopUp.AddButtons("Yes, submit", "true");
            EditSubmissionActionPopUpopUp.AddButtons("Cancel", "false");

            //Defining trigger refs
            //*******To Do*******
            // Implement a function to restrict the e-mail triggers when SAS Admin updated the document
            editSubmissionPostActionSubmit.AddTriggerRefs("0", adminNotificationEmailTrigger.Id, "Admin's Notification Email Trigger");
            if (applicantEmail != null)
            {
                editSubmissionPostActionSubmit.AddTriggerRefs("1", applicantNotificationEmailTrigger.Id, "Owner Submission-notification Email Trigger");
            }

            //Defining state referances
            listSubmissionsAction.AddStateReferances(submittedState.Id)
                .AddAuthorizedRole(adminRole.Id)
                .AddOwnerAuthorization();
            listSubmissionsAction.AddStateReferances(approvedState.Id)
                .AddAuthorizedRole(adminRole.Id);
            listSubmissionsAction.AddStateReferances(deleteState.Id)
                .AddAuthorizedRole(adminRole.Id);



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
            deleteSubmissionPostAction.AddStateMapping(approvedState.Id, deleteState.Id, "Delete");

            //Defining the pop-up for the above postAction action
            PopUp deleteSubmissionActionPopUpopUp = deleteSubmissionPostAction.AddPopUp("Confirmation", "Do you really want to delete this document?", "Once deleted, you cannot access this document.");
            deleteSubmissionActionPopUpopUp.AddButtons("Yes, delete", "true");
            deleteSubmissionActionPopUpopUp.AddButtons("Cancel", "false");

            //Defining state referances
            ////////deleteSubmissionAction.GetStateReference(savedState.Id, true)
            ////////    .AddOwnerAuthorization();
            deleteSubmissionAction.GetStateReference(submittedState.Id, true)
                .AddAuthorizedRole(adminRole.Id);
            deleteSubmissionAction.GetStateReference(approvedState.Id, true)
                .AddAuthorizedRole(adminRole.Id);

            // ================================================
            // Change State submission-instances related workflow items
            // ================================================

            GetAction changeStateAction = workflow.AddAction("Update Document State", nameof(TemplateOperations.ChangeState), "Details");
            changeStateAction.Access = GetAction.eAccess.Restricted;

            //Define Revision Template
            //changeStateAction.AddTemplate(commentsForm.Id, "Comments");
            //Defining post actions
            PostAction changeStatePostAction = changeStateAction.AddPostAction("Change State", @"<p>Application status changed successfully.");

            changeStatePostAction.AddStateMapping(submittedState.Id, approvedState.Id, "Approve");
            changeStatePostAction.AddStateMapping(deleteState.Id, approvedState.Id, "Approve");

            //Defining the pop-up for the above sendForRevisionSubmissionPostAction action
            PopUp changeStateDecisionPopUpopUp = changeStatePostAction.AddPopUp("Confirmation", "Do you really want to Approve? ", "Once approved, you cannot revise this decision.");
            changeStateDecisionPopUpopUp.AddButtons("Yes", "true");
            changeStateDecisionPopUpopUp.AddButtons("Cancel", "false");

            //Defining states and their authorizatios
            changeStateAction.GetStateReference(submittedState.Id, true)
                .AddAuthorizedRole(adminRole.Id);
            changeStateAction.GetStateReference(deleteState.Id, true)
                .AddAuthorizedRole(adminRole.Id);

            // ================================================
            // Delete Comment related workflow items
            // ================================================

            //GetAction deleteCommentAction = workflow.AddAction("Delete Comment", nameof(TemplateOperations.ChildFormDelete), "Details");
            //deleteCommentAction.Access = GetAction.eAccess.Restricted;


            //PostAction deleteCommentPostAction = deleteCommentAction.AddPostAction("Delete Comment", @"<p>Your Comment deleted successfully. 
            //                                                                    You can view the document by <a href='@SiteUrl/items/@Item.Id'>click on here</a></p>");

            ////Defining the pop-up for the above sendForRevisionSubmissionPostAction action
            //PopUp deleteCommentPopUpopUp = deleteCommentPostAction.AddPopUp("Confirmation", "Do you really want to delete this comment ? ", "Once deleted, you cannot revise this comment.");
            //deleteCommentPopUpopUp.AddButtons("Yes", "true");
            //deleteCommentPopUpopUp.AddButtons("Cancel", "false");

            ////Defining states and their authorizatios
            //deleteCommentAction.GetStateReference(submittedState.Id, true)
            //    .AddAuthorizedRole(adminRole.Id)
            //    .AddOwnerAuthorization();

        }

       
        private string[] GetKeywords()
        {
            return new string[]
            {
                "Activism", "Age", "Black Studies", "Body", "Canada",  "Class", "Colonialism", "Culture", " Decolonization", "Disability",
                "Diversity","Environment", "Ethics", "Family", "Feminism", "Feminist Theory",  "Film", "Gender", "Genderqueer","Government",
                "Health", "History", "Human Rights", "Identity", "Immigration",  "Indigenous", "Inequality", " International", "Intersectionality", "Language", "Law", "Literature", "Marginalized population", "Masculinities", "Media", "Mental health"," Mothering",
                "Pedagogy", "Policy", "Politics", "Qualitative", "Research","Queer", "Quare", "Race", "Relation", "Religion", "Sex", "Sexuality",
                "Science", "Sport", "Social justice", "Transgender", " Two-spirit", "Violence", "Work"

            }
            .Select(kw => kw.Trim())
            .ToArray();
        }

        private string[] GetQuestionsToPublicDisplay()
        {
            return new string[] { "Pronouns", "Position", "Living with disability", "Race", "Ethnicity", "Gender identity", "The links to my work", "None" };
        }

        private void setOptionValues(OptionsField field, string optionsValues, string separator, string defaultOption)
        {
            if (string.IsNullOrEmpty(optionsValues))
                if (string.IsNullOrEmpty(defaultOption))
                    return;
                else
                    optionsValues = defaultOption;

            string[] valuess = optionsValues.Split(separator, StringSplitOptions.RemoveEmptyEntries)
                .Select(str => str.Trim())
                .Where(str => !string.IsNullOrEmpty(str))
                .ToArray();
            foreach (var val in valuess)
            {
                var option = field.Options.FirstOrDefault(opt => opt.OptionText.GetConcatenatedContent("").ToLower() == val.ToLower());
                if (option != null)
                    option.Selected = true;
                else
                {
                    option = field.Options.FirstOrDefault(opt => opt.OptionText.GetConcatenatedContent("").ToLower() == "another");
                    if (option != null)
                    {


                        option.Selected = true;
                        if (string.IsNullOrEmpty(option.ExtendedValue))
                            option.ExtendedValue = val;
                        else
                            option.ExtendedValue = option.ExtendedValue + ";" + val;
                    }
                    else
                        throw new Exception(string.Format("Unknown option value {0} found for the option-field {1}", val, field.Name.GetConcatenatedContent("/")));
                }
            }
        }

        private void SetTextValues(TextField field, string values, string separator)
        {
            if (string.IsNullOrEmpty(values))
                return;

            string[] inputVals = values.Split(";", StringSplitOptions.RemoveEmptyEntries)
                .Select(str => str.Trim())
                .Where(str => !string.IsNullOrEmpty(str))
                .ToArray();

            for (int i = 0; i < inputVals.Length; ++i)
                field.SetValue(inputVals[i], lang, i);
        }

        [Test]
        public void ImportData()
        {
            bool clearCurrentData = true;

            //Set maxEntries to a positive value to limit the maximum number of data entries to be imported.
            int maxEntries = -1;

            // string multiValueSeparator = ";";


            if (clearCurrentData)
            {
                var entities = _db.Items.ToList();
                _db.Entities.RemoveRange(entities);
            }

            Guid primaryCollectionId = Guid.Parse("79e652d7-bc9e-4a96-c76a-e8896825234a");
            Collection primaryCollection = _db.Collections.Where(c => c.Id == primaryCollectionId).FirstOrDefault();
            Assert.IsNotNull(primaryCollection);

            SystemStatus submittedStatus = _db.SystemStatuses.Where(s => s.NormalizedStatus == "SUBMITTED").FirstOrDefault();
            Assert.IsNotNull(submittedStatus);
            //Filling the form
            var template = _db.ItemTemplates.Where(it => it.TemplateName == _templateName).FirstOrDefault();
            Assert.IsNotNull(template);

            DataItem dataItem = template.GetRootDataItem(false); //root data item in the template

            int rowCount = 1;
            bool headerRow = true;
            string[] colHeadings = null;
            foreach (RowData row in ReadGoogleSheet())
            {
                if (row.Values.Count == 0 || string.IsNullOrEmpty(row.Values[0].FormattedValue))
                    continue;

                if (headerRow)
                {
                    colHeadings = row.Values.Select(x => x.FormattedValue).ToArray();
                    headerRow = false;
                    continue;
                }

                string lang = "en";
                //Create a new item
                var item = template.Instantiate<Item>();

                //retrieve and populate metadat fields
                var ms = item.GetMetadataSet(_metadataSetName, false, lang);
                // Assert.IsNotNull(ms);

                DataItem _newDataItem = template.InstantiateDataItem(dataItem.Id); //new DataItem();

                _newDataItem.Created = DateTime.Now;

               
                string displayOnProfile = "";
                displayOnProfile = getDisplayOnProfile(row);


                for(int i=0; i<row.Values.Count; ++i)
                {
                    string colHeading = colHeadings[i];
                    string colValue = row.Values.ElementAt(i).FormattedValue;

                    if (string.IsNullOrEmpty(colValue))
                        continue;

                    if (colHeading == "name")
                    {
                        var field = _newDataItem.Fields.FirstOrDefault(f => f.Id == NAME_ID) as TextField;
                        field.SetValue(colValue, lang);
                    }
                    else if (colHeading == "contact_email")
                    {
                        var field = _newDataItem.Fields.FirstOrDefault(f => f.Id == EMAIL_ID) as EmailField;
                        field.SetValue(colValue, lang);
                    }
                    else if (colHeading == "website")
                    {
                        var field = _newDataItem.Fields.FirstOrDefault(f => f.Id == EXTERNAL_LINKS_ID) as TextField;
                        string[] vals = colValue.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim())
                            .Where(s => !string.IsNullOrWhiteSpace(s))
                            .ToArray();
                        for(int valIndex=0; valIndex < vals.Length; ++valIndex)
                            field.SetValue(vals[valIndex], lang, valIndex);
                    }
                    else if (colHeading == "pronouns")
                    {
                        var field = _newDataItem.Fields.FirstOrDefault(f => f.Id == PRONOUNES_ID) as CheckboxField;
                        setOptionValues(field, colValue, ";", null);
                    }
                    else if (colHeading == "display_1")
                    {
                        var field = _newDataItem.Fields.FirstOrDefault(f => f.Id == SHOW_PRONOUNES_ID) as RadioField;
                        foreach (var val in colValue.Split(" ").Select(str => str.Trim()).Where(str => str.Length > 0))
                            setOptionValues(field, val, ";", "yes");
                    }
                    else if (colHeading == "race")
                    {
                        var field = _newDataItem.Fields.FirstOrDefault(f => f.Id == RACE_ID) as CheckboxField;
                        setOptionValues(field, colValue, ";", null);
                    }
                    else if (colHeading == "display_2")
                    {
                        var field = _newDataItem.Fields.FirstOrDefault(f => f.Id == SHOW_RACE_ID) as RadioField;
                        setOptionValues(field, colValue, ";", "yes");
                    }
                    else if (colHeading == "ethnicity")
                    {
                        var field = _newDataItem.Fields.FirstOrDefault(f => f.Id == ETHNICITY_ID) as TextField;
                        field.SetValue(colValue, lang);
                    }
                    else if (colHeading == "display_3")
                    {
                        var field = _newDataItem.Fields.FirstOrDefault(f => f.Id == SHOW_ETHNICITY_ID) as RadioField;
                        setOptionValues(field, colValue, ";", "yes");
                    }
                    else if (colHeading == "gender_identity")
                    {
                        var field = _newDataItem.Fields.FirstOrDefault(f => f.Id == GENDER_IDENTITY_ID) as CheckboxField;
                        setOptionValues(field, colValue, ";", null);
                    }
                    else if (colHeading == "display_4")
                    {
                        var field = _newDataItem.Fields.FirstOrDefault(f => f.Id == SHOW_GENDER_IDENTITY_ID) as RadioField;
                        setOptionValues(field, colValue, ";", "yes");
                    }
                    else if (colHeading == "living-with-disability")
                    {
                        var field = _newDataItem.Fields.FirstOrDefault(f => f.Id == DISABILITY_ID) as CheckboxField;
                        setOptionValues(field, colValue, ";", null);
                    }
                    else if (colHeading == "display_5")
                    {
                        var field = _newDataItem.Fields.FirstOrDefault(f => f.Id == SHOW_DISABILITY_ID) as RadioField;
                        setOptionValues(field, colValue, ";", "yes");
                    }
                    else if (colHeading == "position")
                    {
                        var x = _newDataItem.Fields.FirstOrDefault(f => f.Id == POSITION_ID);
                        var field = _newDataItem.Fields.FirstOrDefault(f => f.Id == POSITION_ID) as CheckboxField;
                        setOptionValues(field, colValue, ";", null);
                        setOptionValues(
                            _newDataItem.Fields.FirstOrDefault(f => f.Id == SHOW_POSITION_ID) as RadioField,
                            "yes", ";", null
                            );
                    }
                    else if (colHeading == "category")
                    {
                        var keywordsField = _newDataItem.Fields.FirstOrDefault(f => f.Id == KEYWORDS_ID) as CheckboxField;
                        string[] controlledKeywords = keywordsField.Options.Select(opt => opt.OptionText.GetConcatenatedContent("").ToLower()).ToArray();
                        string[] inputKeywords = colValue.Split(";", StringSplitOptions.RemoveEmptyEntries)
                            .Select(str => str.Trim().ToLower())
                            .Where(str => !string.IsNullOrEmpty(str))
                            .ToArray();

                        string[] controlledInputKeywords = inputKeywords.Intersect(controlledKeywords).ToArray();
                        setOptionValues(keywordsField, string.Join(";", controlledInputKeywords), ";", null);

                        var additionalKeywords = inputKeywords.Where(kw => !controlledKeywords.Contains(kw)).ToArray();
                        var addionalKeywordsField = _newDataItem.Fields.FirstOrDefault(f => f.Id == ADDITIONAL_KEYWORDS_ID) as TextField;
                        for (int valIndex = 0; valIndex < additionalKeywords.Length; ++valIndex)
                            addionalKeywordsField.SetValue(additionalKeywords[valIndex], lang, valIndex);
                    }
                    else if (colHeading == "faculty_or_department")
                    {
                        var field = _newDataItem.Fields.FirstOrDefault(f => f.Id == ORGANIZATION_ID) as TextField;
                        string[] vals = colValue.Split(new char[] { ';', ',' }).Select(str => str.Trim()).Where(str => !string.IsNullOrEmpty(str)).ToArray();
                        for(int idx=0; idx< vals.Length; ++idx)
                        {
                            string val = vals[idx];
                            if (val.ToLower() == "faculty of arts") val = "Arts";
                            else if (val.ToLower() == "faculty of education") val = "Education";
                            else if (val.ToLower() == "ales") val = "Agricultural, Life and Environmental Sciences";
                            else if (val.ToLower() == "faculty of nursing") val = "Nursing";

                            vals[idx] = val;
                        }
                        SetTextValues(field, string.Join(";", vals), ";");
                    }
                    else if (colHeading == "primary_area_of_research")
                    {
                        var field = _newDataItem.Fields.FirstOrDefault(f => f.Id == RESEARCH_QUESTION_ID) as TextArea;
                        field.SetValue(colValue, lang);
                    }
                    else if (colHeading == "community-based_projects")
                    {
                        var field = _newDataItem.Fields.FirstOrDefault(f => f.Id == COMMUNITY_BASED_PROJECTS_ID) as TextArea;
                        field.SetValue(colValue, lang);
                    }
                    else if (colHeading == "collaborators")
                    {
                        var field = _newDataItem.Fields.FirstOrDefault(f => f.Id == COLLABORATORS_ID) as TextField;
                        SetTextValues(field, colValue, ";");
                    }
                    else if (colHeading == "profile_visibility_consent")
                    {
                        var field = _newDataItem.Fields.FirstOrDefault(f => f.Id == CONSENT_ID) as RadioField;
                        setOptionValues(field, colValue, ";", "yes");
                    }

                    //////////col headding with "*" represent interested heading
                    ////////if (colHeading.Contains("*") && !string.IsNullOrEmpty(colValue))
                    ////////{
                    ////////    for (int k = 0; k < _newDataItem.Fields.Count; k++)//foreach(var f in dataItem.Fields)
                    ////////    {
                    ////////        var f = _newDataItem.Fields[k];
                    ////////        string fieldLabel = _newDataItem.Fields[k].GetName();
                    ////////        string _colHeading = colHeading.Substring(0, colHeading.Length - 1);

                    ////////        //this will work if the header on the form field and the g sheet are the similiar
                    ////////        if (!string.IsNullOrEmpty(fieldLabel) && (_colHeading.Contains(fieldLabel, StringComparison.OrdinalIgnoreCase) || fieldLabel.Contains(_colHeading, StringComparison.OrdinalIgnoreCase)))
                    ////////        {

                    ////////            if (f.ModelType.Contains("TextField"))
                    ////////            {
                    ////////                //position,faculty, add keywords that are specific to your research area not already identified above, collaborating with researchers
                    ////////                if (_colHeading.Contains("position", StringComparison.OrdinalIgnoreCase))
                    ////////                {
                    ////////                    //split on a comma
                    ////////                    if (!string.IsNullOrEmpty(colValue))
                    ////////                    {
                    ////////                        string[] vals = colValue.Split(",");
                    ////////                        _newDataItem.SetFieldValue<TextField>(fieldLabel, lang, vals, lang, false);
                    ////////                    }


                    ////////                } else if (_colHeading.Contains("add keywords", StringComparison.OrdinalIgnoreCase) || _colHeading.Contains("faculty", StringComparison.OrdinalIgnoreCase) || _colHeading.Contains("collaborating with researchers", StringComparison.OrdinalIgnoreCase))
                    ////////                {
                    ////////                    //split on a semicolons
                    ////////                    if (!string.IsNullOrEmpty(colValue))
                    ////////                    {
                    ////////                        string[] vals = colValue.Split(";");
                    ////////                        _newDataItem.SetFieldValue<TextField>(fieldLabel, lang, vals, lang, false);
                    ////////                    }

                    ////////                }
                    ////////                else {
                    ////////                    _newDataItem.SetFieldValue<TextField>(fieldLabel, lang, colValue, lang, false);
                    ////////                }
                    ////////                break;
                    ////////            }
                    ////////            else if (f.ModelType.Contains("EmailField"))
                    ////////            {

                    ////////                (_newDataItem.Fields[k] as EmailField).SetValue(colValue);

                    ////////                break;
                    ////////            }
                    ////////            else if (f.ModelType.Contains("TextArea"))
                    ////////            {
                    ////////                _newDataItem.SetFieldValue<TextArea>(fieldLabel, lang, colValue, lang, false);
                    ////////                break;

                    ////////            }
                    ////////            else if (f.ModelType.Contains("RadioField"))
                    ////////            {
                    ////////                //dataItem.SetFieldValue<EmailField>(fieldLabel, "en", colValue, "en", false, 0);
                    ////////                string[] vals = colValue.Split(","); //THIS NEED TO BE REDO -- CONSIDERING ALSO SPLIT BY A ";"
                    ////////                foreach (string v in vals)
                    ////////                {

                    ////////                    for (int j = 0; j < (f as RadioField).Options.Count; j++)//foreach(Option op in (f as CheckboxField).Options)
                    ////////                    {
                    ////////                        if (v.Contains((f as RadioField).Options[j].OptionText.GetContent("en"), StringComparison.OrdinalIgnoreCase))
                    ////////                        {
                    ////////                            (_newDataItem.Fields[k] as RadioField).Options[j].SetAttribute("selected", true);
                    ////////                            break;
                    ////////                        }
                    ////////                        if (j == (((f as RadioField).Options.Count) - 1))
                    ////////                        {
                    ////////                            (_newDataItem.Fields[k] as RadioField).Options[j].SetAttribute("selected", true);//select "Another"
                    ////////                        }
                    ////////                    }
                    ////////                }

                    ////////                break;


                    ////////            }
                    ////////            //else if (f.ModelType.Contains("FieldContainerReference"))
                    ////////            else if (fieldLabel == "Identify keywords that are related to your research area")
                    ////////            {
                    ////////                //string[] vals = colValue.Split("-"); //THIS NEED TO BE REDO -- CONSIDERING ALSO SPLIT BY A ";"
                    ////////                                                     //check the checkbox in the metadataset
                    ////////                string[] vals = colValue.Split(new String[2]{ "-", ";" }, StringSplitOptions.RemoveEmptyEntries);
                    ////////                var options = (f as CheckboxField).Options;
                    ////////                foreach(var val in vals)
                    ////////                {
                    ////////                    var opt = options.FirstOrDefault(op => op.OptionText.GetConcatenatedContent("").ToLower() == val.ToLower());
                    ////////                    if (opt != null)
                    ////////                        opt.Selected = true;
                    ////////                }
                    ////////                ////for (int l = 0; l < ms.Fields.Count; l++)// foreach (var fld in ms.Fields)
                    ////////                ////{
                    ////////                ////    var fld = ms.Fields[l];
                    ////////                ////    if (fld.ModelType.Contains("CheckboxField"))
                    ////////                ////    {
                    ////////                ////        foreach (string v in vals)
                    ////////                ////        {

                    ////////                ////            for (int j = 0; j < (fld as CheckboxField).Options.Count; j++)//foreach(Option op in (f as CheckboxField).Options)
                    ////////                ////            {
                    ////////                ////                if (v.Contains((fld as CheckboxField).Options[j].OptionText.GetContent("en"), StringComparison.OrdinalIgnoreCase))
                    ////////                ////                {
                    ////////                ////                    (ms.Fields[l] as CheckboxField).Options[j].SetAttribute("selected", true);
                    ////////                ////                    break;
                    ////////                ////                }
                    ////////                ////            }
                    ////////                ////        }
                    ////////                ////    }
                    ////////                ////}

                    ////////            }
                    ////////            else if (f.ModelType.Contains("CheckboxField"))
                    ////////            {
                    ////////                //dataItem.SetFieldValue<EmailField>(fieldLabel, "en", colValue, "en", false, 0);
                    ////////                string[] vals = colValue.Split(","); //THIS NEED TO BE REDO -- CONSIDERING ALSO SPLIT BY A ";"
                    ////////                if (_colHeading.Contains("display"))
                    ////////                    vals = displayOnProfile.Split(",");

                    ////////                foreach (string v in vals)
                    ////////                {

                    ////////                    for (int j = 0; j < (f as CheckboxField).Options.Count; j++)//foreach(Option op in (f as CheckboxField).Options)
                    ////////                    {

                    ////////                        if (v.Contains((f as CheckboxField).Options[j].OptionText.GetContent("en"), StringComparison.OrdinalIgnoreCase))
                    ////////                        {

                    ////////                            (_newDataItem.Fields[k] as CheckboxField).Options[j].SetAttribute("selected", true);
                    ////////                            break;
                    ////////                        }

                    ////////                        if (j == (((f as CheckboxField).Options.Count) - 1))
                    ////////                        {
                    ////////                            (_newDataItem.Fields[k] as CheckboxField).Options[j].SetAttribute("selected", true);//select "Another"
                    ////////                        }
                    ////////                    }
                    ////////                }

                    ////////                break;
                    ////////            }

                    ////////        }// end if matched field
                    ////////         //_newDataItem.Fields.Add(f);
                    ////////    }//end of each field
                    ////////}// if spread sheet col content wanted


                }//end of each col

                item.DataContainer.Add(_newDataItem);
                item.PrimaryCollectionId = primaryCollection.Id;
                item.StatusId = submittedStatus.Id;
                var group = _db.Groups.FirstOrDefault(gr => gr.Name == "IGRD");
                if (group != null)
                    group = _db.Groups.FirstOrDefault();
                if (group != null)
                    item.GroupId = group.Id;

                _db.Items.Add(item);

                if (maxEntries > 0 && rowCount == maxEntries)
                    break;

                rowCount++;
            }//end of each row

            _db.SaveChanges();
        }

        [Test]
        public void CopyFiles()
        {

            //PLEASE CHANGE THE PATH
            string sourcePath = "C:\\Users\\mruaini\\Downloads\\DATA_Directory_Images_Resized-20220613T212538Z-001\\DATA_Directory_Images_Resized";

            //PLEASE CHANGE THE PATH
            string destinationPath = "C:\\ARCProjects\\Catfish\\Catfish2\\Catfish2-dev\\Catfish\\App_Data\\uploads\\attachments";

            IList<Item> Items = _db.Items.ToList();

            foreach (string f in Directory.GetFiles(sourcePath, "*.jpg"))
            {
                string fName = (f.Substring(sourcePath.Length + 1)).Split(".")[0];
                string originalFName = f.Substring(sourcePath.Length + 1);
                
                foreach (Item item in Items)
                {

                    string itemName = ((item.DataContainer[0].GetInputFields())[1].GetValues()).First().Value;
                    if (!string.IsNullOrEmpty(itemName) && (itemName.Contains(fName) || fName.Contains(itemName)))
                    {
                        
                        var fileLength = new FileInfo(f).Length;
                        //copy file to 
                        
                        string imgFile = Guid.NewGuid() + "_" + originalFName;
                       

                      
                        FileReference fileRef = new FileReference();
                        fileRef.OriginalFileName = originalFName;
                        fileRef.FileName = imgFile;
                        
                        fileRef.ContentType = "image/jpeg";
                        fileRef.Thumbnail = "/assets/images/icons/jpg.png";
                        fileRef.Size = fileLength;
                        fileRef.Created = DateTime.Now;
                       
                       var attachField = item.DataContainer[0].Fields.Where(f => f.ModelType.Contains("Catfish.Core.Models.Contents.Fields.AttachmentField")).FirstOrDefault() as AttachmentField;
                        (attachField as AttachmentField).Files.Add(fileRef);

                        _db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                        System.IO.File.Copy(Path.Combine(sourcePath, originalFName), Path.Combine(destinationPath, imgFile), true);
                        break;

                    }

                }
            }
            _db.SaveChanges();
            Assert.NotNull(true);
        }


        private string getDisplayOnProfile(RowData row)
        {
            string displayOnProfile = "";
            string[] colHeadings = GetColHeaders();
            int c = 0;
            //{"Pronouns", "Position", "Living with disability", "Race", "Ethnicity", "Gender identity", "The links to my work", "None"};
            foreach (var col in row.Values)
            {
                string colHeading = colHeadings[c];
                string colValue = col.FormattedValue;
                //col headding with "*" represent interested heading
                if (colHeading.Contains("display") && !string.IsNullOrEmpty(colValue))
                {
                    if (colHeading.Contains("display*"))
                    {
                        if (colValue.Equals("yes", StringComparison.OrdinalIgnoreCase))
                            displayOnProfile += "Pronouns";
                    }

                    else if (colHeading.Contains("display race"))
                    {
                        if (colValue.Equals("yes", StringComparison.OrdinalIgnoreCase))
                            displayOnProfile += ", Race";
                    }
                    else if (colHeading.Contains("display ethnicity"))
                    {
                        if (colValue.Equals("yes", StringComparison.OrdinalIgnoreCase))
                            displayOnProfile += ", Ethnicity";
                    }
                    else if (colHeading.Contains("display gender"))
                    {
                        if (colValue.Equals("yes", StringComparison.OrdinalIgnoreCase))
                            displayOnProfile += ", Gender identity";
                    }
                    else if (colHeading.Contains("display disability"))
                    {
                        if (colValue.Equals("yes", StringComparison.OrdinalIgnoreCase))
                            displayOnProfile += ", Living with disability";
                    }
                }

                c++;
            }

            return displayOnProfile;
        }

        [Test]
        public void ReIndex()
        {
            bool reindexAll = true;
            _testHelper.SolrBatchService.IndexItems(reindexAll);
        }


        public List<RowData> ReadGoogleSheet()
        {
            //https://docs.google.com/spreadsheets/d/e/2PACX-1vSPTFgPPcCiCngUPXFE8PdsOgxg7Xybq91voXFxHMFd4JpjUIZGLj7U_piRJZV4WZx3YEW31Pln7XV4/pubhtml => this is my own copy
            String spreadsheetId = "1x6CeEfZiZcGxtnmkoluaZ6GjUhSmutf5GjwjI6dMOyw"; // "1m -oYJH-15DbqhE31dznAldB-gz75BJu1XAV5p5WJwxo";//==>google sheet Id
            String ranges = "A1:AC"; // "A2:AC";// read from col A to AI, starting 2rd row

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

        /// <summary>
        /// Copy an existing file.
        /// </summary>
        /// <param name="service">Drive API service instance.</param>
        /// <param name="originFileId">ID of the origin file to copy.</param>
        /// <param name="copyTitle">Title of the copy.</param>
        /// <returns>The copied file, null is returned if an API error occurred</returns>
        //private Google.Apis.Drive.v3.Data.File CopyFile(DriveService service, String originFileId, String copyTitle)
        //{
            
        //    Google.Apis.Drive.v3.Data.File copiedFile = new Google.Apis.Drive.v3.Data.File();
        //    copiedFile.Name = copyTitle;
        //    try
        //    {
        //        return service.Files.Copy(copiedFile, originFileId).Execute();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("An error occurred: " + e.Message);
        //    }
        //    return copiedFile;
        //}

        // private static async Task<Google.Apis.Drive.v3.Data.FileList> GetAllFilesInsideFolder(DriveService service, string folderId)
        //{
        //    string FolderId = folderId;
        //    // Define parameters of request.
        //    FilesResource.ListRequest listRequest = service.Files.List();
        //    listRequest.Corpora = "drive";
        //    listRequest.DriveId = folderId;
        //    listRequest.SupportsAllDrives = true;
        //    listRequest.PageSize = 200;
        //    listRequest.IncludeItemsFromAllDrives = true;
        //   // listRequest.Q = "'" + FolderId + "' in parents and trashed=false";
        //    listRequest.Fields = "nextPageToken, files(*)";

        //    // List files.
        //    Google.Apis.Drive.v3.Data.FileList files = await listRequest.ExecuteAsync();

        //    return files;
        //}

        private string[] GetColHeaders()
        {
            //Mr March 29 2022
            //header on the sheet
            //* marked the column contain that we're interested
            return new string[] {
                "name*",
                "email*",
                "links*",//website
                "pronouns*",
                "display*",//display pronouns
                "race*",
                "display race",
                "ethnicity*",
                "display ethnicity",
                "gender identity*",
                "display gender",
                "living with disability*",
                "display disability",
                "Code",    "Sub Code",
                "position*",
                "Identify keywords*", //category
                "add keywords that are specific to your research area not already identified above*", //copy of category
                "main_disciplines",
                "faculty*",//faculty_or_department
                "primary_area_of_research",
                "description in under 50 words*",//long description
                "accepting_grad_students",
                "rig_affiliate",
                "open_to_contact_from_other_researchers_external_organizations_community_groups_and_nfps_and_other_rig-related_groups_",
                "looking_for_assistance_with_or_collaboration_in",
                "community-based projects*",
                "collaborating with researchers*",//collaborators
                "consent*"
            };

        }

        //private Item SetItem  (DataItem value, Guid entityTemplateId, Guid collectionId, Guid? groupId, Guid stateMappingId, string action, List<IFormFile> files = null, List<string> fileKeys = null)
        //{
        //    try
        //    {
        //        EntityTemplate template = _entityTemplateService.GetTemplate(entityTemplateId);
        //        if (template == null)
        //            throw new Exception("Entity template with ID = " + entityTemplateId + " not found.");

        //        //When we instantantiate an instance from the template, we do not need to clone metadata sets
        //        Item newItem = template.Instantiate<Item>();
        //        Mapping stateMapping = _workflowService.GetStateMappingByStateMappingId(template, stateMappingId);

        //        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //        //MR- June 4 2021:  ===  BUG HERE == IF form set to "Public" it will still required user to login, recisely because( User user = _workflowService.GetLoggedUser();) -- where it try to get login user
        //        // To fix this problem we need to:
        //        // Check if the Initiate function template is set to "PUblic"
        //        // if it's "public" the user info should come from the form
        //        // otherwise the current implementation is fine
        //        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //        //

        //        XmlModelList<GetAction> actions = _entityTemplateService.GetTemplateActions(entityTemplateId);
        //        var instantiateAction = actions.Where(a => a.Function == "Instantiate").FirstOrDefault();
        //        string currUserEmail = "";
        //        Guid currUserId = Guid.Empty;
        //        string currUserName = "";
        //        if (instantiateAction.Access == GetAction.eAccess.Public)
        //        {
        //            //===================== TODO  =============================
        //        }
        //        else
        //        {
        //            //User user = _workflowService.GetLoggedUser();
        //            //currUserEmail = user.Email;
        //            //currUserId = user.Id;
        //            //currUserName = user.UserName;
        //        }
        //        //We always pass on the next state with the state mapping irrespective of whether
        //        //or not there is a "condition"
        //        Guid statusId = stateMapping.Next;
        //        newItem.StatusId = statusId;
        //        newItem.PrimaryCollectionId = collectionId;
        //        newItem.TemplateId = entityTemplateId;
        //        newItem.GroupId = groupId;
        //        newItem.UserEmail = currUserEmail; //user.Email;

        //        DataItem newDataItem = template.InstantiateDataItem((Guid)value.TemplateId);
        //        newDataItem.UpdateFieldValues(value);
        //        //if (files != null && fileKeys != null)
        //        //    AttachFiles(files, fileKeys, newDataItem);
        //        //newItem.UpdateReferencedFieldContainers(value);

        //        newItem.DataContainer.Add(newDataItem);
        //        newDataItem.EntityId = newItem.Id;
        //        newDataItem.OwnerId = currUserId.ToString(); //user.Id.ToString();
        //        newDataItem.OwnerName = currUserName; //user.UserName;

        //        //User user = _workflowService.GetLoggedUser();
        //        var fromState = template.Workflow.States.Where(st => st.Value == "").Select(st => st.Id).FirstOrDefault();

        //        DataItem emptyDataItem = new DataItem();
        //        newItem.AddAuditEntry(currUserId,
        //            emptyDataItem,
        //            fromState,
        //            newItem.StatusId.Value,
        //            action
        //            );

        //        if (groupId.HasValue)
        //            newItem.GroupId = groupId;

        //        return newItem;
        //    }
        //    catch (Exception ex)
        //    {
        //        //_errorLog.Log(new Error(ex));
        //        return null;
        //    }

        //}

        private void InitFieldAggregations(ItemTemplate template)
        {
            MetadataSet aggregator = template.GetFieldAggregatorMetadataSet(true);
            aggregator.Fields.Clear(); //Clear any existing field-aggregation defintons

            #region The "all" aggregator field

            //Adding the "all" field that aggregates all fields in an item instance
            AggregateField allField = new AggregateField() { ContentType = AggregateField.eContetType.text };
            allField.SetName("_all_", "en");
            aggregator.Fields.Add(allField);

            //Aggregating all fields in all data container forms
            foreach (var form in template.DataContainer)
                allField.AppendSources(form, FieldReference.eSourceType.Data);

            //////Aggregating all fields in all instance-specific (i.e. non-template) metadata sets
            ////foreach (var form in template.MetadataSets.Where(ms => ms.IsTemplate == false))
            ////    allField.AppendSources(form, FieldReference.eSourceType.Metadata);

            #endregion

            #region The Keyword agregator field
            //Adding all Keyword fields that needs to be aggregated together for keyword-based search
            AggregateField aggregatedKeywordField = new AggregateField() { ContentType = AggregateField.eContetType.str };
            aggregatedKeywordField.SetName("_keywords_", "en");
            aggregator.Fields.Add(aggregatedKeywordField);
            ////aggregatedKeywordField.AppendSource(Guid.Parse("3f79e805-eeba-4f4d-b96a-3488a307cc88"), Guid.Parse("87bd0681-e9f0-4235-abc3-1b267a9b833f"), FieldReference.eSourceType.Metadata);
            aggregatedKeywordField.AppendSource(FORM_ID, KEYWORDS_ID, FieldReference.eSourceType.Data);
            aggregatedKeywordField.AppendSource(FORM_ID, ADDITIONAL_KEYWORDS_ID, FieldReference.eSourceType.Data, ";");

            #endregion

            #region Similarity Source aggregator field
            AggregateField similaritySourceField = new AggregateField() { ContentType = AggregateField.eContetType.text };
            similaritySourceField.SetName("_similarity_source_", "en");
            aggregator.Fields.Add(similaritySourceField);
            //Keywords
            similaritySourceField.AppendSource(FORM_ID, KEYWORDS_ID, FieldReference.eSourceType.Data);
            //Additional keywords
            similaritySourceField.AppendSource(FORM_ID, ADDITIONAL_KEYWORDS_ID, FieldReference.eSourceType.Data, ";");
            //Disability
            similaritySourceField.AppendSource(FORM_ID, DISABILITY_ID, FieldReference.eSourceType.Data, ";");
            //Race
            similaritySourceField.AppendSource(FORM_ID, RACE_ID, FieldReference.eSourceType.Data, ";");
            //Ethnicity
            similaritySourceField.AppendSource(FORM_ID, ETHNICITY_ID, FieldReference.eSourceType.Data, ";");
            //Gender identity
            similaritySourceField.AppendSource(FORM_ID, GENDER_IDENTITY_ID, FieldReference.eSourceType.Data, ";");

            #endregion
        }

        /// <summary>
        /// This test case reinitializes the field-aggregator metadata set
        /// </summary>
        [Test]
        public void InitFieldAggregations()
        {
            string templateFileName = "..\\..\\..\\..\\Examples\\IGRD_Form_production.xml";
            XElement xElement = XElement.Load(templateFileName);
            ItemTemplate template = new ItemTemplate(xElement);

            InitFieldAggregations(template);

            template.Data.Save(templateFileName);
        }

    }
}
