using Catfish.Core.Models.Contents.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class FieldContainerReference : BaseField
    {
        public enum eRefType { undefined, data, metadata }
        public FieldContainerReference() { DisplayLabel = "Field Container"; ChildForm = new MetadataSet(); }
        public FieldContainerReference(XElement data) : base(data) { DisplayLabel = "Field Container"; }
        public FieldContainerReference(string name, string desc, string lang = null) : base(name, desc, lang) { DisplayLabel = "Field Container"; }

        public eRefType RefType 
        {
            get => GetAttribute("ref-type", eRefType.data);
            set => SetAttribute("ref-type", value);
        }
        public Guid RefId
        {
            get => Data.Attribute("ref-id") != null ? Guid.Parse(Data.Attribute("ref-id")?.Value) : Guid.Empty;
            set => SetAttribute("ref-id", value);
        }

        public FieldContainer ChildForm { get; set; }

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
            FieldContainerReference src = srcField as FieldContainerReference;
            RefType = src.RefType;
            RefId = src.RefId;
        }
    }
}
