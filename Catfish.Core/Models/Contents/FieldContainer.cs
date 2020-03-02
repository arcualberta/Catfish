using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class FieldContainer : XmlModel
    {
        public const string FieldContainerTag = "fields";
        public XmlModelList<BaseField> Fields { get; set; }

        public FieldContainer(string tagName) : base(tagName) { Initialize(); }
        public FieldContainer(XElement data) : base(data) { Initialize(); }
        public void Initialize()
        {
            Fields = new XmlModelList<BaseField>(GetElement(FieldContainerTag, true), true, BaseField.FieldTagName);
        }
    }
}
