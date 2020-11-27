using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class State : XmlModel
    {
        public static readonly string TagName = "state";
        public bool IsEditable
        {
            get => GetAttribute("is-editable", false);
            set => SetAttribute("is-editable", value);
        }
        public string Value
        {
            get => Data.Value;
            set => Data.Value = string.IsNullOrEmpty(value) ? "" : value;
        }

        public State(XElement data)
            : base(data)
        {

        }

        public State()
            : base(new XElement(TagName))
        {

        }

    }
}
