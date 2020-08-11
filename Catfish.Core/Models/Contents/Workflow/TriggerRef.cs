using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class TriggerRef : WorkflowReferrence
    {
        public string Order
        {
            get => GetAttribute("order", null as string);
            set => SetAttribute("order", value);
        }
        public TriggerRef(XElement data)
            : base(data)
        {

        }
    }
}
