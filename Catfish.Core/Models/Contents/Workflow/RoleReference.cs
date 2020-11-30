using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class RoleReference : WorkflowReferrence
    {
        public static readonly string TagName = "role-ref";
        public static readonly string OwnerAttribute = "owner";

        public bool Owner
        {
            get => GetAttribute(OwnerAttribute, false); 
            set => SetAttribute(OwnerAttribute, value);
        }
        public RoleReference(XElement data)
                : base(data)
        {

        }

        public RoleReference()
            : base(new XElement(TagName))
        {

        }
    }
}
