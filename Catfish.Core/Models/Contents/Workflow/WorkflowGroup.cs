using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class WorkflowGroup : XmlModel
    {
        public static readonly string TagName = "group";
        public string Value
        {
            get => Data.Value;
            set => Data.Value = string.IsNullOrEmpty(value) ? "" : value;
        }

        public WorkflowGroup(XElement data)
            : base(data)
        {

        }

        public WorkflowGroup()
            : base(new XElement(TagName))
        {

        }

    }
}
