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
        public Guid Id
        {
            get => Guid.Parse(Data.Attribute("id").Value);
            set => Data.SetAttributeValue("id", value);
        }

        public string ConcatenatedContent => GetConcatenatedContent(" | ");
        public string ConcatenatedRichText => GetConcatenatedContent("");

        public XmlModelList<Text> Values { get; protected set; }

        public MultilingualText(string tagName) : base(tagName) { Initialize(eGuidOption.Ensure); }

        public MultilingualText(XElement data) : base(data) { Initialize(eGuidOption.Ensure); }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            // Populating the value list.
            Values = new XmlModelList<Text>(Data, true, Text.TagName);
        }

        public void SetContent(string value, string lang = null, bool append = false)
        {
            if (string.IsNullOrEmpty(lang))
                lang = ConfigHelper.DefaultLanguageCode;

            Text elementInGivenLanguage = Values.Where(n => n.Language == lang).FirstOrDefault();
            if (elementInGivenLanguage == null)
                Values.Add(new Text(value, lang));
            else
                if (append)
                    elementInGivenLanguage.Data.Value += value;
                else
                    elementInGivenLanguage.Data.Value = value;
        }

        public void AppendElement(string elementString, string lang)
        {
            Text elementInGivenLanguage = Values.Where(n => n.Language == lang).FirstOrDefault();
            if (elementInGivenLanguage == null)
            {
                elementInGivenLanguage = new Text(null, lang);
                Values.Add(elementInGivenLanguage);
            }

            XElement child = XElement.Parse(elementString);
            elementInGivenLanguage.Data.Add(child);
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

        public void UpdateValues(MultilingualValue srcValue)
        {
            foreach(var txt in Values)
            {
                var srcTxt = srcValue.Values.Where(t => t.Id == txt.Id).FirstOrDefault();
                txt.Value = srcTxt.Value;
            }
        }

        public void UpdateValue(Guid txtId, string value, string lang)
        {
            var text = Values.Where(txt => txt.Id == txtId).FirstOrDefault();
            if (text != null)
                text.Value = value;
            else
                Values.Add(new Text() { Value = value, Language = lang });
        }
    }
}
