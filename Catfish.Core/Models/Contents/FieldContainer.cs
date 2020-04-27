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
        public Guid Id
        {
            get => Guid.Parse(Data.Attribute("id").Value);
            set => Data.SetAttributeValue("id", value);
        }
        public XmlModelList<BaseField> Fields { get; set; }

        public FieldContainer(string tagName) : base(tagName) { Initialize(eGuidOption.Ignore); }
        public FieldContainer(XElement data) : base(data) { Initialize(eGuidOption.Ignore); }
        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            Fields = new XmlModelList<BaseField>(GetElement(FieldContainerTag, true), true, BaseField.FieldTagName);
        }
    }
}
