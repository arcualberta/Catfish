using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Reports
{
    public class ReportField : XmlModel
    {
        public const string TagName = "field";

        public ReportField() : base(TagName) { }
        public ReportField(XElement data) : base(data) { }

        public Guid TemplateId
        {
            get => Guid.Parse(Data.Attribute("template-id").Value);
            set => Data.SetAttributeValue("template-id", value);
        }

        public Guid FieldId
        {
            get => Guid.Parse(Data.Attribute("field-id").Value);
            set => Data.SetAttributeValue("field-id", value);
        }

    }
}
