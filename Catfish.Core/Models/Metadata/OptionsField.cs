using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Catfish.Core.Models.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catfish.Core.Models.Metadata
{
    [NotMapped]
    public class Option
    {
        public string Value { get; set; }
        public bool Selected { get; set; }

        public Option (string value = "", bool selected = false)
        {
            Value = value;
            Selected = selected;
        }

        public Option()
        {
            Value = "";
            Selected = false;
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
                return GetOptions("en");
            }
            set
            {
                SetOptions(value, "en");
            }
        }

        public List<Option> GetOptions(string lang = "")
        {
            List<Option> options = new List<Option>();
            IEnumerable<XElement> optionElements = GetChildElements("./options/option", Data);
            string val_path = "./text[@xml:lang='" + Lang(lang) + "']";
            foreach (XElement opt in optionElements)
            {
                string value = GetChildElements(val_path, opt).Select(txt => txt.Value).FirstOrDefault();
                bool selected = GetAttribute("selected", false, opt);
                Option option = new Option(value, selected);
                options.Add(option);
            }


            ////XElement optionsElement = Data.Element("options");
            ////if (optionsElement != null)
            ////{
            ////    IEnumerable<XElement> optionElements = GetChildElements("./options/option", Data);
            ////    foreach (XElement optionElement in optionElements)
            ////    {
            ////        string value = GetChildText("option", Data, lang);
                    
            ////        bool selected = false;
            ////        string selectedString = optionElement.Attribute("selected").Value;
            ////        if (selectedString == "true")
            ////        {
            ////            selected = true;
            ////        }
            ////        Option option = new Option(value, selected);
            ////        options.Add(option);
            ////    }
            ////}

            return options;
        }

        public void SetOptions(List<Option> options, string lang)
        {
            string xpath = "./options";
            XElement optionsParent = this.GetChildElements(xpath, Data).FirstOrDefault();
            if(optionsParent == null)
            {
                optionsParent = new XElement("options");
                Data.Add(optionsParent);
            }

            //marking selected property of all existing options false
            List<XElement> optionList = optionsParent.Elements("option").ToList();
            foreach (var opt in optionList)
                opt.SetAttributeValue("selected", false);

            //Iterating through all input "options", updating selections and 
            //inserting any new options
            string val_path = "./text[@xml:lang='" + Lang(lang) + "']";
            foreach (var opt in options)
            {
                bool found = false;
                foreach(XElement opEle in optionList)
                {
                    List<string> vals = GetChildElements(val_path, opEle).Select(txt => txt.Value).ToList();
                    if (vals.Contains(opt.Value))
                    {
                        opEle.SetAttributeValue("selected", opt.Selected);
                        found = true;
                        break;
                    }
                }

                if(!found)
                {
                    XElement opEle = CreateOption(opt.Value, opt.Selected, lang);
                    optionsParent.Add(opEle);
                }
            }

            //////setting selected=true for the apporpriate options
            ////List<Option> selected = options.Where(op => op.Selected).ToList();
            ////foreach (var opt in options)
            ////{
            ////    foreach(XElement dstOpt in optionList)
            ////    {
            ////        IEnumerable<string> vals = GetChildElements(val_path, dstOpt).Select(txt => txt.Value);
            ////        if (vals.Contains(opt.Value))
            ////            dstOpt.SetAttributeValue("selected", true);
            ////    }
            ////}



            ////////////string val_path = "./text[@xml:lang='" + Lang(lang) + "']";
            ////////////foreach (Option opt in  options)
            ////////////{
            ////////////    SetOption(opt.Value, opt.Selected, optionsParent, lang);
            ////////////}

            //////IEnumerable<string> selectedOptionValues = options.Where(op => op.Selected).Select(op => op.Value);
            ////foreach(XElement opt in children)
            ////{
            ////    bool selected = false;
            ////    try
            ////    {
            ////        string val = GetTextElements(opt, lang).First().Value;
            ////        selected = bool.Parse(options.Where(op => op.Value == val).First().Value);
            ////    }
            ////    catch (Exception) { }
            ////    opt.SetAttributeValue("selected", selected);
            ////}
        }

        public void SetOption(string value, bool isSelected, XElement optionParent, string lang)
        {
            IEnumerable<XElement> children = this.GetChildElements("option", optionParent);
            string val_path = "./text[@xml:lang='" + Lang(lang) + "']";
            bool found = false;
            foreach (XElement opt in children)
            {
                XElement txt = GetTextElements(opt, lang).FirstOrDefault();
                if (txt != null && txt.Value == value)
                {
                    found = true;
                    break;
                }
                opt.SetAttributeValue("selected", isSelected);
            }

            if (!found)
            {
                XElement opt = CreateOption(value, isSelected, lang);
                optionParent.Add(opt);
            }

        }

        private XElement CreateOption(string value, bool isSelected, string lang)
        {
            XElement optionElement = new XElement("option");
            XElement textEelemnt = new XElement("text", new XAttribute(XNamespace.Xml + "lang", lang));
            textEelemnt.Value = string.IsNullOrEmpty(value) ? "" : value;
            optionElement.Add(textEelemnt);
            optionElement.SetAttributeValue("selected", isSelected);
            return optionElement;
        }


        ////////public void SetMultipleValues(IEnumerable<string> values, string language = null)
        ////////{
        ////////    this.ClearSelectedOptions(Data);
        ////////    this.SetSelectedOptions(values, Data, Lang(language));
        ////////}


        ////////protected void ClearSelectedOptions(XElement data)
        ////////{
        ////////    string xpath = "./options/option";
        ////////    List<XElement> children = this.GetChildElements(xpath, data).ToList();
        ////////    foreach (XElement child in children)
        ////////    {
        ////////        child.SetAttributeValue("selected", false);
        ////////    }
        ////////}

        ////////protected void SetSelectedOptions(IEnumerable<string> values, XElement data, string language = null)
        ////////{
        ////////    string xpath = "./options/option";
        ////////    List<XElement> children = this.GetChildElements(xpath, data).ToList();

        ////////    foreach (string value in values)
        ////////    {
        ////////        bool found = false;
        ////////        foreach (XElement child in children)
        ////////        {

        ////////            IEnumerable<XElement> texts = this.GetTextElements(child, language);

        ////////            foreach (XElement text in texts)
        ////////            {
        ////////                if (value == text.Value)
        ////////                {
        ////////                    found = true;
        ////////                    break;
        ////////                }
        ////////            }

        ////////            if (found)
        ////////            {
        ////////                child.SetAttributeValue("selected", true);
        ////////                break;
        ////////            }
        ////////        }

        ////////        if (!found)
        ////////        {
        ////////            data.Add(this.CreateSelectedOption(value, language));
        ////////        }
        ////////    }
        ////////}


        public override void UpdateValues(XmlModel src)
        {
            string lang = "";
            OptionsField optionsField = src as OptionsField;
            this.SetOptions(optionsField.GetOptions(), Lang(lang));
            ////this.SetMultipleValues(optionsField.GetValues());
        }        
    }

}
