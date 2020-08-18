using Catfish.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public abstract class BaseField : XmlModel
    {
        public const string FieldTagName = "field";
        public abstract void UpdateValues(BaseField srcField);

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

        public void SetDescription(string containerDescription, string lang)
        {
            Description.SetContent(containerDescription, lang);
        }
    }
}
