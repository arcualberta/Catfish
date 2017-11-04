using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Catfish.Core.Models.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Catfish.Core.Helpers;

namespace Catfish.Core.Models.Metadata
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
            Guid = XmlHelper.GetAttribute(optElement, "guid", System.Guid.NewGuid().ToString("N"));
        }

        public Option()
        {
            Value = new List<TextValue>();
            Selected = false;
            Guid = System.Guid.NewGuid().ToString("N");
        }

        public XElement ToXml()
        {
            XElement optionElement = new XElement("option");
            optionElement.SetAttributeValue("selected", Selected);
            optionElement.SetAttributeValue("guid", Guid);
            foreach (TextValue txt in Value)
            {
                XElement textEelemnt = new XElement("text", new XAttribute(XNamespace.Xml + "lang", txt.Language));
                textEelemnt.Value = string.IsNullOrEmpty(txt.Value) ? "" : txt.Value;
                optionElement.Add(textEelemnt);
            }
            return optionElement;
        }
    }

    [Ignore]
    public class OptionsField: MetadataField
    {
        //[DataType(DataType.MultilineText)]
        [TypeLabel("List of options, one option per line")]
        [NotMapped]
        public List<Option> Options
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

        public override void UpdateValues(XmlModel src)
        {
            OptionsField optionsField = src as OptionsField;
            this.Options = optionsField.Options;
        }        
    }

}
