using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class DateField : MonolingualTextField
    {
        public bool IncludeTime
        {
            get => GetAttribute("include-time", false);
            set => SetAttribute("include-time", value);
        }

        public DateField() : base() { DisplayLabel = "Date"; }
        public DateField(XElement data) : base(data) { DisplayLabel = "Date"; }
        public DateField(string name, string desc, string lang = null) : base(name, desc, lang) { DisplayLabel = "Date"; }

        public override string GetValues(string separator, string lang = null)
        {
            return string.Join(separator, Values.Select(txt => string.IsNullOrEmpty(txt.Value) ? "" : txt.Value.Substring(0, 10)));
        }

        public DateTime[] GetValues()
        {
            return Values.Where(v => !string.IsNullOrEmpty(v.Value))
                .Select(v => DateTime.Parse(v.Value))
                .ToArray();
        }

    }
}
