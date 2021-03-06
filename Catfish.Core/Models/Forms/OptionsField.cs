﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Catfish.Core.Models.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Catfish.Core.Helpers;

namespace Catfish.Core.Models.Forms
{
    [NotMapped]
    public class Option
    {
        public List<TextValue> Value { get; set; }
        public string Guid { get; set; }
        public bool Selected { get; set; }

        public Option (XElement optElement)
        {
            Selected = XmlHelper.GetAttribute(optElement, "selected", false);
            Value = XmlHelper.GetTextValues(optElement, true).ToList();
            Guid = XmlHelper.GetAttribute(optElement, "guid", System.Guid.NewGuid().ToString());
        }

        public Option()
        {
            Value = new List<TextValue>();
            Selected = false;
            Guid = System.Guid.NewGuid().ToString();
        }

        public XElement ToXml()
        {
            XElement optionElement = new XElement("option");
            optionElement.SetAttributeValue("selected", Selected);
            optionElement.SetAttributeValue("guid", Guid);
            foreach (TextValue txt in Value)
            {
                XElement textEelemnt = new XElement("text", new XAttribute(XNamespace.Xml + "lang", txt.LanguageCode));
                textEelemnt.Value = string.IsNullOrEmpty(txt.Value) ? "" : txt.Value;
                optionElement.Add(textEelemnt);
            }
            return optionElement;
        }

        public float ScoreOption(Option option)
        {
            float result = 0.0f;

            if(option.Guid == this.Guid)
            {
                return 1.0f;
            }

            foreach (var value in Value)
            {
                if(option.Value.Where(v => v.LanguageCode == value.LanguageCode && v.Value == value.Value).Any())
                {
                    result += 1.0f;
                }
            }

            return result / (float)Value.Count;
        }
    }

    [CFIgnore]
    public class OptionsField: FormField
    {
        //[DataType(DataType.MultilineText)]
        [CFTypeLabel("List of options, one option per line")]
        [NotMapped]
        public IReadOnlyList<Option> Options
        {
            get
            {
                var wrapper = Data.Element("options");
                if (wrapper == null)
                    return new List<Option>();

                return wrapper.Elements("option").Select(op => new Option(op)).ToList();
            }
            set
            {
                var wrapper = Data.Element("options");
                if (wrapper == null)
                    Data.Add(wrapper = new XElement("options"));

                //Iterate through the source options and check if the same option appears in the destination.
                //If it does, then update the "selected" status of the destination option. Otherwise, add
                //a clone of the source option to the destination option list.
                var dstOptions = wrapper.Elements("option").ToList();
                foreach (var op in value)
                {
                    var dstOp = dstOptions.Where(x => x.Attribute("guid").Value == op.Guid).FirstOrDefault();
                    if (dstOp == null)
                    {
                        dstOp = op.ToXml();
                        dstOptions.Add(dstOp);
                        wrapper.Add(dstOp);
                    }
                    else
                        dstOp.SetAttributeValue("selected", op.Selected);
                }
            }
        }

        protected XElement CreateOption(string value, bool isSelected, string lang)
        {
            XElement optionElement = new XElement("option");
            XElement textEelemnt = new XElement("text", new XAttribute(XNamespace.Xml + "lang", lang));
            textEelemnt.Value = string.IsNullOrEmpty(value) ? "" : value;
            optionElement.Add(textEelemnt);
            optionElement.SetAttributeValue("selected", isSelected);
            return optionElement;
        }

        public override void UpdateValues(CFXmlModel src)
        {
            OptionsField optionsField = src as OptionsField;
            this.Options = optionsField.Options;
        }

        private void ReplaceOptions(IEnumerable<Option> newOptions)
        {
            var wrapper = Data.Element("options");
            if (wrapper == null)
                Data.Add(wrapper = new XElement("options"));

            wrapper.RemoveNodes();

            foreach (var option in newOptions)
            {
                wrapper.Add(option.ToXml());
            }
        }

        public override void Merge(FormField newField)
        {
            base.Merge(newField);
            OptionsField optionsField = (OptionsField)newField;

            List<Option> options = new List<Option>(optionsField.Options.Count);
            IReadOnlyList<Option> oldOptions = this.Options;

            foreach(var newOption in optionsField.Options)
            {
                Option option = oldOptions.Where(o => o.ScoreOption(newOption) > 0.0f).OrderByDescending(o => o.ScoreOption(newOption)).FirstOrDefault();

                if(option == null)
                {
                    options.Add(newOption);
                }
                else
                {
                    option.Value = newOption.Value;
                    options.Add(option);
                }
            }

            ReplaceOptions(options);
        }
    }

}
