using Catfish.Core.Models.Contents.Expressions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class Option : XmlModel
    {
        public const string FieldTagName = "option";

        public MultilingualName OptionText { get; set; }
        public Guid Id
        {
            get => Guid.Parse(Data.Attribute("id").Value);
            set => Data.SetAttributeValue("id", value);
        }
        public bool Selected
        {
            get => GetAttribute("selected", false);
            set => SetAttribute("selected", value);
        }

        public bool ExtendedOption
        {
            get => GetAttribute("extended", false);
            set => SetAttribute("extended", value);
        }

        public string ExtendedValue
        {
            get => GetAttribute("extended-value", null as string);
            set => SetAttribute("extended-value", value);
        }

        private VisibilityCondition mVisibilityCondition;
        public VisibilityCondition VisibilityCondition { get { if (mVisibilityCondition == null) mVisibilityCondition = new VisibilityCondition(GetElement(VisibilityCondition.TagName, true)); return mVisibilityCondition; } }

        public Option() : base(FieldTagName) { }
        public Option(XElement data) : base(data) { }

        public override void Initialize(eGuidOption guidOption)
        {
            //Ensuring that each field has a unique ID
            base.Initialize(guidOption == eGuidOption.Ignore ? eGuidOption.Ensure : guidOption);

            OptionText = new MultilingualName(GetElement(MultilingualName.TagName, true));
        }

        public void SetOptionText(string text, string lang)
        {
            OptionText.SetContent(text, lang);
        }
    }
}
