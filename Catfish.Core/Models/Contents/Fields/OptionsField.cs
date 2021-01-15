using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class OptionsField : BaseField, IValueField
    {
        public static readonly string OptionContainerTag = "options";
        public static readonly string OptionTag = "option";

        public Guid[] SelectedOptionGuids { get; set; }
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
        public void AddOption(string[] optionTexts, string[] langs)
        {
            Option opt = new Option();
            for (int i = 0; i < optionTexts.Length; ++i)
                opt.SetOptionText(optionTexts[i], langs[i]);

            Options.Add(opt);
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

            var selections = src.SelectedOptionGuids == null ? new Guid[0] : src.SelectedOptionGuids;

            foreach (var dstOption in Options)
            {
                dstOption.Selected = selections.Contains(dstOption.Id);
            }
        }

        public IEnumerable<Text> GetValues(string lang = null)
        {
            throw new NotImplementedException();
        }

        public string GetValues(string separator, string lang = null)
        {
            var selectedOption = this.Options.Where(op => op.Selected).FirstOrDefault();

            if (selectedOption == null)
                return "";
            else
                return selectedOption.OptionText.GetConcatenatedContent(separator);
        }


    }
}
