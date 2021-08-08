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
    }
}