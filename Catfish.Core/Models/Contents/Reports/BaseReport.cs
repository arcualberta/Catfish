using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Reports
{
    public class BaseReport : XmlModel
    {
        public readonly string FieldContainerTag = "fields";
        public const string TagName = "report";

        public BaseReport() : base(TagName) { }
        public BaseReport(XElement data) : base(data) { }

        public XmlModelList<ReportField> Fields { get; set; }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Building the values
            Fields = new XmlModelList<ReportField>(GetElement(FieldContainerTag, true), true, ReportField.TagName);
        }

        public ReportField AddField(Guid dataItemId, Guid fieldId)
        {
            ReportField field = new ReportField() { TemplateId = dataItemId, FieldId = fieldId };
            Fields.Add(field);
            return field;
        }

    }
}
