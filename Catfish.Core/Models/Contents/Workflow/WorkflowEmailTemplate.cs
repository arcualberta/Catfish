using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class WorkflowEmailTemplate : XmlModel
    {
        public static readonly string TagName = "email-template";
        public static readonly string MetadataAtt = "metadata-set-id";
        public static readonly string FeildAtt = "feild-id";

        public Guid MetadataSetId
        {
            get => Guid.Parse(Data.Attribute(MetadataAtt).Value);
            set => SetAttribute(MetadataAtt, value);
        }
        public Guid FeildId
        {
            get => Guid.Parse(Data.Attribute(FeildAtt).Value);
            set => SetAttribute(FeildAtt, value);
        }
        public WorkflowEmailTemplate(XElement data)
            : base(data)
        {

        }
        public WorkflowEmailTemplate()
            : base(new XElement(TagName))
        {

        }
    }
}
