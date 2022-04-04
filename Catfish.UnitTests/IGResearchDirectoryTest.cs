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
    public class IGResearchDirectoryTest
    {
        private protected AppDbContext _db;
        private protected TestHelper _testHelper;
        string[] YesNoOptionsText = new string[] { "Yes", "No" };
        string _templateName = "IG Research Directory Submission Form Template";
        string _metadataSetName = "IG Research Directory Submission Metadata";
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

            MetadataSet keywordMeta = template.GetMetadataSet(_metadataSetName, true, lang);
            keywordMeta.IsTemplate = false;

            //TO DO !!!!!!
            string[] keywords = GetKeywords();
           keywordMeta.CreateField<CheckboxField>("Keywords", lang,keywords, true);
          
        //    string[] modes = GetDeliveryModes();
        //    keywordMeta.CreateField<CheckboxField>("Mode", lang, modes, true);


            //Defininig the submission form
            DataItem rdForm = template.GetDataItem(templateName, true, lang);
            rdForm.IsRoot = true;
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
                .AppendContent("div", @"Fields identified by <span style='color: Red;'>*</span> are mandatory",lang, "alert alert-warning");
            rdForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h3", @"Section 1: Contact Information", lang, "alert alert-info");
            var applicantEmail = rdForm.CreateField<EmailField>("Email address", lang, true);
            applicantEmail.IsListEntryTitle = true;
            var name = rdForm.CreateField<TextField>("Name (First and Last)", lang, true);
           
            name.IsListEntryTitle = true;
            string[] publicDisplay = new string[] { "Display this on my public profile?" };
            string[] pronounsList = new string[]{"they/them", "she/her", "he/him", "Would rather not say", "Another" };

            var pronouns = rdForm.CreateField<RadioField>("Pronouns", lang,pronounsList, true);
            var pronounAnother = rdForm.CreateField<TextField>("If you select 'Another, please specify", lang);
            pronounAnother.VisibilityCondition
                .AppendLogicalExpression(pronouns, ComputationExpression.eRelational.EQUAL, pronouns.GetOption("Another", lang));
            pronounAnother.RequiredCondition.AppendLogicalExpression(pronouns, ComputationExpression.eRelational.EQUAL, pronouns.GetOption("Another", lang));

            string[] positionList = new string[] { "Full Professor", "Assistant Professor", "Associate Professor", "Faculty Member", "Post-doc", "Graduate Student", "Research Assistant", "Another" };

            var position = rdForm.CreateField<CheckboxField>("Position", lang, positionList, true);
            position.CssClass = "positionMultiCheck"; 
            var posAnother = rdForm.CreateField<TextField>("If you select 'Another', please specify", lang);
          
            posAnother.VisibilityCondition
                  .AppendLogicalExpression(position, position.GetOption("Another", lang),true);
            posAnother.RequiredCondition.AppendLogicalExpression(position, position.GetOption("Another", lang), true);
            string[] options = new string[] { "Yes", "No" };
           // var pubDisplay = rdForm.CreateField<RadioField>("Display this on my public profile?", lang, options, true);
           // pubDisplay.CssClass = "radio-inline";

            rdForm.CreateField<TextField>("Faculty", lang, true);
            rdForm.CreateField<TextField>("Department", lang, true);
            rdForm.CreateField<TextField>("Organization", lang, true);

            //////////////////////////////////////                         SECTION 2    ////////////////////////////////////////////////////////////////////////////////
            ///
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            rdForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h3", "Section 2: Self-identification", lang, "alert alert-info");

            rdForm.CreateField<InfoSection>(null, null)
                 .AppendContent("div", @"This information will be used to identify equity-seeking groups in order for IG to highlight, support, and mobilize their intersectional research. This information will remain private unless “display this on my public profile” is checked for each identity category. Your completed profile is important as it helps all researchers see themselves in the fabric of the University of Alberta community. We realize self-identification is a complex matter and that multiple categories may be selected and/or that a more thorough self-identification may be provided in the “Another'' field - we welcome feedback on this process. 
", lang, "alert alert-info");

            string[] disabilitiesList = new string[] { "Deaf", "Neurodivergent", "Experiencing Disability", "Not living with disability", "Another" };

            var disabilities = rdForm.CreateField<CheckboxField>("Living with disability", lang, disabilitiesList, true);
            disabilities.CssClass = "disabilitiesMultiCheck";
            var disAnother = rdForm.CreateField<TextField>("If you select 'Another', please specify", lang);
           
            disAnother.VisibilityCondition
                .AppendLogicalExpression(disabilities, disabilities.GetOption("Another", lang), true);
            disAnother.RequiredCondition.AppendLogicalExpression(disabilities, disabilities.GetOption("Another", lang), true);

            //pubDisplay = rdForm.CreateField<RadioField>("Display this on my public profile?", lang, options, true);
           // pubDisplay.CssClass = "radio-inline";
            string[] raceList = new string[] { "Indigenous", "Black", "Person of Colour", "White", "Another" };

            var race = rdForm.CreateField<CheckboxField>("Race", lang, raceList, true);
            race.CssClass = "raceMultiCheck";
            var raceAnother = rdForm.CreateField<TextField>("If you select 'Another', please specify", lang);
            raceAnother.VisibilityCondition
            .AppendLogicalExpression(race, race.GetOption("Another", lang), true);
            raceAnother.RequiredCondition.AppendLogicalExpression(race, race.GetOption("Another", lang), true);

           // pubDisplay = rdForm.CreateField<RadioField>("Display this on my public profile?", lang, options, true);
          //  pubDisplay.CssClass = "radio-inline";

            rdForm.CreateField<TextField>("Ethnicity", lang);
            //pubDisplay = rdForm.CreateField<RadioField>("Display this on my public profile?", lang, options);
           // pubDisplay.CssClass = "radio-inline";


            string[] genderList = new string[] { "Two-Spirit", "Gender non-binary", "Genderfluid", "Transgender", "Woman", "Man", "Another" };

            var gender = rdForm.CreateField<RadioField>("Gender identity", lang, genderList, true);
            var genAnother = rdForm.CreateField<TextField>("If you select 'Another', please specify", lang);
            genAnother.VisibilityCondition
                .AppendLogicalExpression(gender, ComputationExpression.eRelational.EQUAL, gender.GetOption("Another", lang));
            genAnother.RequiredCondition.AppendLogicalExpression(gender, ComputationExpression.eRelational.EQUAL, gender.GetOption("Another", lang));

            string[] pubDisplayList = GetQuestionsToPublicDisplay();
            
          

            //////////////////////////////////////                         SECTION 3    ////////////////////////////////////////////////////////////////////////////////
            ///
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            rdForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h3", "Section 3: Keywords", lang, "alert alert-info");
           var definedkeys =  rdForm.CreateField<FieldContainerReference>("Identify keywords that are related to your research area", lang,
                FieldContainerReference.eRefType.metadata, keywordMeta.Id);
            definedkeys.CssClass = "multiSelectKeywords";

            //var key =  rdForm.CreateField<TextField>("Identify keywords that are related to your research area.", lang, true);
            //key.CssClass = "autocompleteText";
            rdForm.CreateField<TextField>("Please add keywords that are specific to your research area not already identified above.", lang, true);



            //////////////////////////////////////                         SECTION 4    ////////////////////////////////////////////////////////////////////////////////
            ///
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            rdForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h3", "Section 4: Research Area and Community Involvement ", lang, "alert alert-info");

            var researchDes=rdForm.CreateField<TextArea>("Provide your research question or description in under 50 words. Please indicate how your work relates to IG.", lang, true);
            researchDes.Cols = 50;
            researchDes.Rows = 2;

            var commProj = rdForm.CreateField<TextArea>("What, if any,  community-based projects are you involved in? This could include activist work, community-based research and engagement, and/or volunteerism related to your research.", lang);
            commProj.SetDescription("Max text length 100 words.", lang);
            commProj.Cols = 50;
            commProj.Rows = 2;

            rdForm.CreateField<TextField>("Please provide links to your work. We hope directory users can learn more about your research and community work.", lang, true)
                .SetDescription(@"Examples: Websites/blogs, social media pages/accounts (FB, IG, Twitter, tumblr, etc.) <br/>
                             Publications/reports or other digital content relevant to your work (google scholar, Academia.edu). 
                             Digital media (radio, podcast)", lang);
            // pubDisplay = rdForm.CreateField<RadioField>("Display this on my public profile?", lang, options, true);
            //  pubDisplay.CssClass = "radio-inline";

            var pubDisplay = rdForm.CreateField<CheckboxField>("Display this on my public profile?", lang, pubDisplayList, true);
            pubDisplay.CssClass = "publicDisplayMultiCheck";


            //////////////////////////////////////                         SECTION 5    ////////////////////////////////////////////////////////////////////////////////
            ///
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            rdForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h3", "Section 5: Collaboration", lang, "alert alert-info");
            var colaborator = rdForm.CreateField<TextField>("Are you currently collaborating with researchers at the U of A? If so, please use the search button to see if they’re already in our database. ", lang)
              .SetDescription(@"If you cannot find their names, please fill in their name(s) in the form field.", lang);
            


            //////////////////////////////////////                         SECTION 6    ////////////////////////////////////////////////////////////////////////////////
            ///
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            rdForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h3", "Section 6: Image upload", lang, "alert alert-info");
            rdForm.CreateField<AttachmentField>("Please upload an image - it can be a headshot, avatar, or an image that is representative of your work.", lang)
              .SetDescription(@"(JPEG or PNG only, 300X300 dpi). If no image is uploaded, we will use an IG Silhouette", lang);


            //////////////////////////////////////                         SECTION 7    ////////////////////////////////////////////////////////////////////////////////
            ///
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            rdForm.CreateField<InfoSection>(null, null)
                 .AppendContent("h3", "Section 7: Electronic waiver", lang, "alert alert-info");
            var consent = rdForm.CreateField<RadioField>("Do we have your consent for your researcher profile to be shared on this public directory? (This excludes self-identification information provided above.)", lang, options, true);
            consent.CssClass = "radio-inline";
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                         Defininig roles                                             //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //
          
            Define_IGRD_ResourcesForumWorkflow(workflow, ref template, rdForm, applicantEmail, null);

            if (saveChangesToDatabase)
                db.SaveChanges();

            template.Data.Save("..\\..\\..\\..\\Examples\\IGRD_Form_generared.xml");

            //string json = JsonConvert.SerializeObject(template);
            //File.WriteAllText("..\\..\\..\\..\\Examples\\covidWeeklyInspectionWorkflow_generared.json", json);
        }


        private EmailTemplate CreateApplicantEmailTemplate(ref ItemTemplate template, string formName=null)
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

        private EmailTemplate CreateEditorEmailTemplate(ref ItemTemplate template, string formName=null)
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
        private void Define_IGRD_ResourcesForumWorkflow(Workflow workflow, ref ItemTemplate template,DataItem tbltForm,EmailField applicantEmail=null, string formName=null)
        {
            IWorkflowService ws = _testHelper.WorkflowService;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            State emptyState = workflow.AddState(ws.GetStatus(template.Id, "", true));
            State savedState = workflow.AddState(ws.GetStatus(template.Id, "Saved", true));
            State submittedState = workflow.AddState(ws.GetStatus(template.Id, "Submitted", true));
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
            adminNotificationEmailTrigger.AddRecipientByEmail("mruaini@ualberta.ca"); //////////////////////////////NEED TO REPLACE!!!!
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
            PostAction savePostAction = startSubmissionAction.AddPostAction("Save", nameof(TemplateOperations.Update),
                                                                            @"<p>Thank you for saving your resource to the Task-based Language Teaching resource collection. 
                                                                                    Your submission should be view/edit at the <a href='@SiteUrl/resources'>resources page.</a></p>");
            savePostAction.ValidateInputs = false;
            savePostAction.AddStateMapping(emptyState.Id, savedState.Id, "Save");

            PostAction submitPostAction = startSubmissionAction.AddPostAction("Submit", nameof(TemplateOperations.Update),
                                                                                 @"<p>Thank you for submitting your resource to the Task-based Language Teaching resource collection. 
                                                                                    We will review and add it to the  <a href='@SiteUrl/resources'> resources collection </a>.</p>");
            submitPostAction.AddStateMapping(emptyState.Id, submittedState.Id, "Submit");
            submitPostAction.AddStateMapping(savedState.Id, submittedState.Id, "Submit");

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

            // Added state referances
            listSubmissionsAction.AddStateReferances(submittedState.Id)
                .AddAuthorizedRole(adminRole.Id)
                .AddAuthorizedRole(memberRole.Id)
                .AddOwnerAuthorization();

            // ================================================
            // Read submission-instances related workflow items
            // ================================================

            //Defining actions
            GetAction viewDetailsSubmissionAction = workflow.AddAction("Details", nameof(TemplateOperations.Read), "List");

            viewDetailsSubmissionAction.Access = GetAction.eAccess.Restricted;

            // Added state referances
            viewDetailsSubmissionAction.AddStateReferances(submittedState.Id)
                .AddAuthorizedRole(adminRole.Id)
                .AddAuthorizedRole(memberRole.Id)
                .AddOwnerAuthorization();


            // ================================================
            // Edit submission-instances related workflow items
            // ================================================
            GetAction editSubmissionAction = workflow.AddAction("Edit Submission", nameof(TemplateOperations.Update), "Details");
            editSubmissionAction.Access = GetAction.eAccess.Restricted;

            //Defining post actions
            PostAction editSubmissionPostActionSave = editSubmissionAction.AddPostAction("Save",
                                                                                        nameof(TemplateOperations.Update),
                                                                                        @"<p>Your submission saved successfully. 
                                                                                            You can view/edit by <a href='@SiteUrl/items/@Item.Id'>click on here</a></p>");
            editSubmissionPostActionSave.ValidateInputs = false;
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
                editSubmissionAction.GetStateReference(submittedState.Id, true)
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

            //Defining the pop-up for the above postAction action
            PopUp deleteSubmissionActionPopUpopUp = deleteSubmissionPostAction.AddPopUp("Confirmation", "Do you really want to delete this document?", "Once deleted, you cannot access this document.");
            deleteSubmissionActionPopUpopUp.AddButtons("Yes, delete", "true");
            deleteSubmissionActionPopUpopUp.AddButtons("Cancel", "false");

            //Defining state referances
            ////////deleteSubmissionAction.GetStateReference(savedState.Id, true)
            ////////    .AddOwnerAuthorization();
            deleteSubmissionAction.GetStateReference(submittedState.Id, true)
                .AddAuthorizedRole(adminRole.Id);

            // ================================================
            // Change State submission-instances related workflow items
            // ================================================

           // GetAction changeStateAction = workflow.AddAction("Update Document State", nameof(TemplateOperations.ChangeState), "Details");
           // changeStateAction.Access = GetAction.eAccess.Restricted;

           // //Define Revision Template
           //changeStateAction.AddTemplate(commentsForm.Id, "Comments");
           // //Defining post actions
           // PostAction changeStatePostAction = changeStateAction.AddPostAction("Change State", @"<p>Application status changed successfully. 
           //                                                                     You can view the document by <a href='@SiteUrl/items/@Item.Id'>click on here</a></p>");

            
           // //Defining the pop-up for the above sendForRevisionSubmissionPostAction action
           // PopUp adjudicationDecisionPopUpopUp = changeStatePostAction.AddPopUp("Confirmation", "Do you really want to continue? ", "Once changed, you cannot revise this document.");
           // adjudicationDecisionPopUpopUp.AddButtons("Yes", "true");
           // adjudicationDecisionPopUpopUp.AddButtons("Cancel", "false");

           // //Defining states and their authorizatios
           // changeStateAction.GetStateReference(submittedState.Id, true)
           //     .AddAuthorizedRole(adminRole.Id);

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
       
        private void Define_TBLT_DiscussionWorkflow(Workflow workflow, ref ItemTemplate template, DataItem tbltForm, DataItem commentsForm, string formName = null)
        {
            IWorkflowService ws = _testHelper.WorkflowService;
            IAuthorizationService auth = _testHelper.AuthorizationService;

            State emptyState = workflow.AddState(ws.GetStatus(template.Id, "", true));
            State submittedState = workflow.AddState(ws.GetStatus(template.Id, "Submitted", true));
            State deleteState = workflow.AddState(ws.GetStatus(template.Id, "Deleted", true));

            WorkflowRole editorRole = workflow.AddRole(auth.GetRole("Editor", true));
            WorkflowRole memberRole = workflow.AddRole(auth.GetRole("Member", true));

            //============================================================================
            //                                 EMAIL 
            //==============================================================================

            EmailTemplate applicantEmailTemplate = CreateApplicantEmailTemplate(ref template, formName);
            EmailTrigger applicantNotificationEmailTrigger = workflow.AddTrigger("ToApplicant", "SendEmail");
            applicantNotificationEmailTrigger.AddOwnerAsRecipient();
            applicantNotificationEmailTrigger.AddTemplate(applicantEmailTemplate.Id, "Join TBLT Comment Notification");

            EmailTemplate adminEmailTemplate = CreateEditorEmailTemplate(ref template, formName);

            EmailTrigger editorNotificationEmailTrigger = workflow.AddTrigger("ToEditor", "SendEmail");
            editorNotificationEmailTrigger.AddRecipientByEmail("tblt@ualberta.ca");
            editorNotificationEmailTrigger.AddTemplate(adminEmailTemplate.Id, "Join Comment Notification");

            // =======================================
            // start submission related workflow items
            // =======================================

            //Defining actions
            GetAction startSubmissionAction = workflow.AddAction("Start Submission", nameof(TemplateOperations.Instantiate), "Home");

            startSubmissionAction.Access = GetAction.eAccess.Restricted;

            //Defining form template
            startSubmissionAction.AddTemplate(tbltForm.Id, "Task-based Language Teaching Comment Form");

            //Defining post actions
            PostAction submitPostAction = startSubmissionAction.AddPostAction("Submit", nameof(TemplateOperations.Update),
                                                                                 @"<p>Thank you for submitting your post to the Task-based Language Teaching discussion forum. 
                                                                                    Your post should be visible at the <a href='@SiteUrl/discussion-forum'> forum page. </a></p>");
            submitPostAction.AddStateMapping(emptyState.Id, submittedState.Id, "Submit");


            //Defining the pop-up for the above postActionSubmit action
            PopUp submitActionPopUp = submitPostAction.AddPopUp("Confirm Submission", "Do you want to submit this post to the discussion forum? Once submitted, you cannot edit it.", "");
            submitActionPopUp.AddButtons("Yes, submit", "true");
            submitActionPopUp.AddButtons("Cancel", "false");

            //Defining trigger refs
            submitPostAction.AddTriggerRefs("0", editorNotificationEmailTrigger.Id, "Editor's Notification Email Trigger");
            submitPostAction.AddTriggerRefs("1", applicantNotificationEmailTrigger.Id, "Owner Submission-notification Email Trigger");

            startSubmissionAction.AddStateReferances(emptyState.Id)
                .AddAuthorizedRole(memberRole.Id);


            // ================================================
            // List submission-instances related workflow items
            // ================================================
            GetAction listSubmissionsAction = workflow.AddAction("List Submissions", nameof(TemplateOperations.ListInstances), "Home");
            listSubmissionsAction.Access = GetAction.eAccess.Restricted;

            // Added state referances
            listSubmissionsAction.AddStateReferances(submittedState.Id)
                .AddAuthorizedRole(editorRole.Id)
                .AddAuthorizedRole(memberRole.Id)
                .AddOwnerAuthorization();

            // ================================================
            // Read submission-instances related workflow items
            // ================================================

            //Defining actions
            GetAction viewDetailsSubmissionAction = workflow.AddAction("Details", nameof(TemplateOperations.Read), "List");

            viewDetailsSubmissionAction.Access = GetAction.eAccess.Restricted;

            // Added state referances
            viewDetailsSubmissionAction.AddStateReferances(submittedState.Id)
                .AddAuthorizedRole(editorRole.Id)
                .AddAuthorizedRole(memberRole.Id)
                .AddOwnerAuthorization();

            // ================================================
            // Edit submission-instances related workflow items
            // ================================================
            GetAction editSubmissionAction = workflow.AddAction("Edit Submission", nameof(TemplateOperations.Update), "Details");
            editSubmissionAction.Access = GetAction.eAccess.Restricted;

            //Defining post actions
            PostAction editSubmissionPostActionSave = editSubmissionAction.AddPostAction("Save",
                                                                                        nameof(TemplateOperations.Update),
                                                                                        @"<p>Your TBLT application saved successfully. 
                                                                                            You can view/edit by <a href='@SiteUrl/items/@Item.Id'>click on here</a></p>");
            editSubmissionPostActionSave.ValidateInputs = false;
            PostAction editSubmissionPostActionSubmit = editSubmissionAction.AddPostAction("Submit",
                                                                                            nameof(TemplateOperations.Update),
                                                                                             @"<p>Thank you for submitting your post to the Task-based Language Teaching discussion forum. 
                                                                                    Your post should be visible at the <a href='@SiteUrl/discussion-forum'> forum page. </a></p>");
            //Defining state mappings
            editSubmissionPostActionSubmit.AddStateMapping(submittedState.Id, submittedState.Id, "Submit");

            //Defining the pop-up for the above postActionSubmit action
            PopUp EditSubmissionActionPopUpopUp = editSubmissionPostActionSubmit.AddPopUp("Confirm Submission", "Do you want to submit this post to the discussion forum? Once submitted, you cannot edit it.", "");
            EditSubmissionActionPopUpopUp.AddButtons("Yes, submit", "true");
            EditSubmissionActionPopUpopUp.AddButtons("Cancel", "false");

            //Defining trigger refs
            //*******To Do*******
            // Implement a function to restrict the e-mail triggers when SAS Admin updated the document
            editSubmissionPostActionSubmit.AddTriggerRefs("0", editorNotificationEmailTrigger.Id, "Editor's Notification Email Trigger");
            editSubmissionPostActionSubmit.AddTriggerRefs("1", applicantNotificationEmailTrigger.Id, "Owner Submission-notification Email Trigger");

            //Defining state referances
            editSubmissionAction.GetStateReference(submittedState.Id, true)
                .AddAuthorizedRole(editorRole.Id);



            // ================================================
            // Delete submission-instances related workflow items
            // ================================================

            GetAction deleteSubmissionAction = workflow.AddAction("Delete Submission", nameof(CrudOperations.Delete), "Details");
            deleteSubmissionAction.Access = GetAction.eAccess.Restricted;

            //Defining post actions
            PostAction deleteSubmissionPostAction = deleteSubmissionAction.AddPostAction("Delete", "Save");
            deleteSubmissionPostAction.ValidateInputs = false;


            //Defining the pop-up for the above postAction action
            PopUp deleteSubmissionActionPopUpopUp = deleteSubmissionPostAction.AddPopUp("Confirmation", "Do you really want to delete this document?", "Once deleted, you cannot access this document.");
            deleteSubmissionActionPopUpopUp.AddButtons("Yes, delete", "true");
            deleteSubmissionActionPopUpopUp.AddButtons("Cancel", "false");


            // ================================================
            // Change State submission-instances related workflow items
            // ================================================

            GetAction changeStateAction = workflow.AddAction("Update Document State", nameof(TemplateOperations.ChangeState), "Details");
            changeStateAction.Access = GetAction.eAccess.Restricted;

            //Define Revision Template
            changeStateAction.AddTemplate(commentsForm.Id, "Comments");
            //Defining post actions
            PostAction changeStatePostAction = changeStateAction.AddPostAction("Change State", @"<p>Application status changed successfully. 
                                                                                You can view the document by <a href='@SiteUrl/items/@Item.Id'>click on here</a></p>");

            //Defining the pop-up for the above sendForRevisionSubmissionPostAction action
            PopUp adjudicationDecisionPopUpopUp = changeStatePostAction.AddPopUp("Confirmation", "Do you really want to make a decision ? ", "Once changed, you cannot revise this document.");
            adjudicationDecisionPopUpopUp.AddButtons("Yes", "true");
            adjudicationDecisionPopUpopUp.AddButtons("Cancel", "false");

            //Defining states and their authorizatios
            changeStateAction.GetStateReference(submittedState.Id, true)
                .AddAuthorizedRole(editorRole.Id);

            // ================================================
            // Delete Comment related workflow items
            // ================================================

            GetAction deleteCommentAction = workflow.AddAction("Delete Comment", nameof(TemplateOperations.ChildFormDelete), "Details");
            deleteCommentAction.Access = GetAction.eAccess.Restricted;

            //Define Revision Template
            //changeStateAction.AddTemplate(commentsForm.Id, "Comments");
            //Defining post actions
            PostAction deleteCommentPostAction = deleteCommentAction.AddPostAction("Delete Comment", @"<p>Your Comment deleted successfully. 
                                                                                You can view the document by <a href='@SiteUrl/items/@Item.Id'>click on here</a></p>");

            //Defining state mappings
            //deleteCommentPostAction.AddStateMapping(submittedState.Id, approvedState.Id, "Approve");
            //deleteCommentPostAction.AddStateMapping(submittedState.Id, rejectedState.Id, "Reject");

            //Defining the pop-up for the above sendForRevisionSubmissionPostAction action
            PopUp deleteCommentPopUpopUp = deleteCommentPostAction.AddPopUp("Confirmation", "Do you really want to delete this comment ? ", "Once deleted, you cannot revise this comment.");
            deleteCommentPopUpopUp.AddButtons("Yes", "true");
            deleteCommentPopUpopUp.AddButtons("Cancel", "false");

            //Defining states and their authorizatios
            deleteCommentAction.GetStateReference(submittedState.Id, true)
                .AddAuthorizedRole(editorRole.Id)
                .AddOwnerAuthorization();

        }

       private string[] GetKeywords()
        {
            return new string[]
            {
                "Activism", "Age", "Black Studies", "Body", "Canada",  "Class", "Colonialism", "Culture", " Decolonization", "Disability",
                "Diversity","Environment", "Ethics", "Family", "Feminism", "Feminist Theory",  "Film", "Gender", "Genderqueer","Government",
                "Health", "History", "Human Rights", "Identity", "Immigration",  "Indigenous", "Inequality", " International", "Intersectionality", "Language", "Law", "Literature", "Marginalized population", "Masculinities", "Media", "Mental health"," Mothering",
                "Pedagogy", "Policy", "Politics", "Qualitative", "Research"," Queer", "Quare", "Race", "Relation", "Religion", "Sex", "Sexuality",
                "Science", "Sport", "Social justice", "Transgender", " Two-spirit", "Violence", "Work"

            };
        }

        private string[] GetQuestionsToPublicDisplay()
        {
            return new string[] {"Pronouns", "Position", "Living with disability", "Race", "Ethnicity", "Gender identity", "The links to my work", "None"};
        }

        [Test]
        public void ImportData()
        {
            bool clearCurrentData = false;

            //Set maxEntries to a positive value to limit the maximum number of data entries to be imported.
            int maxEntries = -1;

           // string multiValueSeparator = ";";


            if (clearCurrentData)
            {
                var entities = _db.Items.ToList();
                _db.Entities.RemoveRange(entities);
            }


            //Filling the form
            var template = _db.ItemTemplates.Where(it => it.TemplateName == _templateName).FirstOrDefault();
            Assert.IsNotNull(template);

            DataItem dataItem = template.GetRootDataItem(false); //root data item in the template

            int rowCount = 1;
            foreach (RowData row in ReadGoogleSheet())
            {
                string lang = "en";
                //Create a new item
                var item = template.Instantiate<Item>();

                //retrieve and populate metadat fields
                var ms = item.GetMetadataSet(_metadataSetName, false, lang);
                // Assert.IsNotNull(ms);

                DataItem _newDataItem = template.InstantiateDataItem(dataItem.Id); //new DataItem();
              
                _newDataItem.Created = DateTime.Now;

                string[] colHeadings = GetColHeaders();

                int i = 0;
                foreach (var col in row.Values)
                {
                   
                   // FieldList fields = new FieldList();
                    

                    string colHeading = colHeadings[i];
                    string colValue = col.FormattedValue;
                    //col headding with "*" represent interested heading
                    if (colHeading.Contains("*") && !string.IsNullOrEmpty(colValue))
                    {
                        for(int k=0; k < _newDataItem.Fields.Count; k++)//foreach(var f in dataItem.Fields)
                        {
                            var f = _newDataItem.Fields[k];
                            string fieldLabel = _newDataItem.Fields[k].GetName();
                            string _colHeading = colHeading.Substring(0, colHeading.Length - 1);
                            //this will work if the header on the form field and the g sheet are the similiar
                            if (!string.IsNullOrEmpty(fieldLabel) && (_colHeading.Contains(fieldLabel, StringComparison.OrdinalIgnoreCase) || fieldLabel.Contains(_colHeading, StringComparison.OrdinalIgnoreCase))) {
             
                                if (f.ModelType.Contains("TextField")) {
                                 
                                    _newDataItem.SetFieldValue<TextField>(fieldLabel, lang, colValue, lang, false);
                                    break;
                                }
                                else if (f.ModelType.Contains("EmailField"))
                                {
                                    
                                    (_newDataItem.Fields[k] as EmailField).SetValue(colValue);
                                   
                                    break;
                                }
                                else if (f.ModelType.Contains("TextArea"))
                                {
                                    _newDataItem.SetFieldValue<TextArea>(fieldLabel, lang, colValue, lang, false);
                                    break;
                                 
                                }
                                else if (f.ModelType.Contains("RadioField"))
                                {
                                    //dataItem.SetFieldValue<EmailField>(fieldLabel, "en", colValue, "en", false, 0);
                                    string[] vals = colValue.Split(","); //THIS NEED TO BE REDO -- CONSIDERING ALSO SPLIT BY A ";"
                                    foreach (string v in vals)
                                    {

                                        for (int j = 0; j < (f as RadioField).Options.Count; j++)//foreach(Option op in (f as CheckboxField).Options)
                                        {
                                            if (v == (f as RadioField).Options[j].OptionText.GetContent("en"))
                                            {
                                                (_newDataItem.Fields[k] as RadioField).Options[j].SetAttribute("selected", true);
                                                break;
                                            }
                                        }
                                    }

                                    break;

                                    
                                }
                                else if (f.ModelType.Contains("CheckboxField"))
                                {
                                    //dataItem.SetFieldValue<EmailField>(fieldLabel, "en", colValue, "en", false, 0);
                                    string[] vals = colValue.Split(","); //THIS NEED TO BE REDO -- CONSIDERING ALSO SPLIT BY A ";"
                                    foreach(string v in vals)
                                    {
                                        
                                        for(int j=0; j< (f as CheckboxField).Options.Count; j++ )//foreach(Option op in (f as CheckboxField).Options)
                                        {
                                           if(v == (f as CheckboxField).Options[j].OptionText.GetContent("en")){
                                                (_newDataItem.Fields[k] as CheckboxField).Options[j].SetAttribute("selected", true);
                                                break;
                                           }
                                        }
                                    }
                                 
                                    break;
                                }
                                else if (f.ModelType.Contains("FieldContainerReference"))
                                {
                                    string[] vals = colValue.Split(","); //THIS NEED TO BE REDO -- CONSIDERING ALSO SPLIT BY A ";"
                                    //check the checkbox in the metadataset
                                    foreach (var fld in ms.Fields)
                                    {
                                        if (fld.ModelType.Contains("CheckboxField"))
                                        {
                                            foreach (string v in vals)
                                            {

                                                for (int j = 0; j < (fld as CheckboxField).Options.Count; j++)//foreach(Option op in (f as CheckboxField).Options)
                                                {
                                                    if (v == (fld as CheckboxField).Options[j].OptionText.GetContent("en"))
                                                    {
                                                        (ms.Fields[k] as CheckboxField).Options[j].SetAttribute("selected", true);
                                                        break;
                                                    }
                                                }
                                            }
                                    }
                                   
                                   
                                }
                                
                            }

                            //_newDataItem.Fields.Add(f);
                        }//end of each field
                    }


                   
                    i++;
                }

                item.DataContainer.Add(_newDataItem);
                // _db.Items.Add(item);

                if (maxEntries > 0 && rowCount == maxEntries)
                    break;

                rowCount++;
            }

          //  _db.SaveChanges();
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
            String spreadsheetId = "1m-oYJH-15DbqhE31dznAldB-gz75BJu1XAV5p5WJwxo";//==>google sheet Id
            String ranges = "A3:AI";// read from col A to AI, starting 3rd row

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
            //Mr Maarch 29 2022
            //header on the sheet
            //* marked the column contain that we're interested
            return new string[] {
                "sequence_id",
                "status",
                "name*",
                "Code",
                "email*",
                "Sub Code",
                "position*",
                "category",
                "main_disciplines",
                "faculty_or_department",
                "primary_area_of_research",
                "long_description",
                "Length",
                "Follow-up?",
                "website",
                "accepting_grad_students",
                "rig_affiliate",
                "open_to_contact_from_other_researchers_external_organizations_community_groups_and_nfps_and_other_rig-related_groups_",
                "looking_for_assistance_with_or_collaboration_in",
                "username",
                "fee_id",
                "expires_on",
                "Layout",
                "race",
                "ethnicity",
                "gender",
                "pronouns",
                "living with disability",
                "Intersectional",
                "keywords",
                "intersectional",
                "Research",
                "description under 50 words",
                "Community based projects",
                "Collaborations",
                "Consent to share profile",
                "Photo",
                "Profile Link"
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

    }
}
