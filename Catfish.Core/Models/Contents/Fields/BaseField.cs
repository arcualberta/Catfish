using Catfish.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class BaseField : XmlModel
    {
        public const string FieldTagName = "field";

        public string DisplayLabel { get; set; }
        public virtual void UpdateValues(BaseField srcField) 
        {
            throw new Exception("This method must be overridden by sub classes");
        }

        public bool Required
        {
            get => GetAttribute("required", false); 
            set => SetAttribute("required", value);
        }

        public bool AllowMultipleValues
        {
            get => GetAttribute("multiple", false);
            set => SetAttribute("multiple", value);
        }

        public MultilingualName Name { get; protected set; }

        public MultilingualDescription Description { get; protected set; }

        public string VueComponent => GetType().FullName;
       
        public BaseField() : base(FieldTagName) { }
        public BaseField(XElement data) : base(data) { }

        public BaseField(string name, string desc, string lang = null)
            : base(FieldTagName)
        {
            //Name = new MultilingualText(GetElement(Entity.NameTag, true));
            Name.SetContent(name, lang);

            //Description = new MultilingualText(GetElement("description", true));
            Description.SetContent(desc, lang);
        }

        public override void Initialize(eGuidOption guidOption)
        {
            //Ensuring that each field has a unique ID
            base.Initialize(guidOption == eGuidOption.Ignore ? eGuidOption.Ensure : guidOption);

            Name = new MultilingualName(GetElement(MultilingualName.TagName, true));
            Description = new MultilingualDescription(GetElement(MultilingualDescription.TagName, true));
        }

        public string GetName(string lang)
        {
            Text val = Name.Values.Where(val => val.Language == lang).FirstOrDefault();
            return val != null ? val.Value : null;
        }

        public void SetName(string containerName, string lang)
        {
            Name.SetContent(containerName, lang);
        }

        public string GetDescription(string lang)
        {
            Text val = Description.Values.Where(val => val.Language == lang).FirstOrDefault();
            return val != null ? val.Value : null;
        }

        public BaseField SetDescription(string containerDescription, string lang)
        {
            Description.SetContent(containerDescription, lang);
            return this;
        }

        #region Visible-If
        public Guid? VisibleIfOptionFieldId
        {
            get => GetAttribute("visible-if-option-field-id", null as Guid?);
            private set => Data.SetAttributeValue("visible-if-option-field-id", value);
        }
        public Guid[] VisibleIfOptionIds
        {
            get => GetAttribute("visible-if-option-id", null as Guid[]);
            private set => Data.SetAttributeValue("visible-if-option-id", value == null ? "" : string.Join(",", value.Select(v => v.ToString())));
        }
        public BaseField SetVisibleIf(OptionsField controllerField, string triggerOptionValue)
        {
            VisibleIfOptionFieldId = controllerField.Id;
            VisibleIfOptionIds = controllerField.Options
                .Where(op => op.OptionText.Values.Select(txt => txt.Value).Contains(triggerOptionValue))
                .Select(op => op.Id)
                .ToArray();

            return this;
        }
        #endregion

        public string FieldCssClass
        {
            get => GetAttribute("field-css", "");
            private set => Data.SetAttributeValue("field-css", value);
        }
        public BaseField SetFieldCssClass(string value) { FieldCssClass = value; return this; }
        public string FieldLabelCssClass
        {
            get => GetAttribute("label-css", "col-md-4");
            private set => Data.SetAttributeValue("label-css", value);
        }
        public BaseField SetFieldLabelCssClass(string value) { FieldLabelCssClass = value; return this; }

        public string FieldValueCssClass
        {
            get => GetAttribute("value-css", "col-md-8");
            private set => Data.SetAttributeValue("value-css", value);
        }
        public BaseField SetFieldValueCssClass(string value) { FieldValueCssClass = value; return this; }


    }
}
