using Catfish.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class MultilingualText : XmlModel
    {
        public XmlModelList<Text> Values { get; protected set; }

        public MultilingualText(string tagName) : base(tagName) { }

        public MultilingualText(XElement data) : base(data) { }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            // Populating the value list.
            Values = new XmlModelList<Text>(Data, true, Text.TagName);
        }

        public void SetContent(string value, string lang = null)
        {
            if (string.IsNullOrEmpty(lang))
                lang = ConfigHelper.DefaultLanguageCode;

            Text elementInGivenLanguage = Values.Where(n => n.Language == lang).FirstOrDefault();
            if (elementInGivenLanguage == null)
                Values.Add(new Text(value, lang));
            else
                elementInGivenLanguage.Data.Value = value;
        }

        public String GetContent(string lang = null)
        {
            if (string.IsNullOrEmpty(lang))
                lang = ConfigHelper.DefaultLanguageCode;

            Text elementInGivenLanguage = Values.Where(n => n.Language == lang).FirstOrDefault();
            return elementInGivenLanguage != null ? elementInGivenLanguage.Value : null;
        }

        public string GetConcatenatedContent(string separator)
        {
            var selected = Values
                .Where(v => !string.IsNullOrEmpty(v.Value))
                .Select(v => v.Value)
                .ToList();

            return string.Join(separator, selected);
        }

    }
}
