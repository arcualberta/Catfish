using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class EmailTemplateReference : WorkflowReferrence
    {
        public static readonly string TagName = "email-template-ref";
        public EmailTemplateReference(XElement data)
            : base(data)
        {

        }

        public EmailTemplateReference()
            : base(new XElement(TagName))
        {

        }
    }
}
