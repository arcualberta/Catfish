using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class WorkflowRole : XmlModel
    {
        public static readonly string TagName = "role";
        public string Value
        {
            get => Data.Value;
            set => Data.Value = string.IsNullOrEmpty(value) ? "" : value;
        }

        public WorkflowRole(XElement data)
            : base(data)
        {

        }

        public WorkflowRole()
            : base(new XElement(TagName))
        {

        }

    }
}
