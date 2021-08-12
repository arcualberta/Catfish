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

        public OptionField AppendOptions(string[] values)
        {
            foreach (var val in values)
                AppendOption(val, val);

            return this;
        }

        public virtual void UpdateDataField(Core.Models.Contents.Fields.OptionsField dataField)
        {
            string lang = "en";

            base.UpdateDataField(dataField);
            
            //Update existing options and adding new options
            foreach(var viewOpt in Options)
            {
                var dataOpt = dataField.Options.FirstOrDefault(opt => opt.Id == viewOpt.Id);
                if (dataOpt == null)
                {
                    dataOpt = new Core.Models.Contents.Fields.Option() { Id = viewOpt.Id };
                    dataField.Options.Add(dataOpt);
                }
                dataOpt.OptionText.SetContent(viewOpt.Label, lang);
            }

            //Deleting old options that no longer exist
            var viewOptionIds = Options.Select(op => op.Id).ToList();
            var toBeDeleted = dataField.Options.Where(op => !viewOptionIds.Contains(op.Id)).ToList();
            foreach (var item in toBeDeleted)
                dataField.Options.Remove(item);
        }

    }
}