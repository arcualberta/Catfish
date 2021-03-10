using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class FieldReference : XmlModel
    {
        public static readonly string TagName = "field-ref";

        public Guid? DataItemId
        {
            get => GetAttribute("data-item-id", null as Guid?);
            set => SetAttribute("data-item-id", value);
        }

        public Guid? FieldId
        {
            get => GetAttribute("field-id", null as Guid?);
            set => SetAttribute("field-id", value);
        }

        public FieldReference(XElement data)
            : base(data)
        {

        }

        public FieldReference()
            : base(new XElement(TagName))
        {

        }

        public FieldReference SetValue(Guid dataItemId, Guid fieldId)
        {
            DataItemId = dataItemId;
            FieldId = fieldId;
            return this;
        }
    }
}
