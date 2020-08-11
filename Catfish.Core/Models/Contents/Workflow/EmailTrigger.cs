using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class EmailTrigger : Trigger
    {
        public XmlModelList<EmailRecipient> Recipients { get; set; }
        public EmailTrigger(XElement data)
            : base(data)
        {

        }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            Recipients = new XmlModelList<EmailRecipient>(GetElement("recipients", true));
        }


        public void AddRecipientByEmail(string email)
        {
            if (Recipients.FindByAttribute(EmailRecipient.EmailAtt, email) != null)
                throw new Exception(string.Format("Email recipient {0} already exists.", email));

            Recipients.Add(new EmailRecipient() { Email = email });
        }

        public void AddRecipientByRole(Guid roleId)
        {

        }

        public void AddRecipientByDataField(Guid dataItemId, Guid fieldId)
        {

        }

        public void AddRecipientByMetadataField(string metadataSetName, Guid fieldId)
        {

        }
    }
}
