using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class EmailRecipient : XmlModel
    {
        public static readonly string TagName = "recipient";

        public static readonly string EmailAtt = "email";

        public string Email
        {
            get => Data.Attribute(EmailAtt)?.Value;
            set => Data.SetAttributeValue(EmailAtt, value);
        }
        public Guid? MetadataSetId
        {
            get => GetAttribute("metadata-set-id", null as Guid?);
            set => Data.SetAttributeValue("email", value);
        }

        public Guid? DataContainerId
        {
            get => GetAttribute("data-container-id", null as Guid?);
            set => Data.SetAttributeValue("data-container", value);
        }

        public bool Owner
        {
            get => GetAttribute("owner", false);
            set => Data.SetAttributeValue("owner", value);
        }
        public Guid? FieldId
        {
            get => GetAttribute("field-id", null as Guid?);
            set => Data.SetAttributeValue("field-id", value);
        }

        public Guid? RoleId
        {
            get => GetAttribute("role-id", null as Guid?);
            set => SetAttribute("role-id", value);
        }

        public EmailRecipient(XElement data)
            : base(data)
        {

        }

        public EmailRecipient()
            : base(new XElement(TagName))
        {

        }
    }
}
