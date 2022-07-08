using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Reports
{
    public class BaseReport : XmlModel
    {
        public readonly string FieldContainerTag = "field-refs";
        public const string TagName = "report";

        public BaseReport() : base(TagName) { }
        public BaseReport(XElement data) : base(data) { }

       

        //public BaseReport(string name)
        //    : base(TagName)
        //{   
        //    Name.Se
        //}


        public string Name 
        {
            get => GetAttribute("name", "");
            set => SetAttribute("name", value);
        }

        //MR: March 9 2021: entity-template-id --> refer to the DataItem template where this report is belong to
        public string EntityTemplateId
        {
            get => GetAttribute("entity-template-id", "");
            set => SetAttribute("entity-template-id", value);
        }
        public XmlModelList<ReportField> Fields { get; set; }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Building the values
            Fields = new XmlModelList<ReportField>(GetElement(FieldContainerTag, true), true, ReportField.TagName);
        }

        public BaseReport AddField(Guid dataItemId, Guid fieldId, string fieldLabel=null) //return  ReportField
        {
            ReportField field = new ReportField() { TemplateId = dataItemId, FieldId = fieldId, FieldLabel=fieldLabel};
            Fields.Add(field);
            //return field;
            return this;
        }

        public BaseReport AddField(Guid dataItemId, Guid parentFieldId, Guid fieldId, string fieldLabel = null) //return  ReportField
        {
            ReportField field = new ReportField() { TemplateId = dataItemId, ParentFieldId=parentFieldId, FieldId = fieldId, FieldLabel=fieldLabel };
            Fields.Add(field);
            //return field;
            return this;
        }

    }
}
