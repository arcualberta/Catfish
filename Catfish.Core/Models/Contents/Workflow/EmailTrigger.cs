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

        public EmailRecipient AddRecipientByRole(string role)
        {
            if (Recipients.Where(x => x.Role == role).Any())
                throw new Exception(string.Format("Email recipient role {0} already exists.", role));

            EmailRecipient newRecipient = new EmailRecipient() { Role = role };
            Recipients.Add(newRecipient);
            return newRecipient;
        }

        public void AddRecipientByDataField(Guid dataItemId, Guid fieldId)
        {

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

        public override bool Execute(EntityTemplate template, TriggerRef triggerRef, IServiceProvider serviceProvider)
        {
            IEmailService emailService = serviceProvider.GetService<IEmailService>();
            IWorkflowService workflowService = serviceProvider.GetService<IWorkflowService>();

            //get email trigger from workflow triggers using trigger referance.
            EmailTrigger selectedTrigger = (EmailTrigger)template.Workflow.Triggers.Where(tr => tr.Id == triggerRef.RefId).FirstOrDefault();

            //get email template from selected workflow trigger.
            Guid emailReferanceId = selectedTrigger.Templates.Select(t => t.RefId).FirstOrDefault();

            //get email template name from metadate set.
            var emailTemplateName = template.MetadataSets
                                    .Where(ms => ms.Id == emailReferanceId)
                                    .FirstOrDefault().Name.Values
                                    .Select(ms => ms.Value).FirstOrDefault();
            //get email template using workflow service GetEmailTemplate. Inhere need to pass email template.
            EmailTemplate emailTemplate = workflowService.GetEmailTemplate(emailTemplateName, false);

            ////get all recipient in the trigger.
            //var recipients = selectedTrigger.Recipients.ToList();

            ////add recipient to the content
            //foreach (var recipient in recipients)
            //{
            //    string emailRecipient;
            //    if (recipient.Owner)
            //    {
            //        emailRecipient = _authorizationService.GetLoggedUserEmail();
            //    }
            //    else
            //    {
            //        emailRecipient = recipient.Email;
            //    }
            //    //send email using email service
            //    SendEmail(emailTemplate, emailRecipient);
            //}

            return true;
        }

        protected bool SendEmail(EmailTemplate emailTemplate, string recipient, IEmailService emailService)
        {
            /*
            try
            {
                Email email = new Email();
                email.UserName = _authorizationService.GetLoggedUserEmail();
                email.Subject = emailTemplate.GetSubject();
                email.FromEmail = _config.GetSmtpEmail();
                email.RecipientEmail = recipient;
                email.Body = emailTemplate.GetBody();
                emailService.SendEmail(email);
                return true;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return false;
            }
            */
            return false;

        }

    }
}
