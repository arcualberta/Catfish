using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class OptionsField : BaseField
    {
        public static readonly string OptionContainerTag = "options";
        public static readonly string OptionTag = "options";
        public OptionsField() { }
        public OptionsField(XElement data) : base(data) { }
        public OptionsField(string name, string desc, string lang = null) : base(name, desc, lang) { }

        public XmlModelList<Option> Options { get; set; }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Building the values
            XmlModel xml = new XmlModel(Data);
            Options = new XmlModelList<Option>(xml.GetElement(OptionContainerTag, true), true, OptionTag);
        }

        public void AddOption(string optionText, string lang, bool? selectByDefault)
        {
            Option opt = new Option();
            opt.SetOptionText(optionText, lang);
            Options.Add(opt);

            if (selectByDefault.HasValue)
                opt.Selected = selectByDefault.Value;


        }

        public void AddOptions(string[] optionText, string lang, int? selectedOptionIndex = null)
        {
            int idx = 0;
            foreach(string text in optionText)
            {
                Option opt = new Option();
                opt.SetOptionText(text, lang);
                Options.Add(opt);

                if (selectedOptionIndex.HasValue && idx == selectedOptionIndex)
                    opt.Selected = true;

                ++idx;
            }
        }

    }
}
