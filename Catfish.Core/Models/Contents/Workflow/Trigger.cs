using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class Trigger : XmlModel
    {
        public string Order
        {
            get => GetAttribute("function", null);
            set => SetAttribute("function", value);
        }
        public Guid Id
        {
            get => Guid.Parse(Data.Attribute("id").Value);
            set => Data.SetAttributeValue("id", value);
        }

        public Trigger(XElement data)
            : base(data)
        {

        }
    }
}
