using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Models.FormBuilder.Fields
{
    public class OptionField : Field
    {
        public List<Option> Options { get; set; } = new List<Option>();

        public Option AppendOption(string label, string description = null, int? limit =  null, decimal? price = null, bool isExtended = false, bool isDefault = false)
        {
            Option option = new Option()
            {
                Label = label,
                Description = description,
                Limit = limit,
                Price = price,
                IsExtended = isExtended,
                IsDefault = isDefault
            };
            Options.Add(option);
            return option;
        }

        public OptionField() { }

        public OptionField(string templateButtonLabel) : base(templateButtonLabel) { }

        public OptionField AppendOptions(string[] values, bool[] extendedOptions=null)
        {
            if (extendedOptions == null)
            {
                foreach (var val in values)
                    AppendOption(val, val); 
            }
            else
            {
                for (int i = 0; i < values.Length; i++)
                    AppendOption(values[i], values[i], null, null, extendedOptions[i]);  
            }
            return this;
        }

        public override void UpdateDataField(BaseField dataField)
        {
            string lang = "en";

            base.UpdateDataField(dataField);

            OptionsField optionsDataField = dataField as OptionsField;
            //Update existing options and adding new options
            foreach (var viewOpt in Options)
            {
                var dataOpt = optionsDataField.Options.FirstOrDefault(opt => opt.Id == viewOpt.Id);
                if (dataOpt == null)
                {
                    dataOpt = new Core.Models.Contents.Fields.Option() { Id = viewOpt.Id };
                    optionsDataField.Options.Add(dataOpt);
                }

                dataOpt.OptionText.SetContent(viewOpt.Label, lang);
                dataOpt.ExtendedOption = viewOpt.IsExtended;
            }

            //Deleting old options that no longer exist
            var viewOptionIds = Options.Select(op => op.Id).ToList();
            var toBeDeleted = optionsDataField.Options.Where(op => !viewOptionIds.Contains(op.Id)).ToList();
            foreach (var item in toBeDeleted)
                optionsDataField.Options.Remove(item);
        }

    }
}