using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class StateRef : WorkflowReferrence
    {
        public static readonly string TagName = "state-ref";
        
        public StateRef(XElement data)
            : base(data)
        {

        }
        public StateRef()
            : base(new XElement(TagName))
        {

        }
    }
}
