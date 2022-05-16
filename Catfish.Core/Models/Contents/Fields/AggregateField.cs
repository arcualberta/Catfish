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
        public XmlModelList<FieldReference> Sources { get; set; }

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

        public void AppendSource(Guid fieldContainerId,Guid fieldId)
        {
            Sources.Add(new FieldReference() { FieldContainerId = fieldContainerId, FieldId = fieldId });
        }

        public void AppendSources (FieldContainer fieldContainer)
        {
            foreach (var field in fieldContainer.Fields)
                AppendSource(fieldContainer.Id, field.Id);
        }
    }

}
