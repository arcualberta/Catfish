using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Catfish.Core.Models.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Catfish.Core.Models.Metadata
{

    public class Option
    {
        public string Value = "";
        public bool Selected = false;

        public Option (string value = "", bool selected = false)
        {
            this.Value = value;
            this.Selected = selected;
        }
    }

    [Ignore]
    public partial class OptionsField: MetadataField
    {
        [DataType(DataType.MultilineText)]
        [TypeLabel("List of options, one option per line")]
        public IEnumerable<Option> Options
        {
            get
            {
                return GetOptions("en");
            }
            //set;
        }

        public IEnumerable<Option> GetOptions(string lang = "")
        {
            List<Option> options = new List<Option>();

            XElement optionsElement = Data.Element("options");
            if (optionsElement != null)
            {

                IEnumerable<XElement> optionElements = optionsElement.Elements("option");

                
                foreach (XElement optionElement in optionElements)
                {
                    //XXX look fo get text on xmlmodel
                    string value = optionElement.Element("text").Value;
                    bool selected = false;
                    string selectedString = optionElement.Attribute("selected").Value;
                    if (selectedString == "true")
                    {
                        selected = true;
                    }
                    Option option = new Option(value, selected);
                    options.Add(option);
                }
            }

            return options;
        }

        //public string GetOptions(string lang = null)
        //{
        //    string result = null;
        //    if (lang == null)
        //        lang = DefaultLanguage;

        //    XElement options_element = Data.Element("options");
        //    if (options_element != null)
        //    {
        //        IEnumerable<XElement> option_text_elements = GetChildTextElements("option", options_element, lang);
        //        IEnumerable<string> options = option_text_elements.Select(op => op.Value);
        //        result = string.Join("\n", options);
        //    }
        //    return result;
        //}

        
        public override void UpdateValues(XmlModel src)
        {
            OptionsField optionsField = src as OptionsField;
            this.SetMultipleValues(optionsField.GetValues());
        }
        
        ////////public override XElement ToXml()
        ////////{
        ////////    XElement ele = base.ToXml();
        ////////    XElement options = new XElement("Options");
        ////////    ele.Add(options);

        ////////    ////options.Value = Options;
        ////////    foreach (var x in Options.Split(new char[] { '\n' }))
        ////////    {
        ////////        string opStr = x.Trim();
        ////////        if (!string.IsNullOrEmpty(opStr))
        ////////        {
        ////////            XElement op = new XElement("Option") { Value = opStr };
        ////////            options.Add(op);
        ////////        }
        ////////    }
        ////////    return ele;
        ////////}

        ////////public override void Initialize(XElement ele)
        ////////{
        ////////    base.Initialize(ele);
        ////////    var optionEnvelop = ele.Element("Options");
        ////////    List<string> options = new List<string>();
        ////////    foreach(var op in optionEnvelop.Elements("Option"))
        ////////    {
        ////////        options.Add(op.Value);
        ////////    }
        ////////    this.Options = string.Join("\n", options);
        ////////}
    }

}
