using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class WorkflowReferrence : XmlModel
    {
        public static readonly string RefIdAtt = "ref-id";
        public Guid RefId
        {
            get => Guid.Parse(Data.Attribute(RefIdAtt).Value);
            set => Data.SetAttributeValue(RefIdAtt, value);
        }
        public WorkflowReferrence(XElement data)
            : base(data)
        {

        }
    }
}
