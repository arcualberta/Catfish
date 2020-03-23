﻿using Catfish.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class MultilingualElement : XmlModel
    {
        public List<Text> Values = new List<Text>();

        public MultilingualElement(string tagName) : base(tagName) { }

        public MultilingualElement(XElement data) : base(data) { Initialize(); }

        public void Initialize()
        {
            // Creating Text objects for all text elements withing the immediate children
            // and adding them to the Values list.
            Values = new List<Text>();
            foreach (var ele in Data.Elements(Text.TagName))
                Values.Add(XmlModel.InstantiateContentModel(ele) as Text);
        }

        public void SetContent(string value, string lang = null)
        {
            if (string.IsNullOrEmpty(lang))
                lang = ConfigHelper.DefaultLanguageCode;

            Text nameInGivenLanguage = Values.Where(n => n.Language == lang).FirstOrDefault();
            if (nameInGivenLanguage == null)
            {
                nameInGivenLanguage = new Text(value, lang);
                Data.Add(nameInGivenLanguage.Data);
                Values.Add(nameInGivenLanguage);
            }
            else
                nameInGivenLanguage.Data.Value = value;
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
