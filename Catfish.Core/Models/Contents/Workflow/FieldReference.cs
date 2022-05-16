using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class FieldReference : XmlModel
    {
        public static readonly string TagName = "field-ref";

        public Guid? FieldContainerId
        {
            get
            {
                var id = GetAttribute("field-container-id", null as Guid?);
                if (id != null)
                    return id;
                else
                    return GetAttribute("data-item-id", null as Guid?); //Keeping backward compatibility
            }

            set => SetAttribute("field-container-id", value);
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
            FieldContainerId = dataItemId;
            FieldId = fieldId;
            return this;
        }
    }
}
