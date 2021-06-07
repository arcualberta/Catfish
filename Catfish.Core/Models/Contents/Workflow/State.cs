using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class State : XmlModel
    {
        public static readonly string TagName = "state";
        
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
