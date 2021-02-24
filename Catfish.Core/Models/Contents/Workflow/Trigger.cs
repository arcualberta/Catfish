using Catfish.Core.Models.Contents.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public abstract class Trigger : XmlModel
    {
        public static readonly string TagName = "trigger";
        public static readonly string FunctionAtt = "function";
        public static readonly string NameAtt = "name";

        public abstract bool Execute(EntityTemplate template, Item dataItem, TriggerRef triggerRef, IServiceProvider serviceProvider);
        public string Function
        {
            get => GetAttribute(FunctionAtt, null as string);
            set => SetAttribute(FunctionAtt, value);
        }

        public string Name
        {
            get => GetAttribute(NameAtt, null as string);
            set => SetAttribute(NameAtt, value);
        }

        public Trigger(XElement data)
            : base(data)
        {

        }
        public Trigger()
            : base(new XElement(TagName))
        {

        }

    }
}
