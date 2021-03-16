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

        /// <summary>
        /// For Composite Field Id
        /// </summary>
        public Guid? ParentFieldId
        {
            get
            {
                if (Data.Attribute("composite-field-id") == null)
                    return null;

                return Guid.Parse(Data.Attribute("composite-field-id").Value);
            }
            set => Data.SetAttributeValue("composite-field-id", value);
        }

        public string FieldLabel
        {
            get => Data.Attribute("field-label").Value;
            set => Data.SetAttributeValue("field-label", value);
        }

    }
}
