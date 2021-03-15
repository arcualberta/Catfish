using Catfish.Core.Helpers;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class EmailTrigger : Trigger
    {
        public XmlModelList<EmailRecipient> Recipients { get; set; }
        public XmlModelList<EmailTemplateReference> Templates { get; set; }
        public EmailTrigger(XElement data)
            : base(data)
        {

        }
        public EmailTrigger()
            : base(new XElement(TagName))
        {

        }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            Recipients = new XmlModelList<EmailRecipient>(GetElement("recipients", true));
            Templates = new XmlModelList<EmailTemplateReference>(GetElement("email-templates", true));
        }


        public EmailRecipient AddRecipientByEmail(string email)
        {
            if (Recipients.FindByAttribute(EmailRecipient.EmailAtt, email) != null)
                throw new Exception(string.Format("Email recipient {0} already exists.", email));

            EmailRecipient newRecipient = new EmailRecipient() { Email = email };
            Recipients.Add(newRecipient);
            return newRecipient;
        }

        public EmailRecipient AddRecipientByRole(Guid roleId, string roleName)
        {
            if (Recipients.Where(x => x.RoleId == roleId).Any())
                throw new Exception(string.Format("Email recipient role {0} already exists.", roleName));

            EmailRecipient newRecipient = new EmailRecipient() { RoleId = roleId };
            Recipients.Add(newRecipient);
            return newRecipient;
        }

        public EmailRecipient AddRecipientByDataField(Guid dataItemId, Guid fieldId)
        {
            if(Recipients.Where(r=>r.DataContainerId == dataItemId && r.FieldId == fieldId).Any())          
                throw new Exception(string.Format("Email recipient DataItem {0} and Feild {1} already exists.", dataItemId,fieldId));

            EmailRecipient newRecipient = new EmailRecipient() { DataContainerId = dataItemId, FieldId = fieldId };
            Recipients.Add(newRecipient);
            return newRecipient;

        }

        public void AddRecipientByMetadataField(string metadataSetName, Guid fieldId)
        {

        }

        public EmailRecipient AddOwnerAsRecipient()
        {
            if (Recipients.Where(x => x.Owner).Any())
                throw new Exception(string.Format("Owner is already a recipient."));

            EmailRecipient newRecipient = new EmailRecipient() { Owner = true };
            Recipients.Add(newRecipient);
            return newRecipient;
        }

        public EmailTemplateReference AddTemplate(Guid emailTemplateId, string exceptionMessage)
        {
            if (Templates.Where(t => t.RefId == emailTemplateId).Any())
                throw new Exception(string.Format("Email Template {0}: {1} already exists.", emailTemplateId.ToString(), exceptionMessage));

            EmailTemplateReference newRef = new EmailTemplateReference() { RefId = emailTemplateId };
            Templates.Add(newRef);
            return newRef;
        }

        public override bool Execute(EntityTemplate template, Item item,TriggerRef triggerRef, IServiceProvider serviceProvider)
        {
            IEmailService emailService = serviceProvider.GetService<IEmailService>();
            IWorkflowService workflowService = serviceProvider.GetService<IWorkflowService>();
            IGroupService groupService = serviceProvider.GetService<IGroupService>();
            IConfig config = serviceProvider.GetService<IConfig>();
            string lang = "en";

            workflowService.SetModel(template);
            //get email trigger from workflow triggers using trigger referance.
            EmailTrigger selectedTrigger = (EmailTrigger)template.Workflow.Triggers.Where(tr => tr.Id == triggerRef.RefId).FirstOrDefault();
            //get email template from selected workflow trigger.
            Guid emailReferanceId = selectedTrigger.Templates.Select(t => t.RefId).FirstOrDefault();

            //get email template name from metadate set.
            var emailTemplateName = template.MetadataSets
                                    .Where(ms => ms.Id == emailReferanceId)
                                    .FirstOrDefault().Name.Values
                                    .Select(ms => ms.Value)
                                    .FirstOrDefault();

            //get email template using workflow service GetEmailTemplate
            EmailTemplate emailMessage = template.GetEmailTemplate(emailTemplateName, lang, false);
            if (emailMessage == null)
                return false;

            //Make a clone and update references in the email body
            emailMessage = emailMessage.Clone<EmailTemplate>();
            emailMessage.UpdateRerefences("@SiteUrl", ConfigHelper.SiteUrl.TrimEnd('/'));
            emailMessage.UpdateRerefences("@Item.Id", item.Id.ToString());

            //get all recipient in the trigger.
            //Each recipient is identified in one of the following ways:
            //  * recipient's email (which includes a role identified within the workflow)
            //  * owner
            //  * by the content of a field in a data-item
            var recipients = selectedTrigger.Recipients.ToList();

            //add recipient to the content
            foreach (var recipient in recipients)
            {
                List<string> emailRecipients = new List<string>();
                if (recipient.Owner)
                {
                    if (item.UserEmail != null)
                        emailRecipients.Add(item.UserEmail);
                    else
                        emailRecipients.Add(workflowService.GetLoggedUserEmail());
                }
                else if (recipient.FieldId.HasValue && recipient.FieldId != Guid.Empty)
                {
                    DataItem dataItem = item.DataContainer.Where(dc => dc.IsRoot == true).FirstOrDefault();
                    //This means, we should retrieve the email from a data field in the passed data item
                    var recipientEmails = dataItem.GetValues(recipient.FieldId.Value);
                    emailRecipients.AddRange(recipientEmails);
                }
                else if(recipient.RoleId.HasValue && recipient.RoleId != Guid.Empty)
                {
                    //Retrieve the list of email addresses of all recipients that hold
                    //the role identified by the recipient.RoleId within the group under
                    //which this submission has been made.
                    //Add each such email address to the emailRecipients array.
                    emailRecipients.AddRange(groupService.GetUserEmailListByRole((Guid)recipient.RoleId, (Guid)item.GroupId));
                }
                else 
                {
                    emailRecipients.Add(recipient.Email);
                }
                //send email using email service
                foreach(var emailRecipient in emailRecipients)
                    SendEmail(emailMessage, emailRecipient, emailService, workflowService, config);
            }

            return true;
        }

        protected bool SendEmail(EmailTemplate emailTemplate, string recipient, IEmailService emailService, IWorkflowService workflowService, IConfig config)
        {
            try
            {
                Email email = new Email();
                email.UserName = workflowService.GetLoggedUserEmail();
                email.Subject = emailTemplate.GetSubject();
                email.FromEmail = config.GetSmtpEmail();
                email.RecipientEmail = recipient;
                email.Body = emailTemplate.GetBody();
                emailService.SendEmail(email);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
