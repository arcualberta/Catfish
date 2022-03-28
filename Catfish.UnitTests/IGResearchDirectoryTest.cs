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

namespace Catfish.UnitTests
{
    public class IGResearchDirectoryTest
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
        public void IGRD_SubmissionFormTest()
        {
            bool saveChangesToDatabase = true;

            string lang = "en";
            string templateName = "IG Research Directory Submission Form Template";
            string _metadatsetName = "IG Research Directory Submission Metadata";

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

            MetadataSet keywordMeta = template.GetMetadataSet(_metadatsetName, true, lang);
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
    }
}
