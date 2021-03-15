using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Reports
{
    public class ReportField : XmlModel
    {
        public const string TagName = "field-ref";

        public ReportField() : base(TagName) { }
        public ReportField(XElement data) : base(data) { }

        public Guid TemplateId
        {
            get => Guid.Parse(Data.Attribute("data-item-id").Value);
            set => Data.SetAttributeValue("data-item-id", value);
        }

        public Guid FieldId
        {
            get => Guid.Parse(Data.Attribute("field-id").Value);
            set => Data.SetAttributeValue("field-id", value);
        }

    }
}
