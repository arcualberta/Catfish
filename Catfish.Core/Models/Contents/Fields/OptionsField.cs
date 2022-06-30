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

        public Guid[] SelectedOptionGuids => Options.Where(opt => opt.Selected).Select(opt => opt.Id).ToArray();
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

        public void AddOption(string optionText, Guid newOptGuid,  string langs)
        {
            Option opt = new Option();      
            opt.SetOptionText(optionText, langs);
            opt.Id = newOptGuid;

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

        /// <summary>
        /// This method updates the etxt of an existing option.
        /// </summary>
        /// <param name="txtId"></param>
        /// <param name="value"></param>
        /// <param name="lang"></param>
        public void UpdateOption(Guid txtId, string value, string lang)
        {
            foreach (Option opt in Options)
            {
                if (opt.OptionText.Values.Any(v => v.Id == txtId))
                {
                    opt.OptionText.UpdateValue(txtId, value, lang);
                    break;
                }
            }
        }

        public override void UpdateValues(BaseField srcField)
        {
            OptionsField src = srcField as OptionsField;
            if (src == null)
                throw new Exception("The source field is null or is not an OptionsField");

            //var selections = src.SelectedOptionGuids == null ? new Guid[0] : src.SelectedOptionGuids;
            //int i = 0;
            foreach (var dstOption in Options)
            {
                //dstOption.Selected = selections.Contains(dstOption.Id);
                //Options[i].Selected = selections.Contains(dstOption.Id);
                //i++;

                var srcOption = src.Options.FirstOrDefault(opt => opt.Id == dstOption.Id);
                dstOption.Selected = srcOption.Selected;
                dstOption.ExtendedOption = srcOption.ExtendedOption;
                dstOption.ExtendedValues = srcOption.ExtendedValues;
            }
        }

        public override void SetValue(string value, string lang)
        {
            int i = 0;
            foreach (var op in Options)
            {
                if (op.OptionText.Values.Where(txt => txt.Language == lang && txt.Value == value).Any())
                {
                    op.Selected = true;
                    Options[i].Selected = true;
                    break;
                }
                i++;
            }
        }

        public IEnumerable<Text> GetValues(string lang = null)
        {
            List<Text> ret = new List<Text>();
            var selectedOptions = Options.Where(op => op.Selected);
            foreach (var op in selectedOptions)
			{
                if (!string.IsNullOrEmpty(lang))
                    ret.Add(op.OptionText.Values.FirstOrDefault(txt => txt.Language == lang));
                else
                    ret.AddRange(op.OptionText.Values);
            }

            return ret;
        }

        public string GetValues(string separator, string lang = null)
        {
            var selectedOption = this.Options.Where(op => op.Selected).FirstOrDefault();

            if (selectedOption == null)
                return "";
            else
                return selectedOption.OptionText.GetConcatenatedContent(separator);
        }

        public Option GetOption(string optionText, string lang)
        {
            return Options.Where(op => op.OptionText.GetContent(lang) == optionText).FirstOrDefault();
        }

        public override void CopyValue(BaseField srcField, bool overwrite = false)
        {
            if (overwrite || Options.Where(op => op.Selected).Any() == false)
            {
                var src = srcField as OptionsField;
                foreach (var srcOption in src.Options)
                {
                    var option = Options
                        .Where(op => op.OptionText.ConcatenatedContent == srcOption.OptionText.ConcatenatedContent)
                        .FirstOrDefault();

                    if (option != null)
                        option.Selected = srcOption.Selected;
                }
            }
        }

        public bool RemoveOption(Guid optionId)
        {
            try
            {
                Option optionToRemove = Options.Where(o => o.Id == optionId).FirstOrDefault();
                Options.Remove(optionToRemove);
                return true;
            }catch(Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public string[] GetSelectedOptionTexts()
        {
            return Options.Where(op => op.Selected)
                .Select(op => op.OptionText.ConcatenatedContent)
                .ToArray();
        }
    }
}
