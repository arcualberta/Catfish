using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class Option : XmlModel
    {
        public const string FieldTagName = "option";

        public MultilingualText OptionText { get; set; }
        public bool Selected
        {
            get => GetAttribute("selected", false);
            set => SetAttribute("selected", value);
        }

        public Option() : base(FieldTagName) { }
        public Option(XElement data) : base(data) { }

        public override void Initialize(eGuidOption guidOption)
        {
            //Ensuring that each field has a unique ID
            base.Initialize(guidOption == eGuidOption.Ignore ? eGuidOption.Ensure : guidOption);

            OptionText = new MultilingualText(GetElement(Entity.NameTag, true));
        }

        public void SetOptionText(string text, string lang)
        {
            OptionText.SetContent(text, lang);
        }
    }
}
