using Catfish.Core.Models.Contents.Workflow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class AggregateField : BaseField
    {
        public static readonly string SourcesTag = "sources";
        public enum eContetType { text, str }
        public XmlModelList<FieldReference> Sources { get; set; }

        public eContetType ContentType
        {
            get => GetAttribute("content-type", eContetType.text);
            set => SetAttribute("content-type", value);
        }
        public AggregateField() { }
        public AggregateField(XElement data) : base(data) { }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Building the values
            Sources = new XmlModelList<FieldReference>(GetElement(SourcesTag, true), true, FieldReference.TagName);
        }


        public override void CopyValue(BaseField srcField, bool overwrite = false)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(string values, string lang)
        {
            throw new NotImplementedException();
        }

        public override void UpdateValues(BaseField srcField)
        {
            throw new NotImplementedException();
        }

        public void AppendSource(Guid fieldContainerId,Guid fieldId, FieldReference.eSourceType sourceType, string valueDelimiter = null)
        {
            var fieldReference = new FieldReference()
            {
                FieldContainerId = fieldContainerId,
                FieldId = fieldId,
                SourceType = sourceType
            };

            if(!string.IsNullOrEmpty(valueDelimiter))
                fieldReference.ValueDelimiter = valueDelimiter; 

            Sources.Add(fieldReference);
        }

        public void AppendSources (FieldContainer fieldContainer, FieldReference.eSourceType sourceType)
        {
            foreach (var field in fieldContainer.Fields)
                AppendSource(fieldContainer.Id, field.Id, sourceType);
        }
    }

}
