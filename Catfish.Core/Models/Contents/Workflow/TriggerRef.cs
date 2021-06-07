using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class TriggerRef : WorkflowReferrence
    {
        public static readonly string TagName = "trigger-ref";
        public static readonly string OrderAtt = "order";
        public static readonly string ConditionAtt = "condition";
        public static readonly string NextStatusAtt = "next";
        public string Order
        {
            get => GetAttribute(OrderAtt, null as string);
            set => SetAttribute(OrderAtt, value);
        }
        public bool Condition
        {
            get => GetAttribute(ConditionAtt, false);
            set => Data.SetAttributeValue(ConditionAtt, value);
        }
        public Guid? NextStatus
        {
            get => GetAttribute(NextStatusAtt, null as Guid?);
            set => Data.SetAttributeValue(NextStatusAtt, value);
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
