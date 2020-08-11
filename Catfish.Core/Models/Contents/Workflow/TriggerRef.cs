using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class TriggerRef : XmlModel
    {
        public string Order
        {
            get => GetAttribute("order", null);
            set => SetAttribute("order", value);
        }
        public Guid RefId
        {
            get => Guid.Parse(Data.Attribute("ref-id").Value);
            set => Data.SetAttributeValue("ref-id", value);
        }
        public TriggerRef(XElement data)
            : base(data)
        {

        }
    }
}
