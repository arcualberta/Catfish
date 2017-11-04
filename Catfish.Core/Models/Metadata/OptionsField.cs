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
        public bool Selected { get; set; }

        public Option (XElement optElement)
        {
            Selected = XmlHelper.GetAttribute(optElement, "selected", false);
            Value = XmlHelper.GetTextValues(optElement, true).ToList();
        }

        public Option()
        {
            Value = new List<TextValue>();
            Selected = false;
        }

        public XElement ToXml()
        {
            XElement optionElement = new XElement("option");
            optionElement.SetAttributeValue("selected", Selected);
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

                foreach (var op in value)
                    wrapper.Add(op.ToXml());
            }
        }

        ////////public virtual void SetOptions(List<Option> options, string selection = null)
        ////////{
        ////////    throw new NotImplementedException();
        ////////    /*
        ////////    XElement optionsParent = GetOptionParent();
        ////////    ClearOptionSelections();
        ////////    List<XElement> optionList = GetOptionElements();

        ////////    //Iterating through all input "options", updating selections and 
        ////////    //inserting any new options
        ////////    string val_path = "./text[@xml:lang='" + Lang(lang) + "']";
        ////////    foreach (var opt in options)
        ////////    {
        ////////        bool found = false;
        ////////        foreach(XElement opEle in optionList)
        ////////        {
        ////////            List<string> vals = GetChildElements(val_path, opEle).Select(txt => txt.Value).ToList();
        ////////            if (vals.Contains(opt.Value))
        ////////            {
        ////////                opEle.SetAttributeValue("selected", opt.Selected);
        ////////                found = true;
        ////////                break;
        ////////            }
        ////////        }

        ////////        if(!found)
        ////////        {
        ////////            XElement opEle = CreateOption(opt.Value, opt.Selected, lang);
        ////////            optionsParent.Add(opEle);
        ////////        }
        ////////    }
        ////////    */
        ////////}

        ////////protected XElement GetOptionParent()
        ////////{
        ////////    string xpath = "./options";
        ////////    XElement optionsParent = this.GetChildElements(xpath, Data).FirstOrDefault();
        ////////    if (optionsParent == null)
        ////////    {
        ////////        optionsParent = new XElement("options");
        ////////        Data.Add(optionsParent);
        ////////    }

        ////////    return optionsParent;
        ////////}

        ////////protected void ClearOptionSelections()
        ////////{
        ////////    List<XElement> optionList = GetOptionElements();
        ////////    foreach (var opt in optionList)
        ////////        opt.SetAttributeValue("selected", false);
        ////////}

        ////////protected List<XElement> GetOptionElements()
        ////////{
        ////////    string xpath = "./options/option";
        ////////    List<XElement> optionList = this.GetChildElements(xpath, Data).ToList();
        ////////    return optionList;
        ////////}

        ////public void SetOption(string value, bool isSelected, XElement optionParent, string lang)
        ////{
        ////    IEnumerable<XElement> children = this.GetChildElements("option", optionParent);
        ////    string val_path = "./text[@xml:lang='" + Lang(lang) + "']";
        ////    bool found = false;
        ////    foreach (XElement opt in children)
        ////    {
        ////        XElement txt = GetTextElements(opt, lang).FirstOrDefault();
        ////        if (txt != null && txt.Value == value)
        ////        {
        ////            found = true;
        ////            break;
        ////        }
        ////        opt.SetAttributeValue("selected", isSelected);
        ////    }

        ////    if (!found)
        ////    {
        ////        XElement opt = CreateOption(value, isSelected, lang);
        ////        optionParent.Add(opt);
        ////    }

        ////}

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
