using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class TriggerRef : WorkflowReferrence
    {
        public static readonly string TagName = "trigger-ref";
        public string Order
        {
            get => GetAttribute("order", null as string);
            set => SetAttribute("order", value);
        }
        public TriggerRef(XElement data)
            : base(data)
        {

        }
        public TriggerRef()
            : base(new XElement(TagName))
        {

        }
    }
}
