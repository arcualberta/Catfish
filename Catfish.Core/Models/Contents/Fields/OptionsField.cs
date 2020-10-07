using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class OptionsField : BaseField
    {
        public static readonly string OptionContainerTag = "options";
        public static readonly string OptionTag = "option";
        public OptionsField() { }
        public OptionsField(XElement data) : base(data) { }
        public OptionsField(string name, string desc, string lang = null) : base(name, desc, lang) { }

        public XmlModelList<Option> Options { get; set; }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Building the values
            Options = new XmlModelList<Option>(GetElement(OptionContainerTag, true), true, OptionTag);
        }

        ////public Option SetOptionText(string optionText, string lang, Guid? optionId = null, bool? selectByDefault = null)
        ////{
        ////    Option opt = optionId.HasValue ? Options.Find(optionId.Value) : new Option();
        ////    opt.SetOptionText(optionText, lang);
        ////    Options.Add(opt);

        ////    if (selectByDefault.HasValue)
        ////        opt.Selected = selectByDefault.Value;

        ////    return opt;
        ////}

        public void AddOptions(string[] optionText, string lang, int? selectedOptionIndex = null)
        {
            for (int i = 0; i < optionText.Length; ++i)
            {
                Option opt = new Option();
                opt.SetOptionText(optionText[i], lang);
                Options.Add(opt);

                if (selectedOptionIndex.HasValue && i == selectedOptionIndex.Value)
                    opt.Selected = true;
            }
        }

        public void UpdateOptions(string[] optionText, string lang)
        {
            for (int i = 0; i < optionText.Length; ++i)
            {
                Option opt = Options[i];
                opt.SetOptionText(optionText[i], lang);
                Options.Add(opt);
            }
        }

        public override void UpdateValues(BaseField srcField)
        {
            OptionsField src = srcField as OptionsField;
            if (src == null)
                throw new Exception("The source field is null or is not an OptionsField");

            foreach (var dstOption in Options)
            {
                var srcOption = src.Options.Where(opt => opt.Id == dstOption.Id).FirstOrDefault();
                if (srcOption != null)
                    dstOption.Selected = srcOption.Selected;
            }
        }
    }
}
