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
        public XmlModelList<WorkflowEmailTemplate> Templates { get; set; }
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
        }


        public void AddRecipientByEmail(string email)
        {
            if (Recipients.FindByAttribute(EmailRecipient.EmailAtt, email) != null)
                throw new Exception(string.Format("Email recipient {0} already exists.", email));

            Recipients.Add(new EmailRecipient() { Email = email });
        }

        public void AddRecipientByRole(string role)
        {
            if (Recipients.Where(x => x.Value == role).Any())
                throw new Exception(string.Format("Email recipient {0} already exists.", role));

            EmailRecipient newRecipient = new EmailRecipient() { Value = role };
            Recipients.Add(newRecipient);
        }

        public void AddRecipientByDataField(Guid dataItemId, Guid fieldId)
        {

        }

        public void AddRecipientByMetadataField(string metadataSetName, Guid fieldId)
        {

        }

        //public WorkflowEmailTemplate AddTemplate(Guid feild, Guid metadataset)
        //{
        //    if (Templates.Find(feild) != null)
        //        throw new Exception(string.Format("Email Template {0} already exists.", feild));

        //    WorkflowEmailTemplate newRef = new WorkflowEmailTemplate() { MetadataSetId = metadataset, FeildId = feild };
        //    Templates.Add(newRef);
        //    return newRef;
        //}
    }
}
