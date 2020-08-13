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


        public void AddRecipientByEmail(string email)
        {
            if (Recipients.FindByAttribute(EmailRecipient.EmailAtt, email) != null)
                throw new Exception(string.Format("Email recipient {0} already exists.", email));

            Recipients.Add(new EmailRecipient() { Email = email });
        }

        public void AddRecipientByRole(string role)
        {
            if (Recipients.Where(x => x.Role == role).Any())
                throw new Exception(string.Format("Email recipient role {0} already exists.", role));

            EmailRecipient newRecipient = new EmailRecipient() { Role = role };
            Recipients.Add(newRecipient);
        }

        public void AddRecipientByDataField(Guid dataItemId, Guid fieldId)
        {

        }

        public void AddRecipientByMetadataField(string metadataSetName, Guid fieldId)
        {

        }

        public void AddOwnerAsRecipient()
        {
            if (Recipients.Where(x => x.Owner).Any())
                throw new Exception(string.Format("Owner is already a recipient."));

            EmailRecipient newRecipient = new EmailRecipient() { Owner = true };
            Recipients.Add(newRecipient);
        }

        public EmailTemplateReference AddTemplate(Guid emailTemplateId, string exceptionMessage)
        {
            if (Templates.Where(t => t.RefId == emailTemplateId).Any())
                throw new Exception(string.Format("Email Template {0}: {1} already exists.", emailTemplateId.ToString(), exceptionMessage));

            EmailTemplateReference newRef = new EmailTemplateReference() { RefId = emailTemplateId };
            Templates.Add(newRef);
            return newRef;
        }
    }
}
